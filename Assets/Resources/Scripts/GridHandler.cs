using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{

    public int sizeX, sizeY;
    public float blockSize, marginSize;
    public bool actionPerformed;

    private Block[,] grid;
    private float gridResolution;

    void Start()
    {
        Camera.main.orthographicSize = (sizeX*blockSize+(sizeX+1)*marginSize) / (2f * Camera.main.aspect);
        //Camera.main.transform.position = new Vector3(Camera.main.transform.position.x / 2, Camera.main.transform.position.y / 2, -10);
        //blockSize = (float)Camera.main.orthographicSize/sizeX;
        //marginSize = 0f;
        grid = new Block[sizeX, sizeY];
        SpawnBlock();
        SpawnBlock();
    }

    public Block At(int posX, int posY)
    {
        return IsOnBoard(posX, posY)?grid[posX, posY]:null;
    }

    private Block InstantiateBlock()
    {
        return Instantiate(Resources.Load("Prefabs/Block") as GameObject, transform).GetComponent<Block>();
    }

    public bool IsOnBoard(int posX, int posY) {
        if (posX < 0 || posX >= grid.GetLength(0))
        {
            return false;
        }

        if (posY < 0 || posY >= grid.GetLength(1))
        {
            return false;
        }

        return true;
    }
    public bool IsOccupied(int posX, int posY)
    {
        return At(posX, posY) != null;
    }

    public void NewBlock(int posX, int posY, int value)
    {
        if (IsOnBoard(posX, posY) && !IsOccupied(posX, posY))
        {
            Block newBlock = InstantiateBlock();
            newBlock.SetValue(value);
            newBlock.transform.localScale = new Vector3(blockSize, blockSize, blockSize);
            newBlock.PlaceAt(IndexToPos(posX, posY));
            grid[posX, posY] = newBlock;
        }
    }

    private Vector2 IndexToPos(int x, int y)
    {
        return new Vector2(blockSize/2 + x * blockSize + x * marginSize + marginSize, blockSize / 2 + y * blockSize + y * marginSize + marginSize);
    }

    public int SpacesRemaining()
    {
        int count = 0;
        for(int x = 0; x<grid.GetLength(0); x++)
        {
            for(int y = 0; y<grid.GetLength(1); y++)
            {
                if(!IsOccupied(x, y))
                {
                    count++;
                }
            }
        }
        return count;
    }

    private void MoveBlock(int oldX, int oldY, int newX, int newY, bool replace = true, Block targetBlock = null)
    {
        float mod = 0;
        if (oldX != newX)
        {
            mod = Mathf.Abs(oldX - newX) / (float)(sizeX-1);
        }
        else
        {
            mod = Mathf.Abs(oldY - newY) / (float)(sizeY-1);
        }

        if (replace)
        {
            grid[newX, newY] = grid[oldX, oldY];
        }

        grid[oldX, oldY].MoveTo(IndexToPos(newX, newY), mod, targetBlock:targetBlock);
        grid[oldX, oldY] = null;
    }

    public void MoveAll(int dirX, int dirY)
    {
        for (int x = dirX<0?0:grid.GetLength(0)-1; x>=0 && x < grid.GetLength(0); x-= dirX != 0 ? dirX : 1)
        {
            for (int y = dirY < 0 ? 0 : grid.GetLength(1) - 1; y >= 0 && y < grid.GetLength(1); y -= dirY!=0?dirY:1)

            {
                if (IsOnBoard(x, y) && IsOccupied(x, y))
                {
                    Block block = At(x, y);
                    int orginalX = x;
                    int orginalY = y;
                    int distance = 1;

                    while(IsOnBoard(orginalX + dirX * distance, orginalY + dirY * distance) && !IsOccupied(orginalX+dirX*distance, orginalY + dirY*distance))
                    {
                        int newX = orginalX + dirX * distance;
                        int newY = orginalY + dirY * distance;
                        int oldX = orginalX + dirX * (distance-1);
                        int oldY = orginalY + dirY * (distance-1);
                        distance++;

                        MoveBlock(oldX, oldY, newX, newY);
                        actionPerformed = true;
                    }

                }
            }
        }
    }

    public void MergeAll(int dirX, int dirY)
    {
        for (int x = dirX < 0 ? 0 : grid.GetLength(0) - 1; x >= 0 && x < grid.GetLength(0); x -= dirX != 0 ? dirX : 1)
        {
            for (int y = dirY < 0 ? 0 : grid.GetLength(1) - 1; y >= 0 && y < grid.GetLength(1); y -= dirY != 0 ? dirY : 1)
            {
                if (IsOccupied(x, y))
                {
                    Block block = At(x, y);
                    int neighbourX = x - dirX;
                    int neighbourY = y - dirY;
                    int distance = 1;
                    Block neighbour = null;

                    while (IsOnBoard(neighbourX, neighbourY))
                    {
                        neighbour = At(neighbourX, neighbourY);

                        if (neighbour)
                        {
                            if (neighbour.value == block.value)
                            {
                                block.DoubleValue();
                                block.beingMergedInto = true;
                                neighbour.Dispose();
                                MoveBlock(neighbourX, neighbourY, x, y, replace:false, block);
                                actionPerformed = true;
                            }
                            break;
                        }
                        distance++;
                        neighbourX = x - dirX * distance;
                        neighbourY = y - dirY * distance;
                    }
                }
            }
        }
    }

    public void SpawnBlock()
    {
        if (SpacesRemaining() > 0)
        {
            int randVal = 1;
            int randX = 0;
            int randY = 0;

            do
            {
                randX = UnityEngine.Random.Range(0, sizeX);
                randY = UnityEngine.Random.Range(0, sizeY);
            } while (IsOccupied(randX, randY));

            NewBlock(randX, randY, randVal);
            grid[randX, randY].Start();
            grid[randX, randY].DelayAppearance();
        }
    }
}
