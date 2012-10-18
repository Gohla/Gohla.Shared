cd Gohla.Shared
nuget pack
move Gohla.Shared.*.nupkg C:\Development\LocalNuget\
cd..

cd Gohla.Shared.Composition
nuget pack
move Gohla.Shared.Composition.*.nupkg C:\Development\LocalNuget\
cd..

cd Gohla.Shared.Json
nuget pack
move Gohla.Shared.Json.*.nupkg C:\Development\LocalNuget\
cd..

cd Gohla.Shared.XML
nuget pack
move Gohla.Shared.XML.*.nupkg C:\Development\LocalNuget\
cd..