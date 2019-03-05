using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //el objetivo que seguirá la camara
    public Transform target;
    //no queremos que la camara siga perfectamente al char, tendrá un suavizado.
    public float smoothing = 5f;

    //almacenaremos aqui la distancia desde la camara hasta el jugador
    Vector3 offset;

    private void Start()
    {
        offset = transform.position - target.position;
    }

    private void FixedUpdate()
    {
        //la camara se ubica en la posición del jugador + el offset
        Vector3 targetCamPos = target.position + offset;
        /*
         * movemos la camara con lerp, (suavemente)
         * transform.position, mover la camara desde la posición actual de la camara
         * hasta el targetCamPos
         * con la rapidez de smoothing
         * por Time.deltaTime, sino lo hara mas de 50 veces por segundo.
         */
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    }

}
