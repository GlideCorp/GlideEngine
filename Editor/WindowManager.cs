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
        public static List<EditorWindow> Windows { get; private set; }

        public static void Register(EditorWindow window)
        {
            if (Windows.Contains(window))
            {
                return;
            }

            Windows.Add(window);
        }

        static WindowManager()
        {
            Windows = new List<EditorWindow>();
        }
    }
}
