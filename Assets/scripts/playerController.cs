using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class playerController : NetworkBehaviour
{
    public float speed;
    public float runMultiplier;
    private bool isRunning = false;
    private float MAXSPEED;
    private float BASESPEED;
    public float rotateFactor = 500.0f;
    public float pitchFactor = 500.0f;

    private Transform eyeMount;
    public Transform boomBoomStick;
    private CharacterController characterController;
    // Use this for initialization
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        if (!isLocalPlayer ||(!isClient && !isServer))
        {
            //characterController.enabled = false;
        }

        eyeMount = transform.Find("EyeMount");
        boomBoomStick = transform.Find("BoomBoomStick");
        if (eyeMount == null)
        {
            Debug.LogError("Player GameObject error: No EyeMount child.");
        }
        if (!isLocalPlayer)
        {
            this.enabled = false;
        }
        speed = 10;
        BASESPEED = speed;
        MAXSPEED = speed * runMultiplier;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
            speed = BASESPEED;
        }
        if (isRunning)
        {
            speed *= runMultiplier;
            if (speed > MAXSPEED) speed = MAXSPEED;
        }

        //movement
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) moveDirection += transform.right;
        if (Input.GetKey(KeyCode.A)) moveDirection += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDirection += -transform.right;
        if (Input.GetKey(KeyCode.D)) moveDirection += -transform.forward;

        transform.Rotate(Vector3.up, rotateFactor * (Input.GetAxis("Mouse X") * Time.deltaTime));
        if (eyeMount != null)
        {
            eyeMount.Rotate(Vector3.right, -rotateFactor * (Input.GetAxis("Mouse Y") * Time.deltaTime));
            boomBoomStick.Rotate(Vector3.forward, rotateFactor * (Input.GetAxis("Mouse Y") * Time.deltaTime));
        }

        characterController.SimpleMove(moveDirection.normalized * speed);

    }
}
