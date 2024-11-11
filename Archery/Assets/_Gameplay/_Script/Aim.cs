using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Camera cam;
    [SerializeField] bool _invertX = false;
    [SerializeField] bool _invertY = true;
    [SerializeField, Range(0.0f, 10.0f)] float aimSensitivity = 1;
    [SerializeField, Range(-1.00f, 1.00f)] float aimSensitivityRatio = 0;   // 0 represents 1:1; negative reduces X, positive increases Y; value represents the difference from 1, so -0.4 represents 0.6:1
    [SerializeField] float zoomedFOV = 30.0f; // Field of view when zoomed in
    [SerializeField] float normalFOV = 60.0f; // Normal field of view
    [SerializeField] float zoomSpeed = 5.0f;  // Speed of FOV transition

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        // Set initial FOV to normal
        cam.fieldOfView = normalFOV;
    }

    // Update is called once per frame
    void Update()
    {
        AimLogic();
        HandleZoom();
    }

    void AimLogic()
    {
        float _mouseHorizontal = Input.GetAxisRaw("Mouse X");
        float _mouseVertical = Input.GetAxisRaw("Mouse Y");
        Vector3 _rotationX = new(_mouseVertical, 0, 0);
        Vector3 _rotationY = new(0, _mouseHorizontal, 0);

        if (aimSensitivityRatio < 0)
        {
            _rotationX *= (1 + aimSensitivityRatio);
        }
        else if (aimSensitivityRatio > 0)
        {
            _rotationY *= (1 - aimSensitivityRatio);
        }
        if (_invertX)
        {
            _rotationX *= -1;
        }
        if (_invertY)
        {
            _rotationY *= -1;
        }

        rb.MoveRotation(rb.rotation * Quaternion.Euler(_rotationY * aimSensitivity));
        cam.transform.Rotate(_rotationX * aimSensitivity);
    }

    void HandleZoom()
    {
        // Check if the right mouse button is held down
        if (Input.GetMouseButton(1))
        {
            // Smoothly transition to the zoomed FOV
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomedFOV, zoomSpeed * Time.deltaTime);
        }
        else
        {
            // Smoothly transition back to the normal FOV
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, normalFOV, zoomSpeed * Time.deltaTime);
        }
    }
}
