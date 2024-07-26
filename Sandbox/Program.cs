using Core.Logs;
using Engine.Entities;
using Sandbox;

MyApplication app = new();
app.Startup(); 

MyEntity pipo = new();
pipo.Add(new MyBehaviour());
pipo.Add(new MyComp());

Component[] components = pipo.GetAllComponents();
Behaviour[] behaviours = pipo.GetAllBehaviours();

string debugInfo = "\ncomponents:\n";
foreach (Component component in components)
{
    debugInfo += $"\t{component}\n";
}

debugInfo += "\nbehaviours:\n";
foreach (Behaviour behaviour in behaviours)
{
    debugInfo += $"\t{behaviour}\n";
}
Logger.Info(debugInfo);

app.Run();
app.Shutdown();