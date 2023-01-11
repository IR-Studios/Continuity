using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Continuity.Keybinds;

namespace Continuity.Movement 
{



[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    Camera Cam;
    public bool canMove = true;
    float StartFOV;

    private Vector3 moveDir = Vector3.zero;
    private bool queueJump = false;
    [HideInInspector]
    public Vector3 moveAxis = Vector3.zero;
    [HideInInspector]
    public Vector2 lookAxis = Vector2.zero;
    [HideInInspector]
    public bool inJump = false;
    public bool isCrouch = false;
    public bool Sprinting = false;
    public bool isGrounded;
    public Vector3 jump;

    //Movement Variables
    [HideInInspector]
    public float MovementSpeed;

    /** Component accessors **/
    public PlayerSettings Settings
    {
        get { return player.settings; }
    }
    public Rigidbody rb
    {
        get { return GetComponent<Rigidbody>(); }
    }
    public CapsuleCollider cc
    {
        get { return GetComponent<CapsuleCollider>(); }
    }
    private Player player
    {
        get { return GetComponent<Player>(); }
    }

    private HeadBobController HBC 
    {
        get { return GetComponent<HeadBobController>(); }
    }

    public void Start()
    {

        MovementSpeed = Settings.MovementSpeed;
        jump = new Vector3(0.0f, 2.0f, 0.0f);
        Cam = GetComponentInChildren<Camera>();
        StartFOV = Cam.fieldOfView;
    }

    public void Update()
    {
        ChangeStamina();

        //JUMP
        if (Rebind.GetInput("Jump") && !inJump)
        {
            Jump();
        }
        //CROUCH
        if (Rebind.GetInputDown("Crouch"))
        {
            crouch();
        }
        //SPRINT
        if (Rebind.GetInputDown("Sprint") && !isCrouch && !Sprinting)
        {
            MovementSpeed = Settings.MovementSpeed + 200.0f;
            HBC._frequency = 30.0f;
            HBC._amplitude = 0.001f;

            Sprinting = true;
        }
        else if (Rebind.GetInputUp("Sprint") && Sprinting)
        {

            MovementSpeed = Settings.MovementSpeed;
            Sprinting = false;
            HBC._frequency = HBC._startingFrequency;
            HBC._amplitude = HBC._startingAmp;
        }

        if (Sprinting && Cam.fieldOfView <= StartFOV + 10)
        {
            Cam.fieldOfView += 50 * Time.deltaTime;

        }
        else if (!Sprinting)
        {
            if (Cam.fieldOfView >= StartFOV)
            {
                Cam.fieldOfView -= 50 * Time.deltaTime;
            }
        }
    }

    public void FixedUpdate()
    {
        if (!inJump && canMove)
        {
            move();
        }
    }

    public void move()
    {
        //TODO: Update to use custom Keybinds
        float horizontalMovement = Input.GetAxisRaw("Horizontal");
        float verticalMovement = Input.GetAxisRaw("Vertical");
        //moveDir = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;

           Vector3 yVelFix = new Vector3(0, rb.velocity.y, 0);
        

        #region Movement Test
        //Straight Movement
        if (Rebind.GetInput("MoveForward")) 
        {
            moveDir = (1 * transform.forward).normalized;
        }
        if (Rebind.GetInput("MoveBackward")) 
        {
            moveDir = (1 * -transform.forward).normalized;
        }
        if (Rebind.GetInput("MoveRight")) 
        {
            moveDir = (1 * transform.right).normalized;
        }
        if (Rebind.GetInput("MoveLeft")) 
        {
            moveDir = (1 * -transform.right).normalized;
        }

        //Diagonal Movement
        if (Rebind.GetInput("MoveForward") && Rebind.GetInput("MoveRight")) 
        {
             moveDir = ((1 * transform.forward) + (1 * transform.right)).normalized;
        }
        if (Rebind.GetInput("MoveForward") && Rebind.GetInput("MoveLeft")) 
        {
             moveDir = ((1 * transform.forward) + (1 * -transform.right)).normalized;
        }
        if (Rebind.GetInput("MoveBackward") && Rebind.GetInput("MoveRight")) 
        {
             moveDir = ((1 * -transform.forward) + (1 * transform.right)).normalized;
        }
        if (Rebind.GetInput("MoveBackward") && Rebind.GetInput("MoveLeft")) 
        {
             moveDir = ((1 * -transform.forward) + (1 * -transform.right)).normalized;
        }


        #endregion
        if (Rebind.GetInput("MoveForward") || Rebind.GetInput("MoveBackward") || Rebind.GetInput("MoveRight") || Rebind.GetInput("MoveLeft")) 
        {
         
            rb.velocity = moveDir * MovementSpeed * Time.deltaTime;
            rb.velocity += yVelFix;
        } else if (isGrounded) 
        {
            rb.velocity = new Vector3(0,rb.velocity.y,0);
        } 
      
    }
    private void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
        inJump = false;
    }
    public void Jump()
    {
        inJump = true;
        isGrounded = false;
        rb.AddForce(jump * Settings.SpeedInJump, ForceMode.Impulse);

    }

    public void crouch()
    {
        if (!isCrouch)
        {
            MovementSpeed = Settings.crouchSpeed;
            cc.height = cc.height / 2;
            isCrouch = true;
        }
        else if (isCrouch)
        {
            MovementSpeed = Settings.MovementSpeed;
            cc.height = cc.height * 2;
            isCrouch = false;
        }
    }

    public void ChangeStamina()
    {
        if (Sprinting)
        {
            player._stamina -= Settings.StaminaFallRate * Time.deltaTime;
        } 
            else if (!Sprinting)
        {
                if (player._stamina < Settings.MaxStamina)
                {
                    player._stamina += Settings.StaminaRegenRate * Time.deltaTime;
                }
        }
        player.HUD.UpdateStamina(Settings.MaxStamina, player._stamina);
    }

}
}
