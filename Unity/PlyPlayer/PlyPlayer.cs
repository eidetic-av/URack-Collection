using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

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

        public void Start()
        {
            Frames = new List<PointCloud>();
            foreach (var ply in Resources.LoadAll<PointCloud>(FolderName))
            {
                var frame = ScriptableObject.CreateInstance<PointCloud>();
                frame.UsingTextureMaps = ply.UsingTextureMaps;

                if (!frame.UsingTextureMaps)
                {
                    frame.Points = new PointCloud.Point[ply.PointCount];
                    ply.Points.CopyTo(frame.Points, 0);
                }
                else
                {
                    frame.SetPositionMap(ply.PositionMap);
                    frame.SetColorMap(ply.ColorMap);
                }

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

            PointCloudOutput.UsingTextureMaps = CurrentFrame.UsingTextureMaps;

            if (!CurrentFrame.UsingTextureMaps)
            {
                var framePoints = CurrentFrame.Points;

                var transformJob = new TransformPointsJob()
                {
                    rotation = Rotation,
                    translation = Position,
                    scale = Vector3.one * Scale,
                    rgbGain = RGBGain,
                    points = new NativeArray<PointCloud.Point>(framePoints, Allocator.TempJob)
                };

                transformJob.Schedule(framePoints.Length, JobBatchSize).Complete();

                PointCloudOutput.Points = new PointCloud.Point[transformJob.points.Length];
                transformJob.points.CopyTo(PointCloudOutput.Points);

                transformJob.points.Dispose();
            }
            else
            {
                var framePositions = CurrentFrame.PositionMap;
                var frameColors = CurrentFrame.ColorMap;

                var width = framePositions.width;
                var height = framePositions.height;
                var pixelCount = width * height;

                var transformTexturesJob = new TransformTexturesJob()
                {
                    rotation = Rotation,
                    translation = Position,
                    scale = Vector3.one * Scale,
                    rgbGain = RGBGain,
                    positionMap = new NativeArray<Color>(framePositions.GetRawTextureData<Color>(), Allocator.TempJob),
                    colorMap = new NativeArray<Color>(frameColors.GetRawTextureData<Color>(), Allocator.TempJob)
                };

                transformTexturesJob.Schedule(pixelCount, JobBatchSize).Complete();

                var transformedPositions = new Texture2D(framePositions.width, framePositions.height, framePositions.format, false);
                transformedPositions.LoadRawTextureData(transformTexturesJob.positionMap);
                transformedPositions.Apply();
                PointCloudOutput.SetPositionMap(transformedPositions);

                var transformedColors = new Texture2D(framePositions.width, framePositions.height, framePositions.format, false);
                transformedColors.LoadRawTextureData(transformTexturesJob.colorMap);
                transformedColors.Apply();
                PointCloudOutput.SetColorMap(transformedColors);

                transformTexturesJob.positionMap.Dispose();
                transformTexturesJob.colorMap.Dispose();
            }

            LastPosition = Position;
            LastRotation = Rotation;
            LastScale = Scale;
            LastRGBGain = RGBGain;

            ScrubChanged = false;
            TransformChanged = false;
        }

        [BurstCompile]
        struct TransformPointsJob : IJobParallelFor
        {
            public Vector3 rotation;
            public Vector3 translation;
            public Vector3 scale;
            public float rgbGain;
            public NativeArray<PointCloud.Point> points;
            public void Execute(int i)
            {
                var point = points[i];
                point.Position = point.Position
                    .RotateBy(rotation)
                    .TranslateBy(translation)
                    .ScaleBy(scale);
                point.Color = new Color(point.Color.r + rgbGain, point.Color.g + rgbGain, point.Color.b + rgbGain);
                points[i] = point;
            }
        }

        [BurstCompile]
        struct TransformTexturesJob : IJobParallelFor
        {
            public Vector3 rotation;
            public Vector3 translation;
            public Vector3 scale;
            public float rgbGain;
            public NativeArray<Color> positionMap;
            public NativeArray<Color> colorMap;

            public void Execute(int i)
            {
                var positionData = positionMap[i];
                var position = new Vector3(positionData.r, positionData.g, positionData.b)
                    .RotateBy(rotation)
                    .TranslateBy(translation)
                    .ScaleBy(scale);
                positionMap[i] = new Color(position.x, position.y, position.z);
                var colorData = colorMap[i];
                colorMap[i] = new Color(colorData.r + rgbGain, colorData.g + rgbGain, colorData.b + rgbGain);
            }
        }

    }
}