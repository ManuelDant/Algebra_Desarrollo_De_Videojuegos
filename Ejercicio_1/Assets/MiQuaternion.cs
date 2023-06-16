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

        public static MiQuaternion Euler(float x, float y, float z)
        {
            // Convertir los ángulos de grados a radianes
            float radianX = x * Mathf.Deg2Rad;
            float radianY = y * Mathf.Deg2Rad;
            float radianZ = z * Mathf.Deg2Rad;

            // Calcular los senos y cosenos de los ángulos
            float sinX = Mathf.Sin(radianX);
            float cosX = Mathf.Cos(radianX);
            float sinY = Mathf.Sin(radianY);
            float cosY = Mathf.Cos(radianY);
            float sinZ = Mathf.Sin(radianZ);
            float cosZ = Mathf.Cos(radianZ);

            // Calcular los componentes del cuaternion
            float xComponent = sinX * cosY * cosZ + cosX * sinY * sinZ;
            float yComponent = cosX * sinY * cosZ - sinX * cosY * sinZ;
            float zComponent = cosX * cosY * sinZ - sinX * sinY * cosZ;
            float wComponent = cosX * cosY * cosZ + sinX * sinY * sinZ;

            // Crear y devolver el cuaternion resultante
            return new MiQuaternion(xComponent, yComponent, zComponent, wComponent);
        }

        public static MiQuaternion Euler(Vec3 euler)
        {
            return Euler(euler.x, euler.y, euler.z);
        }

        public static MiQuaternion FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            fromDirection.Normalize();
            toDirection.Normalize();

            // Calcular el producto punto entre las direcciones
            float dot = Vec3.Dot(fromDirection, toDirection);

            // Calcular el ángulo entre las direcciones
            float angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;

            // Calcular el eje de rotación perpendicular a las direcciones
            Vec3 cross = Vec3.Cross(fromDirection, toDirection);
            cross.Normalize();

            return AxisAngle(cross, angle);
        }

        public static MiQuaternion Inverse(MiQuaternion rotation)
        {
            float x = -rotation.x;
            float y = -rotation.y;
            float z = -rotation.z;
            float w = rotation.w;

            return new MiQuaternion(x, y, z, w);
        }

        public static MiQuaternion Lerp(MiQuaternion a, MiQuaternion b, float t)
        {
            t = Mathf.Clamp01(t);

            return LerpUnclamped(a, b, t);
        }

        public static MiQuaternion LerpUnclamped(MiQuaternion a, MiQuaternion b, float t)
        {
            float cosHalfTheta = a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;

            if (cosHalfTheta < 0f)
            {
                b = -b;
                cosHalfTheta = -cosHalfTheta;
            }

            if (Mathf.Abs(cosHalfTheta) >= 1f)
            {
                return a;
            }

            float sinHalfTheta = Mathf.Sqrt(1f - cosHalfTheta * cosHalfTheta);

            if (Mathf.Abs(sinHalfTheta) < 0.001f)
            {
                return new MiQuaternion(
                    a.x * (1f - t) + b.x * t,
                    a.y * (1f - t) + b.y * t,
                    a.z * (1f - t) + b.z * t,
                    a.w * (1f - t) + b.w * t
                );
            }
            else
            {
                float halfTheta = Mathf.Acos(cosHalfTheta);
                float ratioA = Mathf.Sin((1f - t) * halfTheta) / sinHalfTheta;
                float ratioB = Mathf.Sin(t * halfTheta) / sinHalfTheta;

                return new MiQuaternion(
                    a.x * ratioA + b.x * ratioB,
                    a.y * ratioA + b.y * ratioB,
                    a.z * ratioA + b.z * ratioB,
                    a.w * ratioA + b.w * ratioB
                );
            }
        }

        public static MiQuaternion LookRotation(Vec3 forward, [DefaultValue("Vector3.up")] Vec3 upwards)
        {
            // Normalizar las direcciones
            forward.Normalize();
            upwards.Normalize();

            // Calcular el eje de rotación utilizando el producto cruz entre el forward y upwards
            Vec3 right = Vec3.Cross(upwards, forward);
            right.Normalize();

            // Calcular el nuevo upwards utilizando el producto cruz entre el forward y right
            upwards = Vec3.Cross(forward, right);
            upwards.Normalize();

            // Calcular los componentes del cuaternion
            float m00 = right.x;
            float m01 = right.y;
            float m02 = right.z;
            float m10 = upwards.x;
            float m11 = upwards.y;
            float m12 = upwards.z;
            float m20 = forward.x;
            float m21 = forward.y;
            float m22 = forward.z;

            float trace = m00 + m11 + m22;
            float w, x, y, z;

            if (trace > 0f)
            {
                float s = Mathf.Sqrt(trace + 1f) * 2f;
                float invS = 1f / s;

                w = 0.25f * s;
                x = (m21 - m12) * invS;
                y = (m02 - m20) * invS;
                z = (m10 - m01) * invS;
            }
            else if (m00 > m11 && m00 > m22)
            {
                float s = Mathf.Sqrt(1f + m00 - m11 - m22) * 2f;
                float invS = 1f / s;

                w = (m21 - m12) * invS;
                x = 0.25f * s;
                y = (m01 + m10) * invS;
                z = (m02 + m20) * invS;
            }
            else if (m11 > m22)
            {
                float s = Mathf.Sqrt(1f + m11 - m00 - m22) * 2f;
                float invS = 1f / s;

                w = (m02 - m20) * invS;
                x = (m01 + m10) * invS;
                y = 0.25f * s;
                z = (m12 + m21) * invS;
            }
            else
            {
                float s = Mathf.Sqrt(1f + m22 - m00 - m11) * 2f;
                float invS = 1f / s;

                w = (m10 - m01) * invS;
                x = (m02 + m20) * invS;
                y = (m12 + m21) * invS;
                z = 0.25f * s;
            }

            // Crear y devolver el cuaternion resultante
            return new MiQuaternion(x, y, z, w);
        }

        public static MiQuaternion LookRotation(Vec3 forward)
        {
            return LookRotation(forward, Vector3.up);
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
