using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using static UnityEngine.Experimental.Rendering.GraphicsFormat;

namespace Eidetic.URack.Collection
{
    public class PlyPlayer : UModule
    {
        const int JobBatchSize = 4096;

        public string FolderName = "Melbourne";

        [Input] public float Run { get; set; }

        [Input] public float Reset { get; set; }

        bool ScrubChanged = true;
        bool TransformChanged = true;

        public float scrubPosition = -1;
        [Input(1)]
        public float Speed
        {
            get => scrubPosition;
            set
            {
                if (scrubPosition == value) return;
                scrubPosition = value;
                ScrubChanged = true;
            }
        }

        [Input(-5, 5, -180, 180)] public float RotationX { get; set; }

        [Input(-5, 5, -180, 180)] public float RotationY { get; set; }

        [Input(-5, 5, -180, 180)] public float RotationZ { get; set; }
        Vector3 LastRotation;
        Vector3 Rotation => new Vector3(RotationX, RotationY, RotationZ);

        [Input] public float PositionX { get; set; }

        [Input] public float PositionY { get; set; }

        [Input] public float PositionZ { get; set; }
        Vector3 LastPosition;
        Vector3 Position => new Vector3(PositionX, PositionY, PositionZ);

        float LastScale;
        [Input] public float Scale { get; set; } = 1;

        float LastRGBGain;
        public float RGBGain = 0;

        PointCloud pointCloudOutput;
        public PointCloud PointCloudOutput => pointCloudOutput ?? (pointCloudOutput = ScriptableObject.CreateInstance<PointCloud>());

        int frameCount = 0;
        List<PointCloud> Frames;

        float lastFrameTime;
        int CurrentFrameNumber => Mathf.FloorToInt(Speed.Map(0, 10, 0, frameCount - 1));
        PointCloud CurrentFrame => Frames[Mathf.Clamp(CurrentFrameNumber, 0, frameCount - 1)];

        ComputeShader transformShader;
        ComputeShader TransformShader => transformShader ??
            (transformShader = Instantiate(Resources.Load("PlyPlayerTransform") as ComputeShader));
        int TransformHandle => TransformShader.FindKernel("Transform");
        RenderTexture TransformedPositions;
        RenderTexture TransformedColors;

        public void Start()
        {
            Frames = new List<PointCloud>();
            foreach (var ply in Resources.LoadAll<PointCloud>(FolderName))
            {
                var frame = ScriptableObject.CreateInstance<PointCloud>();
                frame.SetPositionMap(ply.PositionMap);
                frame.SetColorMap(ply.ColorMap);

                Frames.Add(frame);
                frameCount++;
            }
            Debug.Log($"Loaded {frameCount} frames into PlyPlayer instance from Resources/{FolderName}");
        }

        public void Update()
        {
            if (Frames == null) return;

            if (Position != LastPosition || Rotation != LastRotation || Scale != LastScale || RGBGain != LastRGBGain)
                TransformChanged = true;

            if (!ScrubChanged && !TransformChanged) return;

            var framePositions = CurrentFrame.PositionMap;
            var frameColors = CurrentFrame.ColorMap;

            var width = framePositions.width;
            var height = framePositions.height;

            bool newTexture = false;
            if (TransformedPositions == null || TransformedPositions.height != height)
            {
                Destroy(TransformedPositions);
                Destroy(TransformedColors);
                TransformedPositions = new RenderTexture(width, height, 24, R32G32B32A32_SFloat);
                TransformedPositions.enableRandomWrite = true;
                TransformedPositions.Create();
                TransformedColors = new RenderTexture(width, height, 24, R32G32B32A32_SFloat);
                TransformedColors.enableRandomWrite = true;
                TransformedColors.Create();
                TransformShader.SetTexture(TransformHandle, "Result", TransformedPositions);
                newTexture = true;
            }

            if (ScrubChanged || newTexture)
            {
                TransformShader.SetTexture(TransformHandle, "Input", framePositions);
            }

            if (TransformChanged || newTexture)
            {
                TransformShader.SetVector("Rotation", Rotation.ToRadians());
                TransformShader.SetVector("Position", Position);
                TransformShader.SetFloat("Scale", Scale);
            }

            TransformShader.Dispatch(TransformHandle, width / 8, height / 8, 1);

            PointCloudOutput.SetPositionMap(TransformedPositions);
            PointCloudOutput.SetColorMap(frameColors);

            LastPosition = Position;
            LastRotation = Rotation;
            LastScale = Scale;
            LastRGBGain = RGBGain;

            ScrubChanged = false;
            TransformChanged = false;
        }

    }
}