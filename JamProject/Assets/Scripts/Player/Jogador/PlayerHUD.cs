using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHUD : MonoBehaviour
{
    [Header("Referencias do Player")]
    public PlayerHealth healthScript;
    public Lanterna lanternaScript;
    public PlayerInventory inventoryScript;
    public LevelSystem levelScript;

    [Header("UI - Vida")]
    public Slider healthSlider;

    [Header("UI - Experiencia")]
    public Slider xpSlider;
    public TextMeshProUGUI levelText;

    [Header("UI - Lanterna (Efeito de Desaparecer)")]
    public Image lanternaFillImage; 

    [Header("UI - Inventario")]
    public TextMeshProUGUI soulText;

    void Update()
    {
        UpdateHealth();
        UpdateXP();
        UpdateFuel();
        UpdateSouls();
    }

    void UpdateHealth()
    {
        if (healthScript != null && healthSlider != null)
        {
            healthSlider.maxValue = healthScript.maxHealth;
            healthSlider.value = healthScript.currentHealth;
        }
    }

    void UpdateXP()
    {
        if (levelScript != null)
        {
            if (xpSlider != null)
            {
                xpSlider.maxValue = levelScript.xpToNextLevel;
                xpSlider.value = levelScript.currentXp;
            }
            if (levelText != null)
            {
                levelText.text = "LV. " + levelScript.currentLevel;
            }
        }
    }

    void UpdateFuel()
    {
        if (lanternaScript != null && lanternaFillImage != null)
        {
            float fuelPercentage = lanternaScript.currentFuel / lanternaScript.maxFuel;
            lanternaFillImage.fillAmount = fuelPercentage;
        }
    }

    void UpdateSouls()
    {
        if (inventoryScript != null && soulText != null)
        {
            soulText.text = inventoryScript.totalGems.ToString();
        }
    }
}