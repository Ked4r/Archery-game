using UnityEngine;
using TMPro; // Namespace dla TextMesh Pro

public class WindArea : MonoBehaviour
{
    [SerializeField] Vector3 windForce = new Vector3(0, 0, 0.2f); // Pocz¹tkowa si³a wiatru
    [SerializeField] bool affectOnlyArrows = true; // Czy wiatr dzia³a tylko na strza³y
    [SerializeField] float changeInterval = 15f; // Interwa³ losowej zmiany wiatru
    [SerializeField] float maxWindStrength = 4f; // Maksymalna wartoœæ si³y wiatru

    [SerializeField] RectTransform windIndicator; // Referencja do strza³ki na UI
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

        // Aktualizuj strza³kê i tekst na UI
        UpdateWindIndicator();
    }

    private void UpdateWindDirection()
    {
        // Losowy kierunek wiatru w p³aszczyŸnie XZ
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);

        // Normalizacja wektora i skalowanie przez losow¹ si³ê
        float randomStrength = Random.Range(0f, maxWindStrength);
        Vector3 randomDirection = new Vector3(randomX, 0, randomZ).normalized * randomStrength;

        // Aktualizacja si³y wiatru
        windForce = randomDirection;

        Debug.Log($"New Wind Direction: {windForce}");
    }

    private void UpdateWindIndicator()
    {
        if (windIndicator == null || windStrengthText == null) return;

        // Obliczenie k¹ta z wektora wiatru (na podstawie x i y)
        float angle = Mathf.Atan2(windForce.z, -windForce.x) * Mathf.Rad2Deg;

        // Obrót strza³ki tylko na osi Z
        windIndicator.rotation = Quaternion.Euler(0, 0, angle);

        // Wyœwietlanie si³y wiatru pod strza³k¹
        float windStrength = windForce.magnitude; // Aktualna si³a wiatru
        windStrengthText.text = $"Wind: {windStrength:F1}"; // Wyœwietla si³ê z dok³adnoœci¹ do 1 miejsca po przecinku
    }




    private void OnTriggerStay(Collider other)
    {
        // Sprawdzenie, czy obiekt ma Rigidbody
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        // Opcjonalne ograniczenie dzia³ania tylko do strza³
        if (affectOnlyArrows && !other.CompareTag("Arrow")) return;

        // Dodaj si³ê wiatru do Rigidbody
        rb.AddForce(windForce, ForceMode.Force);
    }
}
