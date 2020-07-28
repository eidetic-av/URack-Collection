using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eidetic.URack.Collection
{
    public class HighestPoint : VFXModule
    {
        public PointCloud PointCloudOutput { get; set; }

        override public void OnSetPointCloud(PointCloud value)
        {
            // Map 1 metre per volt
            var height = -99f;
            foreach (var point in value.Points) {
                if (point.Position.z > height) height = point.Position.z;
            }
            Osc.Server.Send(InstanceAddress + "/" + "Height", height);
            PointCloudOutput = value;
        }
    }
}