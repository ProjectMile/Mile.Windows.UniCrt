/*
 * PROJECT:   Mouri Internal Library Essentials
 * FILE:      Mile.UniCrt.OriginalVCRuntimeWrapper.cpp
 * PURPOSE:   Implementation for Mile.UniCrt.OriginalVCRuntimeWrapper
 *
 * LICENSE:   The MIT License
 *
 * MAINTAINER: MouriNaruto (Kenji.Mouri@outlook.com)
 */

#if defined _M_AMD64

#include <Windows.h>

// Dummy stub for bypassing ARM64EC compilation.
extern "C" void __MileUniCrtMakeCompilerGenerateThunks()
{
    ::__C_specific_handler(nullptr, nullptr, nullptr, nullptr);
}

#endif // defined _M_AMD64 || defined _M_ARM || defined _M_ARM64
