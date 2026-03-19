using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 12f;
    public float runSpeed = 24f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    [SerializeField] private CanvasGroup DebugMenu;
    [SerializeField] private CanvasGroup PauseMenu;
    [SerializeField] private Transform PlayerModel;
    [SerializeField] private CanvasGroup ChilledManager;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;
    //private bool PressedShift = false;


    public bool SprintMoving = false;
    private bool StaminaLock = false;

    public TextMeshProUGUI NotifText;
    public TextMeshProUGUI StaminaBar;

    // Teleport stuff?
    private CharacterController cc;

    // Status Effects and whatnot -------------
    public bool isRunning = false; // Used by player, checked by Milly
    public int Stamina = 100;
    public float DrainBuffer = 0f; // How long it will take to decrease stamina by 1
    public float RegenBuffer = 0f;
    public float RegenIncrement = 0f;
    public int Warns = 0; // Used by Milly, 3 strikes and you're out!
    public bool Chilled = false; // Used by Everest and Kelvin
    public float ChillTimer = 0f;
    public bool InDetention = false; // Used by Milly
    public bool IsInOffice = true; // Used by Milly and Player, true by default as player spawns in the office
    public int DetentionTimer = 0;
    public float Detentionbuffer = 1f;
    public int SlowModifier = 0; // Chilled debuff reduces speed by 5 for both walk and run 
    public bool TutorialComplete = false; // Disables most of the npc mechanics until this is set to true

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }


    void Update()
    {
        // Debug stuff
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(DebugMenu.alpha == 0)
            {
                DebugMenu.alpha = .67f;
            } else
            {
                DebugMenu.alpha = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseMenu.alpha == 0)
            {
                // Game pauses
                Time.timeScale = 0f;
                PauseMenu.alpha = .8f;
                FreezePlayer();
            }
            else
            {
                // Game resumes
                PauseMenu.alpha = 0;
                Time.timeScale = 1f;
                ThawPlayer();
            }
        }



        // Set Walkspeed
        if (Chilled)
        {
            SlowModifier = 5;
        } else
        {
            SlowModifier = 0;
        }
        walkSpeed = 6f - SlowModifier;
        runSpeed = 12f - SlowModifier;


        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Sprint Mechanic 
        //bool isRunning = Input.GetKey(KeyCode.LeftShift);

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Checks if player is sprint moving
        if ((h != 0 || v != 0) && Input.GetKey(KeyCode.LeftShift) && Stamina > 0 && Chilled == false)
        {
            SprintMoving = true;
        } else
        {
            SprintMoving = false;
        }


        // Stamina regen buffs when not moving
        if ((h != 0 || v != 0))
        {
            RegenIncrement = .2f;
        }
        else
        {
            RegenIncrement = .03f;
        }


        // Stamina Drain
        if (SprintMoving && StaminaLock == false && Chilled == false)
        {
            isRunning = true;
            DrainBuffer += Time.deltaTime;
            if (DrainBuffer > .1f)
            {
                Stamina--;
                if (Stamina == 0)
                {
                    StaminaLock = true;
                    StaminaBar.color = new Color32(255, 133, 143, 255);
                }
                StaminaBar.text = $"Stamina: {Stamina}%";
                DrainBuffer = 0f;
            }
        } else
        {
            isRunning = false;
            RegenBuffer += Time.deltaTime;
            if (RegenBuffer > RegenIncrement)
            {
                Stamina++;
                if (Stamina > 100)
                {
                    Stamina = 100;
                } else if (Stamina == 25)
                {
                    StaminaLock = false;
                    StaminaBar.color = Color.white;
                }
                    StaminaBar.text = $"Stamina: {Stamina}%";
                RegenBuffer = 0f;
            }
        }

        // Chill Effect thingy
        if (ChillTimer > 0f)
        {
            ChillTimer -= Time.deltaTime;
            if (ChillTimer <= 0f)
            {
                Chilled = false;
            }
        }
        if (Chilled)
        {
            ChilledManager.alpha = 0.73f;
        } else
        {
            ChilledManager.alpha = 0;
        }

            // -------------------
            float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        // Debug Teleport
        if (Input.GetKeyDown(KeyCode.T))
        {
            SendToDetention();
        }

        // Detention Mechanic

        if (InDetention)
        {
            Detentionbuffer -= Time.deltaTime;
            if (Detentionbuffer < 0)
            {
                DetentionTimer--;
                NotifText.text = $"You're in detention for {DetentionTimer} seconds!";
                if (DetentionTimer == 0)
                {
                    InDetention = false;
                    NotifText.text = "";
                } else
                {
                    Detentionbuffer = 1f;
                }
            }
        }

    }

    public void FreezePlayer()
    {
        canMove = false;
    }

    public void ThawPlayer()
    {
        canMove = true;
    }

    public void SendToDetention()
    {
        //Debug.Log("Send player to office");
        bool wasEnabled = cc.enabled;
        cc.enabled = false;
        transform.position = new Vector3(0f, 1.08f, 0f);
        cc.enabled = true;
        // Post stuff
        DetentionTimer = 15;
        NotifText.text = $"You're in detention for {DetentionTimer} seconds!";
    }

    public void ChillPlayer()
    {
        Chilled = true;
        ChillTimer = 5f; // Freeze player for 5 seconds
    }
}