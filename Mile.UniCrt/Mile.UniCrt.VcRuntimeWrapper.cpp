/*
 * PROJECT:   Mouri Internal Library Essentials
 * FILE:      Mile.UniCrt.VcRuntimeWrapper.cpp
 * PURPOSE:   Implementation for Mile.UniCrt.VcRuntimeWrapper
 *
 * LICENSE:   The MIT License
 *
 * MAINTAINER: MouriNaruto (Kenji.Mouri@outlook.com)
 */

#ifdef _M_AMD64

#include <Windows.h>

// Dummy stub for bypassing ARM64EC compilation.
extern "C" void __MileUniCrtMakeCompilerGenerateThunks()
{
    ::__C_specific_handler(nullptr, nullptr, nullptr, nullptr);
}

#endif // _M_AMD64

#ifdef _M_IX86
#pragma comment(linker, "/alternatename:__CxxLongjmpUnwind@4=__CxxLongjmpUnwind")
#pragma comment(linker, "/alternatename:__CxxThrowException@8=__CxxThrowException")
#pragma comment(linker, "/alternatename:_seh_longjmp_unwind@4=_seh_longjmp_unwind")
#pragma comment(linker, "/alternatename:_seh_longjmp_unwind4@4=_seh_longjmp_unwind4")
#pragma comment(linker, "/alternatename:__seh_longjmp_unwind@4=__seh_longjmp_unwind")
#pragma comment(linker, "/alternatename:__seh_longjmp_unwind4@4=__seh_longjmp_unwind4")
#endif // _M_IX86
