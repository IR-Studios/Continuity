using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (Input.GetKeyDown(KeyCode.Space) && !inJump)
        {
            Jump();
        }
        //CROUCH
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            crouch();
        }
        //SPRINT
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouch && !Sprinting)
        {
            MovementSpeed = Settings.MovementSpeed + 200.0f;

            Sprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && Sprinting)
        {

            MovementSpeed = Settings.MovementSpeed;
            Sprinting = false;
        }

        if (Sprinting && Cam.fieldOfView <= StartFOV + 10)
        {
            Cam.fieldOfView += 100 * Time.deltaTime;

        }
        else if (!Sprinting)
        {
            if (Cam.fieldOfView >= StartFOV)
            {
                Cam.fieldOfView -= 100 * Time.deltaTime;
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

        moveDir = (horizontalMovement * transform.right + verticalMovement * transform.forward).normalized;

        Vector3 yVelFix = new Vector3(0, rb.velocity.y, 0);
        rb.velocity = moveDir * MovementSpeed * Time.deltaTime;
        rb.velocity += yVelFix;
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
