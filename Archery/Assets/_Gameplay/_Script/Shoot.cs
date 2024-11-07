using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] GameObject arrow;
    [SerializeField] int numberOfArrows = 3;
    [SerializeField] GameObject bow;
    bool arrowSlotted = false;
    [SerializeField] float pullSpeed = 100;
    float pullAmount = 0;

    // Start is called before the first frame update
    void Start()
    {
        SpawnArrow();
    }

    // Update is called once per frame
    void Update()
    {
        ShootLogic();
    }

    void SpawnArrow()
    {
        if (numberOfArrows > 0)
        {
            arrowSlotted = true;
            arrow = Instantiate(arrowPrefab, transform.position, transform.rotation) as GameObject;
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

            if (Input.GetMouseButton(0))
            {
                pullAmount += Time.deltaTime * pullAmount;

                if (pullAmount > 100)
                {
                    pullAmount = 100;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                pullAmount = 0;
                arrowSlotted = false;
                _arrowRb.isKinematic = false;
                arrow.transform.parent = null;
                numberOfArrows -= 1;
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
