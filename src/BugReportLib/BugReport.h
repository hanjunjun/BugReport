#include <iostream>
#include <windows.h>   
#include "werapi.h"

#pragma once
extern "C" _declspec(dllexport) void _stdcall SetGlobalExceptionFilter(int timeTick, char* productName);

extern "C" _declspec(dllexport) void _stdcall Test();