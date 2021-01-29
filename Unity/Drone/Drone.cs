using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Eidetic.URack.Collection
{
    public class Drone : UModule
    {

        [Input(-5, 5, -10, 10)]
        public float X
        {
            set => CameraOrigin.transform.position = CameraOrigin.transform.position.Replace(0, value);
        }

        [Input(-5, 5, -10, 10)]
        public float Y
        {
            set => CameraOrigin.transform.position = CameraOrigin.transform.position.Replace(1, value);
        }

        [Input(-5, 5, -10, 10)]
        public float Z
        {
            set => CameraOrigin.transform.position = CameraOrigin.transform.position.Replace(2, value);
        }

        Vector3 orbit = Vector3.zero;
        [Input(-5, 5, -180, 180, false, 1, 1)]
        public float Orbit
        {
            set => orbit = orbit.Replace(1, value);
        }

        [Input(-5, 5, -90, 90, false, 1, 1)]
        public float Elevation
        {
            set => orbit = orbit.Replace(0, value);
        }

        float distance;
        [Input]
        public float Distance
        {
            get => distance;
            set => distance = value > 0.1f ? value : 0.1f;
        }

        public int Target
        {
            set 
            {
                Debug.Log($"set target to {value}");
                OriginTarget.SetActive(value > 0);
            }
        }

        Camera camera;
        Camera Camera => camera ?? (camera = GetComponentsInChildren<Camera>().Single());
        GameObject cameraOrigin;
        GameObject CameraOrigin => cameraOrigin ?? (cameraOrigin = transform.Find("Origin").gameObject);
        GameObject forwardAxis;
        GameObject ForwardAxis => forwardAxis ?? (forwardAxis = Camera.gameObject.transform.Find("ForwardAxis").gameObject);
        GameObject backwardAxis;
        GameObject BackwardAxis => backwardAxis ?? (backwardAxis = Camera.gameObject.transform.Find("BackwardAxis").gameObject);
        GameObject originTarget;
        GameObject OriginTarget => originTarget ?? (originTarget = CameraOrigin.transform.Find("OriginTarget").gameObject);
        Volume postProcessing;
        Volume PostProcessing => postProcessing ?? (postProcessing = GetComponentsInChildren<Volume>().Single());

        public void Update()
        {
            // perform the orbital camera rotaiton
            CameraOrigin.transform.SetPositionAndRotation(CameraOrigin.transform.position, Quaternion.Euler(orbit));

            // perform distance movement                
            if (Vector3.Distance(Camera.transform.position, CameraOrigin.transform.position) > distance + 0.01f)
            {
                while (Vector3.Distance(Camera.transform.position, CameraOrigin.transform.position) > distance + 0.005f)
                    Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, ForwardAxis.transform.position, 0.0005f);
            }
            else if (Vector3.Distance(Camera.transform.position, CameraOrigin.transform.position) < distance - 0.01f)
            {
                while (Vector3.Distance(Camera.transform.position, CameraOrigin.transform.position) < distance - 0.005f)
                    Camera.transform.position = Vector3.MoveTowards(Camera.transform.position, BackwardAxis.transform.position, 0.0005f);
            }
        }
    }

}
