using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    [Header("Configurações de Tempo")]
    public float tempoRestante = 300f;
    public string nomeDaCenaFinal = "Fim"; 
    public bool timerAtivo = true;

    [Header("Referências de UI")]
    public TMP_Text textoTimer;

    void Update()
    {
        if (timerAtivo)
        {
            if (tempoRestante > 0)
            {
                tempoRestante -= Time.deltaTime;
                AtualizarDisplay(tempoRestante);
            }
            else
            {
                tempoRestante = 0;
                timerAtivo = false;
                IrParaProximaCena();
            }
        }
    }

    void AtualizarDisplay(float tempoParaExibir)
    {
        float minutos = Mathf.FloorToInt(tempoParaExibir / 60);
        float segundos = Mathf.FloorToInt(tempoParaExibir % 60);
        textoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    void IrParaProximaCena()
    {        
        Time.timeScale = 1f;        
        SceneManager.LoadScene(nomeDaCenaFinal);
    }
}