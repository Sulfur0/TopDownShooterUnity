using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLenght = 100f;

    private void Awake()
    {
        floorMask = LayerMask.GetMask("Floor");
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();

    }
    private void FixedUpdate()
    {
        /*
         * Si usamos GetAxisRaw obtenemos 1 o 0 cuando nos movamos en esta dirección,
         * si usamos GetAxis obtenemos valores entre 0 y 1 dependiendo de cuanto tiempo
         * o cuanta intensidad apliquemos en el joystick.
         */
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        movement.Set(h, 0.0f, v);
        /*
         * Si el jugador se mueve diagonalmente, h=1 y v=1 entonces el vector 
         * resultante da 1.4 dandole ventaja al jugador al moverse en diagonal,
         * normalizo los vectores.
         */
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        //raycasting desde la camara hacia la posición del mouse
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //obtenemos la información del raycasting
        RaycastHit floorHit;
        /*
         * camRay = posiciones y direcciones para el vector raycast
         * out floorHit es una variable out que almacenará donde ha ocurrido el hit
         * camRayLenght la longitud del ray para colisionar con el plano
         * floorMask el plano donde interceptará el raycast cuando hagamos click, 
         * en el caso de este ejemplo es el plano invisible que creamos al principio
         */
        if(Physics.Raycast(camRay, out floorHit, camRayLenght, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0.0f;
            /*
             * para almacenar una rotación (rotator) utilizamos un quaternion
             * lo que esto hace, es que, como por defecto el eje Z es el que
             * apunta hacia el frente en las camaras y los modelos, ahora
             * apuntaremos hacia donde este playerToMouse (cuando nos movamos,
             * el character mirará hacia donde nos movemos)
             */
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }
    void Animating(float h, float v)
    {
        /*
         * Si nos movemos en horizontal o vertical esta caminando
         */
        bool walking = h != 0.0f || v != 0.0f ? true : false;
        //bool walking = h!=0.0f || v!=0.0f;
        anim.SetBool("IsWalking", walking);
    }
}
