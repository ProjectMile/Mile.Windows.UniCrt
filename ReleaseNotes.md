# Mile.Windows.UniCrt Release Notes

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
