using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineTransposer cinemachineTransposer;

    private const float MAX_FOLLOW_OFFSET_Y = 12f;
    private const float MIN_FOLLOW_OFFSET_Y = 2f;

    private float cameraMoveSpeed = 10f;
    private float cameraRotateSpeed = 100f;
    private float cameraZoomAmount = 10f;
    private float cameraZoomSpeed = 10f;

    private void Awake()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        HandleCameraMovement();
        HandleCameraRotation();
        HandleCameraZoom();
    }

    private void HandleCameraMovement()
    {
        Vector3 inputMoveVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputMoveVector.z += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputMoveVector.z -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputMoveVector.x -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputMoveVector.x += 1f;
        }

        Vector3 cameraMoveVector = transform.forward * inputMoveVector.z + transform.right * inputMoveVector.x;
        transform.position += cameraMoveVector * cameraMoveSpeed * Time.deltaTime;
    }

    private void HandleCameraRotation()
    {
        Vector3 inputRotateVector = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.Q))
        {
            inputRotateVector.y -= 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            inputRotateVector.y += 1f;
        }
        transform.eulerAngles += inputRotateVector * cameraRotateSpeed * Time.deltaTime;
    }

    private void HandleCameraZoom()
    {
        Vector3 targetFollowOffset = cinemachineTransposer.m_FollowOffset;
        
        if (Input.mouseScrollDelta.y > 0)
        {
            targetFollowOffset.y += cameraZoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetFollowOffset.y -= cameraZoomAmount;
        }
        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_OFFSET_Y, MAX_FOLLOW_OFFSET_Y);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, cameraZoomSpeed * Time.deltaTime);
    }
}
