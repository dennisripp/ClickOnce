/*
 * SpiuartMaster.cpp
 *
 * Created: 05.06.2018 18:30:52
 *  Author: micha
 */ 

#include "SpiUsartMaster.h"

 SpiUsartMaster::SpiUsartMaster(USART_t *_module, PORT_t *_port, SPI_MODE_t mode, uint32_t clock)
{
	module = _module;
	port = _port;
	transmissionComplete = true;

	configure(mode, clock);
}

 SpiUsartMaster::SpiUsartMaster(USART_t *_module, PORT_t *_port)
{
	module = _module;
	port = _port;
	transmissionComplete = true;
}

void SpiUsartMaster::configure(SPI_MODE_t mode, uint32_t clock)
{
	// configure USART as SPI Master
	// 1. Set the TxD pin value high, and optionally set the XCK pin low.
	// 2. Set the TxD and optionally the XCK pin as output.
	// 3. Set the baud rate and frame format.
	// 4. Set the mode of operation (enables XCK pin output in synchronous mode).
	// 5. Enable the transmitter or the receiver, depending on the usage
	
	sysclk_enable_peripheral_clock(module);
	
	bool lsbFirst = false;	// data order
	bool cPha;				// clock Phase
	bool invEn;				// clock Polarity
	
	if (mode == SPI_MODE_0_gc || mode == SPI_MODE_2_gc)
		cPha = false;
	else if (mode == SPI_MODE_1_gc || mode == SPI_MODE_3_gc)
		cPha = true;
	if (mode == SPI_MODE_0_gc || mode == SPI_MODE_1_gc)
		invEn = false;
	else if (mode == SPI_MODE_2_gc || mode == SPI_MODE_3_gc)
		invEn = true;
	
	Pin* xckPin;
	uint32_t fPer = sysclk_get_per_hz();					// periphery clock (Hz)
	uint32_t bSel = (fPer / 2 / clock) - 1;					// see manual page 282
	if (fPer % clock > 0)									// round up
		bSel++;
	
	if (module == &USARTC0 || module == &USARTD0 || module == &USARTE0 || module == &USARTF0)
	{
		xckPin = new Pin(port, USART0_XCK_bm);
		xckPin->setInvertEnable(invEn);
		free(xckPin);
		port->OUTSET = USART0_TX_bm;							// TxD pin value high
		port->OUTCLR = USART0_XCK_bm;							// Set the XCK pin low
		port->DIRSET = USART0_TX_bm | USART0_XCK_bm;			// Set the TxD and optionally the XCK pin as output
		port->DIRCLR = USART0_RX_bm;							// Set RxD as input
	}
	else
	{
		xckPin = new Pin(port, USART1_XCK_bm);
		xckPin->setInvertEnable(invEn);
		free(xckPin);
		port->OUTSET = USART1_TX_bm;							// TxD pin value high
		port->OUTCLR = USART1_XCK_bm;							// Set the XCK pin low
		port->DIRSET = USART1_TX_bm | USART1_XCK_bm;			// Set the TxD and optionally the XCK pin as output
		port->DIRCLR = USART1_RX_bm;							// Set RxD as input
	}
	
	module->BAUDCTRLA = (bSel & 0x000000FF) >> 0;			// Set the baud rate 
	module->BAUDCTRLB = (bSel & 0x00000F00) >> 8;
	module->CTRLC = USART_CMODE_MSPI_gc |					// Master SPI Mode
					(lsbFirst ? USART_CHSIZE2_bm : 0) |		// Data order, UDORD = CHSIZE2
					(cPha ? USART_CHSIZE1_bm : 0);			// clock Phase, UCPHA = CHSIZE1
	module->CTRLB = USART_RXEN_bm |							// Receiver Enable
					USART_TXEN_bm;							// Transmitter Enable
	module->CTRLA = USART_RXCINTLVL_OFF_gc |				// Receive Complete Interrupt Level
					USART_TXCINTLVL_OFF_gc |				// Transmit Complete Interrupt Level
					USART_DREINTLVL_OFF_gc;					// Data Register Empty Interrupt Level
	
	if (module->STATUS & USART_RXCIF_bm)
	{
		// read data register to clear interrupt flag
		volatile uint8_t temp = module->DATA;
	}
}

uint8_t SpiUsartMaster::transceive(uint8_t transmitData)
{
	module->DATA = transmitData;
	while (!(module->STATUS & USART_RXCIF_bm));
	return module->DATA;
}

uint8_t SpiUsartMaster::transceiveAsync(uint8_t transmitData)
{
	register uint8_t temp = transmitData;
	if (!transmissionComplete)
	{
		while(!(module->STATUS & USART_TXCIF_bm));
		module->DATA = transmitData;
		while(!(module->STATUS & USART_RXCIF_bm));
		temp = module->DATA;		// data received in previous transmission cycle
	}
	else
	{
		module->DATA = transmitData;
		temp = module->DATA;		// data received in previous transmission cycle
		transmissionComplete = false;
	}
	return temp;
}

uint8_t SpiUsartMaster::waitUntilTransmissionComplete()
{
	uint8_t temp;
	if (!transmissionComplete)
	{
		transmissionComplete = true;
		while(!(module->STATUS & USART_RXCIF_bm));
		temp = module->DATA;	// data received in previous transmission cycle
	}
	return temp;
}
