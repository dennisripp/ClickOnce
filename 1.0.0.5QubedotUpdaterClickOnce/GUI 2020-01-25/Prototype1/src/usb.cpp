/* 
* usb.cpp
*
* Created: 09.07.2018 22:02:41
* Author: Maxi
*/


#include "usb.h"
// usb constructor
usb::usb()
{
}

// usb destructor
usb::~usb()
{
} 

/*
template <typename T> 
void usb::transmit(T* transmitData)
{
	uint8_t byteCount = sizeof(T);
	uint8_t* transmitPtr = reinterpret_cast<uint8_t*>(transmitData);
	for (uint8_t byteIndex = byteCount - 1; byteIndex != 0xFF; byteIndex--)
	{
		while (!udi_cdc_is_tx_ready());
		udi_cdc_putc(transmitPtr[byteIndex]);
	}
}

template <typename T> 
void usb::receive(T* receiveData)
{
	uint8_t byteCount = sizeof(T);
	uint8_t* receivePtr = reinterpret_cast<uint8_t*>(receiveData);
	for (uint8_t byteIndex = byteCount - 1; byteIndex != 0xFF; byteIndex--)
	{
		while (!udi_cdc_is_rx_ready());
		receivePtr[byteIndex] = udi_cdc_getc();
	}
}
*/

