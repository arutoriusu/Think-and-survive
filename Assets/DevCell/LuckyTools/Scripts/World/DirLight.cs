using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirLight : MonoBehaviour
{
    private static bool isDay;

    private Vector3 angle;
    private float rotation = 0f;
    private enum Axis
    {
        X,
        Y,
        Z
    }
    private Axis axis = Axis.X;
    private bool direction = true;

    float sunSpeed = 0.1f;
    private float r;
    private float g;
    private float b;

    public static bool IsDay { get => isDay; set => isDay = value; }
    public Vector3 Angle { get => angle; set => angle = value; }
    public float RotationInfo { get => rotation; set => rotation = value; }
    public float R { get => r; set => r = value; }
    public float G { get => g; set => g = value; }
    public float B { get => b; set => b = value; }

    // Start is called before the first frame update
    void Start()
    {
        R = RenderSettings.fogColor.r;
        G = RenderSettings.fogColor.g;
        B = RenderSettings.fogColor.b;
        Angle = transform.localEulerAngles;
    }

    float Rotation()
    {
        rotation += Time.deltaTime * sunSpeed;
        if (rotation >= 360f)
            rotation -= 360f; // this will keep it to a value of 0 to 359.99...
        return direction ? rotation : -rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localEulerAngles.x > 180f)
        {
            IsDay = false;
        } else if (transform.localEulerAngles.x > 0f)
        {
            IsDay = true;
        }

        switch (axis)
        {
            case Axis.X:
                transform.localEulerAngles = new Vector3(Rotation(), Angle.y, Angle.z);
                break;
            case Axis.Y:
                transform.localEulerAngles = new Vector3(Angle.x, Rotation(), Angle.z);
                break;
            case Axis.Z:
                transform.localEulerAngles = new Vector3(Angle.x, Angle.y, Rotation());
                break;
        }
        if (transform.localEulerAngles.x > 160f)
        {
            if (R > 24f/256f)
            {
                R -= 0.01f;
            }
            if (G > 32f/256f)
            {
                G -= 0.01f;
            }
            if (B > 49f/256f)
            {
                B -= 0.01f;
            }
        }
        else if (transform.localEulerAngles.x > 0f)
        {
            if (R < 90f / 256f)
            {
                R += 0.005f;
            }
            if (G < 108f / 256f)
            {
                G += 0.005f;
            }
            if (B < 144f / 256f)
            {
                B += 0.005f;
            }
        }

        RenderSettings.fogColor = new Color(R, G, B);
    }
}
