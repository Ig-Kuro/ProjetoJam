using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Recompensas")]
    public float xpReward = 20f;

    [Header("Configuracoes")]
    public float health = 50f;
    public float attackRate = 1.5f;
    private float nextAttackTime = 0f;

    [Header("Ataque")]
    public GameObject attackHitbox; 
    public float delayBeforeDamage = 0.5f; 
    public float hitboxDuration = 0.2f;   

    public GameObject Alma;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null) player = playerObject.transform;

        if (attackHitbox != null) attackHitbox.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) <= agent.stoppingDistance)
            {
                if (Time.time >= nextAttackTime)
                {
                    StartCoroutine(PerformAttack());
                    nextAttackTime = Time.time + attackRate;
                }
            }
        }
        if (health <= 0) Die();
    }

    IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(delayBeforeDamage);
        if (attackHitbox != null)
        {
            attackHitbox.SetActive(true);
            yield return new WaitForSeconds(hitboxDuration);
            attackHitbox.SetActive(false);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }

    void Die()
    {
        LevelSystem playerLevel = GameObject.FindGameObjectWithTag("Player").GetComponent<LevelSystem>();
        if (playerLevel != null) playerLevel.GainXp(xpReward);
        if (Alma != null) Instantiate(Alma, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}