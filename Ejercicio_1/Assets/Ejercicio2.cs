using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public class Ejercicio2 : MonoBehaviour
{
    public Vector3 vectorA;
    public Vector3 vectorB;
    
    [SerializeField]
    private float Ejercicio = 1;

    private void Update()
    {
        Uno();
    }

    private void Uno()
    {
        switch (Ejercicio)
        {
            case 1:
                Vector3 vectorSuma = vectorA + vectorB;

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorSuma, Color.green);               
                break;
            case 2:
                Vector3 vectorResta = vectorB - vectorA;

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorResta, Color.green);
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            default:
                break;
        }
        
    }
}
