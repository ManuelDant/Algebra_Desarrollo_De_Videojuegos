using CustomMath;
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

    private void Update()
    {
        // Evaluar cada punto de la grilla para determinar si está dentro de uno de los objetos
        List<Vector3> pointsInObject1 = new List<Vector3>();
        List<Vector3> pointsInObject2 = new List<Vector3>();

        int numPointsX = grid.GetLength(0);
        int numPointsY = grid.GetLength(1);
        int numPointsZ = grid.GetLength(2);

        for (int i = 0; i < numPointsX; i++)
        {
            for (int j = 0; j < numPointsY; j++)
            {
                for (int k = 0; k < numPointsZ; k++)
                {
                    Vector3 point = grid[i, j, k];

                    if (IsPointInsideObject(point, object1))
                    {
                        pointsInObject1.Add(point);
                        Debug.Log("Detecta Obj 1");
                    }
                    if (IsPointInsideObject(point, object2))
                    {
                        Debug.Log("Detecta Obj 2");
                        pointsInObject2.Add(point);
                    }
                }
            }
        }

        foreach (Vector3 point in pointsInObject1)
        {
            if (IsPointInsideObject(point, object2))
            {
                Debug.Log("Colisionan!");
                break;
            }
        }

        foreach (Vector3 point in pointsInObject2)
        {
            if (IsPointInsideObject(point, object1))
            {

                break;
            }
        }

        // Dibujar la grilla
        DrawGrid();
    }


    private bool IsPointInsideObject(Vector3 point, GameObject obj)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        int numIntersections = 0;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = obj.transform.TransformPoint(vertices[triangles[i]]);
            Vector3 p2 = obj.transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 p3 = obj.transform.TransformPoint(vertices[triangles[i + 2]]);

            if (IsPointInsideTriangle(point, p1, p2, p3))
            {
                numIntersections++;
            }
        }
        return numIntersections % 2 != 0;
    }


    private bool IsPointInsideTriangle(Vector3 point, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        Vector3 e1 = p2 - p1;
        Vector3 e2 = p3 - p1;
        Vector3 normal = Vector3.Cross(e1, e2);

        Vector3 toPoint = point - p1;
        float dot1 = Vector3.Dot(normal, toPoint);

        if (dot1 > 0) return false;

        Vector3 fromPoint = p1 - point;
        float dot2 = Vector3.Dot(normal, fromPoint);

        if (dot2 > 0) return false;

        float dot3 = Vector3.Dot(Vector3.Cross(e1, toPoint), normal);

        if (dot3 < 0) return false;

        return true;
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