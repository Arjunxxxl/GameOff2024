using System;
using Unity.Mathematics;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Targets")] 
    private Transform target;
    [SerializeField] private Transform holderT;
    [SerializeField] private Transform pivotT;
    [SerializeField] private Transform camT;

    [Header("Rig Follow Data")] 
    [SerializeField] private float rigFollowSpeed;
    [SerializeField] private Vector3 rigFollowOffset;

    [Header("Rig Rotation")] 
    [SerializeField] private float rigRotationSpeed;
    
    [Header("Holder Rotation")]
    [SerializeField] private float holderRotationSpeed;
    [SerializeField] private float holderMaxAngle;
    [SerializeField] private float holderMinAngle;
    
    [Header("Pivot Offset Data")] 
    [SerializeField] private float pivotOffsetLerpSpeed;
    private Vector3 pivotOffset;
    [SerializeField] private Vector3 pivotLeftOffset;
    [SerializeField] private Vector3 pivotRightOffset;
    private bool isRight;
    private bool isPivotOffUpdating;

    [Header("Pivot Rotation")] 
    [SerializeField] private float pivotRotationSpeed;
    private Vector3 pivotRotation;
    [SerializeField] private Vector3 pivotLeftRotation;
    [SerializeField] private Vector3 pivotRightRotation;

    private void Start()
    {
        target = Player.Instance.transform;

        SetCamPivot();
    }

    private void Update()
    {
        GetInput();
    }

    private void LateUpdate()
    {
        RigFollow();
        RotateRig();
        
        RotateHolder();
        
        UpdatePivot();
    }

    #region Input

    private void GetInput()
    {
        if (UserInput.Instance.ChangeCamPivot)
        {
            UpdateCamPivot();
        }
    }

    #endregion
    
    #region Rig

    private void RigFollow()
    {
        Vector3 destination = target.position + rigFollowOffset;
        transform.position = Vector3.MoveTowards(transform.position, destination,
            1.0f - (float)Math.Pow(0.5f, Time.deltaTime * rigFollowSpeed));
    }

    private void RotateRig()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation,
            1.0f - (float)Math.Pow(0.5f, Time.deltaTime * rigRotationSpeed));

    }

    #endregion

    #region Holder

    public Vector3 eulerAngle;
    private void RotateHolder()
    {
        eulerAngle = holderT.localRotation.eulerAngles;
        eulerAngle.x += UserInput.Instance.MouseLookDelta.y * Constants.PlayerRotationYSen * Time.deltaTime;

        if (eulerAngle.x >= 0)
        {
            if (eulerAngle.x > holderMaxAngle)
            {
                eulerAngle.x = holderMaxAngle;
            }
        }
        else
        {
            eulerAngle.x += 360;
            if (eulerAngle.x < holderMinAngle)
            {
                eulerAngle.x = holderMinAngle;
            }
        }

        Quaternion finalRot = Quaternion.Euler(eulerAngle);

        holderT.localRotation =
            Quaternion.Slerp(holderT.localRotation, finalRot, Time.deltaTime * holderRotationSpeed);
    }

    #endregion
    
    #region Pivot

    private void SetCamPivot()
    {
        isRight = true;
        pivotOffset = isRight ? pivotRightOffset : pivotLeftOffset;
        pivotT.transform.localPosition = pivotOffset;
        
        pivotRotation = isRight ? pivotRightRotation: pivotLeftRotation;
        pivotT.transform.localRotation = Quaternion.Euler(pivotRotation);
        
        isPivotOffUpdating = true;
    }
    
    private void UpdateCamPivot()
    {
        if (isPivotOffUpdating)
        {
            return;
        }

        isRight = !isRight;
        
        pivotOffset = isRight ? pivotRightOffset : pivotLeftOffset;
        pivotRotation = isRight ? pivotRightRotation: pivotLeftRotation;
        
        isPivotOffUpdating = true;
    }
    
    private void UpdatePivot()
    {
        pivotOffset = isRight ? pivotRightOffset : pivotLeftOffset;
        
        pivotT.transform.localPosition = Vector3.Lerp(pivotT.transform.localPosition, pivotOffset,
            1.0f - (float)Math.Pow(0.5f, Time.deltaTime * pivotOffsetLerpSpeed));
        
        pivotT.transform.localRotation = Quaternion.Lerp(pivotT.transform.localRotation, Quaternion.Euler(pivotRotation),
            1.0f - (float)Math.Pow(0.5f, Time.deltaTime * pivotRotationSpeed));

        if (Vector3.Distance(pivotT.transform.localPosition, pivotOffset) < 0.1f)
        {
            isPivotOffUpdating = false;
        }
    }

    #endregion
}
