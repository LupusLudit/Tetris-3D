using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unity.Background
{

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BackgroundFace : MonoBehaviour
    {
        private Vector3 startingPoint = Vector3.zero;
        private int x;
        private int y;
        private int z;

        private Material gridMaterial;
        private Material faceMaterial;
        private MeshFilter meshFilter;

        //These axis determine which where to draw the 2D face (each face has always one coordinate set to 0)
        private int axisA;
        private int axisB;
        private readonly Vector3[] axisVectors = { Vector3.right, Vector3.up, Vector3.forward };
        public void Initialize(Vector3 startingPoint, int x, int y, int z, Material faceMaterial, Material gridMaterial)
        {
            this.startingPoint = startingPoint;
            this.x = x;
            this.y = y;
            this.z = z;
            this.faceMaterial = faceMaterial;
            this.gridMaterial = gridMaterial;

            meshFilter = GetComponent<MeshFilter>();
            FindAxis();
            CreateFace();
            DrawGrid();
        }
        void CreateFace()
        {
            Vector3 axis1 = axisVectors[axisA];
            Vector3 axis2 = axisVectors[axisB];
            int width = GetAxisLength(axisA);
            int height = GetAxisLength(axisB);

            Vector3[] vertices = new Vector3[4];
            int[] triangles = {
                0, 1, 2, // first triangle (bottom-left -> bottom-right -> top-right)
                0, 2, 3  // second triangle (bottom-left -> top-right -> top-left)
            };

            vertices[0] = startingPoint; // bottom-left
            vertices[1] = startingPoint + axis1 * width; // bottom-right
            vertices[2] = startingPoint + axis1 * width + axis2 * height; // top-right
            vertices[3] = startingPoint + axis2 * height; // top-left

            Mesh faceMesh = new Mesh();
            faceMesh.vertices = vertices;
            faceMesh.triangles = triangles;
            faceMesh.RecalculateNormals();

            meshFilter.mesh = faceMesh;
            GetComponent<MeshRenderer>().material = faceMaterial;
        }

        void DrawGrid()
        {
            Vector3 axis1 = axisVectors[axisA];
            Vector3 axis2 = axisVectors[axisB];
            int width = GetAxisLength(axisA);
            int height = GetAxisLength(axisB);

            var vertices = new List<Vector3>();
            var indices = new List<int>();
            int index = 0;

            // lines parallel to axis1
            for (int i = 0; i <= height; i++)
            {
                Vector3 start = startingPoint + i * axis2;
                Vector3 end = start + axis1 * width;
                vertices.Add(start);
                vertices.Add(end);
                indices.Add(index++);
                indices.Add(index++);
            }

            // lines parallel to axis2
            for (int j = 0; j <= width; j++)
            {
                Vector3 start = startingPoint + j * axis1;
                Vector3 end = start + axis2 * height;
                vertices.Add(start);
                vertices.Add(end);
                indices.Add(index++);
                indices.Add(index++);
            }

            Mesh gridMesh = new Mesh();
            gridMesh.SetVertices(vertices);
            gridMesh.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);

            GameObject gridObj = new GameObject("GridLines");
            gridObj.transform.SetParent(transform, false);

            var filter = gridObj.AddComponent<MeshFilter>();
            var renderer = gridObj.AddComponent<MeshRenderer>();

            filter.mesh = gridMesh;
            renderer.material = gridMaterial;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        private int GetAxisLength(int axis)
        {
            return axis switch
            {
                0 => x,
                1 => y,
                2 => z,
                _ => 1
            };
        }

        /*
         * Note that we always set one of the axis to 0, so we can find the other two
         * This is a dev tool
         */
        private void FindAxis()
        {
            if (x != 0 && y != 0)
            {
                axisA = 0;
                axisB = 1;
            }
            else if (x != 0 && z != 0)
            {
                axisA = 2;
                axisB = 0;
            }
            else
            {
                axisA = 1;
                axisB = 2;
            }
        }
    }
}
