using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int width;
    private int height;
    private float cellSize;

    private int[,] gridArray;

    public Grid(int width, int height)//생성자 함수
    {
        this.width=width;
        this.height = height;

        gridArray =new int[width, height];

        for(int x=0; x<gridArray.GetLength(0); x++)
        {
            for(int y=0; y<gridArray.GetLength(1); y++)
            {
                Debug.Log(x+", "+ y);
                Debug.DrawLine(GetWorldPos(0, y), GetWorldPos(width, y), Color.white);
            }
        }
    }

    private Vector3 GetWorldPos(int x, int z)
    {
        return new Vector3(x,0, z)*cellSize;
    }
}
