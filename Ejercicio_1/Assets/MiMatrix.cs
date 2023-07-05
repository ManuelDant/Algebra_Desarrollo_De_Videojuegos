using System;
using UnityEngine;

namespace CustomMath
{
    public struct MiMatrix4x4
    {
        //Elementos individuales de una matriz4x4, donde cada casilla es una posicion especifica, el primer 0 corresponde a la fila en donde se posiciona y el segundo 0 a la columna en donde se posiciona.
        public float m00;
        public float m10;
        public float m20;
        public float m30;
        public float m01;
        public float m11;
        public float m21;
        public float m31;
        public float m02;
        public float m12;
        public float m22;
        public float m32;
        public float m03;
        public float m13;
        public float m23;
        public float m33;

        private static MiMatrix4x4 zeroMatrix = new MiMatrix4x4(new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f), new Vector4(0f, 0f, 0f, 0f));

        //Las posiciones diagonales de los 1 es debido a que no cambia un vector cuando se lo aplica una transformacion. Cada fila representa un eje cartesiano, siendo que cada fila (X, Y , Z, origen/punto de referencia).
        //La presencia de un unico 1 en cada fila, garantiza que cuando esta matriz se multiplica por un vector tridimensional, no se introduciran cambios o transformaciones adicionales al vector.
        private static MiMatrix4x4 identityMatrix = new MiMatrix4x4(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 1f));

        public MiQuaternion rotation => GetRotation();
        public Vec3 lossyScale => GetLossyScale();
        public bool isIdentity => IsIdentity();
        public float determinant => GetDeterminant();
        public MiMatrix4x4 inverse => Inverse(inverse);
        public MiMatrix4x4 transpose => Transpose(transpose);

        public float this[int row, int column]
        {
            get
            {
                //Retorna el calculo del indice correspondiente al elemento de la matriz, se lo multiplica por 4 ya que es un matriz 4x4 siendo asi un arreglo unidimensional de 16 de longitud.
                return this[row + column * 4];
            }
            set
            {
                //Se realiza el mismo procedimiento que el get pero sirve para asignar un valor a la casilla seleccionada.
                this[row + column * 4] = value;
            }
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return m00;
                    case 1:
                        return m10;
                    case 2:
                        return m20;
                    case 3:
                        return m30;
                    case 4:
                        return m01;
                    case 5:
                        return m11;
                    case 6:
                        return m21;
                    case 7:
                        return m31;
                    case 8:
                        return m02;
                    case 9:
                        return m12;
                    case 10:
                        return m22;
                    case 11:
                        return m32;
                    case 12:
                        return m03;
                    case 13:
                        return m13;
                    case 14:
                        return m23;
                    case 15:
                        return m33;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        m00 = value;
                        break;
                    case 1:
                        m10 = value;
                        break;
                    case 2:
                        m20 = value;
                        break;
                    case 3:
                        m30 = value;
                        break;
                    case 4:
                        m01 = value;
                        break;
                    case 5:
                        m11 = value;
                        break;
                    case 6:
                        m21 = value;
                        break;
                    case 7:
                        m31 = value;
                        break;
                    case 8:
                        m02 = value;
                        break;
                    case 9:
                        m12 = value;
                        break;
                    case 10:
                        m22 = value;
                        break;
                    case 11:
                        m32 = value;
                        break;
                    case 12:
                        m03 = value;
                        break;
                    case 13:
                        m13 = value;
                        break;
                    case 14:
                        m23 = value;
                        break;
                    case 15:
                        m33 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        public static MiMatrix4x4 zero => zeroMatrix;
        public static MiMatrix4x4 identity
        {
            get
            {
                return identityMatrix;
            }
        }

        //Devuelve un Quaternion que representa la rotacion por un matriz.
        private MiQuaternion GetRotation()
        {
            //Se crean dos vectores que representan la dirreccion hacia adelante y hacia arriba en el sistema de coordenadas definido por la matriz.
            //Se los normaliza para que su longitud se mantenga en 1.
            Vec3 forward = new Vec3(m02, m12, m22).normalized; //los elementos de la matriz representan a la tercera columna de la matriz, que representa la dirreccion de avance.
            Vec3 upwards = new Vec3(m01, m11, m21).normalized; //los elementos de la matriz representan a la segunda columna de la matriz, que representa la dirreccion hacia arriba.

            //Luego se realiza la creacion del Quaternion utilizando la funcion LookRotation para buscar la rotacion de ambos vectores.
            return MiQuaternion.LookRotation(forward, upwards);
        }

        //Sirve para obtener la escala de transformacion de una matriz.
        private Vector3 GetLossyScale()
        {
            //Crea tres vectores utilizando cada eje de la matriz, el primer vector representa el eje x, el segundo vector el eje y, el tercer vector el z.
            //Se los normaliza para calcular la magnitud o longitud del vector que es lo que representa la escala en cada dimension.
            Vector3 scale = new Vector3(
                new Vector4(m00, m10, m20, m30).magnitude,
                new Vector4(m01, m11, m21, m31).magnitude,
                new Vector4(m02, m12, m22, m32).magnitude
            );
            return scale;
        }

        private bool IsIdentity()
        {
            //Crea la matriz identity
            return m00 == 1f && m10 == 0f && m20 == 0f && m30 == 0f &&
                   m01 == 0f && m11 == 1f && m21 == 0f && m31 == 0f &&
                   m02 == 0f && m12 == 0f && m22 == 1f && m32 == 0f &&
                   m03 == 0f && m13 == 0f && m23 == 0f && m33 == 1f;
        }

        //Devuelve el determinante de la matriz, el determinante es un valor escalar que proporciona informacion sobre las propiedas lineales y geometricas de la matriz.
        private float GetDeterminant()
        {
            //Se multiplican y restan los elementos de la matriz segun la formula del determinante.
            //Se agrupan los terminos de acuerdo con su posicion en la matriz y se realizan las operaciones correspondientes.
            //Los terminos se multiplican por los coeficientes correspondientes y se suman o restan segun la formula del determinante.
            float det =
                m00 * (m11 * (m22 * m33 - m23 * m32) - m21 * (m12 * m33 - m13 * m32) + m31 * (m12 * m23 - m13 * m22)) -
                m10 * (m01 * (m22 * m33 - m23 * m32) - m21 * (m02 * m33 - m03 * m32) + m31 * (m02 * m23 - m03 * m22)) +
                m20 * (m01 * (m12 * m33 - m13 * m32) - m11 * (m02 * m33 - m03 * m32) + m31 * (m02 * m13 - m03 * m12)) -
                m30 * (m01 * (m12 * m23 - m13 * m22) - m11 * (m02 * m23 - m03 * m22) + m21 * (m02 * m13 - m03 * m12));

            return det;
        }

        public static float Determinant(MiMatrix4x4 m)
        {
            return m.determinant;
        }

        //Se utiliza para construir una matriz 4x4 a partir de una posicion, una rotacion y una escala. La matriz resultante representa una transformación compuesta que realiza una traslación, una rotacion y una escala en un objeto.
        public static MiMatrix4x4 TRS(Vec3 pos, MiQuaternion q, Vec3 s)
        {
            MiMatrix4x4 matrix = identity;

            // Se les asignas los ejes utilizando la cuarta columna de la matriz, ya que estos componentes son los encargados de desplazar los puntos o vectores x, y, z.
            matrix.m03 = pos.x;
            matrix.m13 = pos.y;
            matrix.m23 = pos.z;

            // Se les asigna los datos del Quaternion para realizar la rotacion.
            float x = q.x;
            float y = q.y;
            float z = q.z;
            float w = q.w;

            //Se crean version de los ejes duplicados para evitar realizar muchos calculos multiplicando por 2.
            float x2 = x + x;
            float y2 = y + y;
            float z2 = z + z;

            //Estas variables se calculan mediante la multiplicacion de los ejes mismos y con los valores duplicados, estos en especifico se usan para construir los elementos de la matriz de rotacion.
            float xx = x * x2;
            float xy = x * y2;
            float xz = x * z2;
            float yy = y * y2;
            float yz = y * z2;
            float zz = z * z2;

            //Estas variables se calculan mediante la multiplicacion de la w del Quaternion por los valores duplicados de los ejes, tambien se utiliza para la matriz de rotacion.
            float wx = w * x2;
            float wy = w * y2;
            float wz = w * z2;

            //La rotacion utiliza los componentes de las tres primeras filas y las tres primeras columnas, estos mismos son los que estan relacionados con las transformaciones de rotacion en los ejes x, y, z.
            //Se realiza una resta de 1.0f a la suma de ciertos productos de las componentes del quaternion. Esto se hace para asegurar que los elementos de la matriz de rotacion cumplan con la propiedad de ortogonalidad, es decir,
            //que las filas (o columnas) de la matriz sean vectores unitarios y sean mutuamente perpendiculares
            matrix.m00 = 1.0f - (yy + zz);
            matrix.m01 = xy + wz;
            matrix.m02 = xz - wy;

            matrix.m10 = xy - wz;
            matrix.m11 = 1.0f - (xx + zz);
            matrix.m12 = yz + wx;

            matrix.m20 = xz + wy;
            matrix.m21 = yz - wx;
            matrix.m22 = 1.0f - (xx + yy);

            // Se aplica la escala usando los componentes diagonales de la matriz, hace que expandan o contraigan los vectores en funcion a la informacion proporcionada.
            matrix.m00 *= s.x;
            matrix.m11 *= s.y;
            matrix.m22 *= s.z;

            return matrix;
        }

        public void SetTRS(Vec3 pos, MiQuaternion q, Vec3 s)
        {
            TRS(pos, q, s);
        }

        public bool ValidTRS()
        {
            //Si el determinante es igual a cero, significa que la matriz no es invertible, lo que indica una transformacion no valida.
            return GetDeterminant() != 0f;
        }

        //Calcula el inverso de una matriz dada.
        //La matriz inversa de una matriz se utiliza para deshacer una transformacion aplicada por esa matriz. La matriz inversa, cuando se multiplica por la matriz original, produce la matriz de identidad.
        public static MiMatrix4x4 Inverse(MiMatrix4x4 m)
        {
            //Buscamos el determinante para comprobar que la matriz a calcular si se pueda invertir y tambien para obtener los valores de los cofactores necesarios para calcular el inverso de la matriz.
            float det = m.GetDeterminant();

            if (!m.ValidTRS())
            {
                //Si no es valido se lanzara un error debido a que no se puede invertir.
                throw new InvalidOperationException("Matrix is not invertible.");
            }

            MiMatrix4x4 inverseMatrix = new MiMatrix4x4();

            //Se calculan los cofactores de la matriz original para cada elemento de la matriz inversa. Los cofactores se calculan utilizando una combinacion de multiplicaciones, sumas y restas de elementos de la matriz original.
            //Los cofactores son valores que se obtienen al combinar el determinante de submatrices de una matriz

            //Para calcular los cofactores de una matriz, se toma el determinante de cada submatriz, que es la matriz resultante despues de eliminar una fila y una columna especificas de la matriz original.
            //Los cofactores se obtienen aplicando una alternancia de signo positivo y negativo a los determinantes de las submatrices.

            float cof00 = m.m11 * (m.m22 * m.m33 - m.m23 * m.m32) - m.m21 * (m.m12 * m.m33 - m.m13 * m.m32) + m.m31 * (m.m12 * m.m23 - m.m13 * m.m22);
            float cof01 = -(m.m10 * (m.m22 * m.m33 - m.m23 * m.m32) - m.m20 * (m.m12 * m.m33 - m.m13 * m.m32) + m.m30 * (m.m12 * m.m23 - m.m13 * m.m22));
            float cof02 = m.m10 * (m.m21 * m.m33 - m.m23 * m.m31) - m.m20 * (m.m11 * m.m33 - m.m13 * m.m31) + m.m30 * (m.m11 * m.m23 - m.m13 * m.m21);
            float cof03 = -(m.m10 * (m.m21 * m.m32 - m.m22 * m.m31) - m.m20 * (m.m11 * m.m32 - m.m12 * m.m31) + m.m30 * (m.m11 * m.m22 - m.m12 * m.m21));

            float cof10 = -(m.m01 * (m.m22 * m.m33 - m.m23 * m.m32) - m.m21 * (m.m02 * m.m33 - m.m03 * m.m32) + m.m31 * (m.m02 * m.m23 - m.m03 * m.m22));
            float cof11 = m.m00 * (m.m22 * m.m33 - m.m23 * m.m32) - m.m20 * (m.m02 * m.m33 - m.m03 * m.m32) + m.m30 * (m.m02 * m.m23 - m.m03 * m.m22);
            float cof12 = -(m.m00 * (m.m21 * m.m33 - m.m23 * m.m31) - m.m20 * (m.m01 * m.m33 - m.m03 * m.m31) + m.m30 * (m.m01 * m.m23 - m.m03 * m.m21));
            float cof13 = m.m00 * (m.m21 * m.m32 - m.m22 * m.m31) - m.m20 * (m.m01 * m.m32 - m.m02 * m.m31) + m.m30 * (m.m01 * m.m22 - m.m02 * m.m21);

            float cof20 = m.m01 * (m.m12 * m.m33 - m.m13 * m.m32) - m.m11 * (m.m02 * m.m33 - m.m03 * m.m32) + m.m31 * (m.m02 * m.m13 - m.m03 * m.m12);
            float cof21 = -(m.m00 * (m.m12 * m.m33 - m.m13 * m.m32) - m.m10 * (m.m02 * m.m33 - m.m03 * m.m32) + m.m30 * (m.m02 * m.m13 - m.m03 * m.m12));
            float cof22 = m.m00 * (m.m11 * m.m33 - m.m13 * m.m31) - m.m10 * (m.m01 * m.m33 - m.m03 * m.m31) + m.m30 * (m.m01 * m.m13 - m.m03 * m.m11);
            float cof23 = -(m.m00 * (m.m11 * m.m32 - m.m12 * m.m31) - m.m10 * (m.m01 * m.m32 - m.m02 * m.m31) + m.m30 * (m.m01 * m.m12 - m.m02 * m.m11));

            float cof30 = -(m.m01 * (m.m12 * m.m23 - m.m13 * m.m22) - m.m11 * (m.m02 * m.m23 - m.m03 * m.m22) + m.m21 * (m.m02 * m.m13 - m.m03 * m.m12));
            float cof31 = m.m00 * (m.m12 * m.m23 - m.m13 * m.m22) - m.m10 * (m.m02 * m.m23 - m.m03 * m.m22) + m.m20 * (m.m02 * m.m13 - m.m03 * m.m12);
            float cof32 = -(m.m00 * (m.m11 * m.m23 - m.m13 * m.m21) - m.m10 * (m.m01 * m.m23 - m.m03 * m.m21) + m.m20 * (m.m01 * m.m13 - m.m03 * m.m11));
            float cof33 = m.m00 * (m.m11 * m.m22 - m.m12 * m.m21) - m.m10 * (m.m01 * m.m22 - m.m02 * m.m21) + m.m20 * (m.m01 * m.m12 - m.m02 * m.m11);

            //Luego se divide cada cofactor por el determinante para obtener los elementos de la matriz inversa.
            inverseMatrix.m00 = cof00 / det;
            inverseMatrix.m01 = cof10 / det;
            inverseMatrix.m02 = cof20 / det;
            inverseMatrix.m03 = cof30 / det;
            inverseMatrix.m10 = cof01 / det;
            inverseMatrix.m11 = cof11 / det;
            inverseMatrix.m12 = cof21 / det;
            inverseMatrix.m13 = cof31 / det;
            inverseMatrix.m20 = cof02 / det;
            inverseMatrix.m21 = cof12 / det;
            inverseMatrix.m22 = cof22 / det;
            inverseMatrix.m23 = cof32 / det;
            inverseMatrix.m30 = cof03 / det;
            inverseMatrix.m31 = cof13 / det;
            inverseMatrix.m32 = cof23 / det;
            inverseMatrix.m33 = cof33 / det;

            return inverseMatrix;
        }

        //Toma una matriz y la devuelve transpuesta de la matriz original.
        //Transposar se refiere a invertir las columnas y filas de una matriz.
        public static MiMatrix4x4 Transpose(MiMatrix4x4 m)
        {
            // Se crea la matriz para transponer la matriz dada.
            MiMatrix4x4 transposedMatrix = new MiMatrix4x4();

            //Luego, los elementos de la matriz creada para transposar se asignan de acuerdo a los elementos correspondientes de la matriz original, pero con las filas y columnas intercambiadas. 
            transposedMatrix.m00 = m.m00;
            transposedMatrix.m01 = m.m10;
            transposedMatrix.m02 = m.m20;
            transposedMatrix.m03 = m.m30;
            transposedMatrix.m10 = m.m01;
            transposedMatrix.m11 = m.m11;
            transposedMatrix.m12 = m.m21;
            transposedMatrix.m13 = m.m31;
            transposedMatrix.m20 = m.m02;
            transposedMatrix.m21 = m.m12;
            transposedMatrix.m22 = m.m22;
            transposedMatrix.m23 = m.m32;
            transposedMatrix.m30 = m.m03;
            transposedMatrix.m31 = m.m13;
            transposedMatrix.m32 = m.m23;
            transposedMatrix.m33 = m.m33;

            return transposedMatrix;
        }

        public static MiMatrix4x4 Scale(Vec3 vector)
        {
            //Realiza la escala de la matriz utilizando un vector, se les asigna a las casillas diagonales de la matriz ya que estas son encargadas de que se expandan o contraigan los vectores de la matriz, cada fila representa el eje cartesiano.
            //El resto de las casillas se establecen en 0, ya que la matriz de escala no tiene efecto en la translacion ni en la ultima fila.
            MiMatrix4x4 result = default(MiMatrix4x4);
            result.m00 = vector.x;
            result.m01 = 0f;
            result.m02 = 0f;
            result.m03 = 0f;
            result.m10 = 0f;
            result.m11 = vector.y;
            result.m12 = 0f;
            result.m13 = 0f;
            result.m20 = 0f;
            result.m21 = 0f;
            result.m22 = vector.z;
            result.m23 = 0f;
            result.m30 = 0f;
            result.m31 = 0f;
            result.m32 = 0f;
            result.m33 = 1f;
            return result;

            /*
            | sx  0   0   0 |
            | 0   sy  0   0 |
            | 0   0   sz  0 |
            | 0   0   0   1 |
            */
        }

        public static MiMatrix4x4 Translate(Vec3 vector)
        {
            //Las casillas usadas para el escala se igualan a 1 debido a que no se realizara ningun cambio en la escala.
            //Se utilizan las ultimas filas de la cuarta columna para realizar la translacion de la matriz utilizando el vector.
            //El resto de las casillas se establecen en 0, ya que la matriz de traslacion no tiene efecto en la escala ni en la ultima fila.
            MiMatrix4x4 result = default(MiMatrix4x4);
            result.m00 = 1f;
            result.m01 = 0f;
            result.m02 = 0f;
            result.m03 = vector.x;
            result.m10 = 0f;
            result.m11 = 1f;
            result.m12 = 0f;
            result.m13 = vector.y;
            result.m20 = 0f;
            result.m21 = 0f;
            result.m22 = 1f;
            result.m23 = vector.z;
            result.m30 = 0f;
            result.m31 = 0f;
            result.m32 = 0f;
            result.m33 = 1f;
            return result;

            /*
            | 1   0   0   tx |
            | 0   1   0   ty |
            | 0   0   1   tz |
            | 0   0   0   1  |
             */
        }

        public static MiMatrix4x4 Rotate(MiQuaternion q)
        {
            //Se utiliza el mismo metodo que en la funcion TRS para rotar una matriz a partir de un quaternion.

            //Se crean version de los ejes duplicados para evitar realizar muchos calculos multiplicando por 2.
            float num = q.x * 2f;
            float num2 = q.y * 2f;
            float num3 = q.z * 2f;

            //Estas variables se calculan mediante la multiplicacion de los ejes mismos y con los valores duplicados, estos en especifico se usan para construir los elementos de la matriz de rotacion.
            float num4 = q.x * num;
            float num5 = q.y * num2;
            float num6 = q.z * num3;
            float num7 = q.x * num2;
            float num8 = q.x * num3;
            float num9 = q.y * num3;

            //Estas variables se calculan mediante la multiplicacion de la w del Quaternion por los valores duplicados de los ejes, tambien se utiliza para la matriz de rotacion.
            float num10 = q.w * num;
            float num11 = q.w * num2;
            float num12 = q.w * num3;

            //Aqui se realizan los calculos correspondientes para obtener la rotacion de la matriz, los componentes 00 10 20 representan los efectos de rotacion de los 3 ejes x y z respectivamente.
            //las casillas en 0 son debido a que no hay ninguna traslacion en la matriz de rotacion.
            //en el ultimo componente (33) se mantiene el 1 para mantener la propiedad de identidad de la matriz.
            MiMatrix4x4 result = default(MiMatrix4x4);
            result.m00 = 1f - (num5 + num6);
            result.m10 = num7 + num12;
            result.m20 = num8 - num11;
            result.m30 = 0f;
            result.m01 = num7 - num12;
            result.m11 = 1f - (num4 + num6);
            result.m21 = num9 + num10;
            result.m31 = 0f;
            result.m02 = num8 + num11;
            result.m12 = num9 - num10;
            result.m22 = 1f - (num4 + num5);
            result.m32 = 0f;
            result.m03 = 0f;
            result.m13 = 0f;
            result.m23 = 0f;
            result.m33 = 1f;
            return result;
        }

        public Vector4 GetColumn(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector4(m00, m10, m20, m30);
                case 1:
                    return new Vector4(m01, m11, m21, m31);
                case 2:
                    return new Vector4(m02, m12, m22, m32);
                case 3:
                    return new Vector4(m03, m13, m23, m33);
                default:
                    throw new IndexOutOfRangeException("Invalid column index!");
            }
        }

        public Vector4 GetRow(int index)
        {
            switch (index)
            {
                case 0:
                    return new Vector4(m00, m01, m02, m03);
                case 1:
                    return new Vector4(m10, m11, m12, m13);
                case 2:
                    return new Vector4(m20, m21, m22, m23);
                case 3:
                    return new Vector4(m30, m31, m32, m33);
                default:
                    throw new IndexOutOfRangeException("Invalid row index!");
            }
        }

        public Vec3 GetPosition()
        {
            //Cada componente representa el eje de traslacion tanto de X como de Y y de Z. Todas se encuentran en la tercera columna
            return new Vec3(m03, m13, m23);
        }

        //Realiza una multiplicacion de una coordenada de punto con la matriz de transformacion. Devuelve un nuevo vector3 que representa la coordenada del punto transformado.
        public Vec3 MultiplyPoint(Vec3 point)
        {
            //Se realiza una multiplicacion por punto para realizar los calculos posteriores.
            Vec3 result = MultiplyPoint3x4(point);

            //Despues de realizar estos calculos, se realiza una normalizacion dividiendo cada componente del resultado por el componente homogeneo num. Esto asegura que el vector resultante este normalizado y tenga una longitud de 1.
            float num = m30 * point.x + m31 * point.y + m32 * point.z + m33;
            num = 1f / num;
            result.x *= num;
            result.y *= num;
            result.z *= num;
            return result;
        }

        public Vec3 MultiplyPoint3x4(Vec3 point)
        {
            //La matriz de transformacion tiene 3 filas y 4 columnas, lo que implica que no realiza una transformacion completa en el espacio tridimensional,
            //sino que se aplica una transformacion lineal utilizando las primeras 3 columnas de la matriz.

            //Se realiza una multiplicacion lineal por cada eje del punto en cada componente de las filas de la matriz, la primera columna representa la x, la segunda columna representa la y, la tercera representa la z.
            Vec3 result = default(Vec3);
            result.x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
            result.y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
            result.z = m20 * point.x + m21 * point.y + m22 * point.z + m23;
            return result;
        }

        public Vec3 MultiplyVector(Vec3 vector)
        {
            //Multiplica la matriz sin utilizar la cuarta fila utilizando un vector3. Realiza el mismo procedimiento que MultiplyVector pero sin sumas los componentes de la cuarta fila de la cuarta columna.
            Vec3 result = default(Vec3);
            result.x = m00 * vector.x + m01 * vector.y + m02 * vector.z;
            result.y = m10 * vector.x + m11 * vector.y + m12 * vector.z;
            result.z = m20 * vector.x + m21 * vector.y + m22 * vector.z;
            return result;
        }

        public void SetColumn(int index, Vector4 column)
        {
            this[0, index] = column.x;
            this[1, index] = column.y;
            this[2, index] = column.z;
            this[3, index] = column.w;
        }

        public void SetRow(int index, Vector4 row)
        {
            this[index, 0] = row.x;
            this[index, 1] = row.y;
            this[index, 2] = row.z;
            this[index, 3] = row.w;
        }


        public MiMatrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
        {
            m00 = column0.x;
            m01 = column1.x;
            m02 = column2.x;
            m03 = column3.x;
            m10 = column0.y;
            m11 = column1.y;
            m12 = column2.y;
            m13 = column3.y;
            m20 = column0.z;
            m21 = column1.z;
            m22 = column2.z;
            m23 = column3.z;
            m30 = column0.w;
            m31 = column1.w;
            m32 = column2.w;
            m33 = column3.w;
        }

        public static MiMatrix4x4 operator *(MiMatrix4x4 lhs, MiMatrix4x4 rhs)
        {
            MiMatrix4x4 result = default(MiMatrix4x4);
            result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
            result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
            result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
            result.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;
            result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
            result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
            result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
            result.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;
            result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
            result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
            result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
            result.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;
            result.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
            result.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
            result.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
            result.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;
            return result;
        }

        public static Vector4 operator *(MiMatrix4x4 lhs, Vector4 vector)
        {
            Vector4 result = default(Vector4);
            result.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z + lhs.m03 * vector.w;
            result.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z + lhs.m13 * vector.w;
            result.z = lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z + lhs.m23 * vector.w;
            result.w = lhs.m30 * vector.x + lhs.m31 * vector.y + lhs.m32 * vector.z + lhs.m33 * vector.w;
            return result;
        }

        public static bool operator ==(MiMatrix4x4 lhs, MiMatrix4x4 rhs)
        {
            return lhs.GetColumn(0) == rhs.GetColumn(0) && lhs.GetColumn(1) == rhs.GetColumn(1) && lhs.GetColumn(2) == rhs.GetColumn(2) && lhs.GetColumn(3) == rhs.GetColumn(3);
        }

        public static bool operator !=(MiMatrix4x4 lhs, MiMatrix4x4 rhs)
        {
            return !(lhs == rhs);
        }
    }
}