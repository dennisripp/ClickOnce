/*
 * SPI_Master.h
 *
 * Created: 23.03.2018 14:48:10
 *  Author: fahrbach
 */ 


#ifndef SPI_INTERFACE_H_
#define SPI_INTERFACE_H_

#include "AppNotes/avr_compiler.h"
#include "Interfaces/SpiHal.h"
#include "Periphery/Pin.h"

class SPI
{
public:
	SPI(SpiHal *spiHardware, Pin *ssPin);
	SpiHal *getSpiHal();
	template <typename T> void transmit(T* transmitData)
	{
		uint8_t byteCount = sizeof(T);
		uint8_t* transmitPtr = reinterpret_cast<uint8_t*>(transmitData);
		setSS();
		for (uint8_t byteIndex = byteCount - 1; byteIndex != 0xFF; byteIndex--)
			SpiHardware->transceiveAsync(transmitPtr[byteIndex]);
		SpiHardware->waitUntilTransmissionComplete();
		releaseSS();
	}
	template <typename T> void receive(T* receiveData)
	{
		uint8_t byteCount = sizeof(T);
		uint8_t* receivePtr = reinterpret_cast<uint8_t*>(receiveData);
		setSS();
		SpiHardware->transceiveAsync(0);
		for (uint8_t byteIndex = byteCount - 1; byteIndex != 0x00; byteIndex--)
			receivePtr[byteIndex] = SpiHardware->transceiveAsync(0);
		receivePtr[0] = SpiHardware->waitUntilTransmissionComplete();
		releaseSS();
	}
	template <typename T> void transceive(T* transmitData, T* receiveData)
	{
		uint8_t byteCount = sizeof(T);
		uint8_t* transmitPtr = reinterpret_cast<uint8_t*>(transmitData);
		uint8_t* receivePtr = reinterpret_cast<uint8_t*>(receiveData);
		setSS();
		SpiHardware->transceiveAsync(transmitPtr[byteCount - 1]);
		for (uint8_t byteIndex = byteCount - 1; byteIndex != 0x00; byteIndex--)
			receivePtr[byteIndex] = SpiHardware->transceiveAsync(transmitPtr[byteIndex - 1]);
		receivePtr[0] = SpiHardware->waitUntilTransmissionComplete();
		releaseSS();
	}
private:
	void setSS();
	void releaseSS();
	SpiHal *SpiHardware;
	Pin *ssPin;
};

#endif /* SPI_INTERFACE_H_ */