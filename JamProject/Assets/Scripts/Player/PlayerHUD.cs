using UnityEngine;
using UnityEngine.UI;
using TMPro; // Use isso se estiver usando TextMeshPro, senão use apenas Text

public class PlayerHUD : MonoBehaviour
{
    [Header("Referencias do Player")]
    public PlayerHealth healthScript;
    public Lanterna lanternaScript;
    public PlayerInventory inventoryScript;
    public LevelSystem levelScript;

    [Header("Elementos de UI - Vida")]
    public Slider healthSlider;

    [Header("Elementos de UI - Experiencia")]
    public Slider xpSlider;
    public TextMeshProUGUI levelText;

    [Header("Elementos de UI - Lanterna")]
    public Slider fuelSlider;

    [Header("Elementos de UI - Inventario")]
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
        if (lanternaScript != null && fuelSlider != null)
        {
            fuelSlider.maxValue = lanternaScript.maxFuel;
            fuelSlider.value = lanternaScript.currentFuel;
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