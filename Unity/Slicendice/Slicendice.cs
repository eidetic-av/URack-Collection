// using System;
// using System.Linq;
// using UnityEngine;
// using Eidetic.URack;
// using static UnityEngine.Experimental.Rendering.GraphicsFormat;

// namespace Eidetic.URack.Collection
// {
//     public class Slicendice : UModule
//     {
//         public void Start()
//         {
//             PointCloudInput = GetPointCloudAssets("Melbourne").First();
//         }

//         ComputeShader calculateShader;
//         ComputeShader CalculateShader => calculateShader ??
//             (calculateShader = GetAsset<ComputeShader>("Slicendice.compute"));
//         int ShaderHandle => CalculateShader.FindKernel("Calculate");

//         PointCloud pointCloudInput;
//         [Input]
//         public PointCloud PointCloudInput
//         {
//             set
//             {
//                 pointCloudInput = value;
//                 CalculateOutputs();
//             }
//         }

//         int cutMethod = 1;
//         [Input]
//         public float CutMethod
//         {
//             set
//             {
//                 cutMethod = value.Clamp(0f, 10f).Map(0f, 10f, 1f, 4.99f).FloorToInt();
//                 CalculateOutputs();
//             }
//         }

//         float cutWidth;
//         [Input]
//         public float CutWidth
//         {
//             set
//             {
//                 cutWidth = value.Clamp(0f, 10f);
//                 CalculateOutputs();
//             }
//         }

//         float cutOffset;
//         [Input]
//         public float CutOffset
//         {
//             set
//             {
//                 cutOffset = value;
//                 CalculateOutputs();
//             }
//         }

//         float spread = 0f;
//         [Input]
//         public float Spread
//         {
//             set
//             {
//                 spread = value.Clamp(0f, 100f);
//                 CalculateOutputs();
//             }
//         }

//         RenderTexture outputAPositionMap;
//         RenderTexture outputAColorMap;
//         PointCloud outputA;
//         public PointCloud OutputA => outputA ?? (outputA = ScriptableObject.CreateInstance<PointCloud>());

//         PointCloud outputB;
//         public PointCloud OutputB => outputB ?? (outputB = ScriptableObject.CreateInstance<PointCloud>());

//         PointCloud outputC;
//         public PointCloud OutputC => outputC ?? (outputC = ScriptableObject.CreateInstance<PointCloud>());

//         PointCloud outputD;
//         public PointCloud OutputD => outputD ?? (outputD = ScriptableObject.CreateInstance<PointCloud>());

//         PointCloud outputE;
//         public PointCloud OutputE => outputE ?? (outputE = ScriptableObject.CreateInstance<PointCloud>());

//         PointCloud outputF;
//         public PointCloud OutputF => outputF ?? (outputF = ScriptableObject.CreateInstance<PointCloud>());

//         PointCloud outputG;
//         public PointCloud OutputG => outputG ?? (outputG = ScriptableObject.CreateInstance<PointCloud>());

//         PointCloud outputH;
//         public PointCloud OutputH => outputH ?? (outputH = ScriptableObject.CreateInstance<PointCloud>());

//         void CalculateOutputs()
//         {
//             if (pointCloudInput?.PositionMap == null) return;

//             int inputWidth = pointCloudInput.PositionMap.width;
//             int inputHeight = pointCloudInput.PositionMap.height;
//             int inputPointCount = pointCloudInput.PointCount;


//             int outputPointCount = Mathf.FloorToInt(inputPointCount / 8f);

//             // Find a width/height of each output texture map
//             int outputWidth = Constants.MaxTextureWidth;
//             var tempHeight = (float) outputPointCount / outputWidth;
//             while (tempHeight % 1f != 0)
//                 tempHeight = (float) outputPointCount / --outputWidth;
//             var outputHeight = (int) tempHeight;

//             // Create the render textures for the outputs
//             RenderTexture initTex(string shaderParameter)
//             {
//                 var tex = new RenderTexture(outputWidth, outputHeight, 24, R32G32B32A32_SFloat);
//                 tex.enableRandomWrite = true;
//                 tex.Create();
//                 CalculateShader.SetTexture(ShaderHandle, shaderParameter, tex);
//                 return tex;
//             };

//             var outputAPositionMap = initTex("OutputAPositions");
//             var outputAColorMap = initTex("OutputAColors");
//             var outputBPositionMap = initTex("OutputBPositions");
//             var outputBColorMap = initTex("OutputBColors");
//             var outputCPositionMap = initTex("OutputCPositions");
//             var outputCColorMap = initTex("OutputCColors");
//             var outputDPositionMap = initTex("OutputDPositions");
//             var outputDColorMap = initTex("OutputDColors");
//             var outputEPositionMap = initTex("OutputEPositions");
//             var outputEColorMap = initTex("OutputEColors");
//             var outputFPositionMap = initTex("OutputFPositions");
//             var outputFColorMap = initTex("OutputFColors");
//             var outputGPositionMap = initTex("OutputGPositions");
//             var outputGColorMap = initTex("OutputGColors");
//             var outputHPositionMap = initTex("OutputHPositions");
//             var outputHColorMap = initTex("OutputHColors");

//             // Set the shader parameters
//             CalculateShader.SetTexture(ShaderHandle, "InputPositions", pointCloudInput.PositionMap);
//             CalculateShader.SetTexture(ShaderHandle, "InputColors", pointCloudInput.ColorMap);
//             CalculateShader.SetInt("CutMethod", cutMethod);
//             CalculateShader.SetFloat("CutWidth", cutWidth);
//             CalculateShader.SetFloat("CutOffset", cutOffset);
//             CalculateShader.SetFloat("Spread", spread);

//             // Dispatch
//             var threadGroupsX = Mathf.CeilToInt(inputWidth / 8f);
//             var threadGroupsY = Mathf.CeilToInt(inputHeight / 8f);
//             CalculateShader.Dispatch(ShaderHandle, threadGroupsX, threadGroupsY, 1);

//             // Set the output maps
//             OutputA.SetPositionMap(outputAPositionMap);
//             OutputA.SetColorMap(outputAColorMap);
//             OutputB.SetPositionMap(outputBPositionMap);
//             OutputB.SetColorMap(outputBColorMap);
//             OutputC.SetPositionMap(outputCPositionMap);
//             OutputC.SetColorMap(outputCColorMap);
//             OutputD.SetPositionMap(outputDPositionMap);
//             OutputD.SetColorMap(outputDColorMap);
//             OutputE.SetPositionMap(outputEPositionMap);
//             OutputE.SetColorMap(outputEColorMap);
//             OutputF.SetPositionMap(outputFPositionMap);
//             OutputF.SetColorMap(outputFColorMap);
//             OutputG.SetPositionMap(outputGPositionMap);
//             OutputG.SetColorMap(outputGColorMap);
//             OutputH.SetPositionMap(outputHPositionMap);
//             OutputH.SetColorMap(outputHColorMap);

//             // Destroy temp render textures ???
//             // Destroy(outputAPositionMap);
//             // Destroy(outputAColorMap);
//             // Destroy(outputBPositionMap);
//             // Destroy(outputBColorMap);
//             // Destroy(outputCPositionMap);
//             // Destroy(outputCColorMap);
//             // Destroy(outputDPositionMap);
//             // Destroy(outputDColorMap);
//             // Destroy(outputEPositionMap);
//             // Destroy(outputEColorMap);
//             // Destroy(outputFPositionMap);
//             // Destroy(outputFColorMap);
//             // Destroy(outputGPositionMap);
//             // Destroy(outputGColorMap);
//             // Destroy(outputHPositionMap);
//             // Destroy(outputHColorMap);
//         }

//     }
// }
