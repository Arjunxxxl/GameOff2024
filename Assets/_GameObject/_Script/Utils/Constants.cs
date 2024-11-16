using UnityEngine;

public static class Constants
{
    #region Player

    //Movement
    public static readonly float PlayerWalkSpeed = 5f;
    public static readonly float PlayerRunSpeed = 7f;
    public static readonly float PlayerJumpSpeed = 7f;
    public static readonly float PlayerJumpDuration = 0.15f;
    
    //Rotation
    public static readonly float PlayerRotationXSen = 5f;
    public static readonly float PlayerRotationYSen = 5f;
    public static readonly float PlayerRotationSpeed = 10f;
    
    //Collision
    public static readonly float PlayerGroundedPhantomDuration = 0.1f;

    #endregion

    #region Physics

    public static readonly float gravityY = -9.8f;

    #endregion
}
