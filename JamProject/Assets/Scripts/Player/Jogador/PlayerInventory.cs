using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int totalGems = 0;
    public void AddGem(int amount)
    {
        totalGems += amount;
        Debug.Log("Gemas: " + totalGems);
    }
}