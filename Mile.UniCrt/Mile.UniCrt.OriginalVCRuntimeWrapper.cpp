/*
 * PROJECT:   Mouri Internal Library Essentials
 * FILE:      Mile.UniCrt.OriginalVCRuntimeWrapper.cpp
 * PURPOSE:   Implementation for Mile.UniCrt.OriginalVCRuntimeWrapper
 *
 * LICENSE:   The MIT License
 *
 * MAINTAINER: MouriNaruto (Kenji.Mouri@outlook.com)
 */

#include <Windows.h>

#include <cstdlib>
#include <cstring>
#include <cwchar>

// Empty stub implementation because this is only the wrapper to the another
// Universal C Runtime implementation.
extern "C" char** __dcrt_initial_narrow_environment = nullptr;

// Empty stub implementation because this is only the wrapper to the another
// Universal C Runtime implementation.
extern "C" wchar_t* __cdecl __dcrt_get_wide_environment_from_os()
{
    return nullptr;
}

#if defined _M_IX86

#include <eh.h>
#include <winternl.h>

typedef void(__fastcall* PCOOKIE_CHECK)(UINT_PTR);

extern "C" EXCEPTION_DISPOSITION __cdecl _except_handler4(
    _In_ PEXCEPTION_RECORD ExceptionRecord,
    _In_ PEXCEPTION_REGISTRATION_RECORD EstablisherFrame,
    _Inout_ PCONTEXT ContextRecord,
    _Inout_ PVOID DispatcherContext);

extern "C" EXCEPTION_DISPOSITION __cdecl _except_handler4_common(
    _In_ PUINT_PTR CookiePointer,
    _In_ PCOOKIE_CHECK CookieCheckFunction,
    _In_ PEXCEPTION_RECORD ExceptionRecord,
    _In_ PEXCEPTION_REGISTRATION_RECORD EstablisherFrame,
    _Inout_ PCONTEXT ContextRecord,
    _Inout_ PVOID DispatcherContext)
{
    UNREFERENCED_PARAMETER(CookiePointer);
    UNREFERENCED_PARAMETER(CookieCheckFunction);
    return ::_except_handler4(
        ExceptionRecord,
        EstablisherFrame,
        ContextRecord,
        DispatcherContext);
}

// Empty stub for bypassing compilation.
extern "C" BOOL __cdecl __intrinsic_abnormal_termination()
{
    return FALSE;
}

#endif // defined _M_IX86

#if defined _M_AMD64

#include <eh.h>

// Dummy stub for bypassing ARM64EC compilation.
extern "C" void __MileUniCrtMakeCompilerGenerateThunks()
{
    ::__C_specific_handler(nullptr, nullptr, nullptr, nullptr);
}

#endif // defined _M_AMD64 || defined _M_ARM || defined _M_ARM64
