/* 
* generate_frame.cpp
*
* Created: 10.07.2018 14:02:51
* Author: Maxi
*/

		bool Q_bool[8][8] =		{
			{0,0,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0},
			{0,1,1,1,0,0,0,0},
			{0,1,0,0,1,0,0,0},
			{1,0,0,0,1,0,0,0},
			{0,1,0,0,1,1,0,0},
			{0,1,1,1,0,1,0,0},
			{0,0,0,0,0,0,0,0}
		};

		
		bool Q_bool2[8][8] =	{
			{0,1,1,1,1,0,0,0},
			{0,1,0,0,0,1,0,0},
			{1,0,0,0,0,1,0,0},
			{1,0,0,0,0,0,1,0},
			{1,0,0,0,1,1,0,0},
			{0,1,0,0,0,1,1,0},
			{0,1,1,1,1,0,1,0},
			{0,0,0,0,0,0,0,0}
		};

		bool F_bool[8][8] =		{
			{0,1,1,1,1,0,0,0},
			{0,1,0,0,0,0,0,0},
			{0,1,0,0,0,0,0,0},
			{0,1,1,1,1,0,0,0},
			{0,1,0,0,0,0,0,0},
			{0,1,0,0,0,0,0,0},
			{0,1,0,0,0,0,0,0},
			{0,0,0,0,0,0,0,0}
		};

		bool ones[8][8] =		{
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1}
		};

		bool SlpAnim1[8][8] =	{
			{0,1,1,1,0,0,1,0},
			{0,1,0,0,1,0,0,1},
			{0,1,0,0,0,0,1,0},
			{0,0,1,1,1,0,0,1},
			{0,0,0,0,1,0,1,0},
			{0,0,1,0,0,1,0,1},
			{0,0,1,1,1,0,1,1},
			{0,0,0,0,0,0,0,0}
		};

		bool SlpAnim2[8][8] =	{
			{0,0,0,1,1,1,0,0},
			{0,0,0,0,1,0,1,0},
			{0,0,0,1,0,0,1,0},
			{0,0,0,0,1,1,1,0},
			{0,0,0,1,0,0,0,0},
			{0,0,0,0,1,0,0,0},
			{1,1,0,1,0,0,0,0},
			{0,0,0,0,0,0,0,0}
		};
		
		bool SlpAnim3[8][8] = {0};
		bool clear[8][8] = {0};

#include "generate_frame.h"

//Constructor
generate_frame::generate_frame(PixelArray* _pixelArray)
{	
	pixelArray = _pixelArray;		
} 

// Destructor
generate_frame::~generate_frame()
{
} 

// selects a letter from storage
void generate_frame::letter_frame(char letter)
{
	switch(letter)
	{
		case 'F':
			pixelArray->print_letter(F_bool);
			break;
		case 'c':
			pixelArray->print_letter(clear);
			break;
	}	
}


void generate_frame::animateLetters2(bool *letter1, bool *letter2)
{
	uint8_t colCount = pixelArray->getColCount();
	uint8_t rowCount = pixelArray->getRowCount();
	bool* buffer1 = (bool*)malloc(colCount * rowCount);
	bool* buffer2 = (bool*)malloc(colCount * rowCount);
	uint8_t letterCount = 2;
	for (uint8_t col = 0; col < colCount * letterCount; col++)
	{
		uint8_t oldCol;
		uint8_t newCol;
		if (col == 0)
		{
			newCol = colCount - 1;
			oldCol = col;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer2 + row * colCount + newCol) = *(letter1 + row * colCount + oldCol);
		}
		else if (col < colCount)
		{
			newCol = col - 1;
			oldCol = col;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer1 + row * colCount + newCol) = *(letter1 + row * colCount + oldCol);
		}
		else if (col == colCount)
		{
			newCol = col - 1;
			oldCol = col - colCount;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer1 + row * colCount + newCol) = *(letter2 + row * colCount + oldCol);
		}
		else
		{
			newCol = col - colCount - 1;
			oldCol = col - colCount;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer2 + row * colCount + newCol) = *(letter2 + row * colCount + oldCol);
		}
	}
	
	for (uint8_t byteIndex = 0; byteIndex < rowCount * colCount; byteIndex++)
	{
		*(letter1 + byteIndex) = *(buffer1 + byteIndex);
		*(letter2 + byteIndex) = *(buffer2 + byteIndex);
	}
	
	free(buffer1);
	free(buffer2);
}

void generate_frame::animateLetters3(bool *letter1, bool *letter2, bool *letter3)
{
	uint8_t colCount = pixelArray->getColCount();
	uint8_t rowCount = pixelArray->getRowCount();
	bool* buffer1 = (bool*)malloc(colCount * rowCount);
	bool* buffer2 = (bool*)malloc(colCount * rowCount);
	bool* buffer3 = (bool*)malloc(colCount * rowCount);
	uint8_t letterCount = 3;	
	for (uint8_t col = 0; col < colCount * letterCount; col++)
	{
		uint8_t oldCol;
		uint8_t newCol;
		if (col == 0)
		{
			newCol = colCount - 1;
			oldCol = col;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer3 + row * colCount + newCol) = *(letter1 + row * colCount + oldCol);
		}
		else if (col < colCount)
		{
			newCol = col - 1;
			oldCol = col;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer1 + row * colCount + newCol) = *(letter1 + row * colCount + oldCol);
		}
		else if (col == colCount)
		{
			newCol = col - 1;
			oldCol = col - colCount;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer1 + row * colCount + newCol) = *(letter2 + row * colCount + oldCol);
		}
		else if (col < 2 * colCount)
		{
			newCol = col - colCount - 1;
			oldCol = col - colCount;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer2 + row * colCount + newCol) = *(letter2 + row * colCount + oldCol);
		}
		else if (col == 2 * colCount)
		{
			newCol = col - colCount - 1;
			oldCol = col - 2 * colCount;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer2 + row * colCount + newCol) = *(letter3 + row * colCount + oldCol);
		}
		else if (col < 3 * colCount)
		{
			newCol = col - 2 * colCount - 1;
			oldCol = col - 2 * colCount;
			for (uint8_t row = 0; row < rowCount; row++)
			*(buffer3 + row * colCount + newCol) = *(letter3 + row * colCount + oldCol);
		}
	}

	for (uint8_t byteIndex = 0; byteIndex < rowCount * colCount; byteIndex++)
	{
		*(letter1 + byteIndex) = *(buffer1 + byteIndex);
		*(letter2 + byteIndex) = *(buffer2 + byteIndex);
		*(letter3 + byteIndex) = *(buffer3 + byteIndex);
	}
	
	free(buffer1);
	free(buffer2);
	free(buffer3);
}
