using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unity.Background
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="BackgroundFace"]/*'/>
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

        /// <summary>
        /// Initializes the background face with dimensions, position, and materials.
        /// Creates the mesh and draws the grid lines.
        /// </summary>
        /// <param name="startingPoint">World space starting corner of the face.</param>
        /// <param name="x">Length of the face along x-axis.</param>
        /// <param name="y">Length of the face along y-axis.</param>
        /// <param name="z">Length of the face along z-axis.</param>
        /// <param name="faceMaterial">Material used for the face surface.</param>
        /// <param name="gridMaterial">Material used for grid lines.</param>
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

        /// <summary>
        /// Creates the rectangular face mesh based on the axes and dimensions.
        /// The mesh consists of two triangles forming a rectangle.
        /// </summary>
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

        /// <summary>
        /// Draws grid lines on the face mesh using line segments.
        /// Creates a child GameObject with MeshFilter and MeshRenderer to display the grid.
        /// </summary>
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


        /// <summary>
        /// Returns the length along the specified axis (0=x, 1=y, 2=z).
        /// </summary>
        /// <param name="axis">Axis index.</param>
        /// <returns>Length of the face along the given axis.</returns>
        private int GetAxisLength(int axis)
        {
            switch(axis)
            {
                case 0:
                    return x;
                case 1:
                    return y;
                case 2:
                    return z;
                default:
                    return 1;
            }
        }

        /// <summary>
        /// Determines which two axes the face lies on based on the provided dimensions.
        /// Assumes one axis is always zero, the other two are non-zero.
        /// This is used as a development tool to simplify face orientation.
        /// </summary>
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
