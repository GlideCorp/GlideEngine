using Core.Locations;
using Core.Logs;
using Engine;
using Sandbox;


App.Initialize();

Tree tree = new();
MyTrackable trackable = new(5);

tree.Insert(trackable);

foreach (var item in tree.RetriveValues())
{
    Logger.Dump(item);
}

App.Run();

App.End();
