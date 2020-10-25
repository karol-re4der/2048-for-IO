using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!Debug.isDebugBuild)
        {
            gameObject.SetActive(false);
        }
    }

}
