using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{
    public int value;

    private Vector2 startingPos;
    private Vector2 targetPos;
    private Block targetBlock;
    private float movementStartedAt = 0;
    public float speedMod;
    private float secondsPerTurn;
    private bool markedForDisposing = false;
    public bool beingMergedInto = false;

    public void Start()
    {
        secondsPerTurn = GameObject.Find("Grid").GetComponent<InputHandler>().actionLength;
        value = 2;
        speedMod = 0;
        RefreshTexture();
    }
    public void Update()
    {
        Vector2 destination = targetBlock ? targetBlock.targetPos : targetPos;

        if (!transform.position.Equals(destination))
        {
            float actualMod = targetBlock ? (speedMod + targetBlock.speedMod) : speedMod;
            float progress = (Time.time - movementStartedAt) / (secondsPerTurn * actualMod);
            transform.position = Vector2.Lerp(startingPos, destination, progress);
            if (transform.position.Equals(destination))
            {
                if (targetBlock)
                {
                    targetBlock.RefreshTexture();
                    targetBlock.speedMod = 0;
                    targetBlock.beingMergedInto = false;
                }
                else if (!beingMergedInto)
                {
                    speedMod = 0;
                }

                if (markedForDisposing)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public bool IsInMotion()
    {
        return !transform.position.Equals(targetBlock ? targetBlock.targetPos : targetPos);
    }

    public void SetValue(int newValue)
    {
        this.value = newValue;
        RefreshTexture();
    }
    public void DoubleValue()
    {
        value *= 2;
    }

    public void RefreshTexture()
    {
        transform.Find("Value").gameObject.GetComponent<TextMeshPro>().text = "" + value;
    }

    public void Dispose()
    {
        markedForDisposing = true;
    }

    public void MoveTo(Vector2 targetPos, float speedMod, Block targetBlock = null)
    {
        this.targetPos = targetPos;
        this.startingPos = transform.position;
        movementStartedAt = Time.time;
        this.speedMod += speedMod;

        if (targetBlock)
        {
            this.targetBlock = targetBlock;
        }
    }
    public void PlaceAt(Vector2 targetPos)
    {
        transform.position = targetPos;
        this.targetPos = targetPos;
    }

    public void DelayAppearance()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
            Invoke("DelayAppearance", secondsPerTurn);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
