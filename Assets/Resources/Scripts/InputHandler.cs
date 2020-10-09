using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        GetComponent<GridHandler>().actionPerformed = false;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GetComponent<GridHandler>().MoveAll(-1, 0);
            GetComponent<GridHandler>().MergeAll(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            GetComponent<GridHandler>().MoveAll(1, 0);
            GetComponent<GridHandler>().MergeAll(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            GetComponent<GridHandler>().MoveAll(0, 1);
            GetComponent<GridHandler>().MergeAll(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            GetComponent<GridHandler>().MoveAll(0, -1);
            GetComponent<GridHandler>().MergeAll(0, -1);
        }

        if (GetComponent<GridHandler>().actionPerformed)
        {
            GetComponent<GridHandler>().SpawnBlock();
        }
    }
}
