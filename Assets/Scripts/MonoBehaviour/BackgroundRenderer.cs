using Assets.Scripts.Logic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BackgroundRenderer : MonoBehaviour
{

    public Material BackgroundMaterial; 
    public Material LineMaterial;
    public Material RedLineMaterial;
    public Camera MainCamera;

    private Vector3 previousPosition;
    private readonly static Vector3[] hitPoints = new Vector3[]
        {
            new Vector3(40.35534f, 27, 5),
            new Vector3(5, 27, 40.35534f),
            new Vector3(-30.35534f, 27, 5),
            new Vector3(5, 27, -30.35534f)
        }; //precalculated values
    private CircularLinkedList circularList = new CircularLinkedList(hitPoints);
    private Vector3 center = new(5, 5, 5);
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        CreateCuboid();
        DrawGrid(new Vector3(0, 0, 0), 10, 10, 0, 2, LineMaterial);
        DrawGrid(new Vector3(0, 0, 0), 20, 10, 1, 0, LineMaterial);
        DrawGrid(new Vector3(0, 0, 0), 20, 10, 1, 2, LineMaterial);

        DrawGrid(new Vector3(0, 20, 0),2, 10, 1, 0, RedLineMaterial);
        DrawGrid(new Vector3(0, 20, 0),2, 10, 1, 2, RedLineMaterial);
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

            // Right point = next node, rotate to the right
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
        Vector3 rotationCenter = new Vector3(5, 11, 5);
        transform.RotateAround(rotationCenter, Vector3.up, angle);
    }


    private bool CameraHitPoint()
    {
        // Check if the camera is at the target hit point (allowing for a small tolerance for precision)
        foreach (Vector3 point in hitPoints)
        {
            if (Vector3.Distance(MainCamera.transform.position, point) < 0.1f) return true;
        }

        return false;
    }

    private Vector3 GetHitPoint(Vector3 currentPosition)
    {
        float min = int.MaxValue;
        Vector3 closest = new Vector3();
        foreach (Vector3 point in hitPoints)
        {
            if (Vector3.Distance(currentPosition, point) < min)
            {
                min = Vector3.Distance(currentPosition, point);
                closest = point;
            }
        }
        return closest;
    }

    private Vector3 GetClosestFreePoint(Vector3 currentPosition, Vector3 hitPoint)
    {
        float min = int.MaxValue;
        Vector3 closest = new Vector3();
        foreach (Vector3 point in hitPoints)
        {
            if (point != hitPoint && Vector3.Distance(currentPosition, point) < min)
            {
                min = Vector3.Distance(currentPosition, point);
                closest = point;
            }
        }
        return closest;

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

    private void DrawLine(Vector3 start, Vector3 end, Material material)
    {
        GameObject line = new GameObject("Line");
        line.transform.parent = transform; // Setting the line as a child of the cuboid's GameObject
        LineRenderer lr = line.AddComponent<LineRenderer>();

        lr.material = material;
        lr.startWidth = 0.15f;
        lr.endWidth = 0.15f;

        lr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lr.receiveShadows = false;

        // Set positions relative to the cuboid (local space)
        lr.useWorldSpace = false;
        lr.SetPosition(0, transform.InverseTransformPoint(start));
        lr.SetPosition(1, transform.InverseTransformPoint(end));
    }

    public void ResetToDefault()
    {
        MainCamera.transform.position = new Vector3(30, 27, 30);
        MainCamera.transform.rotation = Quaternion.Euler(30,-135,0);

        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }


}