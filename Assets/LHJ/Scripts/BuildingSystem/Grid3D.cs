using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid3D
{
    private int col;
    private int row;
    private int[,] gridArray;
    public float cellSize;
    private int width;
    private int height;
    private Vector3 startPos;

    public Grid3D(int col, int row, float cellSize, Vector3 startPos)
    {
        this.col=col;
        this.row=row;
        this.cellSize=cellSize;
        this.startPos=startPos;

        gridArray=new int[col,row];

        for(int x=0;x<gridArray.GetLength(0); x++)
        {
            for(int z=0; z<gridArray.GetLength(1); z++)
            {
                Debug.Log(x+", "+ z);

                Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x, z+1), Color.white, 100f);
                Debug.DrawLine(GetWorldPos(x, z), GetWorldPos(x+1, z), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPos(0, row), GetWorldPos(col, row), Color.white, 100f);
        Debug.DrawLine(GetWorldPos(col, 0), GetWorldPos(col, row), Color.white, 100f);
    }

    private Vector3 GetWorldPos(int x, int z)
    {
        return new Vector3(x, 0, z)*cellSize +startPos;
    }
}
