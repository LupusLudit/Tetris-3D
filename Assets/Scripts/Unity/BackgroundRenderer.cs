using Assets.Scripts.Logic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BackgroundRenderer : MonoBehaviour
{
    public Material BackgroundMaterial;
    public Material LineMaterial;
    public Material RedLineMaterial;
    public Camera MainCamera;

    public int XMax;
    public int YMax;
    public int ZMax;

    private Vector3 cameraInitialPosition;
    private Vector3 previousPosition;
    private Vector3[] hitPoints;
    private CircularLinkedList circularList;
    private Vector3 center = new(5, 5, 5);
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3[] axisVectors = { Vector3.right, Vector3.up, Vector3.forward }; // right = x, up = y, forward = z

    private Mesh gridMesh;
    private MeshFilter meshFilter;

    void Start()
    {
        cameraInitialPosition = MainCamera.transform.position;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        hitPoints = BoardDimensions.calcHitPoints(new Vector3(XMax, YMax, ZMax), MainCamera);
        circularList = new CircularLinkedList(hitPoints);

        meshFilter = GetComponent<MeshFilter>();
        gridMesh = new Mesh();

        CreateCuboid();
        DrawGrid(new Vector3(0, 0, 0), XMax, ZMax, 0, 2, LineMaterial); // x & z
        DrawGrid(new Vector3(0, 0, 0), YMax - 2, XMax, 1, 0, LineMaterial); // y & x
        DrawGrid(new Vector3(0, 0, 0), YMax - 2, ZMax, 1, 2, LineMaterial); // y & z

        DrawGrid(new Vector3(0, YMax - 2, 0), 2, XMax, 1, 0, RedLineMaterial); // y & x
        DrawGrid(new Vector3(0, YMax - 2, 0), 2, ZMax, 1, 2, RedLineMaterial); // y & z
    }

    void Update()
    {
        CheckHitAndDirection();
        previousPosition = MainCamera.transform.position;
    }

    private void CheckHitAndDirection()
    {
        if (CameraHitPoint())
        {
            Vector3 hitPoint = GetHitPoint(previousPosition);
            Vector3 closest = GetClosestFreePoint(previousPosition, hitPoint);

            if (circularList.FindNodeByValue(hitPoint).Next.Value == closest)
            {
                RotateCuboid(90);
            }
            else
            {
                RotateCuboid(-90);
            }
        }
    }

    private void RotateCuboid(float angle)
    {
        Vector3 rotationCenter = new Vector3(XMax / 2f, YMax / 2f, ZMax / 2f);
        transform.RotateAround(rotationCenter, Vector3.up, angle);
    }

    private bool CameraHitPoint()
    {
        foreach (Vector3 point in hitPoints)
        {
            if (Vector3.Distance(MainCamera.transform.position, point) < 0.1f) return true;
        }
        return false;
    }

    private Vector3 GetHitPoint(Vector3 currentPosition)
    {
        float min = float.MaxValue;
        Vector3 closest = new Vector3();
        foreach (Vector3 point in hitPoints)
        {
            float dist = Vector3.Distance(currentPosition, point);
            if (dist < min)
            {
                min = dist;
                closest = point;
            }
        }
        return closest;
    }

    private Vector3 GetClosestFreePoint(Vector3 currentPosition, Vector3 hitPoint)
    {
        float min = float.MaxValue;
        Vector3 closest = new Vector3();
        foreach (Vector3 point in hitPoints)
        {
            if (point != hitPoint)
            {
                float distance = Vector3.Distance(currentPosition, point);
                if (distance < min)
                {
                    min = distance;
                    closest = point;
                }
            }
        }
        return closest;
    }

    private void CreateCuboid()
    {
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(0, 0, 0),
            new Vector3(XMax, 0, 0),
            new Vector3(XMax, YMax, 0),
            new Vector3(0, YMax, 0),

            new Vector3(0, 0, 0),
            new Vector3(0, 0, ZMax),
            new Vector3(0, YMax, ZMax),
            new Vector3(0, YMax, 0),

            new Vector3(0, 0, 0),
            new Vector3(XMax, 0, 0),
            new Vector3(XMax, 0, ZMax),
            new Vector3(0, 0, ZMax)
        };

        int[] triangles = new int[]
        {
            0, 1, 2,
            0, 2, 3,
            4, 6, 5,
            4, 7, 6,
            8, 10, 9,
            8, 11, 10
        };

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = BackgroundMaterial;
    }

    private void DrawGrid(Vector3 offset, int height, int width, int axisA, int axisB, Material material)
    {
        Vector3 axisVector1 = axisVectors[axisA];
        Vector3 axisVector2 = axisVectors[axisB];

        var vertices = new System.Collections.Generic.List<Vector3>();
        var indices = new System.Collections.Generic.List<int>();

        int index = 0;
        for (int i = 0; i <= height; i++)
        {
            Vector3 start = offset + i * axisVector1;
            Vector3 end = start + width * axisVector2;
            vertices.Add(start);
            vertices.Add(end);
            indices.Add(index++);
            indices.Add(index++);
        }
        for (int j = 0; j <= width; j++)
        {
            Vector3 start = offset + j * axisVector2;
            Vector3 end = start + height * axisVector1;
            vertices.Add(start);
            vertices.Add(end);
            indices.Add(index++);
            indices.Add(index++);
        }

        Mesh grid = new Mesh();
        grid.SetVertices(vertices);
        grid.SetIndices(indices.ToArray(), MeshTopology.Lines, 0);

        GameObject gridObject = new GameObject("GridMesh");
        gridObject.transform.parent = transform;
        gridObject.transform.localPosition = Vector3.zero;
        gridObject.transform.localRotation = Quaternion.identity;

        var mf = gridObject.AddComponent<MeshFilter>();
        var mr = gridObject.AddComponent<MeshRenderer>();

        mf.mesh = grid;
        mr.material = material;
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows = false;
    }

    public void ResetToDefault()
    {
        MainCamera.transform.position = cameraInitialPosition;
        MainCamera.transform.rotation = Quaternion.Euler(30, -135, 0);
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
