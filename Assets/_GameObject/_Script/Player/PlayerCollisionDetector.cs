using UnityEngine;

public class PlayerCollisionDetector : MonoBehaviour
{
    [Header("Ground Detection")] 
    [SerializeField] private float groundSphereCastDist;
    [SerializeField] private float groundSphereCastRadius;
    [SerializeField] private LayerMask groundLayerMask;
    private RaycastHit[] groundHits;
    [SerializeField] private bool isGrounded_Phantom;
    [SerializeField] private bool isGrounded;

    [Header("Ground Phantom Time")]
    private float groundPhantomTimeElapsed;

    public bool IsGrounded => isGrounded;
    
    private void FixedUpdate()
    {
        DetectGround();
        GroundDetectionPhantom();
    }
    
    #region Ground

    private void DetectGround()
    {
        groundHits = Physics.SphereCastAll(transform.position, groundSphereCastRadius,
            Vector3.down, groundSphereCastDist, groundLayerMask);
        
        isGrounded_Phantom = groundHits.Length > 0;

        if (isGrounded_Phantom)
        {
            isGrounded = true;
        }
    }
    
    private void GroundDetectionPhantom()
    {
        if (!isGrounded_Phantom && isGrounded)
        {
            groundPhantomTimeElapsed += Time.deltaTime;
            if (groundPhantomTimeElapsed >= Constants.PlayerGroundedPhantomDuration)
            {
                groundPhantomTimeElapsed = 0;
                isGrounded = false;
            }
        }
    }

    #endregion
    
    #region Debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + Vector3.down * groundSphereCastDist, groundSphereCastRadius);
    }
    #endregion
}
