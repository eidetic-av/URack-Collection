using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace Eidetic.URack
{
public class Insider : UModule
    {
        public VisualEffect VFXTarget;
        public Component ComponentTarget;
        string TargetName 
        {
            set 
            {
                var go = GameObject.Find(value);
                if (!go) 
                {
                    VFXTarget = null;
                    ComponentTarget = null;
                    return;
                }
                var vfx = go.GetComponent<VisualEffect>();
                if (vfx) VFXTarget = vfx;
                else ComponentTarget = go.GetComponent(value);
            }
        }

        [Input]
        public float A { get; set; }
        string AName;

        [Input]
        public float B { get; set; }
        string BName;

        [Input]
        public float C { get; set; }
        string CName;

        [Input]
        public float D { get; set; }
        string DName;

        [Input]
        public float E { get; set; }
        string EName;

        [Action]
        public void UpdateTarget(string targetString)
        {
            var property = targetString.Split('/')[0];
            var target = targetString.Split('/')[1];
            switch(property) {
                case "A": 
                    AName = target;
                    return;
                case "B": 
                    BName = target;
                    return;
                case "C": 
                    CName = target;
                    return;
                case "D": 
                    DName = target;
                    return;
                case "E": 
                    EName = target;
                    return;
                case "TargetName": 
                    TargetName = target;
                    return;
            }
        }

        public void Update()
        {
            if (VFXTarget)
            {
                if (VFXTarget.HasFloat(AName)) VFXTarget.SetFloat(AName, A);
                if (VFXTarget.HasFloat(BName)) VFXTarget.SetFloat(BName, B);
                if (VFXTarget.HasFloat(CName)) VFXTarget.SetFloat(CName, C);
                if (VFXTarget.HasFloat(DName)) VFXTarget.SetFloat(DName, D);
                if (VFXTarget.HasFloat(EName)) VFXTarget.SetFloat(EName, E);
            }
            else if (ComponentTarget)
            {
                // TODO: Use reflection to cache properties we need to set
            }
        }
    }
}
