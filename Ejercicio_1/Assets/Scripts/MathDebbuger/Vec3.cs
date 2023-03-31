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

        public float sqrMagnitude { get { return x * x + y * y + z * z; } } //devuelve el cuadrado de la magnitud del vector.

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
        public float magnitude { get { return Mathf.Sqrt(x * x + y * y + z * z); } } //Devuelve la magnitud del vector realizando raiz de los cuadrados de x y z del vector.
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
            //Devuelve un nuevo vector cuyas componentes son la diferencia de las componentes correspondientes de los dos vectores que se están sumando. 
        }

        public static Vec3 operator -(Vec3 leftV3, Vec3 rightV3)
        {
            return new Vec3(leftV3.x - rightV3.x, leftV3.y - rightV3.y, leftV3.z - rightV3.z);
            //El mismo procedimiento que el anterior operador pero en este caso se restan en ves de sumarse.
        }

        public static Vec3 operator -(Vec3 v3)
        {
            return new Vec3(-v3.x, -v3.y, -v3.z);
            //Devuelve un nuevo vector cuyas componentes son iguales a las del vector original multiplicadas por -1.
        }

        public static Vec3 operator *(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
            //Devuelve un nuevo vector cuyas componentes son iguales a las del vector original multiplicadas por el escalar dado.
        }
        public static Vec3 operator *(float scalar, Vec3 v3)
        {
            return new Vec3(v3.x * scalar, v3.y * scalar, v3.z * scalar);
            //Es el mismo procedimiento que el anterior operador ya que la multiplicacion no altera el producto.
        }
        public static Vec3 operator /(Vec3 v3, float scalar)
        {
            return new Vec3(v3.x / scalar, v3.y / scalar, v3.z / scalar);
            //Divide cada componente del vector por el valor de la escalada y crea un nuevo vector con los resultados.
        }

        public static implicit operator Vector3(Vec3 v3)
        {
            return new Vector3(v3.x, v3.y, v3.z);
            //Define una conversión implícita de un objeto Vec3 a un objeto Vector3.
        }

        public static implicit operator Vector2(Vec3 v2)
        {
            return new Vector2(v2.x, v2.y);
            //Se realiza lo mismo que el anterior operador pero para un Vector de 2 dimensiones.
        }
        #endregion
      
        #region Functions
        public override string ToString()
        {
            return "X = " + x.ToString() + "   Y = " + y.ToString() + "   Z = " + z.ToString();
        }
        public static float Angle(Vec3 from, Vec3 to)
        {
            //Normaliza ambos vectores para asegurarse de que tengan longitud 1, lo que los convierte en vectores unitarios.
            from.Normalize();
            to.Normalize();

            //Calculamos el angulo entre los vectores utilizando la funcion dot product
            float angle = Mathf.Acos(Mathf.Clamp(Vec3.Dot(from, to), -1f, 1f)) * Mathf.Rad2Deg;

            //Luego, utiliza la función de producto punto para calcular el coseno del ángulo entre los dos vectores normalizados.
            //El valor del coseno se restringe dentro del rango [-1, 1] utilizando la función Mathf.Clamp.
            //Luego, se utiliza la función Mathf.Acos para calcular el ángulo en radianes, que se convierte a grados multiplicando por Mathf.Rad2Deg.
            //Finalmente, el ángulo en grados se devuelve como un valor de punto flotante.
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
            Vec3 interpolated = a + (b - a) * t;
            return interpolated;
            //toma dos vectores a y b y una proporción t, y devuelve el vector interpolado que se encuentra en algún lugar entre a y b.
        }

        public static Vec3 LerpUnclamped(Vec3 a, Vec3 b, float t)
        {
            Vec3 interpolated = a * (1 - t) + b * t;
            return interpolated;
            //No limita la proporcion de 0 a 1 como lo hace la anterior funcion Lerp, por lo que se resta t para que se consigan resultados fuera del rango 0 a 1.
        }

        public static Vec3 Max(Vec3 a, Vec3 b)
        {
            float x = Mathf.Max(a.x, b.x);
            float y = Mathf.Max(a.y, b.y);
            float z = Mathf.Max(a.z, b.z);
            return new Vec3(x, y, z);
            //Extraemos las coordenadas x, y y z de los vectores a y b y encontramos el valor máximo para despues crear un nuevo vector.
        }

        public static Vec3 Min(Vec3 a, Vec3 b)
        {
            float x = Mathf.Min(a.x, b.x);
            float y = Mathf.Min(a.y, b.y);
            float z = Mathf.Min(a.z, b.z);
            return new Vec3(x, y, z);
            //Realizamos el mismo procedimiento que el Max pero haciendo con el minimo.
        }

        public static float SqrMagnitude(Vec3 vector)
        {
            float x = vector.x;
            float y = vector.y;
            float z = vector.z;
            return x * x + y * y + z * z;
            // Simplemente elevamos al cuadrado y sumamos los resultados para conseguir la magnitud al cuadrado.
        }

        public static Vec3 Project(Vec3 vector, Vec3 onNormal) 
        {
            float sqrMagnitude = SqrMagnitude(onNormal);
            if (sqrMagnitude < Mathf.Epsilon)
            {
                return Vec3.Zero;
            }
            else
            {
                float dotProduct = Dot(vector, onNormal);
                float projectionFactor = dotProduct / sqrMagnitude;
                return onNormal * projectionFactor;
            }
            /*
            Primero, calculamos la magnitud al cuadrado del vector de normalización utilizando la función SqrMagnitude.
            Si esta magnitud es menor que un valor muy pequeño (epsilon), entonces el vector de normalización es esencialmente cero y devolvemos el vector cero.
            De lo contrario, calculamos el producto punto de los dos vectores utilizando la función Dot.

            Luego, calculamos el factor de proyección dividiendo el producto punto por la magnitud al cuadrado del vector de normalización. 
            Finalmente, multiplicamos el vector de normalización por el factor de proyección y devolvemos el resultado como el vector proyectado.
             */
        }

        public static Vec3 Reflect(Vec3 inDirection, Vec3 inNormal) 
        {
            return inDirection - 2f * Dot(inDirection, inNormal) * inNormal;
            /*
            Para calcular el vector reflejado, primero calculamos el producto punto de inDirection y inNormal utilizando la función Dot.
            Luego, multiplicamos el resultado por 2 y el vector normalizado inNormal. Esto nos da la parte del vector que se reflejará. 
            Finalmente, restamos esta parte del vector de la dirección de entrada inDirection para obtener el vector reflejado.

            El resultado es un vector que apunta en la dirección opuesta a la dirección de entrada inDirection, reflejado por la superficie definida por el vector normalizado inNormal.
            */
        }

        public void Set(float newX, float newY, float newZ)
        {
            x = newX;
            y = newY;
            z = newZ;
            //Setea las posiciones de un vector de tres dimensiones.
        }

        public void Scale(Vec3 scale)
        {
            x *= scale.x;
            y *= scale.y;
            z *= scale.z;
            //Setea la escala de un vector en todas las dimensiones.
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