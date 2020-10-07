using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{

    public int sizeX, sizeY;
    public int blockSize, marginSize;

    private Block[,] grid;

    void Start()
    {
        grid = new Block[sizeX, sizeY];
    }

    public Block At(int posX, int posY)
    {
        return grid[posX, posY];
    }

    private Block InstantiateBlock()
    {
        return Instantiate(Resources.Load("Prefabs/Block") as GameObject, transform).GetComponent<Block>();
    }

    public bool IsOccupied(int posX, int posY)
    {
        if(posX<0 || posX >= grid.GetLength(0))
        {
            return true;
        }

        if (posY < 0 || posY >= grid.GetLength(1))
        {
            return true;
        }

        return At(posX, posY) != null;
    }

    public void NewBlock(int posX, int posY, int value)
    {
        if (!IsOccupied(posX, posY))
        {
            Block newBlock = InstantiateBlock();
            newBlock.SetValue(value);
            newBlock.transform.localScale = new Vector2(blockSize, blockSize);
            PositionBlock(newBlock, posX, posY);
            grid[posX, posY] = newBlock;
        }
    }

    private void PositionBlock(Block block, int posX, int posY)
    {
        block.transform.position = IndexToPos(posX, posY);

    }

    private Vector2 IndexToPos(int x, int y)
    {
        return new Vector2(x * blockSize + x * marginSize, y * blockSize + y * marginSize);
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

    private void MoveBlock(int oldX, int oldY, int newX, int newY)
    {
        grid[newX, newY] = grid[oldX, oldY];
        grid[oldX, oldY] = null;
        PositionBlock(grid[newX, newY], newX, newY);
    }

    public void MoveAll(int dirX, int dirY)
    {
        for (int x = dirX<0?0:grid.GetLength(0)-1; x>=0 && x < grid.GetLength(0); x-= dirX != 0 ? dirX : 1)
        {
            for (int y = dirY < 0 ? 0 : grid.GetLength(1) - 1; y >= 0 && y < grid.GetLength(1); y -= dirY!=0?dirY:1)

            {
                if (IsOccupied(x, y))
                {
                    Block block = At(x, y);
                    int orginalX = x;
                    int orginalY = y;
                    int distance = 1;

                    while(!IsOccupied(orginalX+dirX*distance, orginalY + dirY*distance))
                    {
                        int newX = orginalX + dirX * distance;
                        int newY = orginalY + dirY * distance;
                        int oldX = orginalX + dirX * (distance-1);
                        int oldY = orginalY + dirY * (distance-1);
                        distance++;

                        MoveBlock(oldX, oldY, newX, newY);
                    }

                }
            }
        }
    }

    public void MergeAll(int dirX, int dirY)
    {

    }
}
