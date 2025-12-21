using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;

    [Header("Status")]
    public float health = 50f;
    public float xpReward = 20f;

    [Header("Indicadores Visuais")]
    public GameObject damagePopupPrefab;

    [Header("Ataque")]
    public GameObject attackHitbox;
    public float attackRate = 1.5f;
    public float delayBeforeDamage = 0.5f;
    public float hitboxDuration = 0.2f;
    private float nextAttackTime = 0f;

    [Header("Drops")]
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
        if (player != null && health > 0)
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

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (damagePopupPrefab != null)
        {
            Vector3 spawnPos = transform.position + Vector3.up * 0.1f;
            GameObject popup = Instantiate(damagePopupPrefab, spawnPos, Quaternion.identity);
            DamagePopup script = popup.GetComponent<DamagePopup>();
            if (script != null)
            {
                script.Setup(amount);
            }
        }
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

    void Die()
    {
        LevelSystem playerLevel = Object.FindAnyObjectByType<LevelSystem>();
        if (playerLevel != null) playerLevel.GainXp(xpReward);

        if (Alma != null) Instantiate(Alma, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}