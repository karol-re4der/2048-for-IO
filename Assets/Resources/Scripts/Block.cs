using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int value;

    public void SetValue(int newValue)
    {
        this.value = newValue;
    }
    public void DoubleValue()
    {
        value *= 2;
    }
}
