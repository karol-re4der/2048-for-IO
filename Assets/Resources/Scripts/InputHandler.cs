using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class InputHandler : MonoBehaviour
{
    private List<Vector2> actionQueue = new List<Vector2>();
    public float actionLength;
    public int maxActions;

    private Vector2 touchPivot;

    void Update() 
    {
        if (!GameObject.Find("Canvas/Popup").GetComponent<PopupHandler>().IsOn())
        {
            if (!Application.isEditor)
            {
                if (Input.touches.Length == 1)
                {
                    if (Input.touches[0].phase == TouchPhase.Began)
                    {
                        touchPivot = Input.touches[0].position;
                    }
                    else if (Input.touches[0].phase == TouchPhase.Ended)
                    {
                        float xShift = touchPivot.x - Input.touches[0].position.x;
                        float yShift = touchPivot.y - Input.touches[0].position.y;
                        if (Mathf.Abs(xShift) > Mathf.Abs(yShift))
                        {
                            if (xShift > 0)
                            {
                                actionQueue.Add(new Vector2(-1, 0));
                            }
                            else if (xShift < 0)
                            {
                                actionQueue.Add(new Vector2(1, 0));
                            }
                        }
                        else if (Mathf.Abs(xShift) < Mathf.Abs(yShift))
                        {
                            if (yShift > 0)
                            {
                                actionQueue.Add(new Vector2(0, -1));
                            }
                            else if (yShift < 0)
                            {
                                actionQueue.Add(new Vector2(0, 1));
                            }
                        }
                        GameObject.Find("Canvas/DebugText").GetComponent<TextMeshProUGUI>().text += "\n" + xShift + " " + yShift;
                    }
                }
            }
            else
            {
                if (actionQueue.Count() < maxActions)
                {
                    if (Input.GetKeyDown(KeyCode.LeftArrow))
                    {
                        actionQueue.Add(new Vector2(-1, 0));
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow))
                    {
                        actionQueue.Add(new Vector2(1, 0));

                    }
                    else if (Input.GetKeyDown(KeyCode.UpArrow))
                    {
                        actionQueue.Add(new Vector2(0, 1));

                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow))
                    {
                        actionQueue.Add(new Vector2(0, -1));
                    }
                }
            }
        }

        if (!ActionInProgress() && actionQueue.Count() > 0)
        {
            if (!GameObject.Find("Grid").GetComponent<GridHandler>().IsGameOver())
            {
                GetComponent<GridHandler>().actionPerformed = false;

                GetComponent<GridHandler>().MergeAll((int)actionQueue.First().x, (int)actionQueue.First().y);
                GetComponent<GridHandler>().MoveAll((int)actionQueue.First().x, (int)actionQueue.First().y);
                actionQueue.RemoveAt(0);

                if (GetComponent<GridHandler>().IsGameWon())
                {
                    actionQueue.Clear();
                    GameObject.Find("Canvas/Popup").GetComponent<PopupHandler>().Enter("Game Won!");
                }
                else if (GetComponent<GridHandler>().actionPerformed)
                {
                    GetComponent<GridHandler>().SpawnBlock();
                }
            }
            else
            {
                actionQueue.Clear();
                GameObject.Find("Canvas/Popup").GetComponent<PopupHandler>().Enter("Game over...");
            }
        }
    }

    public bool ActionInProgress()
    {
        foreach(GameObject block in GameObject.FindGameObjectsWithTag("Block"))
        {
            if (block.GetComponent<Block>().IsInMotion())
            {
                return true;
            }
        }
        return false;
    }
}
