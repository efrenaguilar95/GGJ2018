using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSplineWalker : MonoBehaviour {

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

    private BezierSpline spline;
    private PlayerController pc;
    public bool faceDirection;

    public float duration;
    private float progress;
    public TravelDirection direction;

    public void Start()
    {
        this.enabled = false;
    }

    public void enableSplineMovement(TravelDirection direction, BezierSpline foundSpline)
    {
        this.enabled = true;
        spline = foundSpline;
        float speed = 10f;
        duration = foundSpline._TotalDistance() / speed;
        if(direction == TravelDirection.Forward)
        {
            progress = 0;
        }
        else
        {
            progress = 1f;
        }
        this.GetComponent<Rigidbody2D>().isKinematic = true;
    }

    public void disableSplineMovement()
    {
        float speed = this.GetComponent<Rigidbody2D>().velocity.magnitude;
        if(direction == TravelDirection.Backward)
        {
            speed *= -1f;
        }

        this.GetComponent<Rigidbody2D>().velocity = spline.GetVelocity(progress).normalized * speed;
        this.enabled = false;
        this.GetComponent<Rigidbody2D>().isKinematic = false;
    }

    private void FixedUpdate()
    {
        switch (direction)
        {
            case (TravelDirection.Forward):
                progress += Time.fixedDeltaTime / duration;
                if (progress > 1f)
                {
                    progress = 1f;
                }
                transform.localPosition = spline.GetPosition(progress);
                if (faceDirection)
                {
                    transform.LookAt((Vector2)transform.position + spline.GetVelocity(progress));
                    transform.rotation *= Quaternion.Euler(0, 90, 0);
                }
                if (progress == 1f)
                {
                    disableSplineMovement();
                }
                break;
            case (TravelDirection.Backward):
                progress -= Time.fixedDeltaTime / duration;
                if (progress < 0f)
                {
                    progress = 0f;
                }
                transform.localPosition = spline.GetPosition(progress);
                if (faceDirection)
                {
                    transform.LookAt((Vector2)transform.position + spline.GetVelocity(progress));
                    transform.rotation *= Quaternion.Euler(0, 90, 0);
                }
                if (progress == 0f)
                {
                    disableSplineMovement();
                }
                break;
        }
    }
}
