/* 
* PixelArray.cpp
*
* Created: 11.07.2018 01:40:16
* Author: Maxi
*/


#include "PixelArray.h"

//Constructor
PixelArray::PixelArray(uint8_t _colCount, uint8_t _rowCount, Pixel* _pixels[8][8], NCV7608* _drivers[8])
{
	colCount = _colCount;
	rowCount = _rowCount;
	pixel00Ptr = reinterpret_cast<uint16_t*>(&(_pixels[0][0]));
	driver0Ptr = reinterpret_cast<uint16_t*>(&(_drivers[0]));
	pixelInitV1(_pixels,_drivers);
} 

//Destructor
PixelArray::~PixelArray()
{
} 

//returns absolute number of columns
uint8_t PixelArray::getColCount()
{
	return colCount;
}

//returns absolute number of rows
uint8_t PixelArray::getRowCount()
{
	return rowCount;
}

//setting up each pixel to print 8x8 frame
void PixelArray::print_letter(bool letter[8][8])
{
	for (uint8_t col = 0; col < colCount; col++)
	{
		for (uint8_t row = 0; row < rowCount; row++)
		{
			uint16_t pixelColRowPtr = pixel00Ptr[row * colCount + col];
			Pixel* pixelColRow = reinterpret_cast<Pixel*>(pixelColRowPtr);
			pixelColRow->setEnable(letter[row][col]);
		}
	}
	refresh_drivers();
}

//8x8 Arrays
void PixelArray::PrintFrame(uint64_t _data)
{
	for (uint8_t col = 0; col < colCount; col++)
	{
		for (uint8_t row = 0; row < rowCount; row++)
		{
			uint16_t pixelColRowPtr = pixel00Ptr[row * colCount + col];
			Pixel* pixelColRow = reinterpret_cast<Pixel*>(pixelColRowPtr);
			pixelColRow->setEnable(_data & 0b1);
			_data = _data >> 1;
		}
	}
	refresh_drivers();
}

//setting up one pixel
void PixelArray::print_pixel(uint8_t _col,uint8_t _row ,bool on)
{
	uint16_t pixelColRowPtr = pixel00Ptr[_row * colCount + _col];
	Pixel* pixelColRow = reinterpret_cast<Pixel*>(pixelColRowPtr);
	pixelColRow->setEnable(on);
	refresh_drivers();
}

//driving all Changes
void PixelArray::refresh_drivers()
{
	for (uint8_t driverIndex = 0; driverIndex < 8; driverIndex++)
	{
		uint16_t driverPtr = driver0Ptr[driverIndex];
		NCV7608* driver = reinterpret_cast<NCV7608*>(driverPtr);
		driver->writeChanges();
	}
}

//initialize pixels
void PixelArray::pixelInitV1(Pixel* pixels[8][8], NCV7608* drivers[8])
{

	pixels[0][0] = new Pixel(drivers[6], 4);
	pixels[0][1] = new Pixel(drivers[6], 5);
	pixels[0][2] = new Pixel(drivers[6], 3);
	pixels[0][3] = new Pixel(drivers[6], 6);
	pixels[0][4] = new Pixel(drivers[7], 4);
	pixels[0][5] = new Pixel(drivers[7], 1);
	pixels[0][6] = new Pixel(drivers[7], 7);
	pixels[0][7] = new Pixel(drivers[4], 3);
	
	pixels[1][0] = new Pixel(drivers[0], 7);
	pixels[1][1] = new Pixel(drivers[6], 2);
	pixels[1][2] = new Pixel(drivers[6], 7);
	pixels[1][3] = new Pixel(drivers[7], 5);
	pixels[1][4] = new Pixel(drivers[7], 0);
	pixels[1][5] = new Pixel(drivers[7], 6);
	pixels[1][6] = new Pixel(drivers[4], 5);
	pixels[1][7] = new Pixel(drivers[4], 2);
	
	pixels[2][0] = new Pixel(drivers[0], 1);
	pixels[2][1] = new Pixel(drivers[0], 6);	// dead
	pixels[2][2] = new Pixel(drivers[0], 3);
	pixels[2][3] = new Pixel(drivers[0], 2);
	pixels[2][4] = new Pixel(drivers[6], 1);
	pixels[2][5] = new Pixel(drivers[7], 3);
	pixels[2][6] = new Pixel(drivers[4], 0);
	pixels[2][7] = new Pixel(drivers[4], 4);
	
	pixels[3][0] = new Pixel(drivers[0], 4);
	pixels[3][1] = new Pixel(drivers[0], 0);
	pixels[3][2] = new Pixel(drivers[1], 1);
	pixels[3][3] = new Pixel(drivers[6], 0);
	pixels[3][4] = new Pixel(drivers[4], 7);
	pixels[3][5] = new Pixel(drivers[7], 2);
	pixels[3][6] = new Pixel(drivers[5], 2);
	pixels[3][7] = new Pixel(drivers[4], 1);
	
	pixels[4][0] = new Pixel(drivers[1], 6);
	pixels[4][1] = new Pixel(drivers[0], 5);
	pixels[4][2] = new Pixel(drivers[2], 5);
	pixels[4][3] = new Pixel(drivers[1], 0);
	pixels[4][4] = new Pixel(drivers[3], 7);
	pixels[4][5] = new Pixel(drivers[4], 6);
	pixels[4][6] = new Pixel(drivers[5], 7);
	pixels[4][7] = new Pixel(drivers[5], 3);
	
	pixels[5][0] = new Pixel(drivers[1], 3);
	pixels[5][1] = new Pixel(drivers[1], 7);	// dead
	pixels[5][2] = new Pixel(drivers[2], 4);
	pixels[5][3] = new Pixel(drivers[3], 6);
	pixels[5][4] = new Pixel(drivers[5], 5);
	pixels[5][5] = new Pixel(drivers[5], 4);
	pixels[5][6] = new Pixel(drivers[5], 1);
	pixels[5][7] = new Pixel(drivers[5], 6);
	
	pixels[6][0] = new Pixel(drivers[1], 5);
	pixels[6][1] = new Pixel(drivers[1], 2);
	pixels[6][2] = new Pixel(drivers[2], 1);
	pixels[6][3] = new Pixel(drivers[2], 7);
	pixels[6][4] = new Pixel(drivers[2], 2);
	pixels[6][5] = new Pixel(drivers[3], 0);
	pixels[6][6] = new Pixel(drivers[3], 5);
	pixels[6][7] = new Pixel(drivers[5], 0);
	
	pixels[7][0] = new Pixel(drivers[1], 4);
	pixels[7][1] = new Pixel(drivers[2], 0);
	pixels[7][2] = new Pixel(drivers[2], 6);
	pixels[7][3] = new Pixel(drivers[2], 3);
	pixels[7][4] = new Pixel(drivers[3], 1);
	pixels[7][5] = new Pixel(drivers[3], 4);
	pixels[7][6] = new Pixel(drivers[3], 2);
	pixels[7][7] = new Pixel(drivers[3], 3);
}

//initialize pixels
void PixelArray::pixelInitV2(Pixel* pixels[8][8], NCV7608* drivers[8])
{
	pixels[0][0] = new Pixel(drivers[6], 3);
	pixels[0][1] = new Pixel(drivers[0], 1);
	pixels[0][2] = new Pixel(drivers[0], 7);
	pixels[0][3] = new Pixel(drivers[0], 2);
	pixels[0][4] = new Pixel(drivers[1], 4);
	pixels[0][5] = new Pixel(drivers[1], 2);
	pixels[0][6] = new Pixel(drivers[2], 2);
	pixels[0][7] = new Pixel(drivers[1], 3);
	
	pixels[1][0] = new Pixel(drivers[0], 5);
	pixels[1][1] = new Pixel(drivers[0], 0);
	pixels[1][2] = new Pixel(drivers[0], 4);
	pixels[1][3] = new Pixel(drivers[1], 7);
	pixels[1][4] = new Pixel(drivers[1], 0);
	pixels[1][5] = new Pixel(drivers[1], 5);
	pixels[1][6] = new Pixel(drivers[2], 7);
	pixels[1][7] = new Pixel(drivers[2], 6);
	
	pixels[2][0] = new Pixel(drivers[6], 2);
	pixels[2][1] = new Pixel(drivers[6], 5);
	pixels[2][2] = new Pixel(drivers[0], 6);
	pixels[2][3] = new Pixel(drivers[0], 3);
	pixels[2][4] = new Pixel(drivers[1], 6);
	pixels[2][5] = new Pixel(drivers[2], 1);
	pixels[2][6] = new Pixel(drivers[2], 3);
	pixels[2][7] = new Pixel(drivers[2], 0);
	
	pixels[3][0] = new Pixel(drivers[6], 4);
	pixels[3][1] = new Pixel(drivers[6], 0);
	pixels[3][2] = new Pixel(drivers[6], 6);
	pixels[3][3] = new Pixel(drivers[1], 1);
	pixels[3][4] = new Pixel(drivers[3], 6);
	pixels[3][5] = new Pixel(drivers[2], 4);
	pixels[3][6] = new Pixel(drivers[3], 0);
	pixels[3][7] = new Pixel(drivers[2], 5);
	
	pixels[4][0] = new Pixel(drivers[7], 2);
	pixels[4][1] = new Pixel(drivers[6], 7);
	pixels[4][2] = new Pixel(drivers[7], 3);
	pixels[4][3] = new Pixel(drivers[6], 1);
	pixels[4][4] = new Pixel(drivers[4], 6);
	pixels[4][5] = new Pixel(drivers[3], 1);
	pixels[4][6] = new Pixel(drivers[3], 7);
	pixels[4][7] = new Pixel(drivers[3], 3);
	
	pixels[5][0] = new Pixel(drivers[7], 7);
	pixels[5][1] = new Pixel(drivers[7], 4);
	pixels[5][2] = new Pixel(drivers[7], 6);
	pixels[5][3] = new Pixel(drivers[4], 1);
	pixels[5][4] = new Pixel(drivers[5], 4);
	pixels[5][5] = new Pixel(drivers[5], 1);
	pixels[5][6] = new Pixel(drivers[3], 2);
	pixels[5][7] = new Pixel(drivers[3], 5);
	
	pixels[6][0] = new Pixel(drivers[7], 1);
	pixels[6][1] = new Pixel(drivers[7], 0);
	pixels[6][2] = new Pixel(drivers[4], 2);
	pixels[6][3] = new Pixel(drivers[4], 7);
	pixels[6][4] = new Pixel(drivers[4], 0);
	pixels[6][5] = new Pixel(drivers[5], 3);
	pixels[6][6] = new Pixel(drivers[5], 7);
	pixels[6][7] = new Pixel(drivers[5], 2);
	
	pixels[7][0] = new Pixel(drivers[4], 4);
	pixels[7][1] = new Pixel(drivers[7], 5);
	pixels[7][2] = new Pixel(drivers[4], 5);
	pixels[7][3] = new Pixel(drivers[4], 3);
	pixels[7][4] = new Pixel(drivers[5], 5);
	pixels[7][5] = new Pixel(drivers[5], 0);
	pixels[7][6] = new Pixel(drivers[5], 6);
	pixels[7][7] = new Pixel(drivers[3], 4);
}

//initialize pixels
void PixelArray::pixelInitV25(Pixel* pixels[8][8], NCV7608* drivers[8])
{
	pixels[0][0] = new Pixel(drivers[0], 7);
	pixels[0][1] = new Pixel(drivers[6], 2);
	pixels[0][2] = new Pixel(drivers[6], 7);
	pixels[0][3] = new Pixel(drivers[7], 5);
	pixels[0][4] = new Pixel(drivers[7], 4);
	pixels[0][5] = new Pixel(drivers[7], 1);
	pixels[0][6] = new Pixel(drivers[7], 2);
	pixels[0][7] = new Pixel(drivers[7], 7);
	
	pixels[1][0] = new Pixel(drivers[0], 2);
	pixels[1][1] = new Pixel(drivers[6], 4);
	pixels[1][2] = new Pixel(drivers[6], 1);
	pixels[1][3] = new Pixel(drivers[6], 0);
	pixels[1][4] = new Pixel(drivers[7], 0);
	pixels[1][5] = new Pixel(drivers[7], 6);
	pixels[1][6] = new Pixel(drivers[4], 3);
	pixels[1][7] = new Pixel(drivers[4], 5);	// dead
	
	pixels[2][0] = new Pixel(drivers[0], 1);
	pixels[2][1] = new Pixel(drivers[0], 6);
	pixels[2][2] = new Pixel(drivers[6], 5);
	pixels[2][3] = new Pixel(drivers[6], 3);
	pixels[2][4] = new Pixel(drivers[7], 3);
	pixels[2][5] = new Pixel(drivers[4], 2);	// dead
	pixels[2][6] = new Pixel(drivers[4], 6);
	pixels[2][7] = new Pixel(drivers[4], 0);
	
	pixels[3][0] = new Pixel(drivers[0], 4);
	pixels[3][1] = new Pixel(drivers[0], 0);
	pixels[3][2] = new Pixel(drivers[0], 3);
	pixels[3][3] = new Pixel(drivers[6], 6);	// dead
	pixels[3][4] = new Pixel(drivers[4], 1);
	pixels[3][5] = new Pixel(drivers[4], 4);
	pixels[3][6] = new Pixel(drivers[4], 7);
	pixels[3][7] = new Pixel(drivers[5], 2);
	
	pixels[4][0] = new Pixel(drivers[0], 5);
	pixels[4][1] = new Pixel(drivers[1], 0);
	pixels[4][2] = new Pixel(drivers[1], 3);
	pixels[4][3] = new Pixel(drivers[1], 6);
	pixels[4][4] = new Pixel(drivers[3], 1);
	pixels[4][5] = new Pixel(drivers[5], 4);
	pixels[4][6] = new Pixel(drivers[5], 7);
	pixels[4][7] = new Pixel(drivers[5], 3);
	
	pixels[5][0] = new Pixel(drivers[1], 7);
	pixels[5][1] = new Pixel(drivers[1], 1);
	pixels[5][2] = new Pixel(drivers[1], 5);
	pixels[5][3] = new Pixel(drivers[2], 4);
	pixels[5][4] = new Pixel(drivers[3], 4);
	pixels[5][5] = new Pixel(drivers[3], 2);
	pixels[5][6] = new Pixel(drivers[4], 1);
	pixels[5][7] = new Pixel(drivers[5], 6);	// dead
	
	pixels[6][0] = new Pixel(drivers[1], 2);
	pixels[6][1] = new Pixel(drivers[1], 4);
	pixels[6][2] = new Pixel(drivers[2], 1);
	pixels[6][3] = new Pixel(drivers[2], 7);
	pixels[6][4] = new Pixel(drivers[3], 7);
	pixels[6][5] = new Pixel(drivers[3], 6);	// dead
	pixels[6][6] = new Pixel(drivers[3], 3);
	pixels[6][7] = new Pixel(drivers[5], 5);
	
	pixels[7][0] = new Pixel(drivers[2], 0);
	pixels[7][1] = new Pixel(drivers[2], 5);
	pixels[7][2] = new Pixel(drivers[2], 6);
	pixels[7][3] = new Pixel(drivers[2], 3);
	pixels[7][4] = new Pixel(drivers[2], 2);
	pixels[7][5] = new Pixel(drivers[3], 0);
	pixels[7][6] = new Pixel(drivers[3], 5);
	pixels[7][7] = new Pixel(drivers[5], 0);
}