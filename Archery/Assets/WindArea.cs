using UnityEngine;
using TMPro; // Namespace dla TextMesh Pro

public class WindArea : MonoBehaviour
{
    [SerializeField] Vector3 windForce = new Vector3(0, 0, 0.2f); // Pocz�tkowa si�a wiatru
    [SerializeField] bool affectOnlyArrows = true; // Czy wiatr dzia�a tylko na strza�y
    [SerializeField] float changeInterval = 15f; // Interwa� losowej zmiany wiatru
    [SerializeField] float maxWindStrength = 4f; // Maksymalna warto�� si�y wiatru

    [SerializeField] RectTransform windIndicator; // Referencja do strza�ki na UI
    [SerializeField] TMP_Text windStrengthText; // Referencja do TextMesh Pro tekstu na UI

    private float timeSinceLastChange = 0f;

    private void Start()
    {
        UpdateWindDirection();
    }

    private void Update()
    {
        // Co 15 sekund losuj nowy kierunek wiatru
        timeSinceLastChange += Time.deltaTime;
        if (timeSinceLastChange >= changeInterval)
        {
            UpdateWindDirection();
            timeSinceLastChange = 0f;
        }

        // Aktualizuj strza�k� i tekst na UI
        UpdateWindIndicator();
    }

    private void UpdateWindDirection()
    {
        // Losowy kierunek wiatru w p�aszczy�nie XZ
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        // Normalizacja wektora i skalowanie przez losow� si��
        float randomStrength = Random.Range(0f, maxWindStrength);
        Vector3 randomDirection = new Vector3(randomX, 0, randomZ).normalized * randomStrength;

        // Aktualizacja si�y wiatru
        windForce = randomDirection;

        Debug.Log($"New Wind Direction: {windForce}");
    }

    private void UpdateWindIndicator()
    {
        if (windIndicator == null || windStrengthText == null) return;

        // Obliczenie k�ta z wektora wiatru (na podstawie x i y)
        float angle = Mathf.Atan2(windForce.z, -windForce.x) * Mathf.Rad2Deg;

        // Obr�t strza�ki tylko na osi Z
        windIndicator.rotation = Quaternion.Euler(0, 0, angle);

        // Wy�wietlanie si�y wiatru pod strza�k�
        float windStrength = windForce.magnitude; // Aktualna si�a wiatru
        windStrengthText.text = $"Wind: {windStrength:F1}"; // Wy�wietla si�� z dok�adno�ci� do 1 miejsca po przecinku
    }




    private void OnTriggerStay(Collider other)
    {
        // Sprawdzenie, czy obiekt ma Rigidbody
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        // Opcjonalne ograniczenie dzia�ania tylko do strza�
        if (affectOnlyArrows && !other.CompareTag("Arrow")) return;

        // Dodaj si�� wiatru do Rigidbody
        rb.AddForce(windForce, ForceMode.Force);
    }
}
