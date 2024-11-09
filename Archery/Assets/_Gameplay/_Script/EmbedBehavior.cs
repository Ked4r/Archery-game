using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EmbedBehavior : MonoBehaviour
{
    Rigidbody rigidB;
    // Start is called before the first frame update
    void Start()
    {
        rigidB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider coll)
    {
        Embed();
        transform.parent = coll.transform.parent;
    }

    void Embed()
    {
        GetComponent<Collider>().enabled = false;
        //impactSound.Play();
        transform.GetComponent<ProjectileAddForce>().enabled = false;
        rigidB.velocity = Vector3.zero;
        rigidB.useGravity = false;
        rigidB.isKinematic = true;
    }
}
