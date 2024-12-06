using System.Collections;
using UnityEngine;

public class Aim : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Camera mainCamera; // G³ówna kamera
    [SerializeField] Camera arrowCamera; // Kamera œledz¹ca strza³ê
    [SerializeField] bool _invertX = true;
    [SerializeField] bool _invertY = false;
    [SerializeField, Range(0.0f, 10.0f)] float aimSensitivity = 1f;
    [SerializeField, Range(0.0f, 10.0f)] float aimZoomedSensitivity = 0.25f;
    float actualAimSensitivity = 1f;
    [SerializeField, Range(-1.00f, 1.00f)] float aimSensitivityRatio = 0;
    [SerializeField] float zoomedFOV = 5.0f;
    [SerializeField] float normalFOV = 60.0f;
    [SerializeField] float zoomSpeed = 5.0f;

    [SerializeField] float minVerticalAngle = -5f;
    [SerializeField] float maxVerticalAngle = 5f;
    [SerializeField] float minHorizontalAngle = 25f;
    [SerializeField] float maxHorizontalAngle = 35f;

    private bool isActive = false;
    private Transform arrowTransform = null;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isFollowingArrow = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // G³ówna kamera jest aktywna na starcie, kamera strza³y wy³¹czona
        mainCamera.enabled = true;
        arrowCamera.enabled = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        mainCamera.fieldOfView = normalFOV;
        originalCameraPosition = mainCamera.transform.position;
        originalCameraRotation = mainCamera.transform.rotation;

        StartCoroutine(EnableAfterDelay(9.0f));
    }

    void Update()
    {
        if (!isActive) return;

        if (isFollowingArrow && arrowTransform != null)
        {
            FollowArrow();
        }
        else
        {
            AimLogic();
            HandleZoom();
        }
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

        mainCamera.transform.Rotate(_rotationX * actualAimSensitivity, Space.Self);
        float currentXAngle = mainCamera.transform.localEulerAngles.x;
        if (currentXAngle > 180) currentXAngle -= 360;
        currentXAngle = Mathf.Clamp(currentXAngle, minVerticalAngle, maxVerticalAngle);

        mainCamera.transform.localEulerAngles = new Vector3(currentXAngle, mainCamera.transform.localEulerAngles.y, mainCamera.transform.localEulerAngles.z);

        Quaternion newRotation = rb.rotation * Quaternion.Euler(_rotationY * actualAimSensitivity);
        Vector3 newEuler = newRotation.eulerAngles;
        if (newEuler.y > 180) newEuler.y -= 360;
        newEuler.y = Mathf.Clamp(newEuler.y, minHorizontalAngle, maxHorizontalAngle);

        rb.rotation = Quaternion.Euler(newEuler.x, newEuler.y, newEuler.z);
    }

    void HandleZoom()
    {
        if (Input.GetMouseButton(1))
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, zoomedFOV, zoomSpeed * Time.deltaTime);
            actualAimSensitivity = Mathf.Lerp(actualAimSensitivity, aimZoomedSensitivity, zoomSpeed * Time.deltaTime);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, normalFOV, zoomSpeed * Time.deltaTime);
            actualAimSensitivity = Mathf.Lerp(actualAimSensitivity, aimSensitivity, zoomSpeed * Time.deltaTime);
        }
    }

    public void StartFollowingArrow(Transform arrow)
    {
        arrowTransform = arrow;
        isFollowingArrow = true;

        // Prze³¹cz na kamerê œledz¹c¹ strza³ê
        mainCamera.enabled = false;
        arrowCamera.enabled = true;

        // Pocz¹tkowe ustawienie kamery wzglêdem strza³y
        arrowCamera.transform.position = arrow.position - arrow.forward * 2 + Vector3.up * 1.0f;
        arrowCamera.transform.LookAt(arrow);
    }

    public void StopFollowingArrow()
    {
        StartCoroutine(ResetCameraAfterDelay(1.0f));
    }

    void FollowArrow()
    {
        if (arrowTransform != null)
        {
            // Aktualizuj pozycjê i rotacjê kamery œledz¹cej strza³ê
            arrowCamera.transform.position = arrowTransform.position - arrowTransform.forward * 2 + Vector3.up * 1.0f;
            arrowCamera.transform.LookAt(arrowTransform);
        }
    }

    IEnumerator ResetCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isFollowingArrow = false;
        arrowTransform = null;

        // Prze³¹cz z powrotem na g³ówn¹ kamerê
        mainCamera.enabled = true;
        arrowCamera.enabled = false;

        // Przywróæ pozycjê i rotacjê g³ównej kamery
        mainCamera.transform.position = originalCameraPosition;
        mainCamera.transform.rotation = originalCameraRotation;
    }

    IEnumerator EnableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isActive = true;
    }
}
