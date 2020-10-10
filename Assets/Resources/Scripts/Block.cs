using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{
    public int value;

    private Vector2 startingPos;
    private Vector2 targetPos;
    private float movementStartedAt = 0;
    private float secondsPerTurn = 0.5f;
    public float speedMod;

    public void Start()
    {
        value = 2;
        speedMod = 1;
        RefreshTexture();
    }
    public void Update()
    {
        if (!transform.position.Equals(targetPos))
        {
            float progress = (Time.time - movementStartedAt)/(secondsPerTurn*speedMod);
            transform.position = Vector2.Lerp(startingPos, targetPos, progress);
        }
    }

    public void SetValue(int newValue)
    {
        this.value = newValue;
        RefreshTexture();
    }
    public void DoubleValue()
    {
        value *= 2;
        RefreshTexture();
    }

    public void RefreshTexture()
    {
        transform.Find("Value").gameObject.GetComponent<TextMeshPro>().text = "" + value;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    public void MoveTo(Vector2 targetPos, float speedMod)
    {
        this.targetPos = targetPos;
        this.startingPos = transform.position;
        movementStartedAt = Time.time;
        this.speedMod += speedMod;
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
