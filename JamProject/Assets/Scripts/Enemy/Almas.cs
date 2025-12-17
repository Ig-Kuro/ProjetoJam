using UnityEngine;

public class Almas : MonoBehaviour
{
    private Transform playerTransform;
    private bool isBeingAttracted = false;

    [Header("Configuracoes")]
    public float attractionSpeed = 10f;
    public float attractionRadius = 5f;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance <= attractionRadius)
        {
            isBeingAttracted = true;
        }

        if (isBeingAttracted)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, attractionSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.AddGem(1);
            }
            Destroy(gameObject);
        }
    }
}