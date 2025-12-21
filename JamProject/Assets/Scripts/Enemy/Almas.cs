using UnityEngine;
using UnityEngine.VFX;

public class Almas : MonoBehaviour
{
    private Transform playerTransform;
    private bool isBeingAttracted = false;

    [Header("Configurações de Movimento")]
    public float attractionSpeed = 10f;
    public float attractionRadius = 5f;
    public float smoothSpeed = 5f; 

    [Header("VFX")]
    public VisualEffect vfxAlma; 

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        if (vfxAlma != null) vfxAlma.Play();

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
            if (vfxAlma != null) vfxAlma.SetBool("IsAttracted", true);
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

            if (vfxAlma != null)
            {
                vfxAlma.Stop();
                if (vfxAlma.gameObject.scene.name != null)
                {
                    vfxAlma.transform.SetParent(null);
                    Destroy(vfxAlma.gameObject, 2f);
                }
            }

            Destroy(gameObject);
        }
    }
}