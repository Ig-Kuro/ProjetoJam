using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;


    [Header("Configuracoes")]
    public float health = 50f;
    public float damage = 10f;
    public float attackRate = 1f;
    private float nextAttackTime = 0f;

    public GameObject Alma;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) <= agent.stoppingDistance)
            {
                Attack();
            }
        }

        if (health <= 0) Die();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    void Die()
    {
        if (Alma != null)
        {
            Instantiate(Alma, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    void Attack()
    {
        if (Time.time >= nextAttackTime)
        {
            Debug.Log("Inimigo atacou");
            nextAttackTime = Time.time + attackRate;
        }
    }
}