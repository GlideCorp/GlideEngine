using Core.Extensions;
using Core.Logs;
using Core.Maths.Vectors;
using Engine.Rendering;
using Silk.NET.Assimp;
using Silk.NET.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
            //AssimpMesh* mesh = scene->MMeshes[0];
            return ProcessMesh(mesh, scene);
        }

        private static unsafe Mesh ProcessMesh(AssimpMesh* mesh, Scene* scene)
        {
            Mesh m = new Mesh();
            List<Vertex> vertices = new List<Vertex>();
            List<uint> indices = new List<uint>();

            for (int i = 0; i < mesh->MNumVertices; i++)
            {
                Vertex v = new();
                Vector3 vPos = mesh->MVertices[i];
                v.Position = new Vector3Float(vPos.X, vPos.Y, vPos.Z);

                // normals
                if (mesh->MNormals != null)
                {
                    Vector3 vNorm = mesh->MNormals[i];
                    v.Normal = new Vector3Float(vNorm.X, vNorm.Y, vNorm.Z);
                }

                if (mesh->MTextureCoords[0] != null)
                {
                    Vector3 nativeUv = mesh->MTextureCoords[0][i];
                    v.UV = new Vector2Float(nativeUv.X, nativeUv.Y);
                }
                else
                    v.UV = Vector2Float.Zero;

                vertices.Add(v);
                //m.Data.InsertVertex(v);
                //m.Data.InsertVertex(2.0f);
            }

            for (int i = 0; i < mesh->MNumFaces; i++)
            {
                Face face = mesh->MFaces[i];

                for (uint j = 0; j < face.MNumIndices; j++)
                {
                    indices.Add(face.MIndices[j]);
                    //m.Data.InsertIndex(face.MIndices[j]);
                }
            }

            m.Vertices = vertices;
            m.Indices = indices;
            m.Build();
            return m;
        }
    }
}
