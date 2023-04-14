using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

namespace CustomMath
{
   public struct MiPlano
   {
       const int size = 16;
       private Vec3 _normal;
       private float _distance;

       public MiPlano flipped => new MiPlano(- _normal, 0f - _distance); //Invierte la normal y nega la distancia

       public MiPlano(Vec3 inNormal, Vec3 inPoint)
            {
            //se utiliza para crear un nuevo plano en 3D a partir de una normal y un punto en el espacio.
            //calcula la normalizada de la normal y la distancia al origen utilizando el producto.
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
            // La distancia del plano al punto se calcula utilizando la fórmula de la distancia de un punto a un plano. El producto punto entre la normal y el punto se multiplica por -1

            //La razón de multiplicar -1 es que la fórmula de la distancia devuelve un valor con signo, que indica de qué lado del plano se encuentra el punto.
            //Si el signo es negativo, entonces el punto está detrás del plano; si es positivo, está delante del plano. Para asegurarse de que el signo sea correcto, se multiplica por -1.
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

    }
}

