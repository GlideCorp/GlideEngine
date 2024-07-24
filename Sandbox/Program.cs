using Core.Locations;
using Engine;

App.Initialize();

Node root = new();

root.Append("1a", out _);
root.Append("1b", out Node? second);

second.Append("2", out Node? third);

second.Detatch("2");

App.Run();

App.End();
