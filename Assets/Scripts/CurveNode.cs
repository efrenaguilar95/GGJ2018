using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveNode : MonoBehaviour{
    public Vector2 position
    { get { return this.transform.position; }
      set { this.transform.position = value; }
    }

    public float intervalKeyframe;
    public Vector2 outTangent;
    public Vector2 inTangent;

    public int? index; // To be set by parent curve data structure

    //AG: Cubic Hermite Spline Interpolation
    //According to https://en.wikipedia.org/wiki/Cubic_Hermite_spline
    
    //AG: Currently unused, look at GetIntervalPosition below
    public static float DetermineIntervalPositon(CurveNode lhs, CurveNode rhs, float currentInverval)
    {
        float intervalLength = rhs.intervalKeyframe - lhs.intervalKeyframe;
        float m0 = lhs.outTangent.normalized.y/lhs.outTangent.normalized.x * intervalLength;
        float m1 = rhs.inTangent.normalized.y/rhs.inTangent.normalized.y * intervalLength;

        float interval = currentInverval;
        float interval2 = interval * interval;
        float interval3 = interval2 * interval;

        float a = 2 * interval3 - 3 * interval2 + 1; //2t^3 - 3t^2 + 1
        float b = interval3 - 2 * interval2 + interval; //t^3 - 2t^2 + t
        float c = -2 * interval3 + 3 * interval2; //-2t^3 + 3t^2
        float d = interval3 - interval2;

        return a * lhs.position.y + b * m0 + c * m1 + d * rhs.position.y;
    }

    //Bezier Interpolation
    public static Vector2 GetInvervalPosition(CurveNode lhs, CurveNode rhs, float t)
    {
        Vector2 lhsOutBezierPoint = lhs.transform.TransformPoint(lhs.outTangent);
        Vector2 rhsInBezierPoint = rhs.transform.TransformPoint(rhs.inTangent);

        t = Mathf.Clamp01(t);
        float t2 = t * t;
        float t3 = t2 * t;

        float oneMinusT = 1f - t;
        float oneMinusT2 = oneMinusT * oneMinusT;
        float oneMinusT3 = oneMinusT2 * oneMinusT;

        return oneMinusT3 * lhs.position + //(1-t)^3 * Point0
            3f * oneMinusT2 * t * lhsOutBezierPoint + //3 * (1-t)^2 * t * Point1
            3f * oneMinusT * t2 * rhsInBezierPoint + //3 * (1-t) * t^2 + Point2
            t3 * rhs.position; //t^3 
    }

    public static Vector2 GetFirstDerivative (CurveNode lhs, CurveNode rhs, float t)
    {
        Vector2 lhsOutBezierPoint = lhs.transform.TransformPoint(lhs.outTangent);
        Vector2 rhsInBezierPoint = rhs.transform.TransformPoint(rhs.inTangent);

        t = Mathf.Clamp01(t);
        float t2 = t * t;

        float oneMinusT = 1f - t;
        float oneMinusT2 = oneMinusT * oneMinusT;

        return 3f * oneMinusT2 * (lhsOutBezierPoint - lhs.position) +
            6f * oneMinusT * t * (rhsInBezierPoint - lhsOutBezierPoint) +
            3f * t2 * (rhs.position - rhsInBezierPoint);
    }
}
