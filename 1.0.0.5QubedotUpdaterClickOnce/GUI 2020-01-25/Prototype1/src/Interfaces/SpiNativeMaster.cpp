/*
 * SpiNative.cpp
 *
 * Created: 05.06.2018 18:24:59
 *  Author: micha
 */ 

#include "SpiNativeMaster.h"

SpiNativeMaster::SpiNativeMaster(SPI_t *_module, PORT_t *_port, SPI_MODE_t mode, uint32_t clock)
{
	module = _module;
	port = _port;
	transmissionComplete = true;

	configure(mode, clock);
}

SpiNativeMaster::SpiNativeMaster(SPI_t *_module, PORT_t *_port)
{
	module = _module;
	port = _port;
	transmissionComplete = true;
}

void SpiNativeMaster::configure(SPI_MODE_t mode, uint32_t clock)
{
	sysclk_enable_peripheral_clock(module);
	
	/* Init MOSI, SCK and SS pin as output */
	port->DIRSET = SPI_MOSI_bm | SPI_SCK_bm | SPI_SS_bm;
	
	bool lsbFirst = false;
	SPI_INTLVL_t intLevel = SPI_INTLVL_OFF_gc;
	SPI_PRESCALER_t clockDivision;
	bool clk2x;
	
	
	uint32_t fPer = sysclk_get_per_hz();	// periphery clock (Hz)
	uint32_t prescaler = fPer / clock;
	if (fPer % clock > 0)
		prescaler++;
	
	if (prescaler <= 2)
	{
		clockDivision = SPI_PRESCALER_DIV4_gc;
		clk2x = true;
	}
	else if (prescaler <= 4)
	{
		clockDivision = SPI_PRESCALER_DIV4_gc;
		clk2x = false;
	}
	else if (prescaler <= 8)
	{
		clockDivision = SPI_PRESCALER_DIV16_gc;
		clk2x = true;
	}
	else if (prescaler <= 16)
	{
		clockDivision = SPI_PRESCALER_DIV16_gc;
		clk2x = false;
	}
	else if (prescaler <= 32)
	{
		clockDivision = SPI_PRESCALER_DIV64_gc;
		clk2x = true;
	}
	else if (prescaler <= 64)
	{
		clockDivision = SPI_PRESCALER_DIV64_gc;
		clk2x = false;
	}
	else
	{
		clockDivision = SPI_PRESCALER_DIV128_gc;
		clk2x = false;
	}

	/* Initialize SPI master */
	module->CTRL = clockDivision |						/* SPI prescaler. */
					(clk2x ? SPI_CLK2X_bm : 0) |		/* SPI Clock double. */
					SPI_ENABLE_bm |						/* Enable SPI module. */
					(lsbFirst ? SPI_DORD_bm  : 0) |		/* Data order. */
					SPI_MASTER_bm |						/* SPI master. */
					mode;								/* SPI mode. */

	/* Interrupt level. */
	module->INTCTRL = intLevel;
	
	if (module->STATUS & SPI_IF_bm)
	{
		// read data register to clear interrupt flag
		volatile uint8_t temp = module->DATA;
	}
}

uint8_t SpiNativeMaster::transceive(uint8_t transmitData)
{
	/* MASTER: Transmit data from master to slave. */
	/* Wait for transmission complete. */
	module->DATA = transmitData;
	while(!(module->STATUS & SPI_IF_bm));
	
	return module->DATA;
}

uint8_t SpiNativeMaster::transceiveAsync(uint8_t transmitData)
{
	register uint8_t temp = transmitData;
	if (!transmissionComplete)
		while(!(module->STATUS & SPI_IF_bm));
	module->DATA = temp;
	temp = module->DATA;		// data received in previous transmission cycle
	transmissionComplete = false;
	return temp;
}

uint8_t SpiNativeMaster::waitUntilTransmissionComplete()
{
	uint8_t temp;
	if (!transmissionComplete)
	{
		transmissionComplete = true;
		while(!(module->STATUS & SPI_IF_bm));
		temp = module->DATA;	// data received in previous transmission cycle
	}
	return temp;
}