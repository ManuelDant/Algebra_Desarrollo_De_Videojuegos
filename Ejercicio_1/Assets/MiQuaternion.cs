using System;
using System.ComponentModel;
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
        //Se crean con w = 1 para mantener su longitud en 1, es util ya que si se normaliza el quaternion su longitud/magnitud se mantiene igual , ademas es util para realizar operaciones de rotacion mas sencillamente.


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
                //el roll, pitch y yaw es la forma de representar los tres angulos de Euler que representa una rotacion en un sistema tridimensional.
                //Roll es el angulo de rotacion alrededor del eje X. Se refiere al movimiento de inclinacion lateral de un objeto.
                //Pitch es el angulo de rotacion alrededor del eje Y. Se refiere al movimiento de cabeceo hacia arriba o hacia abajo de un objeto.
                //Yaw es el angulo de rotacion alrededor del eje Z. Se refiere al movimiento de giro horizontal de un objeto.
                //Se calculan los angulos Euler a partir de los componentes del quaternion actual.

                //El roll devuelve el angulo en radianes entre -pi y pi.
                float roll = Mathf.Atan2(2f * (w * x + y * z), 1f - 2f * (x * x + y * y));
                //El pitch devuelve el angulo en radianes entre -pi/2 y pi/2.
                float pitch = Mathf.Asin(2f * (w * y - z * x));
                //El yaw realiza la misma formula que el roll cambiando la posicion de los componentes del quaternion.
                float yaw = Mathf.Atan2(2f * (w * z + x * y), 1f - 2f * (y * y + z * z));

                //se crea un vector3 donde se usan los 3 angulos creados a partir del quaternion.
                Vec3 angles = new Vec3(roll, pitch, yaw);
                return angles;
            }
            set
            {
                //Se establecen los angulos de Euler del quaternion en funcion de los valores proporcionados. Los angulos de Euler son convertidos de radianes a medio angulo utilizando la formula (value.y/x/z * 0.5f)
                //"cy" y "sy" representan el coseno y el seno de la mitad del angulo de "yaw"
                //"cp" y "sp" representan el coseno y el seno de la mitad del angulo de "pitch" 
                //"cr" y "sr" representan el coseno y el seno de la mitad del angulo de "roll"
                float cy = Mathf.Cos(value.y * 0.5f);
                float sy = Mathf.Sin(value.y * 0.5f);
                float cp = Mathf.Cos(value.x * 0.5f);
                float sp = Mathf.Sin(value.x * 0.5f);
                float cr = Mathf.Cos(value.z * 0.5f);
                float sr = Mathf.Sin(value.z * 0.5f);

                //Se utilizan funciones trigonometricas para calcular los valores de los componentes w, x, y y z del quaternion a partir de los angulos de Euler proporcionados.
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
            //Normalizamos los quaterniones para dividir sus componentes y obtener su longitud de 1 para convertirlo en quaternion unitario que sirve para que los calculos sean mas consistentes y correctos.
            a.Normalize();
            b.Normalize();

            // Calculamos el producto interno entre los cuaterniones normalizados
            float dot = Dot(a, b);
            //El calculo del producto interno entre los cuaterniones normalizados es una forma de medir la similitud entre ellos. Si los cuaterniones estan perfectamente alineados, el producto interno sera 1.
            //Si estan opuestos entre si, el producto interno sera -1. Valores intermedios indicaran un grado de alineacion en algun punto entre estos extremos.

            // Clampeamos para que el producto interno este dentro del rango valido para evitar errores con la funcion Acos que arroje resultados indefinidos.
            dot = Mathf.Clamp(dot, -1f, 1f);

            // Calcular el angulo entre los cuaterniones
            float angle = Mathf.Acos(2f * dot * dot - 1f) * Mathf.Rad2Deg;
            // Se utiliza la formula trigonometrica en relacion entre el producto interno y el angulo entre dos quaterniones.
            //En esta formula se usa el cuadrado del producto interno para obtener un numero entre -1 a 1 pero se lo multiplica por 2 para poder utilizar la formula del coseno duplicado y asi que devuelva un valor
            //Entre -2 y 2. luego se le resta 1 debido a la identidad trigonometrica (cos^2(x) + sin^2(x) = 1), lo cual sirve para que la funcion del arcoseno nos devuelva un rango valido de entre -1 a 1

            //Todo esto utilizando arcoseno para obtener el valor en radianes y luego multiplicarlo por Rad2Deg para obtener su valor en grados.

            return angle;
        }

        public static MiQuaternion AngleAxis(float angle, Vec3 axis)
        {
            // Convertir el angulo de grados a radianes ya que el mathf de unity trabaja usando radianes
            float radianAngle = angle * Mathf.Deg2Rad;

            // Calcular la mitad del angulo para el eje de rotacion ya que se evita la duplicidad de la rotacion al combinar varios quaterniones
            float halfAngle = radianAngle * 0.5f;

            // Calcular el seno y coseno de la mitad del angulo para obtener los componentes para el posterior calculo de la rotacion
            float sinHalfAngle = Mathf.Sin(halfAngle);
            float cosHalfAngle = Mathf.Cos(halfAngle);

            // Normalizar el eje de rotacion para que su longitud sea 1 y el resultado sea valido para la rotacion.
            axis.Normalize();

            // Calcular los componentes del cuaternion
            float x = axis.x * sinHalfAngle;
            float y = axis.y * sinHalfAngle;
            float z = axis.z * sinHalfAngle;
            float w = cosHalfAngle;

            //Aclarar que se esta utilizando la formula para obtener la rotacion de un quaternion con un vector (q = cos(theta/2) + (x * i + y * j + z * k) * sin(theta/2)) donde theta representa el angulo de rotacion.
            return new MiQuaternion(x, y, z, w);
        }

        public static MiQuaternion AxisAngle(Vec3 axis, float angle)
        {
           return AngleAxis(angle, axis);
        }

        public static float Dot(MiQuaternion a, MiQuaternion b)
        {
            // Calcula el producto escalar o producto interno entre dos cuaterniones a y b.
            // El producto interno de dos cuaterniones es una operacion matematica que devuelve un numero que representa la magnitud de la proyección de un cuaternion sobre el otro.
            return a.x * b.x + a.y * b.y + a.z * b.z + a.w * b.w;
        }

        public static MiQuaternion Normalize(MiQuaternion q)
        {
            //Se calcula la magnitud del quaternion realizando la raiz cuadrada del producto interno del mismo.
            float num = Mathf.Sqrt(Dot(q, q));

            //En caso de que sea un valor menor que epsilon devolvera un quaternion.identity. que representaria un quaternion sin ninguna rotacion.
            if (num < Mathf.Epsilon)
            {
                return identity;
            }

            //Luego se lo normaliza diviendo cada componente del cuaternion por su magnitud
            return new MiQuaternion(q.x / num, q.y / num, q.z / num, q.w / num);
        }

        public void Normalize()
        {
            this = Normalize(this);
        }

        //Se utiliza para construir un quaternion a partir de los angulos de Euler especificados en grados.
        // Los angulos de Euler son una forma comun de representar la rotacion en tres dimensiones mediante tres angulos: YAW PITCH ROLL
        public static MiQuaternion Euler(float x, float y, float z)
        {
            // Convertir los angulos de grados a radianes para trabajar con mathf.
            float radianX = x * Mathf.Deg2Rad;
            float radianY = y * Mathf.Deg2Rad;
            float radianZ = z * Mathf.Deg2Rad;

            // Se calculan los cosenos y senos de los angulos en radianes para obtener los componentes del quaternion que representara la rotacion.
            // Esto se debe a que los quaterniones estan basados en la funcion exponencial compleja y utilizan los senos y cosenos de los angulos para calcular las partes imaginarias y reales del quaternion.
            float sinX = Mathf.Sin(radianX);
            float cosX = Mathf.Cos(radianX);
            float sinY = Mathf.Sin(radianY);
            float cosY = Mathf.Cos(radianY);
            float sinZ = Mathf.Sin(radianZ);
            float cosZ = Mathf.Cos(radianZ);

            // Calcular los componentes del quaternion a partir de las formulas matematicas para la conversion de angulos de Euler a quaterniones, en este caso se utiliza la formula XYZ.
            float xComponent = sinX * cosY * cosZ + cosX * sinY * sinZ; //se calcula como el producto de los senos y cosenos de los angulos correspondientes.
            float yComponent = cosX * sinY * cosZ - sinX * cosY * sinZ; //se calcula como una combinación lineal de senos y cosenos de los angulos.
            float zComponent = cosX * cosY * sinZ - sinX * sinY * cosZ; //se calcula como otra combinacion lineal de senos y cosenos de los angulos.
            float wComponent = cosX * cosY * cosZ + sinX * sinY * sinZ; //se calcula como una combinacion lineal de senos y cosenos de los angulos.

            // Crear y devolver el cuaternion resultante
            return new MiQuaternion(xComponent, yComponent, zComponent, wComponent);
        }

        public static MiQuaternion Euler(Vec3 euler)
        {
            return Euler(euler.x, euler.y, euler.z);
        }

        public static MiQuaternion FromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            //Se normalizan para que su longitud sea 1
            fromDirection.Normalize();
            toDirection.Normalize();

            // Calcular el producto punto entre las direcciones para obtenerel coseno del angulo entre ellos
            float dot = Vec3.Dot(fromDirection, toDirection);

            // Calcular el angulo entre las direcciones que devuelve el arcoseno del valor dado y este al devolver en radianes, se lo multiplica en Rad2Deg para convertirlo a grados
            // Se clampea el resultado del producto punto para que arroje resultados definidos entre -1 y 1
            float angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;

            // Se realiza el producto cruz entre los vectores dados ya que es necesario para calcular el eje de rotacion del quaternion que se calcula encontrando un vector perpendicular entre esos dos vectores
            Vec3 cross = Vec3.Cross(fromDirection, toDirection);
            // Luego se lo normaliza para realizar el calculo del AxisAngle
            cross.Normalize();

            //Devuelve el quaternion de rotacion correspondiente.
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

        // Implementa la interpolacion lineal entre dos quaterniones usando un intervalo (t) que varia entre 0 y 1.
        public static MiQuaternion LerpUnclamped(MiQuaternion a, MiQuaternion b, float t)
        {
            //se calcula el coseno de medio angulo entre los dos quaterniones usando el producto interno, este valor representa la similitud entre los quaterniones y se determina si se invierten o no.
            float cosHalfTheta = Dot(a,b);

            //En caso de que sean menor a 0 quiere decir que estan opuestos los quaterniones y se invierten el quaternion b y se toma el opuesto del coseno de medio angulo para que sean positivos.
            if (cosHalfTheta < 0f)
            {
                b = Inverse(b);
                cosHalfTheta = -cosHalfTheta;
            }

            //Se calcula el absoluto de si son mayor o igual a 1 que represantaria que estos quaterniones son identicos o estan muy cercanos, en ese caso se devuelve el quaternion a
            if (Mathf.Abs(cosHalfTheta) >= 1f)
            {
                return a;
            }

            //En caso de que sea menor a 1, se calcula el seno de medio angulo para que el resultado se use para determinar los factores de interpolacion.
            //La formula para evitar que se realice la raiz cuadrada de un resultado negativo se le resta 1 ya que puede dar un resultado mayor a 1 entre cosHalfTheta * cosHalfTheta.
            //Y en ese caso la raiz cuadrada arrojara un resultado negativo lo que seria un error.
            float sinHalfTheta = Mathf.Sqrt(1f - cosHalfTheta * cosHalfTheta);

            //Si ese resultado es un valor muy pequeño quiere decir que los quaterniones son casi colineales y se realiza una interpolacion lineal simple entre los quaterniones.
            if (Mathf.Abs(sinHalfTheta) < 0.001f)
            {
                return new MiQuaternion(
                    a.x * (1f - t) + b.x * t,
                    a.y * (1f - t) + b.y * t,
                    a.z * (1f - t) + b.z * t,
                    a.w * (1f - t) + b.w * t
                );
            }
            else //Si es mayor o igual que el valor pequeño, se calcula en angulo de medio theta utilizando el arcoseno.
            {
                float halfTheta = Mathf.Acos(cosHalfTheta); //representa la mitad del angulo de rotacion entre los dos quaterniones.

                //Se calculan los factores de interpolacion, se realizan para determinar los pesos o proporciones en los que se deben combinar los cuaterniones a y b en la interpolacion.
                //Esto permite distinguir donde se encuentra la interpolacion entre los quaterniones, si t es 0 esta en el quaternion a, si es 1 esta en el quaternion b.
                float ratioA = Mathf.Sin((1f - t) * halfTheta) / sinHalfTheta; //se resta -1 para asegurar que el valor dentro de la raiz cuadrada sea no negativo y este en el rango valido.
                float ratioB = Mathf.Sin(t * halfTheta) / sinHalfTheta; //el valor t ya representa la proporcion de interpolacion entre a y b, por lo que no es necesario restar 1 - t en el segundo ratio.

                //Se devolvera un nuevo quaternion que combina linealmente los componentes de los dos quaterniones utilizando los factores de interpolacion calculados.
                return new MiQuaternion(
                    a.x * ratioA + b.x * ratioB,
                    a.y * ratioA + b.y * ratioB,
                    a.z * ratioA + b.z * ratioB,
                    a.w * ratioA + b.w * ratioB
                );
            }
        }

        //se utiliza para calcular un quaternion que representa una rotacion desde la direccion "forward" hacia una direccion deseada, manteniendo una direccion "upwards" específica en relacion con la rotacion resultante
        public static MiQuaternion LookRotation(Vec3 forward, [DefaultValue("Vector3.up")] Vec3 upwards)
        {
            // Normalizar las direcciones para que su longitud sea 1
            forward.Normalize();
            upwards.Normalize();

            // Se Calcula un vector ortogonal realizando el eje de rotacion utilizando el producto cruz entre el forward y upwards, luego se lo normaliza
            Vec3 right = Vec3.Cross(upwards, forward);
            right.Normalize();

            // Calcular el nuevo upwards utilizando el producto cruz entre el forward y right para que este sea perpendicular a estos dos vectores y luego se le normaliza
            upwards = Vec3.Cross(forward, right);
            upwards.Normalize();

            // Usando los tres vectores se crea una matriz para poder realizar los calculos necesarios para la rotacion tridimensional, esta matriz almacena la orientacion de un objeto en el espacio tridimensional.
            float m00 = right.x;
            float m01 = right.y;
            float m02 = right.z;
            float m10 = upwards.x;
            float m11 = upwards.y;
            float m12 = upwards.z;
            float m20 = forward.x;
            float m21 = forward.y;
            float m22 = forward.z;

            //Se calcula la traza de la matriz de rotacion sumando los componentes diagonales (00,11,22)
            float trace = m00 + m11 + m22;
            float w, x, y, z;

            //Se realiza una serie de calculos para determinar todos los componentes del quaternion usando la matriz de rotacion.
            //Si la traza es mayor a 0 quiere decir que la matriz de rotacion no contiene ninguna reflexion y se calculan el quaternion directamente utilizando los elementos de la matriz.
            if (trace > 0f)
            {
                // S se calcula para poder normalizar los componenes x y z del quaternion
                float s = Mathf.Sqrt(trace + 1f) * 2f; //se le suma 1 al a traza para garantizar que la raiz cuadrada arroje un resultado positivo o cero.
                float invS = 1f / s;

                //se realiza para garantizar que el quaternion resultante tenga una magnitud cercana a 1 cuando se normalice, manteniendo la consistencia entre todos los componentes y
                //preservando la interpretacion correcta del quaternion como una representacion de la rotacion, Esto aplica a cada serie de calculos.
                w = 0.25f * s;
                x = (m21 - m12) * invS;
                y = (m02 - m20) * invS;
                z = (m10 - m01) * invS;
            }
            else if (m00 > m11 && m00 > m22) //En los siguientes 3 casos se verifica si los diagonales de la matriz (00, 11,22) son los mayores, eso quiere decir que hay una reflexion presente en la matriz de rotacion,
                                             //el calculo es practicamente el mismo que el anterior pero modificandolo para que encaje con las matrices
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

        //Permite rotar gradualmente desde un quaternion inicial (from) hacia un quaternion objetivo (to).
        //La rotacion se realiza en incrementos controlados por el parametro maxDegreesDelta, que especifica la cantidad maxima de grados que se puede rotar en un solo paso.
        public static MiQuaternion RotateTowards(MiQuaternion from, MiQuaternion to, float maxDegreesDelta)
        {
            //Se realiza el angulo entre el from y to. 
            //Si el angulo es cero, significa que los quaterniones son iguales y no es necesario realizar ninguna rotacion adicional. En este caso, simplemente se devuelve el quaternion objetivo to.
            float num = Angle(from, to);
            if (num == 0f)
            {
                return to;
            }

            //Si el angulo es diferente de cero, se realiza una interpolacion esferica (Slerp) no restringida entre los quaterniones from y to utilizando la funcion SlerpUnclamped.
            //se aplica un factor de escala en la interpolacion para limitar la cantidad de rotacion realizada en un solo paso.
            //El factor de escala se calcula dividiendo maxDegreesDelta por el angulo entre los quaterniones (num) y tomando el minimo entre 1 y ese valor.
            //Esto asegura que la rotacion no exceda maxDegreesDelta y permite controlar la velocidad de la rotacion.
            return SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / num));
        }

        public static MiQuaternion Slerp(MiQuaternion a, MiQuaternion b, float t)
        {
            //Se realiza el producto punto entre los quaterniones que se utiliza para ajustar la orientacion de los quaterniones a y b antes de realizar la interpolacion.
            float dot = Dot(a, b);

            //Si el resultado del producto punto es negativo, se invierte el cuaternion b y se actualiza el valor de dot a su valor absoluto negativo.
            //Esto se hace para garantizar que los quaterniones a y b esten en la misma "mitad" de la esfera de interpolacion.
            if (dot < 0f)
            {
                b = Inverse(b);
                dot = -dot;
            }

            //Se verifica si el resultado del producto punto esta cerca de 1.0. Si es así, se considera que los quaterniones a y b son casi paralelos, y en lugar de realizar la interpolacion esferica,
            //se utiliza la interpolacion lineal entre los quaterniones a y b mediante la funcion Lerp(a, b, t). Esto se hace para evitar problemas numericos y obtener un resultado más eficiente en este caso especial.
            const float kThreshold = 0.9995f;
            if (dot > kThreshold)
            {
                return Lerp(a, b, t);
            }

            //Si el producto punto no esta cera de 1, se realiza la interpolacion esferica.
            //Se calculan el angulo, el seno del angulo y el seno del angulo invertido para posteriormente normalizarlos para los factores de ponderizacion.
            float angle = Mathf.Acos(dot);
            float sinAngle = Mathf.Sin(angle);
            float invSinAngle = 1f / sinAngle;

            //Se calculan los factores de ponderizacion de cada componente (a y b) que se usaran para crear el quaternion interpolado
            float ratioA = Mathf.Sin((1f - t) * angle) * invSinAngle;
            float ratioB = Mathf.Sin(t * angle) * invSinAngle;

            //Crea un nuevo quaternion usando los componentes interpolados ponderados.
            return new MiQuaternion(
                ratioA * a.x + ratioB * b.x,
                ratioA * a.y + ratioB * b.y,
                ratioA * a.z + ratioB * b.z,
                ratioA * a.w + ratioB * b.w
            );
        }

        public static MiQuaternion SlerpUnclamped(MiQuaternion a, MiQuaternion b, float t)
        {
            //Se realiza el producto punto entre los quaterniones que se utiliza para ajustar la orientacion de los quaterniones a y b antes de realizar la interpolacion.
            float dot = Dot(a, b);

            //Si el resultado del producto punto es negativo, se invierte el cuaternion b y se actualiza el valor de dot a su valor absoluto negativo.
            //Esto se hace para garantizar que los quaterniones a y b esten en la misma "mitad" de la esfera de interpolacion.
            if (dot < 0f)
            {
                b = Inverse(b);
                dot = -dot;
            }

            //Se calculan el angulo, el seno del angulo y el seno del angulo invertido para posteriormente normalizarlos para los factores de ponderizacion.
            float angle = Mathf.Acos(dot);
            float sinAngle = Mathf.Sin(angle);
            float invSinAngle = 1f / sinAngle;

            //Se calculan los factores de ponderizacion de cada componente (a y b) que se usaran para crear el quaternion interpolado
            float ratioA = Mathf.Sin((1f - t) * angle) * invSinAngle;
            float ratioB = Mathf.Sin(t * angle) * invSinAngle;

            //Crea un nuevo quaternion usando los componentes interpolados ponderados.
            return new MiQuaternion(
                ratioA * a.x + ratioB * b.x,
                ratioA * a.y + ratioB * b.y,
                ratioA * a.z + ratioB * b.z,
                ratioA * a.w + ratioB * b.w
            );

            //La interpolacion ponderada es un metodo de interpolacion en el cual se asignan pesos o ponderaciones a los valores que se estan interpolando.
            //Estos pesos determinan la influencia relativa de cada valor en el resultado final de la interpolacion.
            //En el contexto de los cuaterniones, la interpolacion ponderada se utiliza para calcular un nuevo quaternion que se encuentra en algun punto entre dos quaterniones iniciales.
            //Los pesos o ponderaciones se aplican a cada componente del cuaternion(x, y, z, w) de acuerdo con ciertas formulas o ecuaciones. (Se utiliza el mismo metodo para Lerp y LerpUnclamped).

        }


        public void Set(float newX, float newY, float newZ, float newW)
        {
            x = newX;
            y = newY;
            z = newZ;
            w = newW;
        }

        public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection)
        {
            FromToRotation(fromDirection, toDirection);
        }

        public void SetLookRotation(Vec3 view)
        {
            Vec3 up = Vector3.up;
            SetLookRotation(view, up);
        }

        public void SetLookRotation(Vec3 view, [DefaultValue("Vector3.up")] Vec3 up)
        {
            LookRotation(view, up);
        }

        public void ToAngleAxis(float angle, Vec3 axis)
        {
            AngleAxis(angle, axis);
        }

        public static MiQuaternion operator *(MiQuaternion lhs, MiQuaternion rhs)
        {
            //Se aplica la formula para poder multiplicar el quaternion con otro quaternion
            /*
            w = (q1.w * q2.w) - (q1.x * q2.x) - (q1.y * q2.y) - (q1.z * q2.z)
            x = (q1.w * q2.x) + (q1.x * q2.w) + (q1.y * q2.z) - (q1.z * q2.y)
            y = (q1.w * q2.y) - (q1.x * q2.z) + (q1.y * q2.w) + (q1.z * q2.x)
            z = (q1.w * q2.z) + (q1.x * q2.y) - (q1.y * q2.x) + (q1.z * q2.w)            
            */
            return new MiQuaternion(lhs.w * rhs.x + lhs.x * rhs.w + lhs.y * rhs.z - lhs.z * rhs.y, lhs.w * rhs.y + lhs.y * rhs.w + lhs.z * rhs.x - lhs.x * rhs.z, lhs.w * rhs.z + lhs.z * rhs.w + lhs.x * rhs.y - lhs.y * rhs.x, lhs.w * rhs.w - lhs.x * rhs.x - lhs.y * rhs.y - lhs.z * rhs.z);
        }

        public static Vec3 operator *(MiQuaternion rotation, Vec3 point)
        {
            //Se multiplican los componentes de rotation por dos
            float num = rotation.x * 2f;
            float num2 = rotation.y * 2f;
            float num3 = rotation.z * 2f;

            //Se calculan diversos productos cruzados y productos directos entre los componentes del quaternion y los componentes del vector (point).
            float num4 = rotation.x * num;
            float num5 = rotation.y * num2;
            float num6 = rotation.z * num3;
            float num7 = rotation.x * num2;
            float num8 = rotation.x * num3;
            float num9 = rotation.y * num3;
            float num10 = rotation.w * num;
            float num11 = rotation.w * num2;
            float num12 = rotation.w * num3;

            //Se utilizan los resultados de los calculos anteriores para calcular los nuevos valores de los componentes del vector resultante (result).
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

        public MiQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}
