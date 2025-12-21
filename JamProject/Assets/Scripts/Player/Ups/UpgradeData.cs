using UnityEngine;

[CreateAssetMenu(fileName = "NovoUpgrade", menuName = "Upgrades/UpgradeData")]
public class Upgrade : ScriptableObject
{
    public string nome;
    public Sprite icone;
    public UpgradeType tipo;
    public float valor;

    [Header("Configurações de Progressão")]
    public int nivelAtual = 0;
    public int nivelMaximo = 5;
    public int custoBase = 20;

    public int GetCusto()
    {
        if (nivelAtual == 0) return 0;       
        return custoBase * nivelAtual * 2;
    }
        public void ResetProgress()
    {
        nivelAtual = 0;
    }
}
public enum UpgradeType
{
    AlcanceLanterna,
    DanoLanterna,
    VelocidadePlayer,
    VidaMax,
    ConsumoLanterna,
    BonusXP,
    RecuperarVida
}