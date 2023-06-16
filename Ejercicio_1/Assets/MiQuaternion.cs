using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CustomMath
{
    public struct MiQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        private static readonly MiQuaternion identityQuaternion = new MiQuaternion(0f, 0f, 0f, 1f);

        public const float kEpsilon = 1E-06f;

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    case 2:
                        return z;
                    case 3:
                        return w;
                    default:
                        throw new IndexOutOfRangeException("Invalid MiQuaternion index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        break;
                    case 1:
                        y = value;
                        break;
                    case 2:
                        z = value;
                        break;
                    case 3:
                        w = value;
                        break;
                }
            }
        }

        public static MiQuaternion identity
        {
            get
            {
                return identityQuaternion;
            }
        }

        public Vec3 eulerAngles
        {
            get
            {
                float roll = Mathf.Atan2(2f * (w * x + y * z), 1f - 2f * (x * x + y * y));
                float pitch = Mathf.Asin(2f * (w * y - z * x));
                float yaw = Mathf.Atan2(2f * (w * z + x * y), 1f - 2f * (y * y + z * z));

                Vec3 angles = new Vec3(roll, pitch, yaw);
                return angles;
            }
            set
            {
                float cy = Mathf.Cos(value.y * 0.5f);
                float sy = Mathf.Sin(value.y * 0.5f);
                float cp = Mathf.Cos(value.x * 0.5f);
                float sp = Mathf.Sin(value.x * 0.5f);
                float cr = Mathf.Cos(value.z * 0.5f);
                float sr = Mathf.Sin(value.z * 0.5f);

                w = cr * cp * cy + sr * sp * sy;
                x = sr * cp * cy - cr * sp * sy;
                y = cr * sp * cy + sr * cp * sy;
                z = cr * cp * sy - sr * sp * cy;
            }
        }

        public MiQuaternion normalized
        {
            get
            {
                return Normalize(this);
            }
        }

        public static float Angle(MiQuaternion a, MiQuaternion b)
        {
            // Calculamos la magnitud de los cuaterniones
            float magnitudeA = Mathf.Sqrt(a.x * a.x + a.y * a.y + a.z * a.z + a.w * a.w);
            float magnitudeB = Mathf.Sqrt(b.x * b.x + b.y * b.y + b.z * b.z + b.w * b.w);

            // Normalizamos los cuaterniones dividiendo la magnitud
            MiQuaternion normalizedA = new MiQuaternion(a.x / magnitudeA, a.y / magnitudeA, a.z / magnitudeA, a.w / magnitudeA);
            MiQuaternion normalizedB = new MiQuaternion(b.x / magnitudeB, b.y / magnitudeB, b.z / magnitudeB, b.w / magnitudeB);

            // Calculamos el producto interno entre los cuaterniones normalizados
            float dot = Dot(normalizedA, normalizedB);

            // Clampeamos para que el producto interno esté dentro del rango válido para evitar errores con la funcion Acos que arroje resultados indefinidos.
            dot = Mathf.Clamp(dot, -1f, 1f);

            // Calcular el ángulo entre los cuaterniones
            float angle = Mathf.Acos(2f * dot * dot - 1f) * Mathf.Rad2Deg;

            return angle;
        }

        public static MiQuaternion AngleAxis(float angle, Vec3 axis)
        {
            // Convertir el angulo de grados a radianes
            float radianAngle = angle * Mathf.Deg2Rad;

            // Calcular la mitad del angulo
            float halfAngle = radianAngle * 0.5f;

            // Calcular el seno y coseno de la mitad del ángulo
            float sinHalfAngle = Mathf.Sin(halfAngle);
            float cosHalfAngle = Mathf.Cos(halfAngle);

            // Normalizar el eje de rotación
            axis.Normalize();

            // Calcular los componentes del cuaternion
            float x = axis.x * sinHalfAngle;
            float y = axis.y * sinHalfAngle;
            float z = axis.z * sinHalfAngle;
            float w = cosHalfAngle;

            return new MiQuaternion(x, y, z, w);
        }

        public static MiQuaternion AxisAngle(Vec3 axis, float angle)
        {
           return AngleAxis(angle, axis);
        }

        public static float Dot(MiQuaternion a, MiQuaternion b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static MiQuaternion Normalize(MiQuaternion q)
        {
            float num = Mathf.Sqrt(Dot(q, q));
            if (num < Mathf.Epsilon)
            {
                return identity;
            }

            return new MiQuaternion(q.x / num, q.y / num, q.z / num, q.w / num);
        }


        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        public static MiQuaternion operator *(MiQuaternion lhs, MiQuaternion rhs)
        {
            return new MiQuaternion(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        public static Vec3 operator *(MiQuaternion rotation, Vec3 point)
        {
            float num = rotation.x * 2f;
            float num2 = rotation.y * 2f;
            float num3 = rotation.z * 2f;
            float num4 = rotation.x * num;
            float num5 = rotation.y * num2;
            float num6 = rotation.z * num3;
            float num7 = rotation.x * num2;
            float num8 = rotation.x * num3;
            float num9 = rotation.y * num3;
            float num10 = rotation.w * num;
            float num11 = rotation.w * num2;
            float num12 = rotation.w * num3;
            Vec3 result = default(Vec3);
            result.x = (1f - (num5 + num6)) * point.x + (num7 - num12) * point.y + (num8 + num11) * point.z;
            result.y = (num7 + num12) * point.x + (1f - (num4 + num6)) * point.y + (num9 - num10) * point.z;
            result.z = (num8 - num11) * point.x + (num9 + num10) * point.y + (1f - (num4 + num5)) * point.z;
            return result;
        }

        public static bool operator ==(MiQuaternion lhs, MiQuaternion rhs)
        {
            return lhs == rhs;
        }

        public static bool operator !=(MiQuaternion lhs, MiQuaternion rhs)
        {
            return !(lhs == rhs);
        }

        public static MiQuaternion operator -(MiQuaternion q)
        {
            return -q;
        }

        public MiQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}
