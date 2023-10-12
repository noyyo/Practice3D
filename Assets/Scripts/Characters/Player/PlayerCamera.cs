using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public Transform cameraContainer;
    private Camera cam;
    private Player player;

    public float minXLook;
    public float maxXLook;
    public float lookSensitivity;
    public bool canLook;

    private float camCurXRot;
    private Vector2 mouseDelta;

    private void Awake()
    {
        cam = Camera.main;
        player = GetComponent<Player>();
        
    }
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    private void CameraLook()
    {
        mouseDelta = player.PlayerInput.PlayerActions.Look.ReadValue<Vector2>();
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }
}
