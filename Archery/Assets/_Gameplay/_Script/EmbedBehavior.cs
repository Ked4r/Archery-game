using UnityEngine;

public class EmbedBehavior : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("WindArea")) return;

        Embed();
        transform.parent = coll.transform.parent;
    }

    void Embed()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<ProjectileAddForce>().enabled = false;
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.isKinematic = true;

        FindObjectOfType<Aim>().StopFollowingArrow();
    }

}
