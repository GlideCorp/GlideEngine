using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public static class Time
    {
        static float _deltaTime;
        public static float DeltaTime
        {
            get => _deltaTime * Scale;
            internal set => _deltaTime = value;
        }

        public static float Scale { get; set; } = 1f;

        public static int FPS { get => (int)(1 / DeltaTime); }
    }
}
