using CustomMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVec3 : MonoBehaviour
{

    void Start()
    {
        TestAngle();
        TestClampMagnitude();
        TestMagnitude();
        TestCross();

    }

    private void TestAngle()
    {
        Vec3 from = new Vec3(1f, 0f, 0f);
        Vec3 to = new Vec3(0f, 1f, 0f);

        float angle = Vec3.Angle(from, to);
        float angle2 = Vector3.Angle(from, to);

        Debug.Log("El ángulo entre los vectores de mi propio Angle() es: " + angle);
        Debug.Log("El ángulo entre los vectores del Angle() en Unity es: " + angle2);
    }

    private void TestClampMagnitude()
    {
        Vec3 vector = new Vec3(3f, 4f, 0f);

        Vec3 clampedVector = Vec3.ClampMagnitude(vector, 2f);
        Vector3 clampedVector2 = Vector3.ClampMagnitude(vector, 2f);

        Debug.Log("Vector original: " + vector);
        Debug.Log("Vector limitado propio: " + clampedVector);
        Debug.Log("Vector limitado unity: " + clampedVector2);
    }

    private void TestMagnitude()
    {
        Vec3 vector = new Vec3(3f, 4f, 0f);

        float magnitude = Vec3.Magnitude(vector);
        float magnitude2 = Vector3.Magnitude(vector);

        Debug.Log("Vector original: " + vector);
        Debug.Log("Magnitud propio: " + magnitude);
        Debug.Log("Magnitud en unity: " + magnitude2);
    }

    private void TestCross()
    {

    }
}
