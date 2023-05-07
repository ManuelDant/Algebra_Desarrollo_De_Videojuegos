using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour
{
    public float delta = 0.1f;

    public GameObject object1;
    public GameObject object2;

    public Vector3Int gridSize = new Vector3Int(10, 10, 10);

    private Vector3[,,] grid;

    private void Start()
    {
        // Crear la grilla de puntos
        int numPointsX = Mathf.RoundToInt(gridSize.x / delta) + 1;
        int numPointsY = Mathf.RoundToInt(gridSize.y / delta) + 1;
        int numPointsZ = Mathf.RoundToInt(gridSize.z / delta) + 1;
        grid = new Vector3[numPointsX, numPointsY, numPointsZ];

        Vector3 start = transform.position - Vector3.Scale(gridSize, Vector3.one * 0.5f);

        for (int i = 0; i < numPointsX; i++)
        {
            for (int j = 0; j < numPointsY; j++)
            {
                for (int k = 0; k < numPointsZ; k++)
                {
                    grid[i, j, k] = start + new Vector3(i * delta, j * delta, k * delta);
                }
            }
        }

        Debug.Log("Cant Puntos: " + (numPointsX * numPointsY * numPointsZ));
    }

    void Update()
    {
        DrawGrid();
    }

    public void DrawGrid()
    {
        int numPointsX = grid.GetLength(0);
        int numPointsY = grid.GetLength(1);
        int numPointsZ = grid.GetLength(2);

        // Dibujar líneas horizontales
        for (int j = 0; j < numPointsY; j++)
        {
            for (int k = 0; k < numPointsZ; k++)
            {
                for (int i = 0; i < numPointsX - 1; i++)
                {
                    Debug.DrawLine(grid[i, j, k], grid[i + 1, j, k], Color.white, 0.1f);
                }
            }
        }

        // Dibujar líneas verticales
        for (int i = 0; i < numPointsX; i++)
        {
            for (int k = 0; k < numPointsZ; k++)
            {
                for (int j = 0; j < numPointsY - 1; j++)
                {
                    Debug.DrawLine(grid[i, j, k], grid[i, j + 1, k], Color.white, 0.1f);
                }
            }
        }

        // Dibujar líneas profundidad
        for (int i = 0; i < numPointsX; i++)
        {
            for (int j = 0; j < numPointsY; j++)
            {
                for (int k = 0; k < numPointsZ - 1; k++)
                {
                    Debug.DrawLine(grid[i, j, k], grid[i, j, k + 1], Color.white, 0.1f);
                }
            }
        }
    }
}
