﻿using Engine.Rendering;

namespace Engine.Utilities
{
    public static class MeshPrimitives
    {
        static Mesh? _quad;
        public static Mesh Quad
        {
            get
            {
                if (_quad is null)
                {
                    _quad = new Mesh();
                    _quad.Vertices=
                    [                    
                        new(){ Position = new(1, 1, 0.0f), Normal = new(0, 0, 1)},
                        new(){ Position = new (1, -1, 0.0f), Normal = new(0, 0, 1)},
                        new(){ Position = new (-1, -1, 0.0f), Normal = new(0, 0, 1)},
                        new(){ Position =  new(-1, 1, 0.0f), Normal = new(0, 0, 1)}
                    ];
                    _quad.Indices =
                    [
                        0, 1, 3,  // first Triangle
                        1, 2, 3   // second Triangle
                    ];
                    _quad.Build();
                }

                return _quad;
            }
        }
    }
}
