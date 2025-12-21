using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public GameObject hudDoJogo;
    public List<Upgrade> poolInicial7;
    public List<Upgrade> escolhidas4 = new List<Upgrade>();
    public UpgradeUI[] cartasUI;

    [Header("Referências")]
    public PlayerInventory playerInventory;
    public Lanterna lanterna;
    public Player player;
    public PlayerHealth playerHealth;
    public LevelSystem levelSystem;

    private bool primeiraCompraDoJogoRealizada = false;

    void Awake()
    {
        foreach (var up in poolInicial7) up.ResetProgress();
    }

    public void AbrirMenuUpgrade()
    {
        List<Upgrade> candidatos = GetCandidatosDisponiveis();
        if (candidatos.Count == 0) return;

        if (hudDoJogo != null) hudDoJogo.SetActive(false);
        upgradePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        List<Upgrade> sorteados = SortearLogico(3);
        StartCoroutine(ExibirCartasComAtraso(sorteados));
    }

    public void SelecionarUpgrade(Upgrade up, UpgradeUI uiDaCarta)
    {
        int custoFinal = (up.nivelAtual == 0 || !primeiraCompraDoJogoRealizada) ? 0 : up.GetCusto();

        if (playerInventory.totalGems >= custoFinal)
        {
            playerInventory.totalGems -= custoFinal;

            if (!escolhidas4.Contains(up) && escolhidas4.Count < 4)
                escolhidas4.Add(up);

            up.nivelAtual++;
            primeiraCompraDoJogoRealizada = true;
            AplicarEfeito(up);
            FecharMenu();
        }
        else
        {
            uiDaCarta.TocarErroShake();
        }
    }

    public void BotaoSair() => FecharMenu();

    private void FecharMenu()
    {
        if (hudDoJogo != null) hudDoJogo.SetActive(true);
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void DestacarCarta(UpgradeUI cartaFocada)
    {
        foreach (var ui in cartasUI)
        {
            if (ui != null && ui.gameObject.activeSelf && ui != cartaFocada)
                ui.SetEscalaDiminuida(0.85f);
        }
    }

    public void ResetarEscalas()
    {
        foreach (var ui in cartasUI)
        {
            if (ui != null && ui.gameObject.activeSelf)
                ui.SetEscalaDiminuida(1f);
        }
    }

    private List<Upgrade> GetCandidatosDisponiveis()
    {
        List<Upgrade> candidatos = new List<Upgrade>();
        if (escolhidas4.Count < 4)
        {
            foreach (var up in poolInicial7)
                if (up.nivelAtual < up.nivelMaximo) candidatos.Add(up);
        }
        else
        {
            foreach (var up in escolhidas4)
                if (up.nivelAtual < up.nivelMaximo) candidatos.Add(up);
        }
        return candidatos;
    }

    List<Upgrade> SortearLogico(int quantidade)
    {
        List<Upgrade> candidatos = GetCandidatosDisponiveis();
        List<Upgrade> resultado = new List<Upgrade>();
        if (candidatos.Count == 0) return resultado;

        List<Upgrade> temp = new List<Upgrade>(candidatos);
        while (resultado.Count < quantidade && temp.Count > 0)
        {
            int index = Random.Range(0, temp.Count);
            resultado.Add(temp[index]);
            temp.RemoveAt(index);
        }
        while (resultado.Count < quantidade)
        {
            resultado.Add(candidatos[Random.Range(0, candidatos.Count)]);
        }
        return resultado;
    }

    IEnumerator ExibirCartasComAtraso(List<Upgrade> sorteados)
    {
        foreach (var ui in cartasUI) ui.gameObject.SetActive(false);
        for (int i = 0; i < sorteados.Count; i++)
        {
            cartasUI[i].gameObject.SetActive(true);
            bool gratis = (sorteados[i].nivelAtual == 0) || !primeiraCompraDoJogoRealizada;
            cartasUI[i].ConfigurarCarta(sorteados[i], this, gratis);
            yield return new WaitForSecondsRealtime(0.15f);
        }
    }

    void AplicarEfeito(Upgrade up)
    {
        switch (up.tipo)
        {
            case UpgradeType.AlcanceLanterna: lanterna.maxAuraRadius += up.valor; break;
            case UpgradeType.DanoLanterna: lanterna.damagePerSecond += up.valor; break;
            case UpgradeType.VidaMax:
                playerHealth.maxHealth += up.valor;
                playerHealth.currentHealth += up.valor;
                break;
            case UpgradeType.ConsumoLanterna: lanterna.consumptionRate -= up.valor; break;
            case UpgradeType.BonusXP: levelSystem.expoent -= up.valor; break;
            case UpgradeType.RecuperarVida: playerHealth.healthRegenPerSecond += up.valor; break;
            case UpgradeType.VelocidadePlayer: player.playerSpeed += up.valor; break;
        }
    }
}