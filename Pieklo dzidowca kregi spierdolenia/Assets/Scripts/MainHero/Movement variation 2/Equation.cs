using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Storing first-degree equation. Inputs are two Vector3. Calculating equation based only on x and z.
/// </summary>

// by SilverWalker

public class Equation
{
    private Vector2 first;
    private Vector2 second;

    private float a;
    private float b;

    public float A
    {
        get
        {
            return a;
        }

        private set
        {
            a = value;
        }
    }

    public float B
    {
        get
        {
            return b;

        }

        private set
        {
            b = value;
        }
    }

    public Equation(Vector3 firstPoint, Vector3 secondPoint)
    {
        first.x = firstPoint.x;
        first.y = firstPoint.z;
        second.x = secondPoint.x;
        second.y = secondPoint.z;

        CalculateA();
        CalculateB();
    }

    private void CalculateA()
    {
        A = (first.y - second.y) / (first.x - second.x);
    }

    private void CalculateB()
    {
        B = first.y - (A * first.x);
    }

    public Vector3 GetPointFromEquation(float x)
    {
        Vector3 Point = Vector3.zero;

        float y;

        y = A * x + B;

        Point.x = x;
        Point.z = y;

        return Point;
    }

}
