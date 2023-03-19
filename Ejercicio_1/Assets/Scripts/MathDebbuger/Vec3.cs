using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace CustomMath
{
    public struct Vec3 : IEquatable<Vec3>
    {
        #region Variables
        public float x;
        public float y;
        public float z;

        public float sqrMagnitude { get { throw new NotImplementedException(); } }

        public Vector3 normalized { 
            get 
            {
                float magnitude = Mathf.Sqrt(x * x + y * y + z * z);
                if (magnitude > epsilon)
                {
                    float invMagnitude = 1f / magnitude;
                    return new Vector3(x * invMagnitude, y * invMagnitude, z * invMagnitude);
                }
                else
                {
                    return Vector3.zero;
                }
            } 
        }
        public float magnitude { get { throw new NotImplementedException(); } }
        #endregion

        #region constants
        public const float epsilon = 1e-05f;
        #endregion

        #region Default Values
        public static Vec3 Zero { get { return new Vec3(0.0f, 0.0f, 0.0f); } }
        public static Vec3 One { get { return new Vec3(1.0f, 1.0f, 1.0f); } }
        public static Vec3 Forward { get { return new Vec3(0.0f, 0.0f, 1.0f); } }
        public static Vec3 Back { get { return new Vec3(0.0f, 0.0f, -1.0f); } }
        public static Vec3 Right { get { return new Vec3(1.0f, 0.0f, 0.0f); } }
        public static Vec3 Left { get { return new Vec3(-1.0f, 0.0f, 0.0f); } }
        public static Vec3 Up { get { return new Vec3(0.0f, 1.0f, 0.0f); } }
        public static Vec3 Down { get { return new Vec3(0.0f, -1.0f, 0.0f); } }
        public static Vec3 PositiveInfinity { get { return new Vec3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity); } }
        public static Vec3 NegativeInfinity { get { return new Vec3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity); } }
        #endregion                                                                                                                                                                               

        #region Constructors
        public Vec3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0.0f;
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vec3(Vec3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector3 v3)
        {
            this.x = v3.x;
            this.y = v3.y;
            this.z = v3.z;
        }

        public Vec3(Vector2 v2)
        {
            this.x = v2.x;
            this.y = v2.y;
            this.z = 0.0f;
        }
        #endregion

        #region Operators
        public static bool operator ==(Vec3 left, Vec3 right)
        {
            float diff_x = left.x - right.x;
            float diff_y = left.y - right.y;
            float diff_z = left.z - right.z;
            float sqrmag = diff_x * diff_x + diff_y * diff_y + diff_z * diff_z;
            return sqrmag < epsilon * epsilon;
        }
        public static bool operator !=(Vec3 left, Vec3 right)
        {
            return !(left == right);
        }

        public static Vec3 operator +(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x + rightV3.x, leftV3.y + rightV3.y, leftV3.z + rightV3.z);
        }

        public static Vec3 operator -(Vec3 leftV3, Vec3 rightV3)
        {
            throw new NotImplementedException();
        }

        public static Vec3 operator -(Vec3 v3)
        {
            throw new NotImplementedException();
        }

        public static Vec3 operator *(Vec3 v3, float scalar)
        {
            throw new NotImplementedException();
        }
        public static Vec3 operator *(float scalar, Vec3 v3)
        {
            throw new NotImplementedException();
        }
        public static Vec3 operator /(Vec3 v3, float scalar)
        {
            throw new NotImplementedException();
        }

        public static implicit operator Vector3(Vec3 v3)
        {
            return new Vector3(v3.x, v3.y, v3.z);
        }

        public static implicit operator Vector2(Vec3 v2)
        {
            throw new NotImplementedException();
        }
        #endregion
      
        #region Functions
        public override string ToString()
        {
            return "X = " + x.ToString() + "   Y = " + y.ToString() + "   Z = " + z.ToString();
        }
        public static float Angle(Vec3 from, Vec3 to)
        {
            //Normalizamos los vectores
            from.Normalize();
            to.Normalize();

            //Calculamos el angulo entre los vectores utilizando la funcion dot product
            float angle = Mathf.Acos(Mathf.Clamp(Vec3.Dot(from, to), -1f, 1f)) * Mathf.Rad2Deg;

            //Devolvemos el angulo
            return angle;
        }
        public static Vec3 ClampMagnitude(Vec3 vector, float maxLength)
        {
           
            float sqrMagnitude = vector.x * vector.x + vector.y * vector.y + vector.z * vector.z;
            if (sqrMagnitude > maxLength * maxLength)
            {
                float magnitude = Mathf.Sqrt(sqrMagnitude);
                float normalizedX = vector.x / magnitude;
                float normalizedY = vector.y / magnitude;
                float normalizedZ = vector.z / magnitude;
                return new Vec3(normalizedX * maxLength, normalizedY * maxLength, normalizedZ * maxLength);
            }
            return vector;
            /*
            calculando el cuadrado de la magnitud del vector dado. Luego, si el cuadrado de la magnitud es mayor que el cuadrado del valor máximo permitido,
            se normaliza el vector y se multiplica cada componente por el valor máximo para obtener un vector con la magnitud limitada. 
            Si la magnitud del vector dado es menor o igual que el valor máximo permitido, simplemente devuelve el vector sin cambios.
            */
        }
        public static float Magnitude(Vec3 vector)
        {
            float x = vector.x;
            float y = vector.y;
            float z = vector.z;
            float magnitude = (float)MathF.Sqrt(x * x + y * y + z * z);
            return magnitude;
            //calcula la magnitud del vector utilizando la fórmula de la distancia euclidiana en tres dimensiones.
            //la formula euclidiana se utiliza para calcular la distancia entre dos puntos que se puede realizar en un espacio tridimensional
            //su formula es la siguiente: d = sqrt((x2 - x1)^2 + (y2 - y1)^2 + (z2 - z1)^2). d = distancia | x1 y x2= coordenadas del primer y segundo punto.
        }
        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
            //se utiliza la fórmula matemática del producto cruz, que es un vector perpendicular a ambos vectores a y b.
            //La función simplemente crea un nuevo objeto Vec3 con las componentes resultantes del cálculo del producto cruz utilizando las componentes x, y y z de ambos vectores.
        }
        public static float Distance(Vec3 a, Vec3 b)
        {
            float distance = Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2) + Mathf.Pow(a.z - b.z, 2));
            return distance;
            //Se utiliza la misma formula euclidania para calcular la distancia entre los vectores. (Similar al teorema de pitagoras pero para tres dimensiones).

        }
        public static float Dot(Vec3 a, Vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
            //Simplemente se multiplica cada componente x, y y z del vector a con su componente correspondiente del vector b y luego los suma.
        }
        public static Vec3 Lerp(Vec3 a, Vec3 b, float t)
        {
            throw new NotImplementedException();
        }
        public static Vec3 LerpUnclamped(Vec3 a, Vec3 b, float t)
        {
            throw new NotImplementedException();
        }
        public static Vec3 Max(Vec3 a, Vec3 b)
        {
            throw new NotImplementedException();
        }
        public static Vec3 Min(Vec3 a, Vec3 b)
        {
            throw new NotImplementedException();
        }
        public static float SqrMagnitude(Vec3 vector)
        {
            throw new NotImplementedException();
        }
        public static Vec3 Project(Vec3 vector, Vec3 onNormal) 
        {
            throw new NotImplementedException();
        }
        public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal) 
        {
            throw new NotImplementedException();
        }
        public void Set(float newX, float newY, float newZ)
        {
            throw new NotImplementedException();
        }
        public void Scale(Vec3 scale)
        {
            throw new NotImplementedException();
        }
        public void Normalize()
        {
            float magnitude = Mathf.Sqrt(x * x + y * y + z * z);
            if (magnitude > epsilon)
            {
                float invMagnitude = 1f / magnitude;
                x *= invMagnitude;
                y *= invMagnitude;
                z *= invMagnitude;
            }
            else
            {
                x = 0f;
                y = 0f;
                z = 0f;
            }
            /*
            La función comienza calculando la longitud del vector utilizando la fórmula de la raíz cuadrada 
            de la suma de los cuadrados de las componentes x, y, y z del vector.
            Luego, verifica si la longitud es mayor que un pequeño valor epsilon para evitar la división por cero.
            Si la longitud es mayor que epsilon, se calcula el inverso de la longitud y se multiplica cada componente del vector por él para normalizarlo.
            Si la longitud es menor o igual que epsilon, el vector se establece en cero.
            */
        }
        #endregion

        #region Internals
        public override bool Equals(object other)
        {
            if (!(other is Vec3)) return false;
            return Equals((Vec3)other);
        }

        public bool Equals(Vec3 other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2) ^ (z.GetHashCode() >> 2);
        }
        #endregion
    }
}