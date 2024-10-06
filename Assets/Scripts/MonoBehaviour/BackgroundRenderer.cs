using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BackgorundRenderer : MonoBehaviour
{

    public Material BackgroundMaterial; 
    public Material LineMaterial;
    public Material RedLineMaterial;
    private void Start()
    {
        CreateCuboid();
        DrawGrid(new Vector3(0, 0, 0), 10, 10, 0, 2, LineMaterial);
        DrawGrid(new Vector3(0, 0, 0), 20, 10, 1, 0, LineMaterial);
        DrawGrid(new Vector3(0, 0, 0), 20, 10, 1, 2, LineMaterial);

        DrawGrid(new Vector3(0, 20, 0),2, 10, 1, 0, RedLineMaterial);
        DrawGrid(new Vector3(0, 20, 0),2, 10, 1, 2, RedLineMaterial);
    }

    private void CreateCuboid()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(10, 0, 0),
            new Vector3(10, 22, 0),
            new Vector3(0, 22, 0),

            new Vector3(0, 0, 0),
            new Vector3(0, 0, 10),
            new Vector3(0, 22, 10),
            new Vector3(0, 22, 0),

            new Vector3(0, 0, 0),
            new Vector3(10, 0, 0),
            new Vector3(10, 0, 10),
            new Vector3(0, 0, 10)
        };

        int[] triangles = new int[]
        {
            // Side 1 (Front)
            0, 1, 2,
            0, 2, 3,

            // Side 2 (Left)
            4, 6, 5,
            4, 7, 6,

            // Side 3 (Bottom)
            8, 10, 9,
            8, 11, 10
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = BackgroundMaterial;
    }

    private void DrawGrid(Vector3 offset, int width, int height, int axis1, int axis2, Material material)
    {
        // Determine which axis corresponds to x, y, or z
        Vector3[] axisVectors = { Vector3.right, Vector3.up, Vector3.forward };
        Vector3 axisVector1 = axisVectors[axis1];
        Vector3 axisVector2 = axisVectors[axis2];

        for (int i = 0; i <= width; i++)
        {
            Vector3 start = offset + i * axisVector1;
            Vector3 end = start + height * axisVector2;
            DrawLine(start, end, material);
        }
        for (int j = 0; j <= height; j++)
        {
            Vector3 start = offset + j * axisVector2;
            Vector3 end = start + width * axisVector1;
            DrawLine(start, end, material);
        }
    }


    void DrawLine(Vector3 start, Vector3 end, Material material)
    {
        GameObject line = new GameObject("Line");
        line.transform.parent = this.transform;
        LineRenderer lr = line.AddComponent<LineRenderer>();

        lr.material = material;
        lr.startWidth = 0.15f;
        lr.endWidth = 0.15f;

        // Disable shadows for the line renderer
        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;

        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}