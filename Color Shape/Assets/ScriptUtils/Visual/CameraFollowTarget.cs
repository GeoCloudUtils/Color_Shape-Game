using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    /// <summary>
    /// Follows to given target on specified axis
    /// </summary>
    public enum UpdateType
    {
        Update,
        FixedUpdate,
        LateUpdate
    }
    public Transform target;
    public bool followOnY = false;
    public bool followOnX = false;
    public bool constrainedOnX = false;
    public bool constrainedOnY = false;
    public bool useSmoothFollow = false;
    public bool useOffsetForSmooth = false;
    public float smoothing = 0.8f;
    public UpdateType updateToUse = UpdateType.Update;
    public float minY = 0f;
    public float maxY = 0f;
    public float minX = 0f;
    public float maxX = 0f;
    public Vector3 offset;
    private Vector3 originalPosition;
    private Vector3 targetLastPostion;
    private Vector3 lookAheadPosition;
    private Vector3 currentVelocity;

    private void Start()
    {
        InitCamera();
    }
    public void InitCamera()
    {
        originalPosition = transform.position;
        offset = transform.position - target.position;
    }
    private void LateUpdate()
    {
        if (updateToUse == UpdateType.LateUpdate)
            doMove();
    }
    private void FixedUpdate()
    {
        if (updateToUse == UpdateType.FixedUpdate)
            doMove();
    }
    private void Update()
    {
        if (updateToUse == UpdateType.Update)
            doMove();
    }
    private void doMove()
    {
        Vector3 normalizedPosition;
        if ((!followOnX && !followOnY) || target == null)
            return;
        if (!useSmoothFollow)
        {
            transform.position = target.position + offset;
            normalizedPosition = transform.position;
        }
        else
        {
            float lookAheadFactor = 2;
            float lookAheadReturnSpeed = 0.7f;
            float lookAheadMoveThreshold = 0.1f;
            Vector3 targetPosition = useOffsetForSmooth ? target.position + offset : target.position;
            float xMoveDelta = (target.position - targetLastPostion).x;
            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;
            if (updateLookAheadTarget)
                lookAheadPosition = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            else
                lookAheadPosition = Vector3.MoveTowards(lookAheadPosition, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            float offsetZ = (transform.position - targetPosition).z;
            Vector3 aheadTargetPos = targetPosition + lookAheadPosition + Vector3.forward * offsetZ;
            normalizedPosition = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref currentVelocity, smoothing);

        }
        if (followOnX)
        {
            if (constrainedOnX)
                normalizedPosition.x = Mathf.Clamp(normalizedPosition.x, minX, maxX);
        }
        else
            normalizedPosition.x = originalPosition.x;
        if (followOnY)
        {
            if (constrainedOnY)
                normalizedPosition.y = Mathf.Clamp(normalizedPosition.y, minY, maxY);
        }
        else
            normalizedPosition.y = originalPosition.y;
        normalizedPosition.z = originalPosition.z;
        transform.position = normalizedPosition;
        targetLastPostion = target.position;
    }
}

