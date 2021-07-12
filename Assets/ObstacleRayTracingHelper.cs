using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ObstacleRayTracingHelper
{
    private const int numberOfViewPoints = 300;

    public static readonly Vector3[] directions;

    
    //Fills directions array with points evenly-ish distributed around the unit circle
    static ObstacleRayTracingHelper()
    {
        //fibonacci_sphere adapted from https://stackoverflow.com/questions/9600801/evenly-distributing-n-points-on-a-sphere
        
        directions = new Vector3[numberOfViewPoints];

        float goldenRatio = (1 + Mathf.Sqrt (5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numberOfViewPoints; i++)
        {
            float t = (float) i / numberOfViewPoints;
            float inclination = Mathf.Acos (1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin (inclination) * Mathf.Cos (azimuth);
            float y = Mathf.Sin (inclination) * Mathf.Sin (azimuth);
            float z = Mathf.Cos (inclination);
            directions[i] = new Vector3 (x, y, z);
        }
    }
}
