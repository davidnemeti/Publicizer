$localNugetPackagesDirectory = "d:\LocalNugetPackages\"

Remove-Item $env:USERPROFILE\.nuget\packages\publicizer -Recurse -ErrorAction Ignore
Remove-Item $localNugetPackagesDirectory\*.* -Recurse
Copy-Item ".\Publicizer\bin\Debug\*.nupkg" -Destination $localNugetPackagesDirectory
