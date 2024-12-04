using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;
    GameObject arrow;
    [SerializeField] int numberOfArrows = 100;
    [SerializeField] GameObject bow;
    bool arrowSlotted = false;
    float pullAmount = 0;
    [SerializeField] float pullSpeed = 500;

    // Referencja do g³ównej kamery i kamery strza³y
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera arrowCamera;

    void Start()
    {
        SpawnArrow();
    }

    void Update()
    {
        // Strzelanie jest zablokowane, jeœli kamera œledz¹ca strza³ê jest aktywna
        if (arrowCamera.enabled)
        {
            return;
        }

        ShootLogic();
    }

    void SpawnArrow()
    {
        if (numberOfArrows > 0)
        {
            arrowSlotted = true;
            arrow = Instantiate(arrowPrefab, transform.position, transform.rotation);
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
                arrowSlotted = false;
                _arrowRb.isKinematic = false;

                arrow.transform.position = bow.transform.parent.transform.position;
                arrow.transform.localRotation *= Quaternion.Inverse(bow.transform.localRotation);
                arrow.transform.parent = null;
                numberOfArrows--;
                _arrowProjectile.shootForce *= (pullAmount / 100f);

                _arrowProjectile.enabled = true;

                // Rozpocznij œledzenie strza³y
                FindObjectOfType<Aim>().StartFollowingArrow(arrow.transform);

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
