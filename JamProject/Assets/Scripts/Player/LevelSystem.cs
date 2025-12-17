using UnityEngine;

public class LevelSystem : MonoBehaviour
{
    [Header("Configuracoes de Nivel")]
    public int currentLevel = 1;
    public float currentXp = 0;
    public float xpToNextLevel = 100;

    [Range(1.1f, 2.0f)]
    public float expoent = 1.2f;

    public void GainXp(float amount)
    {
        currentXp += amount;

        if (currentXp >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentXp -= xpToNextLevel;
        currentLevel++;

        xpToNextLevel = Mathf.Floor(xpToNextLevel * expoent);

        Debug.Log("Subiu de Nivel! Nivel Atual: " + currentLevel + " | Proximo Nivel: " + xpToNextLevel + " XP");

        if (currentXp >= xpToNextLevel)
        {
            LevelUp();
        }
    }
}