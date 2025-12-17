using UnityEngine;
using UnityEngine.UI;

public class HUDColorAnimator : MonoBehaviour
{
    private Image targetImage;

    [Header("Configuracoes de Cor")]
    public Color colorA; 
    public Color colorB; 
    public float speed = 2f;

    void Start()
    {
        targetImage = GetComponent<Image>();
    }

    void Update()
    {
        if (targetImage != null)
        {
            float pingPong = Mathf.PingPong(Time.time * speed, 1);
            targetImage.color = Color.Lerp(colorA, colorB, pingPong);
        }
    }
}