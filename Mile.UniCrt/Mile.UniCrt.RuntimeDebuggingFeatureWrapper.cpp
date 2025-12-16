/*
 * PROJECT:    Mouri Internal Library Essentials
 * FILE:       Mile.UniCrt.RuntimeDebuggingFeatureWrapper.cpp
 * PURPOSE:    Implementation for Mile.UniCrt.RuntimeDebuggingFeatureWrapper
 *
 * LICENSE:    The MIT License
 *
 * MAINTAINER: MouriNaruto (Kenji.Mouri@outlook.com)
 */

// Workaround for warning C4273
#undef _DEBUG

#include <ctype.h>
#include <direct.h>
#include <malloc.h>
#include <mbstring.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#ifndef UNREFERENCED_PARAMETER
#define UNREFERENCED_PARAMETER(P) (P)
#endif // !UNREFERENCED_PARAMETER

#ifndef TRUE
#define TRUE 1
#endif // !TRUE

#ifndef FALSE
#define FALSE 0
#endif // !FALSE

typedef int (__CRTDECL* _CRT_ALLOC_HOOK)(
    _In_ int _AllocType,
    _In_ void* _UserData,
    _In_ size_t _Size,
    _In_ int _BlockType,
    _In_ long _RequestNumber,
    _In_ unsigned char const* _FileName,
    _In_ int _LineNumber);

typedef void (__CRTDECL* _CRT_DUMP_CLIENT)(
    _In_ void* _Block,
    _In_ size_t _Size);

typedef void(__cdecl* _CrtDoForAllClientObjectsCallback)(
    _In_ void* _Block,
    _In_ void* _Context);

typedef struct _CrtMemState _CrtMemState;

typedef void* _HFILE;

typedef int (__CRTDECL* _CRT_REPORT_HOOK)(int, char*, int*);
typedef int (__CRTDECL* _CRT_REPORT_HOOKW)(int, wchar_t*, int*);

extern "C" int* __cdecl __p__crtDbgFlag()
{
    static int crtDbgFlag = 0;
    return &crtDbgFlag;
}

extern "C" long* __cdecl __p__crtBreakAlloc()
{
    static long crtBreakAlloc = 0;
    return &crtBreakAlloc;
}

extern "C" _CRT_ALLOC_HOOK __cdecl _CrtGetAllocHook()
{
    return nullptr;
}

extern "C" _CRT_ALLOC_HOOK __cdecl _CrtSetAllocHook(
    _In_opt_ _CRT_ALLOC_HOOK _PfnNewHook)
{
    UNREFERENCED_PARAMETER(_PfnNewHook);

    return nullptr;
}

extern "C" _CRT_DUMP_CLIENT __cdecl _CrtGetDumpClient()
{
    return nullptr;
}

extern "C" _CRT_DUMP_CLIENT __cdecl _CrtSetDumpClient(
    _In_opt_ _CRT_DUMP_CLIENT _PFnNewDump)
{
    UNREFERENCED_PARAMETER(_PFnNewDump);

    return nullptr;
}

extern "C" int __cdecl _CrtCheckMemory()
{
    return TRUE;
}

extern "C" void __cdecl _CrtDoForAllClientObjects(
    _In_ _CrtDoForAllClientObjectsCallback _Callback,
    _In_ void* _Context)
{
    UNREFERENCED_PARAMETER(_Callback);
    UNREFERENCED_PARAMETER(_Context);

    return;
}

extern "C" int __cdecl _CrtDumpMemoryLeaks()
{
    return FALSE;
}

extern "C" int __cdecl _CrtIsMemoryBlock(
    _In_opt_ void const* _Block,
    _In_ unsigned int _Size,
    _Out_opt_ long* _RequestNumber,
    _Out_opt_ char** _FileName,
    _Out_opt_ int* _LineNumber)
{
    UNREFERENCED_PARAMETER(_Block);
    UNREFERENCED_PARAMETER(_Size);

    if (_RequestNumber)
        *_RequestNumber = 0;

    if (_FileName)
        *_FileName = const_cast<char*>("");

    if (_LineNumber)
        *_LineNumber = 0;

    return 1;
}

extern "C" int __cdecl _CrtIsValidHeapPointer(
    _In_opt_ void const* _Pointer)
{
    UNREFERENCED_PARAMETER(_Pointer);

    return TRUE;
}

extern "C" int __cdecl _CrtIsValidPointer(
    _In_opt_ void const* _Pointer,
    _In_ unsigned int _Size,
    _In_ int _ReadWrite)
{
    UNREFERENCED_PARAMETER(_Pointer);
    UNREFERENCED_PARAMETER(_Size);
    UNREFERENCED_PARAMETER(_ReadWrite);

    return TRUE;
}

extern "C" void __cdecl _CrtMemCheckpoint(
    _Out_ _CrtMemState* _State)
{
    UNREFERENCED_PARAMETER(_State);

    return;
}

extern "C" int __cdecl _CrtMemDifference(
    _Out_ _CrtMemState* _State,
    _In_ _CrtMemState const* _OldState,
    _In_ _CrtMemState const* _NewState)
{
    UNREFERENCED_PARAMETER(_State);
    UNREFERENCED_PARAMETER(_OldState);
    UNREFERENCED_PARAMETER(_NewState);

    return FALSE;
}

extern "C" void __cdecl _CrtMemDumpAllObjectsSince(
    _In_opt_ _CrtMemState const* _State)
{
    UNREFERENCED_PARAMETER(_State);

    return;
}

extern "C" void __cdecl _CrtMemDumpStatistics(
    _In_ _CrtMemState const* _State)
{
    UNREFERENCED_PARAMETER(_State);

    return;
}

extern "C" int __cdecl _CrtReportBlockType(
    _In_opt_ void const* _Block)
{
    UNREFERENCED_PARAMETER(_Block);

    return -1;
}

extern "C" long __cdecl _CrtSetBreakAlloc(
    _In_ long _NewValue)
{
    UNREFERENCED_PARAMETER(_NewValue);

    return 0;
}

extern "C" int __cdecl _CrtSetDbgFlag(
    _In_ int _NewFlag)
{
    UNREFERENCED_PARAMETER(_NewFlag);

    return 0;
}

extern "C" int __cdecl _CrtDbgReport(
    _In_ int _ReportType,
    _In_opt_ char const* _FileName,
    _In_ int _LineNumber,
    _In_opt_ char const* _ModuleName,
    _In_opt_ char const* _Format,
    ...)
{
    UNREFERENCED_PARAMETER(_ReportType);
    UNREFERENCED_PARAMETER(_FileName);
    UNREFERENCED_PARAMETER(_LineNumber);
    UNREFERENCED_PARAMETER(_ModuleName);
    UNREFERENCED_PARAMETER(_Format);

    return FALSE;
}

extern "C" int __cdecl _CrtDbgReportW(
    _In_ int _ReportType,
    _In_opt_ wchar_t const* _FileName,
    _In_ int _LineNumber,
    _In_opt_ wchar_t const* _ModuleName,
    _In_opt_ wchar_t const* _Format,
    ...)
{
    UNREFERENCED_PARAMETER(_ReportType);
    UNREFERENCED_PARAMETER(_FileName);
    UNREFERENCED_PARAMETER(_LineNumber);
    UNREFERENCED_PARAMETER(_ModuleName);
    UNREFERENCED_PARAMETER(_Format);

    return FALSE;
}

extern "C" int __cdecl _VCrtDbgReportA(
    _In_ int _ReportType,
    _In_opt_ void* _ReturnAddress,
    _In_opt_ char const* _FileName,
    _In_ int _LineNumber,
    _In_opt_ char const* _ModuleName,
    _In_opt_ char const* _Format,
    _In_ va_list _ArgList)
{
    UNREFERENCED_PARAMETER(_ReportType);
    UNREFERENCED_PARAMETER(_ReturnAddress);
    UNREFERENCED_PARAMETER(_FileName);
    UNREFERENCED_PARAMETER(_LineNumber);
    UNREFERENCED_PARAMETER(_ModuleName);
    UNREFERENCED_PARAMETER(_Format);
    UNREFERENCED_PARAMETER(_ArgList);

    return FALSE;
}

extern "C" int __cdecl _VCrtDbgReportW(
    _In_ int _ReportType,
    _In_opt_ void* _ReturnAddress,
    _In_opt_ wchar_t const* _FileName,
    _In_ int _LineNumber,
    _In_opt_ wchar_t const* _ModuleName,
    _In_opt_ wchar_t const* _Format,
    _In_ va_list _ArgList)
{
    UNREFERENCED_PARAMETER(_ReportType);
    UNREFERENCED_PARAMETER(_ReturnAddress);
    UNREFERENCED_PARAMETER(_FileName);
    UNREFERENCED_PARAMETER(_LineNumber);
    UNREFERENCED_PARAMETER(_ModuleName);
    UNREFERENCED_PARAMETER(_Format);
    UNREFERENCED_PARAMETER(_ArgList);

    return FALSE;
}

extern "C" size_t __cdecl _CrtSetDebugFillThreshold(
    _In_ size_t _NewDebugFillThreshold)
{
    UNREFERENCED_PARAMETER(_NewDebugFillThreshold);

    return 0;
}

extern "C" size_t __cdecl _CrtGetDebugFillThreshold()
{
    return 0;
}

extern "C" _HFILE __cdecl _CrtSetReportFile(
    _In_ int _ReportType,
    _In_opt_ _HFILE _ReportFile)
{
    UNREFERENCED_PARAMETER(_ReportType);
    UNREFERENCED_PARAMETER(_ReportFile);

    return nullptr;
}

extern "C" int __cdecl _CrtSetReportMode(
    _In_ int _ReportType,
    _In_ int _ReportMode)
{
    UNREFERENCED_PARAMETER(_ReportType);
    UNREFERENCED_PARAMETER(_ReportMode);

    return 0;
}

extern "C" _CRT_REPORT_HOOK __cdecl _CrtGetReportHook()
{
    return nullptr;
}

extern "C" _CRT_REPORT_HOOK __cdecl _CrtSetReportHook(
    _In_opt_ _CRT_REPORT_HOOK _PFnNewHook)
{
    UNREFERENCED_PARAMETER(_PFnNewHook);

    return nullptr;
}

extern "C" int __cdecl _CrtSetReportHook2(
    _In_ int _Mode,
    _In_opt_ _CRT_REPORT_HOOK _PFnNewHook)
{
    UNREFERENCED_PARAMETER(_Mode);
    UNREFERENCED_PARAMETER(_PFnNewHook);

    return 0;
}

extern "C" int __cdecl _CrtSetReportHookW2(
    _In_ int _Mode,
    _In_opt_ _CRT_REPORT_HOOKW _PFnNewHook)
{
    UNREFERENCED_PARAMETER(_Mode);
    UNREFERENCED_PARAMETER(_PFnNewHook);

    return 0;
}

extern "C" void __cdecl _CrtSetDbgBlockType(
    _In_ void* const _Block,
    _In_ int const _BlockUse)
{
    UNREFERENCED_PARAMETER(_Block);
    UNREFERENCED_PARAMETER(_BlockUse);

    return;
}

extern "C" int __cdecl _chvalidator(
    _In_ int _Ch,
    _In_ int _Mask)
{
    return ::_isctype(_Ch, _Mask);
}

extern "C" int __cdecl _chvalidator_l(
    _In_opt_ _locale_t const _Locale,
    _In_ int const _C,
    _In_ int const _Mask)
{
    return ::_isctype_l(_C, _Mask, _Locale);
}

extern "C" void __cdecl _invalid_parameter(
    _In_opt_z_ wchar_t const* _Expression,
    _In_opt_z_ wchar_t const* _FunctionName,
    _In_opt_z_ wchar_t const* _FileName,
    _In_ unsigned int _LineNumber,
    _In_ uintptr_t _Reserved)
{
    UNREFERENCED_PARAMETER(_Expression);
    UNREFERENCED_PARAMETER(_FunctionName);
    UNREFERENCED_PARAMETER(_FileName);
    UNREFERENCED_PARAMETER(_LineNumber);
    UNREFERENCED_PARAMETER(_Reserved);

    return ::_invalid_parameter_noinfo();
}
