using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    private static TooltipManager instancia;
    public GameObject painelTooltip;
    public TextMeshProUGUI tituloTxt;
    public TextMeshProUGUI descricaoTxt;

    void Awake()
    {
        instancia = this;
        painelTooltip.SetActive(false);
    }

    public static void Mostrar(string titulo, string descricao)
    {
        instancia.tituloTxt.text = titulo;
        instancia.descricaoTxt.text = descricao;
        instancia.painelTooltip.SetActive(true);
    }

    public static void Esconder()
    {
        instancia.painelTooltip.SetActive(false);
    }

    void Update()
    {
        if (painelTooltip.activeSelf)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            painelTooltip.transform.position = mousePos + new Vector2(15, -15);
        }
    }
}