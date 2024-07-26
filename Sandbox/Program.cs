using Core.Locations;
using Core.Logs;

Logger.Startup();

string path = "primo_livello:secondo_livello:primo_livello:secondo_livello";
Location location = new(path);


ReadOnlyMemory<char> memory = path.AsMemory();

Print(location);
Print(path);


Logger.Shutdown();

void Print(Location location)
{
    Logger.Dump(location);
}