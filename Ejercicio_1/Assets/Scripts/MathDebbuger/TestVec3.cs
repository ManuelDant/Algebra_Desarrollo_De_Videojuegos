using CustomMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVec3 : MonoBehaviour
{

    void Start()
    {
        TestAngle();

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

}
