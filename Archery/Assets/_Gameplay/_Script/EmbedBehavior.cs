using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmbedBehavior : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("WindArea")) return;
        Embed();
        transform.parent = coll.transform.parent;
    }

    void Embed()
    {
        GetComponent<Collider>().enabled = false;
        //impactSound.Play();
        transform.GetComponent<ProjectileAddForce>().enabled = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;
    }
}
