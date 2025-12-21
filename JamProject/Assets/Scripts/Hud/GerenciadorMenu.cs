using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GerenciadorMenu : MonoBehaviour
{
    [Header("Interfaces (UI)")]
    public GameObject painelMenu;
    public GameObject painelHUD;
    public GameObject painelCreditos;
    public GameObject contadorAlmas;
    public CanvasGroup fadeScreen;

    [Header("Volumes")]
    public Volume volumeMenu;

    [Header("Player & Sistemas")]
    public MonoBehaviour playerMovement;
    public MonoBehaviour scriptLanterna;

    private void Start()
    {
        ConfigurarMenuInicial();
        StartCoroutine(ExecutarFade(0f));
    }

    public void ConfigurarMenuInicial()
    {
        painelMenu.SetActive(true);
        painelHUD.SetActive(false);
        if (painelCreditos != null) painelCreditos.SetActive(false);
        if (contadorAlmas != null) contadorAlmas.SetActive(false);

        if (volumeMenu != null) volumeMenu.gameObject.SetActive(true);

        if (playerMovement != null) playerMovement.enabled = false;
        if (scriptLanterna != null) scriptLanterna.enabled = false;

        if (fadeScreen != null) fadeScreen.alpha = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Botao_ChamarGameplay()
    {
        StopAllCoroutines();
        StartCoroutine(RotinaTransicaoJogo());
    }

    public void Botao_ChamarCreditos()
    {
        StopAllCoroutines();
        StartCoroutine(RotinaCreditos(true));
    }

    public void Botao_FecharCreditos()
    {
        StopAllCoroutines();
        StartCoroutine(RotinaCreditos(false));
    }

    private IEnumerator RotinaTransicaoJogo()
    {
        yield return StartCoroutine(ExecutarFade(1f));

        painelMenu.SetActive(false);
        if (painelCreditos != null) painelCreditos.SetActive(false);
        if (volumeMenu != null) volumeMenu.gameObject.SetActive(false);

        painelHUD.SetActive(true);
        if (contadorAlmas != null) contadorAlmas.SetActive(true);
               
        if (playerMovement != null) playerMovement.enabled = true;
        if (scriptLanterna != null) scriptLanterna.enabled = true; 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ExecutarFade(0f));
    }

    private IEnumerator RotinaCreditos(bool abrir)
    {
        yield return StartCoroutine(ExecutarFade(1f));

        if (abrir)
        {
            painelMenu.SetActive(false);
            painelCreditos.SetActive(true);
        }
        else
        {
            painelCreditos.SetActive(false);
            painelMenu.SetActive(true);
        }

        yield return new WaitForSeconds(0.2f);
        yield return StartCoroutine(ExecutarFade(0f));
    }

    private IEnumerator ExecutarFade(float targetAlpha)
    {
        if (fadeScreen == null) yield break;
        float duration = 0.8f;
        float startAlpha = fadeScreen.alpha;
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeScreen.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        fadeScreen.alpha = targetAlpha;
    }

    public void SairDoJogo() => Application.Quit();
}