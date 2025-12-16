/*
 * PROJECT:    Mouri Internal Library Essentials
 * FILE:       Mile.UniCrt.VcRuntimeWrapper.cpp
 * PURPOSE:    Implementation for Mile.UniCrt.VcRuntimeWrapper
 *
 * LICENSE:    The MIT License
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
