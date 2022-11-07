using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playerscript : MonoBehaviour
{
    public CharacterController CC;
    public float Speed;

    //turning
    public float Sensitivity;
    public Transform CamTransform;
    private float camRotation = 0f;

    //jump
    public float VerticalSpeed;
    public float JumpSpeed;
    public float Gravity;

    public bool CanMove;

    //items
    private float Boost = 0f;

    public GameObject ArmCannon;

    //UI
    public GameObject Target;
    public GameObject Winscreen;
    public GameObject Losescreen;
    public GameObject BoostUI;
    public GameObject CannonUI;

    //health bar
    public Image Health;

    //lava state
    private bool InLava;

    void Start()
    {
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        //start movement
        CanMove = true;

        //items
        ArmCannon.SetActive(false);

        //UI
        Target.SetActive(false);
        Winscreen.SetActive(false);
        Losescreen.SetActive(false);
        BoostUI.SetActive(false);
        CannonUI.SetActive(false);

        //Not in lava
        InLava = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(CanMove == true)
        {
            // X/Z movement
            Vector3 movement = Vector3.zero;

            float forwardMovement = Input.GetAxis("Vertical") * Speed * Time.deltaTime;
            float sideMovement = Input.GetAxis("Horizontal") * Speed * Time.deltaTime;

            movement += (transform.forward * forwardMovement) + (transform.right * sideMovement);

            //Camera Rotation/sensitivity
            float mouseY = Input.GetAxis("Mouse Y") * Sensitivity;
            camRotation -= mouseY;
            camRotation = Mathf.Clamp(camRotation, -60f, 70f);
            CamTransform.localRotation = Quaternion.Euler(new Vector3(camRotation, 0f, 0f));

            float mouseX = Input.GetAxis("Mouse X") * Sensitivity;
            transform.rotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0f, mouseX, 0f));

            //Jump and Ground check
            if (CC.isGrounded)
            {
                VerticalSpeed = 0f;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    VerticalSpeed = JumpSpeed + Boost;
                }
            }

            VerticalSpeed += Gravity * Time.deltaTime;
            movement += transform.up * VerticalSpeed * Time.deltaTime;

            CC.Move(movement);
        }

        //lava state
        if (InLava == true)
        {
            Speed = 3;
            JumpSpeed = 6;
        }

        if (InLava == false)
        {
            Speed = 7;
            JumpSpeed = 12;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //collect Jump Boost
        Boostscript bs = hit.gameObject.GetComponent<Boostscript>();
        if (bs)
        {
            Destroy(hit.gameObject);

            Boost = 8f;

            BoostUI.SetActive(true);
        }

        //collect arm cannon
        Cannonscript ca = hit.gameObject.GetComponent<Cannonscript>();
        if (ca)
        {
            Destroy(hit.gameObject);

            ArmCannon.SetActive(true);
            Target.SetActive(true);

            CannonUI.SetActive(true);
        }

        //Lava State
        Lavascript la = hit.gameObject.GetComponent<Lavascript>();
        if (la)
        {
            Health.fillAmount -= Time.deltaTime * 1.8f;

            InLava = true;
        }
        else
        {
            Health.fillAmount += Time.deltaTime * 0.1f;

            InLava = false;
        }

        //Lose
        if (Health.fillAmount == 0f)
        {
            CanMove = false;

            Losescreen.SetActive(true);
        }

        //Victory
        Shipscript sh = hit.gameObject.GetComponent<Shipscript>();
        if (sh)
        {
            Winscreen.SetActive(true);
        }
        
    }
}
