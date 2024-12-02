using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    GameObject arrow;
    [SerializeField] int numberOfArrows = 3;
    [SerializeField] GameObject bow;
    bool arrowSlotted = false;
    float pullAmount = 0;
    [SerializeField] float pullSpeed = 50;

    void Start()
    {
        SpawnArrow();
    }

    void Update()
    {
        ShootLogic();
    }

    void SpawnArrow()
    {
        if (numberOfArrows > 0)
        {
            arrowSlotted = true;
            Debug.Log("inst");
            arrow = Instantiate(arrowPrefab, transform.position, transform.rotation) as GameObject;
            Debug.Log("done");
            arrow.transform.parent = transform;
        }
    }

    void ShootLogic()
    {
        if (numberOfArrows > 0)
        {
            if (pullAmount > 100)
            {
                pullAmount = 100;
            }

            SkinnedMeshRenderer _bowSkin = bow.transform.GetComponent<SkinnedMeshRenderer>();
            SkinnedMeshRenderer _arrowSkin = arrow.transform.GetComponent<SkinnedMeshRenderer>();
            Rigidbody _arrowRb = arrow.transform.GetComponent<Rigidbody>();

            ProjectileAddForce _arrowProjectile = arrow.transform.GetComponent<ProjectileAddForce>();

            if (Input.GetMouseButton(0))
            {
                pullAmount += Time.deltaTime * pullSpeed;
            }
            if (Input.GetMouseButtonUp(0))
            {
                //Time.timeScale = 0.1f;
                arrowSlotted = false;
                _arrowRb.isKinematic = false;

                arrow.transform.position = bow.transform.parent.transform.position;
                arrow.transform.localRotation *= Quaternion.Inverse(bow.transform.localRotation);
                //arrow.transform.rotation *= Quaternion.Inverse(bow.transform.localRotation);

                arrow.transform.parent = null;
                numberOfArrows--;
                _arrowProjectile.shootForce *= (pullAmount / 100f);

                _arrowProjectile.enabled = true;

                pullAmount = 0;
            }

            _bowSkin.SetBlendShapeWeight(0, pullAmount);
            _arrowSkin.SetBlendShapeWeight(0, pullAmount);

            if (Input.GetMouseButtonDown(0) && arrowSlotted == false)
            {
                SpawnArrow();
            }
        }
    }
}
