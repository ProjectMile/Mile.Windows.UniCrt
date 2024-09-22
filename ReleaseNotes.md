# Mile.Windows.UniCrt Release Notes

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
