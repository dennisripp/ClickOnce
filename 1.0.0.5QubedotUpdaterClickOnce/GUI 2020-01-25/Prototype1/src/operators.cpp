/*
 * operators.cpp
 *
 * Created: 17.05.2017 00:45:30
 *  Author: Michael
 */ 

#include "operators.h"

void * operator new(size_t size)
{
	return malloc(size);
}

void operator delete(void * ptr)
{
	free(ptr);
}