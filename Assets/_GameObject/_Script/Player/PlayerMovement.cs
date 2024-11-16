using UnityEngine;
// ReSharper disable All

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private Vector3 moveInput;
    private float moveSpeed;
    
    [Header("Rotation")]

    [Header("Jump")]
    [SerializeField] private bool jump;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool isJumpActive;
    private float jumpTimeElapsed;
    private float ySpeed;

    [Header("Grounded")] 
    [SerializeField] private bool isGrounded;
    
    //Components Ref
    private CharacterController characterController;
    
    //Script Ref
    private Player player;
    
    // Update is called once per frame
    void Update()
    {
        GetUserInput();

        Rotate();
        
        CheckForGround();
        Move();
        Jump();
        ApplyGravity();
    }

    #region SetUp

    public void SetUp(Player player)
    {
        this.player = player;

        characterController = GetComponent<CharacterController>();

        moveSpeed = Constants.PlayerWalkSpeed;
    }

    #endregion

    #region Input

    private void GetUserInput()
    {
        moveInput = player.UserInput.MoveInput;
        jump = player.UserInput.Jump;
    }

    #endregion

    #region Move

    private void Move()
    {
        moveDir = moveInput.x * transform.right + moveInput.y * transform.forward;
        moveDir *= moveSpeed;
        
        moveDir.y = ySpeed;

        characterController.Move(moveDir * Time.deltaTime);
    }

    #endregion

    #region Rotation

    private void Rotate()
    {
        Vector3 eulerAngle = transform.rotation.eulerAngles;
        eulerAngle.y += player.UserInput.MouseLookDelta.x * Constants.PlayerRotationXSen * Time.deltaTime;
        eulerAngle.y = Mathf.Repeat(eulerAngle.y, 360);

        Quaternion finalRot = Quaternion.Euler(eulerAngle);

        transform.rotation =
            Quaternion.Slerp(transform.rotation, finalRot, Time.deltaTime * Constants.PlayerRotationSpeed);
    }

    #endregion
    
    #region Jump

    private void Jump()
    {
        if (jump && !isJumping && isGrounded)
        {
            if (!isJumpActive)
            {
                jumpTimeElapsed = 0;
            }
            
            isJumping = true;
            isJumpActive = true;
        }

        if (isJumpActive)
        {
            jumpTimeElapsed += Time.deltaTime;

            if (jumpTimeElapsed >= Constants.PlayerJumpDuration)
            {
                ySpeed = 0;
                isJumpActive = false;
            }
        }

        if (isJumpActive && isGrounded)
        {
            ySpeed = Constants.PlayerJumpSpeed;
        }
    }

    #endregion

    #region Ground

    private void CheckForGround()
    {
        isGrounded = player.PlayerCollisionDetector.IsGrounded;
    }

    #endregion

    #region Gravity

    private void ApplyGravity()
    {
        if (isGrounded)
        {
            if (!isJumpActive)
            {
                ySpeed = 0;
                isJumping = false;
            }
        }
        else
        {
            ySpeed += Constants.gravityY * Time.deltaTime;
        }
    }

    #endregion
}
