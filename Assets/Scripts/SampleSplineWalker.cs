using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleSplineWalker : MonoBehaviour {
    public enum TravelMode
    {
        Straight,
        Loop,
        Warp
    }

    public enum TravelDirection
    {
        Forward,
        Backward
    }

    public BezierSpline spline;
    public bool faceDirection;

    public float duration;
    public float progress;

    private void Start()
    {
        this.transform.position = spline.points[0].position;
    }

    private void FixedUpdate()
    {
        progress += Time.fixedDeltaTime / duration;
        if(progress > 1f)
        {
            progress = 0f;
        }
        transform.localPosition = spline.GetPosition(progress);
        if(faceDirection)
        {
            transform.LookAt((Vector2)transform.position + spline.GetVelocity(progress));
            transform.rotation *= Quaternion.Euler(0, 90, 0);
        }
    }
}
