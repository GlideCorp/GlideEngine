using Core.Locations;
using Core.Logs;
using Engine;
using System.Numerics;

App.Initialize();

Location location = new("test", "qualcosa:qualcos'altro");
Vector3 vector = Vector3.One;
Logger.Info(vector);

App.Run();

App.End();
