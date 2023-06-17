using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Le da a lo que sea que tenga este script el character controller//
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Referencias")]
    public Camera Playercamera;
    //esto sirve para asociar una variable de cámara al scrpt//

    [Header("General")]
    public float gravityscale = -9.8f;
    //variable que controla la gravedad//



    [Header("Salto")]
    public float jumpheight = 1.9f;
    //variable para controlar que tan alto salta el coso//



    [Header("Movimiento")]
    public float walkspeed = 5f;
    //variable para controlar la velocidad al caminar//

    public float runspeed = 10f;
    //variable para controlar la velocidad al correr//



    [Header("Rotación")]
    public float rotationsensibility = 10f;
    //variable para la sensibilidad de rotación de la cámara//



    private float cameraverticalangle;

    //esto hace que todas las variables de movimiento empiezen en 0//
    Vector3 moveInput = Vector3.zero;

    //esto hace que todas las variables de rotación de cámara empiezen en 0//
    Vector3 rotationInput = Vector3.zero;

    //esto asocia el scrpt con el charactercontroller//
    CharacterController charactercontroller;


    //cuando se inicia el script//
    private void Awake()
    {
        //le otorga al objeto este scrpt//
        charactercontroller = GetComponent<CharacterController>();
    }


    //esto pasa cada frame de juego//
    private void Update()
    {
        Look();
        Move();
    }

    //esto lee el input del jugador (cuando preciona una tecla)//
    private void Move()
    {

        //coso que hace que se pueda mover siempre y cuando esté en el suelo//
        if (charactercontroller.isGrounded)
        {

            //esto les da un valor a las variables de movimiento cuando se preciona//
            moveInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            //esto limita la velocidad a donde sea que vaya para que no cambie de magnitud//
            moveInput = Vector3.ClampMagnitude(moveInput, 1f);

            //hace que el coso se mueva más rápudo multiplicando el runspeed por el input cuando se preciona shift//
            if (Input.GetButton("Sprint"))
            {
                moveInput = transform.TransformDirection(moveInput) * runspeed;
            }

            else
            {
                //esto hace que se mueva el coso con velocidad normal//
                moveInput = transform.TransformDirection(moveInput) * walkspeed;
            }

            //coso que se activa si el jugador está presionando para saltar//
            if (Input.GetButtonDown("Jump"))
            {
                moveInput.y = Mathf.Sqrt(jumpheight * -2f * gravityscale);
            }
        }

        moveInput.y += gravityscale * Time.deltaTime;

        charactercontroller.Move(moveInput * Time.deltaTime);

    }

    private void Look()
    {

        //estos sirven para rotar la cámara a donde sea que se mueva el mouse//
        rotationInput.x = Input.GetAxis("Mouse X") * rotationsensibility * Time.deltaTime;
        rotationInput.y = Input.GetAxis("Mouse Y") * rotationsensibility * Time.deltaTime;

        //este sirve para rotar el personaje con le movimiento en x//
        transform.Rotate(Vector3.up * rotationInput.x);

        cameraverticalangle += rotationInput.y;
        cameraverticalangle = Mathf.Clamp(cameraverticalangle, -90, 90);
        Playercamera.transform.localRotation = Quaternion.Euler(-cameraverticalangle, 0, 0);
    }



}