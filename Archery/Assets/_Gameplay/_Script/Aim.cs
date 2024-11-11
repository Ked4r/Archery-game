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
	[SerializeField, Range(-1.00f, 1.00f)] float aimSensitivityRatio = 0;	// 0 reprezentuje 1:1; ujemne powoduje mniejszy X, dodatnie powoduje wiêkszy Y; wartoœæ reprezentuje róŸnicê od 1, wiêc np. -0.4 reprezentuje 0.6:1

	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		cam = GetComponentInChildren<Camera>();
		Cursor.visible = false;
	}

	// Update is called once per frame
	void Update()
	{
		AimLogic();
	}

	void AimLogic()
	{
		float _mouseHorizontal = Input.GetAxisRaw("Mouse X");
		float _mouseVertical = Input.GetAxisRaw("Mouse Y");
		Vector3 _rotationX = new(_mouseVertical,0,0);
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
}
