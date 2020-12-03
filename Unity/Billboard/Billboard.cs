using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Eidetic.URack.Collection {
    public class Billboard : VFXModule
    {
        public void Start()
        {
            // Debug.Log("URACK_DEBUG: Initialised Billboard");
            // var ktree = GetPointCloudAssets("test-sequence").SingleOrDefault();
            // Debug.Log("URACK_DEBUG: ktree null? " + (ktree == null));
            // if (ktree == null) return;
            // VisualEffect.SetTexture(PointCloudBinder.PositionsProperty, ktree.PositionMap);
            // VisualEffect.SetTexture(PointCloudBinder.ColorsProperty, ktree.ColorMap);
            // var size = ktree.PositionMap.width * ktree.PositionMap.height;
            // VisualEffect.SetInt(PointCloudBinder.PointCountProperty, size);
            // PointCloudBinder.UpdateMaps = false;

            // gameObject.transform.position = new Vector3(0, 0, 4);

            VisualEffect.Play();
        }
    }
}
