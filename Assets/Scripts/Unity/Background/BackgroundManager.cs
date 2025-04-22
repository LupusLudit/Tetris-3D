using Assets.Scripts.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unity.Background
{
    enum Quadrant
    {
        I,
        II,
        III,
        IV
    }

    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class BackgroundManager : MonoBehaviour
    {
        public Material BackgroundMaterial;
        public Material LineMaterial;
        public Material WarningMaterial;
        public Camera MainCamera;
        public Camera SideCamera;

        public int XMax;
        public int YMax;
        public int ZMax;

        private Vector3 mainCameraInitialPosition;
        private Vector3 sideCameraInitialPosition;
        private Vector3 initialPosition;
        private Quaternion initialRotation;

        private Quadrant currentQuadrant;
        private int quadrantAngle;

        Dictionary<Quadrant, int> quadrantAngles = new Dictionary<Quadrant, int>
        {
            { Quadrant.I, 90 },
            { Quadrant.II, 180 },
            { Quadrant.III, 270 },
            { Quadrant.IV, 0 }
        };

        void Start()
        {
            InitializeFaces();

            currentQuadrant = GetQuadrant(CalculateCameraAngle());
            quadrantAngle = quadrantAngles[currentQuadrant];
            mainCameraInitialPosition = MainCamera.transform.position;
            sideCameraInitialPosition = SideCamera.transform.position;
            initialPosition = transform.position;
            initialRotation = transform.rotation;
        }

        void Update()
        {
            Quadrant quadrant = GetQuadrant(CalculateCameraAngle());
            if (quadrant != currentQuadrant)
            {
                currentQuadrant = quadrant;
                int rotationAngle = quadrantAngles[quadrant] - quadrantAngle;
                quadrantAngle = quadrantAngles[quadrant];
                RotateGameBoard(rotationAngle);

                AdjustKeyEvents.RaiseBoardRotated(rotationAngle == -90 || rotationAngle == 270);
            }

        }

        public void ResetToDefault()
        {
            MainCamera.transform.position = mainCameraInitialPosition;
            MainCamera.transform.rotation = Quaternion.Euler(30, -135, 0);
            SideCamera.transform.position = sideCameraInitialPosition;
            SideCamera.transform.rotation = Quaternion.Euler(0, -90, 0);

            transform.position = initialPosition;
            transform.rotation = initialRotation;

            currentQuadrant = GetQuadrant(CalculateCameraAngle());
            quadrantAngle = quadrantAngles[currentQuadrant];

            AdjustKeyEvents.RaiseBoardReset();
        }

        private Quadrant GetQuadrant(float angle)
        {
            if (angle >= 0 && angle < 90) return Quadrant.I;
            else if (angle >= 90 && angle < 180) return Quadrant.II;
            else if (angle >= 180 && angle < 270) return Quadrant.III;
            else return Quadrant.IV;
        }

        private float CalculateCameraAngle()
        {
            Vector3 direction = MainCamera.transform.position - new Vector3(XMax / 2f, MainCamera.transform.position.y, ZMax / 2f);
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (angle < 0) angle += 360f;
            return angle;
        }

        private void RotateGameBoard(float angle)
        {
            Vector3 rotationCenter = new Vector3(XMax / 2f, YMax / 2f, ZMax / 2f);
            transform.RotateAround(rotationCenter, Vector3.up, angle);
            SideCamera.transform.RotateAround(rotationCenter, Vector3.up, angle);
        }
        private void InitializeFaces()
        {
            CreateFace("BottomFace", Vector3.zero, XMax, 0, ZMax, BackgroundMaterial, LineMaterial);
            CreateFace("SideFaceA", Vector3.zero, XMax, YMax - 2, 0, BackgroundMaterial, LineMaterial);
            CreateFace("SideFaceB", Vector3.zero, 0, YMax - 2, ZMax, BackgroundMaterial, LineMaterial);
            CreateFace("warningFaceA", new Vector3(0, YMax - 2, 0), XMax, 2, 0, BackgroundMaterial, WarningMaterial);
            CreateFace("warningFaceB", new Vector3(0, YMax - 2, 0), 0, 2, ZMax, BackgroundMaterial, WarningMaterial);
        }

        private void CreateFace(string name, Vector3 start, int x, int y, int z, Material background, Material line)
        {
            BackgroundFace face = new GameObject(name).AddComponent<BackgroundFace>();
            face.transform.parent = transform;
            face.Initialize(start, x, y, z, background, line);
        }
    }
}
