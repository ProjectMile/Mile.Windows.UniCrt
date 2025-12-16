/*
 * PROJECT:    Mouri Internal Library Essentials
 * FILE:       Mile.UniCrt.Wrapper.cpp
 * PURPOSE:    Implementation for Mile.UniCrt.Wrapper
 *
 * LICENSE:    The MIT License
 *
 * MAINTAINER: MouriNaruto (Kenji.Mouri@outlook.com)
 */

#include <Windows.h>

#ifdef _DEBUG

#include <mbstring.h>

extern "C" unsigned char* __cdecl _mbsdup_dbg(
    _In_ unsigned char const* _String,
    _In_ int _BlockUse,
    _In_opt_ char const* _FileName,
    _In_ int _LineNumber)
{
    UNREFERENCED_PARAMETER(_BlockUse);
    UNREFERENCED_PARAMETER(_FileName);
    UNREFERENCED_PARAMETER(_LineNumber);

    return ::_mbsdup(_String);
}

#endif // _DEBUG

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
