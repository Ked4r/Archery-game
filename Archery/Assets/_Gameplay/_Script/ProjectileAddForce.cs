using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ProjectileAddForce : MonoBehaviour
{
    Rigidbody rb;
    public float shootForce = 10f;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        ApplyForce();
    }


    // Update is called once per frame
    void Update()
    {
        SpinObjectInAir();
    }

    void ApplyForce()
    {
        rb.AddRelativeForce(Vector3.forward * shootForce);

    }

    void SpinObjectInAir()
    {
        float _yVelocity = rb.velocity.y;
        float _zVelocity = rb.velocity.z;
        float _xVelocity = rb.velocity.x;
        float _combinedVelocity = Mathf.Sqrt(_xVelocity * _xVelocity + _zVelocity * _zVelocity);

        float _fallAngle = -1 * Mathf.Atan2(_yVelocity, _combinedVelocity) * 180 / Mathf.PI;

        transform.eulerAngles = new Vector3(_fallAngle, transform.eulerAngles.y, transform.eulerAngles.x);
    }


}
