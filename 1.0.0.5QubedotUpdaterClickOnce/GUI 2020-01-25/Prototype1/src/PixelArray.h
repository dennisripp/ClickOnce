/* 
* PixelArray.h
*
* Created: 11.07.2018 01:40:17
* Author: Maxi
*/


#ifndef __PIXELARRAY_H__
#define __PIXELARRAY_H__

#include "Pixel.h"


class PixelArray
{
//variables
	public:
	protected:
	private:
		uint16_t* pixel00Ptr;
		uint16_t* driver0Ptr;
		uint8_t colCount;
		uint8_t rowCount;

//functions
	public:
		PixelArray();
		~PixelArray();
		void print_letter(bool letter[8][8]);
		PixelArray(uint8_t _colCount, uint8_t _rowCount, Pixel* _pixels[8][8], NCV7608* _drivers[8]);
		void pixelInitV1(Pixel* pixels[8][8], NCV7608* drivers[8]);
		void pixelInitV2(Pixel* pixels[8][8], NCV7608* drivers[8]);
		void pixelInitV25(Pixel* pixels[8][8], NCV7608* drivers[8]);
		void print_pixel(uint8_t _col,uint8_t _row ,bool on);
		void PrintFrame(uint64_t _data);
		void refresh_drivers();
		uint8_t getColCount();
		uint8_t getRowCount();			
	protected:
	private:		
		PixelArray& operator=( const PixelArray &c );		
};

#endif
