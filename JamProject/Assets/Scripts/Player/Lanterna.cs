using UnityEngine;
using UnityEngine.InputSystem;

public class Lanterna : MonoBehaviour
{
    [Header("Dano")]
    public float damagePerSecond = 20f;
    public float maxAuraRadius = 5f;
    public LayerMask enemyLayer;

    [Header("Combustivel")]
    public float maxFuel = 100f;
    public float currentFuel;
    public float consumptionRate = 5f;
    public bool isRefilling = false;

    [Header("Efeitos Visuais")]
    public Light lampLight;
    public float maxLightIntensity = 5f;

    [Header("Sway")]
    public float swayAmount = 2.0f;
    public float maxSwayAmount = 5.0f;
    public float swaySmoothness = 8.0f;

    [Header("Efeito de andar")]
    public float bobSpeed = 10.0f;
    public float bobAmount = 0.05f;

    [Header("Inputs")]
    public InputActionReference lookAction;
    public InputActionReference moveAction;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float timer = 0f;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
        currentFuel = maxFuel;
    }

    void Update()
    {
        ApplySway();
        ApplyBobbing();

        if (!isRefilling && currentFuel > 0)
        {
            currentFuel -= consumptionRate * Time.deltaTime;
        }

        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        float fuelPercentage = currentFuel / maxFuel;
        float currentRadius = maxAuraRadius * fuelPercentage;

        if (lampLight != null)
        {
            lampLight.intensity = maxLightIntensity * fuelPercentage;
        }

        if (currentFuel > 0)
        {
            HandleDamage(currentRadius);
        }
    }

    public void RefillFuel(float amount)
    {
        currentFuel += amount;
    }

    void ApplySway()
    {
        Vector2 mouseDelta = lookAction.action.ReadValue<Vector2>();

        float mouseX = mouseDelta.x * swayAmount;
        float mouseY = mouseDelta.y * swayAmount;

        mouseX = Mathf.Clamp(mouseX, -maxSwayAmount, maxSwayAmount);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAmount, maxSwayAmount);

        Quaternion targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x - mouseY, initialRotation.eulerAngles.y + mouseX, 0);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * swaySmoothness);
    }

    void ApplyBobbing()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        if (Mathf.Abs(moveInput.x) > 0.1f || Mathf.Abs(moveInput.y) > 0.1f)
        {
            timer += Time.deltaTime * bobSpeed;
            float newY = initialPosition.y + Mathf.Sin(timer) * bobAmount;
            float newX = initialPosition.x + Mathf.Cos(timer / 2) * bobAmount;
            transform.localPosition = new Vector3(newX, newY, initialPosition.z);
        }
        else
        {
            timer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * swaySmoothness);
        }
    }

    void HandleDamage(float radius)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);

        foreach (Collider enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damagePerSecond * Time.deltaTime);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxAuraRadius);

        if (Application.isPlaying)
        {
            Gizmos.color = Color.red;
            float currentRadius = maxAuraRadius * (currentFuel / maxFuel);
            Gizmos.DrawWireSphere(transform.position, currentRadius);
        }
    }
}