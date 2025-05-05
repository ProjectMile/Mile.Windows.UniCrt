# Mile.Windows.UniCrt Release Notes

**Mile.Windows.UniCrt 1.2.410.0**

- Fix the missing symbols issue at runtime for the x86-32 target 
  MileUniCrtEnableVcRuntimeWrapper mode. (Thanks to yzytom.)

**Mile.Windows.UniCrt 1.2.328.0**

- Provide the Static Base support which can make people build their binaries
  which links with the static Windows Universal C Runtime library.
- Update Mile.UniCrt.References.
  - Add Windows 10 Build 14393 UCRT binaries as the reference baseline. (Thanks
    to gailium119 for the arm64 binary.)
  - Add Windows 10 Build 10240, 10586 and 15063 arm64 UCRT binaries as the
    historic references. (Thanks to gailium119.)
  - Move Windows 10 Build 10586.1000 and 16299 UCRT binaries as the historic
    references.
  - Add Notes\UniCrtNewSymbols.xlsx as the reference note.
- Update Mile.Project.Helpers to 1.0.643.
- Split the compile time linker test from Mile.UniCrt.Wrapper project to
  Mile.UniCrt.SymbolTest project.
- Rewrite Mile.UniCrt.Wrapper project.
  - Move specific implement from Mile.UniCrt.OriginalVCRuntimeWrapper to
    Mile.UniCrt.Wrapper for simplifying the Mile.Windows.UniCrt toolchain.
  - Add UCRT OS mode symbols redirection support.
  - Provide the redistribution binaries package.

**Mile.Windows.UniCrt 1.1.278.0**

- Eliminate the debugging symbol not found warning when linking softmemtag.obj
  which contains in the vcruntime140(d).dll and vcruntime140_1(d).dll to
  ucrtbase(d).dll wrapper.
- Fix symbol filtering issue in the vcruntime140(d).dll and
  vcruntime140_1(d).dll to ucrtbase(d).dll wrapper generator.
- Fix the debug target generator logic for the vcruntime140(d).dll and
  vcruntime140_1(d).dll to ucrtbase(d).dll wrapper.
- Revise documents.

**Mile.Windows.UniCrt 1.1.274.0**

- Provide the vcruntime140(d).dll and vcruntime140_1(d).dll to ucrtbase(d).dll
  wrapper which can make people build more smaller binaries. But it will cause
  binaries only support Windows 10 Build 19041 or later.
- Remove MileUniCrtEnableSimplifiedBase dependency for
  MileUniCrtDisableRuntimeDebuggingFeature because may users want to disable
  runtime debugging feature but with the standard Windows Universal C Runtime
  imports.
- Update Mile.Project.Helpers to 1.0.632.
- Update dependency Microsoft.Build.Utilities.Core and Microsoft.Build.Framework
  to 17.12.6. (Contributed by Malus-risus.)

**Mile.Windows.UniCrt 1.0.199.0**

- Remove the dependency of ForceImportAfterCppProps feature.

**Mile.Windows.UniCrt 1.0.187.0**

- Fix NuGet package is not applied issue.

**Mile.Windows.UniCrt 1.0.186.0**

- Initial release.
- Provide x86, x64 and ARM64 targets support.
- Provide ARM64X and ARM64EC modes support for ARM64 targets.
- Provide the Simplified Base support which can make people build their binaries
  which directly depends on ucrtbase.dll instead of api-ms-win-crt-*.dll.
- Provide the disabling runtime debugging feature support which can make people
  build their debug binaries which directly depends on ucrtbase.dll instead of
  ucrtbased.dll.
- Provide NuGet package.
