/*
 * main.h
 *
 * Created: 01.06.2018 09:07:19
 *  Author: micha
 */ 


#ifndef MAIN_H_
#define MAIN_H_

#define F_CPU 62000000

#include <asf.h>
#include <stdio.h>
#include <sleepmgr.h>
#include <sysclk.h>
#include <util/delay.h>
#include "Interfaces/SPI.h"
#include "Interfaces/SpiHal.h"
#include "Interfaces/SpiNativeMaster.h"
#include "Interfaces/SpiUsartMaster.h"
#include "Devices/NCV7608.h"
#include "Pixel.h"
#include "Periphery/AnalogInput.h"
#include "Periphery/AnalogInputSE.h"
#include "Periphery/AnalogInputDE.h"
#include "Periphery/DAC.h"
#include "Periphery/AnalogOutput.h"
#include "udc.h"
#include "usb.h"
#include "generate_frame.h"
#include "PixelArray.h"
#include "SystemState.h"
#include "CommandDecoder.h"
#include "CommunicationControl.h"
#include "SystemValues.h"

int main(void);
void usbInit();
void driverInit();
void analogInputInit();
void analogOutputInit();
void timerInit();
void setLedVoltage(uint16_t voltage);
void setLedCurrentLimit(uint16_t current);
void pixelArrayInit();
void frameInit();
void systemStateInit();
void CommandDecoderInit();
void CommunicationControlInit();
void SystemValuesInit();



#endif /* MAIN_H_ */