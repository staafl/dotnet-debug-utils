@pushd src
@call msbuild
@xcopy bin\debug\* \\Velko-PC\share\* /Y
@echo System.Reflection.Assembly.LoadFrom(@"\\Velko-PC\share\debug-utils.exe")| clip
@echo System.Reflection.Assembly.LoadFrom(@"\\Velko-PC\share\debug-utils.exe")
@popd