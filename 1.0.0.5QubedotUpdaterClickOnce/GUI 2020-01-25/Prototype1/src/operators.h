/*
 * operators.h
 *
 * Created: 17.05.2017 00:45:37
 *  Author: Michael
 */ 


#ifndef OPERATORS_H_
#define OPERATORS_H_

#include <stddef.h>
#include <stdlib.h>

void * operator new(size_t size);
void operator delete(void * ptr);



#endif /* OPERATORS_H_ */