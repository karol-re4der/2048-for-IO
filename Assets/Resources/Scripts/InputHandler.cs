using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (GetComponent<GridHandler>().SpacesRemaining() > 0)
            {
                int randVal = 1;
                int randX = 0;
                int randY = 0;

                do
                {
                    randX = UnityEngine.Random.Range(0, GetComponent<GridHandler>().sizeX);
                    randY = UnityEngine.Random.Range(0, GetComponent<GridHandler>().sizeY);
                } while (GetComponent<GridHandler>().IsOccupied(randX, randY));

                GetComponent<GridHandler>().NewBlock(randX, randY, randVal);
            }
        }

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
    }
}
