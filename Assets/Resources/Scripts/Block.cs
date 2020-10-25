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
        Color newBlockColor = Color.white;
        Color newTextColor = Color.white;
        switch (value)
        {
            case 2:
                ColorUtility.TryParseHtmlString("#ece4db", out newBlockColor);
                ColorUtility.TryParseHtmlString("#756e66", out newTextColor);

                break;
            case 4:
                ColorUtility.TryParseHtmlString("#ece2d0", out newBlockColor);
                ColorUtility.TryParseHtmlString("#756e66", out newTextColor);
                break;
            case 8:
                ColorUtility.TryParseHtmlString("#e9b485", out newBlockColor);
                break;
            case 16:
                ColorUtility.TryParseHtmlString("#e99b73", out newBlockColor);
                break;
            case 32:
                ColorUtility.TryParseHtmlString("#e7846e", out newBlockColor);
                break;
            case 64:
                ColorUtility.TryParseHtmlString("#e56b51", out newBlockColor);
                break;
            case 128:
                ColorUtility.TryParseHtmlString("#ead698", out newBlockColor);
                break;
            case 256:
                ColorUtility.TryParseHtmlString("#e9d48e", out newBlockColor);
                break;
            case 512:
                ColorUtility.TryParseHtmlString("#f0d571", out newBlockColor);
                break;
            case 1024:
                ColorUtility.TryParseHtmlString("#e7ce79", out newBlockColor);
                break;
            case 2048:
                ColorUtility.TryParseHtmlString("#e5c542", out newBlockColor);
                break;
        }
        GetComponent<Renderer>().material.color = newBlockColor;
        transform.Find("Value").gameObject.GetComponent<TextMeshPro>().color = newTextColor;
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
