using Core.Locations;
using Core.Logs;
using Engine;

App.Initialize();

Location location = new("test", "qualcosa:qualcos'altro");

Logger.Info(location.ToString());

App.Run();

App.End();
