using Core.Locations;
using Core.Logs;
using Silk.NET.OpenGL;

Logger.Startup();

Location location = new("primo_livello:secondo_livello");


Print(location);
Print("test:sperochefunzioni");


Logger.Shutdown();

void Print(Location location)
{
    Logger.Dump(location);
}