$files = (Get-ChildItem -Path "./**.csproj" -recurse).FullName | resolve-path -relative 
$files | ForEach-Object  -process {
    dotnet build $_ --configuration Release
    dotnet pack $_ --configuration Release
    $packages = $("$(split-path $_  -parent)\**.nupkg" | Get-ChildItem -recurse).FullName
    $packages | ForEach-Object -Process {
      dotnet nuget push $_ --skip-duplicate --api-key SET_API_KEY_HERE --source SET_SOURCE_HERE
    }
}
