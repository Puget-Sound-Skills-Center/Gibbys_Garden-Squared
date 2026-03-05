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

    [SerializeField] private Transform PlayerModel;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    public TextMeshProUGUI NotifText;

    // Teleport stuff?
    private CharacterController cc;

    // Status Effects and whatnot -------------
    public bool IsSprinting = false; // Used by player, checked by Milly
    public int Warns = 0; // Used by Milly, 3 strikes and you're out!
    public bool Chilled = false; // Used by Everest and Kelvin
    public bool InDetention = false; // Used by Milly
    public bool IsInOffice = true; // Used by Milly and Player, true by default as player spawns in the office
    public int DetentionTimer = 0;
    public float Detentionbuffer = 1f;

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
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (canMove)
        {
            IsSprinting = isRunning;
        } else
        {
            IsSprinting = false;
        }
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
}