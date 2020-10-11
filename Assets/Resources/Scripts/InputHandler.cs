using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour
{
    private List<Vector2> actionQueue = new List<Vector2>();
    public float actionLength;
    public int maxActions;

    void Update() 
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
        

        if (!ActionInProgress() && actionQueue.Count()>0)
        {
            GetComponent<GridHandler>().actionPerformed = false;

            GetComponent<GridHandler>().MergeAll((int)actionQueue.First().x, (int)actionQueue.First().y);
            GetComponent<GridHandler>().MoveAll((int)actionQueue.First().x, (int)actionQueue.First().y);
            actionQueue.RemoveAt(0);

            if (GetComponent<GridHandler>().actionPerformed)
            {
                GetComponent<GridHandler>().SpawnBlock();
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
