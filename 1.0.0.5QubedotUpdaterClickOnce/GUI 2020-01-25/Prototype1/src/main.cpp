#include "main.h"

	
/* Global variables */

NCV7608 *drivers[8];
Pixel *pixels[8][8];
AnalogInput *currentSense;
AnalogOutput *ledVoltage;
AnalogOutput *ledCurrent;
SystemValues* systemValues;
usb* usb_connection = new usb();
PixelArray* _pixelArray;
generate_frame* frame_generator;
SystemState* _systemState;
CommandDecoder* commandDecoder = new CommandDecoder();
CommunicationControl* communicationControl;

uint16_t CurrentMaxDefault = 100;
uint16_t VoltageMaxDefault = 11000;

uint8_t state = -1;
uint8_t colCount = 8;
uint8_t rowCount = 8;


int main (void)
{
	board_init();
	irq_initialize_vectors();
	cpu_irq_enable();	
	sleepmgr_init();
	sysclk_init();
	driverInit();
	pixelArrayInit();
	analogInputInit();
	analogOutputInit();
	SystemValuesInit();
	sysclk_enable_peripheral_clock(&PORTB);
	systemStateInit();
	usbInit();
	frameInit();
	timerInit();
	CommunicationControlInit();
	
	
	while (true)
	{
		communicationControl->USB_Communication();
	}	
	return 0;
}

void SystemValuesInit(){
	systemValues = new SystemValues(CurrentMaxDefault, VoltageMaxDefault, ledCurrent, ledVoltage);
}

void CommunicationControlInit(){
	communicationControl = new CommunicationControl(usb_connection, commandDecoder, _systemState, _pixelArray);
}

void pixelArrayInit() 
{
	_pixelArray = new PixelArray(colCount,rowCount,pixels,drivers);
}

void frameInit()
{
	frame_generator = new generate_frame(_pixelArray);
}

void systemStateInit(){
	_systemState = new SystemState(_pixelArray, systemValues);
}

void CommandDecoderInit(){
}

void usbInit() {
	udc_start();
	udc_attach();
}

void driverInit()
{
	SpiHal* tempSpiHw;
	Pin* tempSS;
	
	tempSpiHw = new SpiNativeMaster(&SPIC, &PORTC);
	tempSS = new Pin(&PORTF, PIN4_bm);
	drivers[0] = new NCV7608(new SPI(tempSpiHw, tempSS));
	tempSS = new Pin(&PORTA, PIN2_bm);
	drivers[1] = new NCV7608(new SPI(tempSpiHw, tempSS));
	tempSS = new Pin(&PORTA, PIN3_bm);
	drivers[2] = new NCV7608(new SPI(tempSpiHw, tempSS));
	tempSS = new Pin(&PORTC, PIN1_bm);
	drivers[3] = new NCV7608(new SPI(tempSpiHw, tempSS));
	
	tempSpiHw = new SpiNativeMaster(&SPIE, &PORTE);
	tempSS = new Pin(&PORTD, PIN5_bm);
	drivers[4] = new NCV7608(new SPI(tempSpiHw, tempSS));
	tempSS = new Pin(&PORTD, PIN4_bm);
	drivers[5] = new NCV7608(new SPI(tempSpiHw, tempSS));
	tempSS = new Pin(&PORTE, PIN4_bm);
	drivers[6] = new NCV7608(new SPI(tempSpiHw, tempSS));
	tempSS = new Pin(&PORTE, PIN0_bm);
	drivers[7] = new NCV7608(new SPI(tempSpiHw, tempSS));
}



void analogInputInit()
{
	currentSense = new AnalogInputDE(&ADCA, ADCCH_POS_PIN7, ADCCH_NEG_PIN0, ADC_CH0);
}

void analogOutputInit()
{
	DAC *dac = new DAC(&DACB, DAC_REFSEL_INT1V_gc, false);
	ledCurrent = new AnalogOutput(dac, 0, 0x16, 0x0A);
	ledVoltage = new AnalogOutput(dac, 1, 0x10, 0x01);
}

void timerInit()
{
	// LED array refresh rate in Hz
	int refreshRate = 1000;
	
	// state machine timer
	sysclk_enable_peripheral_clock(&TCD0);
	// set prescaler
	TCD0.CTRLA = TC_CLKSEL_DIV1_gc;
	TCD0.CTRLE = TC_BYTEM_NORMAL_gc;
	// period in seconds:
	// PER(t) = CPU frequency / (prescaler * f)
	TCD0.PER = F_CPU / ((uint32_t)1 * (uint32_t)refreshRate);
	// reset counter
	TCD0.CNT = 0x0000;
	// set high interrupt level
	TCD0.INTCTRLA = PORT_INT0LVL_MED_gc;
}

ISR(TCD0_OVF_vect)
{
	nop();
}





