using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public class Ejercicio2 : MonoBehaviour
{
    [SerializeField]
    private Vector3 vectorA;
    [SerializeField]
    private Vector3 vectorB;

    [SerializeField]
    private float Ejercicio = 1;

    float tiempoInicio = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        Uno();
    }

    private void Uno()
    {
        switch (Ejercicio)
        {
            case 1:
                Vec3 vectorSuma = vectorA + vectorB;

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorSuma, Color.green);               
                break;
            case 2:
                Vec3 vectorResta = vectorB - vectorA;

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorResta, Color.green);
                break;
            case 3:
                Vec3 vectorMultiplicacion = new Vec3(
                      vectorA.x * vectorB.x,
                      vectorA.y * vectorB.y,
                      vectorA.z * vectorB.z);

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorMultiplicacion, Color.green);
                break;
            case 4:
                Vec3 vectorRaro = new Vec3(
                    vectorA.z * vectorB.y - vectorA.y,
                    vectorA.z * -vectorB.x + vectorA.x,
                    vectorA.x * -vectorB.y + vectorA.y * vectorB.x);

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorRaro, Color.green);

                break;
            case 5:
                
                tiempoInicio += 0.99f * Time.deltaTime;
                Vec3 vectorCinco;
                float interpolacion = 0;

                interpolacion = Mathf.PingPong(tiempoInicio, 1.0f);

                if (interpolacion >= 0.99)
                {
                   tiempoInicio = 0;
                }

                vectorCinco = Vec3.Lerp(vectorA, vectorB, interpolacion);

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorCinco, Color.green);

                break;
            default:
                break;
        }
        
    }
}
