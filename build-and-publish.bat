@pushd src
@call msbuild
@xcopy bin\debug\* \\Velko-PC\share\*
@echo System.Reflection.Assembly.LoadFrom(@"\\Velko-PC\share\debug-utils.exe")
@popd