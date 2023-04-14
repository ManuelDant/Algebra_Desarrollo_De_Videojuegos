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

      
   }
}

