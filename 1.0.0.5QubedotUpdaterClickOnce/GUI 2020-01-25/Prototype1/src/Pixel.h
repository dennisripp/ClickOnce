/*
 * Pixel.h
 *
 * Created: 01.06.2018 09:53:58
 *  Author: micha
 */ 


#ifndef PIXEL_H_
#define PIXEL_H_

#include "Devices/NCV7608.h"

class Pixel
{
	public:
		Pixel(NCV7608* _driver, uint8_t _channel);
		void setEnable(bool on);
		void toggle();
		bool getEnable();
		void setDiagnostics(bool on);
		bool getDiagnostics();
	private:
		NCV7608 *driver;
		uint8_t channel;
};


#endif /* PIXEL_H_ */