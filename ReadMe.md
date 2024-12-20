# Mile.Windows.UniCrt

[![NuGet Package](https://img.shields.io/nuget/vpre/Mile.Windows.UniCrt)](https://www.nuget.org/packages/Mile.Windows.UniCrt)

The Windows Universal C Runtime derivative intended for the lightweight
applications.

**Work In Progress**

## Features

- Provide x86, x64 and ARM64 targets support.
- Provide ARM64X and ARM64EC modes support for ARM64 targets.
- Provide the Simplified Base support which can make people build their binaries
  which directly depends on ucrtbase.dll instead of api-ms-win-crt-*.dll.
- Provide the disabling runtime debugging feature support which can make people
  build their debug binaries which directly depends on ucrtbase.dll instead of
  ucrtbased.dll.
- Provide the vcruntime140(d).dll and vcruntime140_1(d).dll to ucrtbase(d).dll
  wrapper which can make people build more smaller binaries. But it will cause
  binaries only support Windows 10 Build 19041 or later.
- Provide NuGet package.

## Available MSBuild project options

If you don't want to use the simplified base, please set the following option.

```
<MileUniCrtEnableSimplifiedBase>false</MileUniCrtEnableSimplifiedBase>
```

If you want to make your debug configuration binary which directly depends on
ucrtbase.dll instead of ucrtbased.dll, please set the following option.

```
<MileUniCrtDisableRuntimeDebuggingFeature>true</MileUniCrtDisableRuntimeDebuggingFeature>
```

If you don't want to use the vcruntime140(d).dll and vcruntime140_1(d).dll to
ucrtbase(d).dll wrapper if you want to make your binary support Windows 10 Build
18363 or earlier, please set the following option.

```
<MileUniCrtEnableVcRuntimeWrapper>false</MileUniCrtEnableVcRuntimeWrapper>
```

If you want to disable all features from Mile.Windows.UniCrt, please set the
following option.

```
<MileUniCrtDisableAllFeatures>true</MileUniCrtDisableAllFeatures>
```

## Why named UniCrt?

Read https://github.com/microsoft/WindowsAppSDK/commit/34cf5fb8d2312dcbb1591595c37d385a24339d18,
you will know UniCrt is Microsoft's internal alias name for Universal C Runtime.

## Documents

- [License](License.md)
- [Release Notes](ReleaseNotes.md)
- [Versioning](Versioning.md)
