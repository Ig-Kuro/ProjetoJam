using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class Lanterna : MonoBehaviour
{
    [Header("Dano por Ticks")]
    public float damagePerSecond = 20f;
    public float tickRate = 0.5f;
    private float tickTimer = 0f;
    public float maxAuraRadius = 5f;
    [Range(0f, 1f)]
    public float minRangePercentage = 0.4f;
    public LayerMask enemyLayer;

    [Header("Combustível")]
    public float maxFuel = 100f;
    public float currentFuel;
    public float consumptionRate = 5f;
    public bool isRefilling = false;

    [Header("Efeitos Visuais")]
    public Light lampLight;
    public VisualEffect vfxFogo;
    public Transform indicadorBorda;
    public float maxLightIntensity = 5f;

    [Header("Sway & Bobbing")]
    public float swayAmount = 2.0f;
    public float maxSwayAmount = 5.0f;
    public float swaySmoothness = 8.0f;
    public float bobSpeed = 10.0f;
    public float bobAmount = 0.05f;

    [Header("Inputs")]
    public InputActionReference lookAction;
    public InputActionReference moveAction;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float bobTimer = 0f;

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

        if (lampLight != null)
            lampLight.intensity = maxLightIntensity * fuelPercentage;

        if (vfxFogo != null)
            vfxFogo.SetFloat("IntensidadeCombustivel", fuelPercentage < 0.01f ? 0f : fuelPercentage);

        float rangeFactor = Mathf.Lerp(minRangePercentage, 1f, fuelPercentage);
        float currentRadius = maxAuraRadius * rangeFactor;

        if (indicadorBorda != null)
        {
            float visualScale = currentRadius * 2f;
            indicadorBorda.localScale = new Vector3(visualScale, 0.01f, visualScale);
        }

        if (currentFuel > 0)
        {
            tickTimer += Time.deltaTime;
            if (tickTimer >= tickRate)
            {
                HandleDamage(currentRadius);
                tickTimer = 0f;
            }
        }
    }

    public void RefillFuel(float amount)
    {
        currentFuel += amount;
    }

    void HandleDamage(float radius)
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        foreach (Collider enemyCollider in hitEnemies)
        {
            Enemy enemy = enemyCollider.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damagePerSecond * tickRate);
        }
    }

    void ApplySway()
    {
        Vector2 mouseDelta = lookAction.action.ReadValue<Vector2>();
        float mouseX = Mathf.Clamp(mouseDelta.x * swayAmount, -maxSwayAmount, maxSwayAmount);
        float mouseY = Mathf.Clamp(mouseDelta.y * swayAmount, -maxSwayAmount, maxSwayAmount);
        Quaternion targetRotation = Quaternion.Euler(initialRotation.eulerAngles.x - mouseY, initialRotation.eulerAngles.y + mouseX, 0);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * swaySmoothness);
    }

    void ApplyBobbing()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();
        if (moveInput.magnitude > 0.1f)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            float newY = initialPosition.y + Mathf.Sin(bobTimer) * bobAmount;
            float newX = initialPosition.x + Mathf.Cos(bobTimer / 2) * bobAmount;
            transform.localPosition = new Vector3(newX, newY, initialPosition.z);
        }
        else
        {
            bobTimer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * swaySmoothness);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        float fuelPercentage = currentFuel / maxFuel;
        float currentRadius = maxAuraRadius * Mathf.Lerp(minRangePercentage, 1f, fuelPercentage);
        Gizmos.DrawWireSphere(transform.position, currentRadius);
    }
}