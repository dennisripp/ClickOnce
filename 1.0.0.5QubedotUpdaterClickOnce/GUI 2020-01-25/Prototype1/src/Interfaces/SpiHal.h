/*
 * SpiHal.h
 *
 * Created: 23.01.2018 17:06:52
 *  Author: fahrbach
 */ 


#ifndef SPIHAL_H_
#define SPIHAL_H_

#include "asf.h"
#include "Periphery/Pin.h"

/* Hardware defines SPI */
#define SPI_SS_bm			0x10	/*!< \brief Bit mask for the SS pin. */
#define SPI_MOSI_bm			0x20	/*!< \brief Bit mask for the MOSI pin. */
#define SPI_MISO_bm			0x40	/*!< \brief Bit mask for the MISO pin. */
#define SPI_SCK_bm			0x80	/*!< \brief Bit mask for the SCK pin. */
/* Hardware defines USART */
#define USART0_XCK_bm		0x02	// USART XCK pin Bit mask
#define USART0_RX_bm		0x04	// USART RX pin Bit mask
#define USART0_TX_bm		0x08	// USART TX pin Bit mask
#define USART1_XCK_bm		0x20	// USART XCK pin Bit mask
#define USART1_RX_bm		0x40	// USART RX pin Bit mask
#define USART1_TX_bm		0x80	// USART TX pin Bit mask

class SpiHal
{
	public:
		virtual void configure(SPI_MODE_t mode, uint32_t clock) = 0;
		virtual uint8_t transceive(uint8_t transmitData) = 0;
		virtual uint8_t transceiveAsync(uint8_t transmitData) = 0;
		virtual uint8_t waitUntilTransmissionComplete() = 0;
};

#endif /* SPIHAL_H_ */