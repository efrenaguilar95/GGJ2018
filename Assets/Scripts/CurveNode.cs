﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CurveNode : MonoBehaviour{
    public Vector2 position;
    public float intervalKeyframe;
    public Vector2 outTangent;
    public Vector2 inTangent;

    //AG: Cubic Hermite Spline Interpolation
    //According to https://en.wikipedia.org/wiki/Cubic_Hermite_spline
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
}