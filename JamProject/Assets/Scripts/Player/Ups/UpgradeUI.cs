using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class UpgradeUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image iconeExibicao;
    public TMP_Text textoPreco;

    [Header("Animações")]
    public float amplitudeFlutuacao = 8f;
    public float velocidadeFlutuacao = 2.5f;
    public float velocidadeEscala = 12f;

    [Header("Efeito Shake (Erro)")]
    public float duracaoShake = 0.3f;
    public float forcaShake = 6f;

    private Upgrade meuUpgrade;
    private UpgradeManager manager;
    private Vector3 posicaoInicial;
    private float offsetAleatorio;
    private float escalaAlvo = 1f;
    private bool estaTremendo = false;
    private Canvas meuCanvas;
    void Awake()
    {
        posicaoInicial = transform.localPosition;
        offsetAleatorio = Random.Range(0f, 10f);
        meuCanvas = GetComponent<Canvas>();
    }

    void OnEnable()
    {
        transform.localScale = Vector3.zero;
        escalaAlvo = 1f;
        transform.localPosition = posicaoInicial;
    }

    void Update()
    {
        float novoY = posicaoInicial.y + Mathf.Sin(Time.unscaledTime * velocidadeFlutuacao + offsetAleatorio) * amplitudeFlutuacao;

        if (!estaTremendo)
        {
            transform.localPosition = new Vector3(posicaoInicial.x, novoY, posicaoInicial.z);
        }
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * escalaAlvo, Time.unscaledDeltaTime * velocidadeEscala);
    }

    public void ConfigurarCarta(Upgrade up, UpgradeManager m, bool eDeGraca)
    {
        meuUpgrade = up;
        manager = m;
        if (iconeExibicao != null) iconeExibicao.sprite = up.icone;

        if (textoPreco != null)
        {
            if (eDeGraca)
            {
                textoPreco.text = "Grátis";
                textoPreco.color = Color.green;
            }
            else
            {
                int preco = up.GetCusto();
                textoPreco.text = preco + "Almas";
                textoPreco.color = (manager.playerInventory.totalGems < preco) ? Color.red : Color.white;
            }
        }
    }

    public void TocarErroShake()
    {
        if (!estaTremendo) StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        estaTremendo = true;
        float tempoPassado = 0f;

        while (tempoPassado < duracaoShake)
        {
            float x = Random.Range(-1f, 1f) * forcaShake;
            transform.localPosition = new Vector3(posicaoInicial.x + x, transform.localPosition.y, posicaoInicial.z);

            tempoPassado += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = new Vector3(posicaoInicial.x, transform.localPosition.y, posicaoInicial.z);
        estaTremendo = false;
    }

    public void SetEscalaDiminuida(float fator) => escalaAlvo = fator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        escalaAlvo = 1.15f;
        if (meuCanvas != null) meuCanvas.sortingOrder = 10;
        manager.DestacarCarta(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        escalaAlvo = 1f;

        if (meuCanvas != null) meuCanvas.sortingOrder = 0;

        manager.ResetarEscalas();
    }
    public void OnClick()
    {
        if (manager != null && meuUpgrade != null)
            manager.SelecionarUpgrade(meuUpgrade, this);
    }
}