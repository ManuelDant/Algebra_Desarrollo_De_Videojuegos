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


    private enum Ejercicio {
        Uno,
        Dos,
        Tres,
        Cuatro,
        Cinco,
        Seis,
        Siete,
        Ocho,
        Nueve,
        Diez
    }

    [SerializeField]
    private Ejercicio ejercicio;

    float tiempoInicio = 0;
    float interpolationValue = 1;

    private void Update()
    {
        Ejercicios();
    }

    private void Ejercicios()
    {
        switch (ejercicio)
        {
            case Ejercicio.Uno:
                Vec3 vectorSuma = vectorA + vectorB;

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorSuma, Color.green);
                Debug.Log(vectorSuma);
                break;
            case Ejercicio.Dos:
                Vec3 vectorResta = vectorB - vectorA;

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorResta, Color.green);
                Debug.Log(vectorResta);
                break;
            case Ejercicio.Tres:
                Vec3 vectorMultiplicacion = new Vec3(
                      vectorA.x * vectorB.x,
                      vectorA.y * vectorB.y,
                      vectorA.z * vectorB.z);

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorMultiplicacion, Color.green);
                Debug.Log(vectorMultiplicacion);
                break;
            case Ejercicio.Cuatro:
                Vec3 vectorCross = Vec3.Cross(vectorB, vectorA);

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorCross, Color.green);
                Debug.Log(vectorCross);

                break;
            case Ejercicio.Cinco:

                tiempoInicio += 0.99f * Time.deltaTime;
                Vec3 vectorCinco;

                float interpolacion = Mathf.PingPong(tiempoInicio, 1.0f);

                if (interpolacion >= 0.99)
                {
                    tiempoInicio = 0;
                }

                vectorCinco = Vec3.Lerp(vectorA, vectorB, interpolacion);

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorCinco, Color.green);

                break;
            case Ejercicio.Seis:
                Vec3 maxVector = Vec3.Max(vectorA, vectorB);

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, maxVector, Color.green);
                Debug.Log(maxVector);

                break;
            case Ejercicio.Siete:
                Vec3 vectorProyect = Vec3.Project(vectorA, vectorB);

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorProyect, Color.green);
                Debug.Log(vectorProyect);

                break;
            case Ejercicio.Ocho:
                Vec3 sumaNormalizada = (vectorA + vectorB).normalized;
                float distance = Vec3.Distance(vectorA, vectorB);

                Vec3 vectorC = sumaNormalizada * distance;

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorC, Color.green);
                Debug.Log(vectorC);

                break;
            case Ejercicio.Nueve:
                
                Vec3 vectorReflect = Vec3.Reflect(vectorA, vectorB.normalized);

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, vectorReflect, Color.green);
                Debug.Log(vectorReflect);      

                break;
            case Ejercicio.Diez:
                Vec3 newPosition = Vec3.LerpUnclamped(vectorA, vectorB, interpolationValue);
                interpolationValue -= Time.deltaTime * 1;

                if (interpolationValue < -9f)
                {
                    interpolationValue = 1f;
                }

                Debug.DrawLine(Vector3.zero, vectorA, Color.red);
                Debug.DrawLine(Vector3.zero, vectorB, Color.blue);
                Debug.DrawLine(Vector3.zero, newPosition, Color.green);
                break;
            default:
                break;
        }
        
    }
}
