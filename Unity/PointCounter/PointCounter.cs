using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Eidetic.URack.Collection
{
    public class PointCounter : VFXModule
    {
        public PointCloud PointCloudOutput { get; set; }

        override public void OnSetPointCloud(PointCloud value)
        {
            // Map 10000 points per volt
            var countVolts = value.PointCount / 10000f;
            Osc.Server.Send(InstanceAddress + "/" + "PointCount", countVolts);
        }
    }
}