using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerCollisionDetector playerCollisionDetector;

    private UserInput userInput;
    
    public UserInput UserInput => userInput;
    public PlayerCollisionDetector PlayerCollisionDetector => playerCollisionDetector;

    #region Singleton

    public static Player Instance;

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
        userInput = UserInput.Instance;

        playerMovement = GetComponent<PlayerMovement>();
        playerCollisionDetector = GetComponent<PlayerCollisionDetector>();
        
        playerMovement.SetUp(this);
    }
}
