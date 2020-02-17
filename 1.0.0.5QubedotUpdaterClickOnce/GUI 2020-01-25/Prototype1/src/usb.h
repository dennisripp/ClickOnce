/* 
* usb.h
*
* Created: 09.07.2018 22:02:41
* Author: Maxi
*/


#ifndef __USB_H__
#define __USB_H__

#include "PixelArray.h"
#include "CommandDecoder.h"
#include "SystemState.h"


class usb
{
//variables
	public:

	protected:

	private:
		uint8_t channel;

//functions
	public:
		usb();
		~usb();
		
		template <typename T>
		void receive(T* receiveData)
		{
			uint8_t byteCount = sizeof(T);
			uint8_t* receivePtr = reinterpret_cast<uint8_t*>(receiveData);
			for (uint8_t byteIndex = byteCount - 1; byteIndex != 0xFF; byteIndex--)
			{
				while (!udi_cdc_is_rx_ready());
				receivePtr[byteIndex] = udi_cdc_getc();
			}
		}
		
		template <typename T>
		void transmit(T* transmitData)
		{
			uint8_t byteCount = sizeof(T);
			uint8_t* transmitPtr = reinterpret_cast<uint8_t*>(transmitData);
			for (uint8_t byteIndex = byteCount - 1; byteIndex != 0xFF; byteIndex--)
			{
				while (!udi_cdc_is_tx_ready());
				udi_cdc_putc(transmitPtr[byteIndex]);
			}
		}
	protected:
	private:


};

#endif
