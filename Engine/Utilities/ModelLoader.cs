﻿using Core.Extensions;
using Core.Logs;
using Engine.Rendering;
using Silk.NET.Assimp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AssimpMesh = Silk.NET.Assimp.Mesh;
using Mesh = Engine.Rendering.Mesh;

namespace Engine.Utilities
{
    public class ModelLoader
    {
        static Assimp _assimp;
        static Assimp @Assimp
        {
            get 
            {
                if(_assimp == null)
                {
                    _assimp = Assimp.GetApi();
                }

                return _assimp;
            }
        }

        public static unsafe Mesh Load(string modelFilePath)
        {
            Scene* scene = @Assimp.ImportFile(modelFilePath, (uint)PostProcessSteps.Triangulate);
        
            if(scene == null || scene->MFlags == Assimp.SceneFlagsIncomplete || scene->MRootNode == null)
            {
                Logger.Error(Assimp.GetErrorStringS());
                throw new Exception();
            }

            //Per il momento prende la prima mesh che trova.
            //TODO: Cambiare questa cosa per includere tutte le mesh della scena, usare o sub-meshes o entità-padre/figlio
            AssimpMesh* mesh = scene->MMeshes[scene->MRootNode->MMeshes[0]];
            return ProcessMesh(mesh, scene);
        }

        private static unsafe Mesh ProcessMesh(AssimpMesh* mesh, Scene* scene)
        {
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            for (int i = 0; i < mesh->MNumVertices; i++)
            {
                Vertex v = new();
                v.Position = mesh->MVertices[i].ToSilk();

                // normals
                if (mesh->MNormals != null)
                    v.Normal = mesh->MNormals[i].ToSilk();
                
                vertices.Add(v);
            }

            for (int i = 0; i < mesh->MNumFaces; i++)
            {
                Face face = mesh->MFaces[i];

                for (uint j = 0; j < face.MNumIndices; j++)
                {
                    indices.Add(face.MIndices[j]);
                }
            }

            return new Mesh(vertices, indices);
        }
    }
}
