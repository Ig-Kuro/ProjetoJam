using UnityEngine;

public class Calor : MonoBehaviour
{
    public float refillSpeed = 20f; 
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {           
            Lanterna lamp = other.GetComponentInChildren<Lanterna>();
            if (lamp != null)
            {
                lamp.isRefilling = true;
                lamp.RefillFuel(refillSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Lanterna lamp = other.GetComponentInChildren<Lanterna>();
            if (lamp != null)
            {
                lamp.isRefilling = false;
            }
        }
    }
}