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
        public static List<Tool> Windows { get; private set; }

        public static void SaveWindowsState()
        {
        }

        public static void LoadWindowsState()
        {
        }

        public static void Register(Tool window)
        {
            if (Windows.Contains(window))
            {
                return;
            }

            Windows.Add(window);
        }

        static WindowManager()
        {
            Windows = new List<Tool>();
        }
    }
}
