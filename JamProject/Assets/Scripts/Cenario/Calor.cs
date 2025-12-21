using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using System.Collections;

public class Calor : MonoBehaviour
{
    [Header("Configurações de Energia")]
    public float energiaMaxima = 100f;
    public float energiaAtual;
    public float tempoDeRecargaTotal = 5f;
    public float tempoParaIniciarRecarga = 2f;
    public float consumoPorSegundo = 15f;

    [Header("Indicador UI")]
    public Image preenchimentoCirculo;
    public GameObject canvasIndicador;

    [Header("Visual (Light & VFX)")]
    public Light luzDaFonte;
    public VisualEffect vfxFogueira;
    public string nomeParametroForca = "TargetPosition";
    public float forcaMaximaVFX = 1.0f;
    private float intensidadeOriginal;

    [Header("Status")]
    public bool emRecarga = false;
    public bool jogadorNoColisor = false;
    public float refillSpeed = 20f;

    private Coroutine coroutineRecarga;
    private Coroutine coroutineEspera;

    void Start()
    {
        energiaAtual = energiaMaxima;
        if (luzDaFonte != null) intensidadeOriginal = luzDaFonte.intensity;

        if (canvasIndicador != null) canvasIndicador.SetActive(false);
    }

    void Update()
    {
        if (luzDaFonte != null)
        {
            float porcentagem = energiaAtual / energiaMaxima;
            luzDaFonte.intensity = intensidadeOriginal * porcentagem;
        }

        if (canvasIndicador != null && canvasIndicador.activeSelf)
        {
            canvasIndicador.transform.LookAt(canvasIndicador.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNoColisor = true;
            PararRecarga();

            Lanterna lamp = other.GetComponentInChildren<Lanterna>();
            if (lamp == null) return;

            if (emRecarga)
            {
                PararEfeitoVisual(lamp);
                return;
            }

            if (energiaAtual > 0)
            {
                lamp.isRefilling = true;
                lamp.RefillFuel(refillSpeed * Time.deltaTime);
                energiaAtual -= consumoPorSegundo * Time.deltaTime;

                if (vfxFogueira != null)
                {
                    vfxFogueira.SetFloat(nomeParametroForca, forcaMaximaVFX);
                    vfxFogueira.SetVector3("TargetPosition", lamp.transform.position);
                }
                AtualizarUI(true);
            }

            if (energiaAtual <= 0)
            {
                energiaAtual = 0;
                PararEfeitoVisual(lamp);
                IniciarRecarga();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jogadorNoColisor = false;
            Lanterna lamp = other.GetComponentInChildren<Lanterna>();
            PararEfeitoVisual(lamp);

            if (energiaAtual < energiaMaxima)
            {
                coroutineEspera = StartCoroutine(EsperarParaRecarregar());
            }
        }
    }

    void AtualizarUI(bool ativo)
    {
        if (canvasIndicador != null)
        {
            bool deveMostrar = energiaAtual < energiaMaxima;
            canvasIndicador.SetActive(deveMostrar);

            if (preenchimentoCirculo != null)
                preenchimentoCirculo.fillAmount = energiaAtual / energiaMaxima;
        }
    }

    void PararEfeitoVisual(Lanterna lamp)
    {
        if (lamp != null) lamp.isRefilling = false;
        if (vfxFogueira != null) vfxFogueira.SetFloat(nomeParametroForca, 0f);
    }

    IEnumerator EsperarParaRecarregar()
    {
        yield return new WaitForSeconds(tempoParaIniciarRecarga);
        IniciarRecarga();
    }

    void IniciarRecarga()
    {
        PararRecarga();
        coroutineRecarga = StartCoroutine(RotinaRecargaGradual());
    }

    void PararRecarga()
    {
        if (coroutineEspera != null) StopCoroutine(coroutineEspera);
        if (coroutineRecarga != null) StopCoroutine(coroutineRecarga);
    }

    IEnumerator RotinaRecargaGradual()
    {
        if (energiaAtual <= 0) emRecarga = true;
        AtualizarUI(true);

        float velocidadeRecarga = energiaMaxima / tempoDeRecargaTotal;

        while (energiaAtual < energiaMaxima)
        {
            energiaAtual += velocidadeRecarga * Time.deltaTime;
            AtualizarUI(true); 
            yield return null;
        }

        energiaAtual = energiaMaxima;
        emRecarga = false;
        AtualizarUI(false); 
    }
}