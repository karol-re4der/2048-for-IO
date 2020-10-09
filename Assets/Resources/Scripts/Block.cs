using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Block : MonoBehaviour
{
    public int value;

    public void Start()
    {
        value = 2;
        RefreshTexture();
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
}
