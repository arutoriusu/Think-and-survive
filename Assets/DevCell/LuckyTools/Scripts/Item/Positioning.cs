using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting.APIUpdating;

public class Positioning : MonoBehaviour
{
    private bool onGroundOrHand = false;
    private bool inHands = false;
    private float groundPositionX;
    private float groundPositionY;
    private bool inHandsMoved;
    private bool onGroundMoved;
    private float zPosition;
    private float[] xCubePositionsOnGround;
    private float[] xCubePositionsOnHand;
    private bool effectAdded;

    public bool InHands { get => inHands; set => inHands = value; }
    public bool OnGroundOrHand { get => onGroundOrHand; set => onGroundOrHand = value; }
    public bool EffectAdded { get => effectAdded; set => effectAdded = value; }

    private void Start()
    {
        groundPositionX = 0;
        groundPositionY = 0;
        xCubePositionsOnGround = new float[transform.childCount];
        xCubePositionsOnHand = new float[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            xCubePositionsOnGround[i] = transform.GetChild(i).localPosition.x - 7f;
            xCubePositionsOnHand[i] = transform.GetChild(i).localPosition.x - 2f;
        }
    }

    private void FixedUpdate()
    {
        if (!gameObject.transform.parent)
        {
            zPosition = 0;
        }
        else
        {
            zPosition = transform.position.z;
        }

        if (!OnGroundOrHand)
        {
            gameObject.transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y - 5 * Time.deltaTime,
                    zPosition);

            if (!onGroundMoved)
            {
                MoveCubes();
                onGroundMoved = true;
                inHandsMoved = false;
            }
        }
        else
        {
            if (!inHands)
            {
                Rotation();
                FindCenter();
            }
            else if (inHands && !inHandsMoved)
            {
                MoveCubes();
                inHandsMoved = true;
                onGroundMoved = false;
            }
        }
    }

    private void FindCenter()
    {
        float differenceX = groundPositionX - transform.position.x;
        if (Math.Abs(differenceX) > 0.005f)
        {
            float xMove = 0.03f;
            gameObject.transform.position = new Vector3(
                    transform.position.x + xMove * Math.Sign(differenceX),
                    transform.position.y,
                    zPosition);
        }
    }

    private void Rotation()
    {
        if (transform.rotation.y < 180)
        {
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 50 * Time.fixedDeltaTime, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, -180, 0);
        }
    }

    public void PutItemInHands()
    {
        OnGroundOrHand = true;
        InHands = true;
    }

    public void DropItemFromHands()
    {
        OnGroundOrHand = false;
        InHands = false;
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public void MoveCubes()
    {
        float moveX = 0f;
        float[] xCubePositions = new float[transform.childCount];
        if (!onGroundMoved)
        {
            //moveX = -7f;
            gameObject.GetComponent<BoxCollider>().center = new Vector3(
                0,
                gameObject.GetComponent<BoxCollider>().center.y,
                gameObject.GetComponent<BoxCollider>().center.z
            );
            xCubePositions = xCubePositionsOnGround;
        }
        else if (!inHandsMoved)
        {
            moveX = 6f;
            gameObject.GetComponent<BoxCollider>().center = new Vector3(
                moveX,
                gameObject.GetComponent<BoxCollider>().center.y,
                gameObject.GetComponent<BoxCollider>().center.z
            );
            xCubePositions = xCubePositionsOnHand;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (!transform.GetChild(i).name.Contains("ItemEffect"))
            {
                transform.GetChild(i).localPosition = new Vector3(
                xCubePositions[i],
                transform.GetChild(i).localPosition.y,
                transform.GetChild(i).localPosition.z
                );
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!EffectAdded && !inHands && 
            (other.gameObject.CompareTag("Bench") ||
            other.gameObject.CompareTag("Furnace") ||
            other.gameObject.CompareTag("Ground")))
        {
            /*if (transform.parent && transform.parent.GetComponent<Animator>()) {
                if (!transform.parent.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ItemFall"))
                {
                    OnGroundOrHand = true;
                }
            }
            else
            {
                OnGroundOrHand = true;
            }*/
            OnGroundOrHand = true;

            GameManager.AddItemEffect(gameObject);
            groundPositionX = other.transform.position.x;
            groundPositionY = other.transform.position.y;
        }
    }
}
