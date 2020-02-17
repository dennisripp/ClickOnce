/* 
* generate_frame.h
*
* Created: 10.07.2018 14:02:51
* Author: Maxi
*/


#ifndef __generate_frame_H__
#define __generate_frame_H__

#include "PixelArray.h"

class generate_frame
{
//variables
	public:
	
	protected:
	
	private:
		PixelArray* pixelArray;
		
		
//functions
	public:
		generate_frame(PixelArray* _pixelArray);
		~generate_frame();
		void animateLetters2(bool *letter1, bool *letter2);
		void animateLetters3(bool *letter1, bool *letter2, bool *letter3);
		void letter_frame(char letter);
	protected:
	private:
		generate_frame( const generate_frame &c );
		generate_frame& operator=( const generate_frame &c );

};

#endif
