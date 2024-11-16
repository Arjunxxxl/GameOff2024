using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInput : MonoBehaviour
{
    [Header("Move Input")] 
    [SerializeField] private Vector3 moveInput;
    [SerializeField] private bool jump;

    [Header("Mouse")] 
    [SerializeField] private Vector2 mouseLookDelta;
    
    [Header("Camera")] 
    [SerializeField] private bool changeCamPivot;
    
    private InputAction moveAction;
    private InputAction jumpAction;
    
    private InputAction mouseLookAction;
    
    private InputAction camPivotChangeAction;

    public Vector3 MoveInput => moveInput;
    public bool Jump => jump;
    
    public Vector3 MouseLookDelta => mouseLookDelta;
    
    public bool ChangeCamPivot => changeCamPivot;

    #region Singleton

    public static UserInput Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    #endregion
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        
        mouseLookAction = InputSystem.actions.FindAction("Look");
        
        camPivotChangeAction = InputSystem.actions.FindAction("CamPivotChange");
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        jump = jumpAction.IsPressed();
        
        mouseLookDelta = mouseLookAction.ReadValue<Vector2>();
        
        changeCamPivot = camPivotChangeAction.IsPressed();
    }
}
