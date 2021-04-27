using Eidetic.PointClouds;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Experimental.Rendering.GraphicsFormat;

namespace Eidetic.URack.Collection
{
    public class ABBox : UModule
    {
        const int JobBatchSize = 4096;

        [Input] public float PositionX { set { Position.x = value; } }
        [Input] public float PositionY { set { Position.y = value; } }
        [Input] public float PositionZ { set { Position.z = value; } }
        Vector3 Position = Vector3.zero;

        [Input] public float ScaleX { set { Scale.x = value; } }
        [Input] public float ScaleY { set { Scale.y = value; } }
        [Input] public float ScaleZ { set { Scale.z = value; } }
        Vector3 Scale = Vector3.one;

        ComputeShader abBoxShader;
        ComputeShader ABBoxShader => abBoxShader ??
            (abBoxShader = GetAsset<ComputeShader>("ABBoxShader.compute"));
        int ShaderHandle => ABBoxShader.FindKernel("ABBox");

        PointCloud pointCloud;
        [Input]
        public PointCloud PointCloud
        {
            set
            {
                pointCloud = value;
                if (pointCloud?.PositionMap != null)
                {
                    Debug.Log("Running set textures");
                    ABBoxShader.SetTexture(ShaderHandle, "InputPositions", pointCloud.PositionMap);
                    ABBoxShader.SetTexture(ShaderHandle, "InputColors", pointCloud.ColorMap);
                }
            }
            get => pointCloud;
        }

        PointCloud inside;
        public PointCloud Inside
        {
            get
            {
                CalculateOutputMaps();
                return inside;
            }
        }

        PointCloud outside;
        public PointCloud Outside
        {
            get
            {
                CalculateOutputMaps();
                return outside;
            }
        }

        RenderTexture InsidePositionMap;
        RenderTexture InsideColorMap;
        RenderTexture OutsidePositionMap;
        RenderTexture OutsideColorMap;

        int lastCalculationFrame;
        
        void CalculateOutputMaps()
        {
            if (lastCalculationFrame == Time.frameCount) return;
            lastCalculationFrame = Time.frameCount;
            Debug.Log("Running calculate maps");
            
            if (inside == null) inside = PointCloud.CreateInstance();
            if (outside == null) outside = PointCloud.CreateInstance();

            if (PointCloud?.PositionMap == null) return;
            Debug.Log("PointCloud?.PositionMap not null");

            var inputPositions = PointCloud.PositionMap;
            var inputColors = PointCloud.ColorMap;

            var width = inputPositions.width;
            var height = inputPositions.height;

            // ABBoxShader.SetTexture(ShaderHandle, "InputPositions", inputPositions);
            // ABBoxShader.SetTexture(ShaderHandle, "InputColors", inputColors);

            if (InsidePositionMap == null || InsidePositionMap.height != height)
            {
                Destroy(InsidePositionMap);
                InsidePositionMap = new RenderTexture(width, height, 24, R32G32B32A32_SFloat);
                InsidePositionMap.enableRandomWrite = true;
                InsidePositionMap.Create();
                ABBoxShader.SetTexture(ShaderHandle, "InsidePositions", InsidePositionMap);

                Destroy(InsideColorMap);
                InsideColorMap = new RenderTexture(width, height, 24, R32G32B32A32_SFloat);
                InsideColorMap.enableRandomWrite = true;
                InsideColorMap.Create();
                ABBoxShader.SetTexture(ShaderHandle, "InsideColors", InsideColorMap);

                Destroy(OutsidePositionMap);
                OutsidePositionMap = new RenderTexture(width, height, 24, R32G32B32A32_SFloat);
                OutsidePositionMap.enableRandomWrite = true;
                OutsidePositionMap.Create();
                ABBoxShader.SetTexture(ShaderHandle, "OutsidePositions", OutsidePositionMap);

                Destroy(OutsideColorMap);
                OutsideColorMap = new RenderTexture(width, height, 24, R32G32B32A32_SFloat);
                OutsideColorMap.enableRandomWrite = true;
                OutsideColorMap.Create();
                ABBoxShader.SetTexture(ShaderHandle, "OutsideColors", OutsideColorMap);
            }

            float minX = Position.x - (Scale.x / 2);
            float minY = Position.y - (Scale.y / 2);
            float minZ = Position.z - (Scale.z / 2);
            float maxX = Position.x + (Scale.x / 2);
            float maxY = Position.y + (Scale.y / 2);
            float maxZ = Position.z + (Scale.z / 2);

            ABBoxShader.SetFloat("MinX", minX);
            ABBoxShader.SetFloat("MinY", minY);
            ABBoxShader.SetFloat("MinZ", minZ);
            ABBoxShader.SetFloat("MaxX", maxX);
            ABBoxShader.SetFloat("MaxY", maxY);
            ABBoxShader.SetFloat("MaxZ", maxZ);

            var threadGroupsX = Mathf.CeilToInt(width / 8f);
            var threadGroupsY = Mathf.CeilToInt(height / 8f);
            ABBoxShader.Dispatch(ShaderHandle, threadGroupsX, threadGroupsY, 1);
            Debug.Log("Dispatched shader");

            inside.SetPositionMap(InsidePositionMap);
            inside.SetColorMap(InsideColorMap);

            outside.SetPositionMap(OutsidePositionMap);
            outside.SetColorMap(OutsideColorMap);
            Debug.Log("Set all maps");
        }

    }
}
