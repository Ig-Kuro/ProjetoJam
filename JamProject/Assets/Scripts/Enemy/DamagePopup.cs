using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float speedY = 3f;        
    public float fadeTime = 1f;
    public float gravidade = 5f;    

    private Vector3 moveDirection;   
    private TextMeshProUGUI textMesh;
    private Color textColor;
    private bool inicializado = false;

    public void Setup(float damageAmount)
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        if (textMesh == null) return;
        textMesh.text = damageAmount.ToString("F0");
        textColor = textMesh.color;
        moveDirection = new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)).normalized;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-20f, 20f));
        inicializado = true;
        Destroy(gameObject, fadeTime);
    }

    void Update()
    {
        if (!inicializado || textMesh == null) return;
        transform.position += moveDirection * speedY * Time.deltaTime;
        moveDirection.y -= gravidade * Time.deltaTime;
        float alpha = textColor.a - (1.0f / fadeTime) * Time.deltaTime;
        textColor.a = Mathf.Clamp01(alpha);
        textMesh.color = textColor;
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                             Camera.main.transform.rotation * Vector3.up);
        }
    }
}