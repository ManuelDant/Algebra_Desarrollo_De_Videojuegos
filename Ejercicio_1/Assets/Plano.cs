using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

namespace CustomMath
{
   public struct MiPlano
   {
       private Vec3 _normal;
       private float _distance;

       public MiPlano flipped => new MiPlano(- _normal, 0f - _distance); //Invierte la normal y nega la distancia

       public MiPlano(Vec3 inNormal, Vec3 inPoint)
            {
            //se utiliza para crear un nuevo plano en 3D a partir de una normal en un punto en el espacio.
            //calcula la normalizada de la normal y la distancia al origen utilizando el producto.
            //se calcula la distancia utilizando el producto punto para obtener la magnitud de la proyección de "inPoint" sobre la normal.
            //siempre se normaliza la normal para que de una magnitud de 1 para que se pueda utilizar de manera adecuada en los calculos.
                _normal = inNormal.normalized;
                _distance = 0f - Vec3.Dot(_normal, inPoint);
            }

       public MiPlano(Vec3 inNormal, float d)
            {
            //tambien se utiliza para crear un plano 3D pero en vez de un punto te pide una distancia.
                _normal = inNormal.normalized;
                _distance = d;
            }

       public MiPlano(Vec3 a, Vec3 b, Vec3 c)
       {
           _normal = Vec3.Normalize(Vec3.Cross(b - a, c - a)); //se calcula la normal del plano utilizando la fórmula del producto cruz entre los vectores y luego los normaliza
            _distance = 0f - Vec3.Dot(_normal, a);
            //se calcula la distancia del plano al punto a mediante la fórmula de la distancia de un punto a un plano,
            //que es el producto punto entre la normal del plano y el punto a, y se invierte el resultado mediante la resta con cero.
            //Esto es necesario para que el signo de la distancia sea correcto, ya que la fórmula de la distancia de un punto a un plano normalmente devuelve un valor con un signo
            //dependiendo de qué lado del plano esté el punto.
        }

        public void SetNormalAndPosition(Vec3 inNormal, Vec3 inPoint)
       {
           _normal = inNormal.normalized; //Se normaliza la normal para asegurar que su longitud sea de 1 para saber que la direccion sea correcto.
            _distance = 0f - Vec3.Dot(inNormal, inPoint);
            // La distancia del plano al punto se calcula utilizando la fórmula de la distancia de un punto a un plano. El producto punto entre la normal
            // y el punto se multiplica por -1

            //La razón de multiplicar -1 es que la fórmula de la distancia devuelve un valor con signo, que indica de qué lado del plano se encuentra el punto.
            //Si el signo es negativo, entonces el punto está detrás del plano; si es positivo, está delante del plano. Para asegurarse de que el signo sea correcto,
            //se multiplica por -1.
       }

        public void Set3Points(Vec3 a, Vec3 b, Vec3 c)
        {
            //Setea un plano con 3 puntos, se utiliza la misma funcion que para crear un plano con 3 puntos.
            _normal = Vec3.Normalize(Vec3.Cross(b - a, c - a));
            _distance = 0f - Vec3.Dot(_normal, a);
        }

        public void Flip()
        {
            //voltea el plano negando el normal y su distancia.
            _normal = -_normal;
            _distance = - _distance;
        }

        public void Translate(Vec3 translation)
        {
            _distance += Vec3.Dot(_normal, translation);
            //se calcula el componente del vector de traslación que está en la dirección de la normal del plano utilizando el producto punto entre _normal y translation.
            //Se agrega este valor a _distance para actualizar la posición del plano.
        }

        public static MiPlano Translate(MiPlano plane, Vec3 translation)
        {
            return new MiPlano(plane._normal, plane._distance += Vec3.Dot(plane._normal, translation));
            //se calcula sumando el producto punto entre la normal del plano original y el vector de traslación al valor de distancia original del plano.
            //Esto se hace para actualizar la posición del plano en la dirección del vector de traslación.

        }

        public Vec3 ClosestPointOnPlane(Vec3 point)
        {
            float distance = Vec3.Dot(_normal, point) + _distance;
            return point - _normal * distance;
            //Para encontrar el punto más cercano, primero se calcula la distancia del punto al plano utilizando la fórmula de la ecuación del plano.
            //Se proyecta el vector desde el punto al plano, lo cual equivale a restar el producto punto entre la normal del plano y el punto a la normal multiplicado
            //por la distancia del plano al origen.
            //El punto proyectado se encuentra restando la normal del plano multiplicada por la distancia del punto point, dando asi el punto mas cercano.

        }

        public float GetDistanceToPoint(Vec3 point)
        {
            return Vec3.Dot(_normal, point) + _distance;
            //La fórmula utilizada para encontrar esta distancia se basa en la ecuación general de un plano, que es: Ax + By + Cz + D = 0
            //Para encontrar la distancia entre un punto P y el plano, podemos sustituir las coordenadas del punto en la ecuación del plano: A* Px +B * Py + C * Pz + D = d
            //Donde d es la distancia desde el punto P al plano. Si el punto P está por encima del plano, d será un valor positivo y si está por debajo del plano será negativo.

            //En esta función, se utiliza el producto punto entre el vector normal del plano y el punto para calcular la distancia.
            //Luego se agrega la distancia desde el plano al origen para obtener la distancia total desde el punto al plano.
        }

        public bool GetSide(Vec3 point)
        {
            //Utiliza la misma formula que GetDistanceToPoint()
            return Vec3.Dot(_normal, point) + _distance > 0f;
            //Para determinar en qué lado del plano se encuentra un punto, podemos evaluar la ecuación del plano usando las coordenadas del punto.
            //Si el resultado es mayor que cero, entonces el punto está en el lado del plano en el que apunta la normal; de lo contrario, está en el lado opuesto.
            //Si es mayor que 0 devuelve true y si es menor o igual a 0 se devuelve false que significa que el punto esta al lado opuesto donde apunta la normal.
        }

        public bool SameSide(Vec3 inPt0, Vec3 inPt1)
        {
            //comprueba si dos puntos dados se encuentran en el mismo lado del plano.
            float distanceToPoint = GetDistanceToPoint(inPt0);
            float distanceToPoint2 = GetDistanceToPoint(inPt1);
            return (distanceToPoint > 0f && distanceToPoint2 > 0f) || (distanceToPoint <= 0f && distanceToPoint2 <= 0f);
            //primero calcula la distancia de cada punto al plano mediante la función GetDistanceToPoint.
            //Si ambas distancias son positivas o negativas entonces los puntos se encuentran en el mismo lado del plano.
            //Por otro lado, si una distancia es positiva y la otra negativa, los puntos están en lados opuestos del plano.

            //Por lo tanto, la función devuelve true si los puntos están en el mismo lado del plano y false en caso contrario.
        }
    }
}

