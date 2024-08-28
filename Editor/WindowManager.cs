using Core.Logs;
using Core.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor
{
    public static class WindowManager
    {
        const string WINDOWSTATE_FILENAME = "windows_state.ini";
        public static Dictionary<string, Tool> RegisteredWindows { get; private set; }
        public static List<Tool> Windows { get => RegisteredWindows.Values.ToList(); }


        //TODO: find a better method for this, idealy with serialization or something close to it
        public static void SaveWindowsState()
        {
            FileInfo saveFileInfo = new FileInfo(WINDOWSTATE_FILENAME);

            string output = "";
            foreach (var tool in RegisteredWindows)
            {
                output += $"{tool.Key}#{tool.Value.Open}\n";
            }

            File.WriteAllText(saveFileInfo.FullName, output);
        }

        public static void LoadWindowsState()
        {
            FileInfo saveFileInfo = new FileInfo(WINDOWSTATE_FILENAME);
            if(!saveFileInfo.Exists)
            {
                return;
            }

            string input = File.ReadAllText(saveFileInfo.FullName);
            string[] infos = input.Split("\n");

            foreach (var toolInfos in infos)
            {
                string[] info = toolInfos.Split("#");

                if (RegisteredWindows.TryGetValue(info[0], out Tool tools))
                {
                    tools.Open = bool.Parse(info[1]);
                }
            }
        }

        public static void Register(Tool window)
        {
            if (RegisteredWindows.ContainsKey(window.Name))
            {
                return;
            }

            RegisteredWindows.Add(window.Name, window);
        }

        static WindowManager()
        {
            RegisteredWindows = new Dictionary<string, Tool>();
        }
    }
}
