using Eidetic.PointClouds;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

namespace Eidetic.URack.Collection
{
public class Populator : VFXModule
{
    public bool AutoReSeed;

    // select parameter change needs to trigger a re-generation because it requires a new emission
    float lastSelectValue;
    bool forceReseed;

    public void Start()
    {
        base.Start();
        // PointCloudInput = GetPointCloudAssets("melbourne").FirstOrDefault();
    }

    override public void OnSetPointCloud(PointCloud value)
    {
        VisualEffect.SetInt("PointCount", value.PointCount);
        Generate(AutoReSeed);
    }

    public void Update()
    {
        var selectValue = VisualEffect.GetFloat("Select");
        var reseed = AutoReSeed || forceReseed;
        if (selectValue != lastSelectValue || forceReseed) Generate(reseed);
        lastSelectValue = selectValue;
        forceReseed = false;
    }

    public void ReSeed() => forceReseed = true;

    public void Generate(bool reseed)
    {
        if (reseed)
            VisualEffect.startSeed = (uint)UnityEngine.Random.Range(uint.MinValue, uint.MaxValue);
        VisualEffect.Reinit();
        VisualEffect.SendEvent("OnPlay");
    }
}
}
