using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

namespace Eidetic.URack.Collection
{
    public class Mirage2x : VFXModule
    {
        public void Start()
        {
            lastSequence = ImageSequence;
            images = Resources.LoadAll<Texture2D>("beeple").ToList();
        }

        public void Update()
        {
            if (nextTextureDirectory)
            {
                NextTextureDirectory = true;
                nextTextureDirectory = false;
            }
            if (previousTextureDirectory)
            {
                PreviousTextureDirectory = true;
                previousTextureDirectory = false;
            }
        }

        public bool nextTextureDirectory;
        public bool NextTextureDirectory
        {
            set
            {
                if (value = false) return;
                var assetDirectories = Application.ModuleAssetDirectories[ModuleType];
                var directoryCount = assetDirectories.Count();
                if (directoryCount <= 1) return;
                var currentIndex = Array.IndexOf(assetDirectories,
                    assetDirectories.ToList().First(d => d.Contains(ImageSequence)));
                var newIndex = currentIndex + 1;
                if (newIndex == directoryCount) newIndex = 0;
                ImageSequence = Path.GetFileName(
                    Path.GetDirectoryName(assetDirectories[newIndex] + "/"));
            }
        }
        public bool previousTextureDirectory;
        public bool PreviousTextureDirectory
        {
            set
            {
                if (value = false) return;
                var assetDirectories = Application.ModuleAssetDirectories[ModuleType];
                var directoryCount = assetDirectories.Count();
                if (directoryCount <= 1) return;
                var currentIndex = Array.IndexOf(assetDirectories,
                    assetDirectories.ToList().First(d => d.Contains(ImageSequence)));
                var newIndex = currentIndex - 1;
                if (newIndex == -1) newIndex = directoryCount - 1;
                ImageSequence = Path.GetFileName(
                    Path.GetDirectoryName(assetDirectories[newIndex] + "/"));
            }
        }

        public string ImageSequence = "DefaultSequence";

        string lastSequence;
        List<Texture2D> images;
        List<Texture2D> Images
        {
            get
            {
                if (ImageSequence != lastSequence)
                {
                    images?.ForEach(i => Destroy(i));
                    images = null;
                }
                if (images != null) return images;
                images = GetTextureAssets(ImageSequence);
                lastSequence = ImageSequence;
                return images;
            }
        }

        [Input]
        public float TextureSelectA
        {
            set
            {
                if (Images.Count == 0) return;
                var sequenceLength = Images.Count - 1;
                var selection = Mathf.RoundToInt(value.Map(0, 10, 0, sequenceLength));
                selection = Mathf.Clamp(selection, 0, sequenceLength);
                var image = Images[selection];
                VisualEffect.SetTexture("TextureA", image);
                VisualEffect.SetUInt("WidthA", (uint)image.width);
                VisualEffect.SetUInt("HeightA", (uint)image.height);
            }
        }

        [Input]
        public float TextureSelectB
        {
            set
            {
                if (Images.Count == 0) return;
                var sequenceLength = Images.Count - 1;
                var selection = Mathf.RoundToInt(value.Map(0, 10, 0, sequenceLength));
                selection = Mathf.Clamp(selection, 0, sequenceLength);
                var image = Images[selection];
                VisualEffect.SetTexture("TextureB", image);
                VisualEffect.SetUInt("WidthB", (uint)image.width);
                VisualEffect.SetUInt("HeightB", (uint)image.height);
            }
        }

        [Input]
        public float SimulationSpeed
        {
            set => VisualEffect.playRate = value;
        }

        [Input]
        public float PositionX
        {
            set => transform.position = transform.position.Replace(0, value);
        }

        [Input]
        public float PositionY
        {
            set => transform.position = transform.position.Replace(1, value);
        }

        [Input]
        public float PositionZ
        {
            set => transform.position = transform.position.Replace(2, value);
        }

        [Input]
        public float RotationX
        {
            set
            {
                var euler = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(value.Map(-5, 5, -180, 180), euler.y, euler.z);
            }
        }

        [Input]
        public float RotationY
        {
            set
            {
                var euler = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(euler.x, value.Map(-5, 5, -180, 180), euler.z);
            }
        }

        [Input]
        public float RotationZ
        {
            set
            {
                var euler = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(euler.x, euler.y, value.Map(-5, 5, -180, 180));
            }
        }
    }
}
