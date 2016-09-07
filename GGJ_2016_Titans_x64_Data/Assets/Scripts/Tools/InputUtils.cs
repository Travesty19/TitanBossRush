using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

static class InputHelper
{
    public const string k_axisX = "Horizontal";
    public const string k_axisY = "Vertical";

    public static Vector2 GetInputVector()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    public static float GetInputAngle()
    {
        Vector2 inputVec = GetInputVector();

        float inputAngle = Vector2.Angle(Vector2.up, inputVec);
        if (inputVec.x < 0)
            inputAngle = 360 - inputAngle;

        return inputAngle;
    }

    public static float GetInputMagnitude()
    {
        Vector2 inputVec = GetInputVector();

        return Mathf.Min(inputVec.magnitude, 1.0f);
    }
}