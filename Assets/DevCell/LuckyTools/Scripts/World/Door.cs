using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool locked;
    private bool isOpen;
    private bool opening;
    private bool closing;
    private int directionOpen;
    private int directionClose;
    private float originalY;

    public bool Locked { get => locked; set => locked = value; }

    private void Start()
    {
        originalY = transform.localEulerAngles.y;
        isOpen = false;
        opening = false;
        closing = false;

        Locked = false;
    }

    private void Update()
    {
        if (opening)
        {
            if (Math.Abs(transform.localEulerAngles.y - originalY - 90 * directionOpen) > 0.5f)
            {
                
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                transform.localEulerAngles.y + 90 * directionOpen * Time.deltaTime,
                                                transform.localEulerAngles.z);
            }
            else
            {
                opening = false;
            }
        }
        else if (closing)
        {
            if (Math.Abs(transform.localEulerAngles.y - originalY) > 0.5f)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                                                transform.localEulerAngles.y + 90 * directionClose * Time.deltaTime,
                                                transform.localEulerAngles.z);

            }
            else
            {
                closing = false;
            }

        }
    }

    public void Open()
    {
        opening = true;
    }

    public void Close()
    {
        closing = true;
    }

    public bool Interact(int dir, bool useKey)
    {
        if (!Locked || useKey)
        {
            if (!isOpen)
            {
                directionOpen = dir;
                directionClose = dir * -1;
                isOpen = true;
                locked = false;
                Open();
                return true;
            }
            else
            {
                isOpen = false;
                Close();
                return true;
            }
        }
        return false;
    }
}
