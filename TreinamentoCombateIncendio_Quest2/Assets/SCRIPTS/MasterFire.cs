using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireState
{
    Apagado,
    Min,
    Normal,
    Max
}

public enum FireType 
{
    A,
    B,
    C,
    K
}

public class MasterFire : MonoBehaviour
{
    [Header("Estado do Fogo")]
    public FireState estadoDoFogo = FireState.Normal;
    public FireType tipoDeFogo;
    
    [Header("Sistemas de Partículas")]
    public ParticleSystem particulasFogoNormal;
    public ParticleSystem particulasFogoMin;
    public ParticleSystem particulasFogoMax;
    
    [Header("Efeitos Sonoros")]
    public AudioSource somFogo;
    public AudioClip somAumentando;
    public AudioClip somApagando;
    
    [Header("Configurações")]
    public float tempoParaApagar = 3f;
    public float tempoParaDiminuir = 2f;
    private float tempoAcumuladoEspuma = 0f;
    private int contadorEspumaErrada = 0;
    
    void Start()
    {
        AtualizarVisualizacaoFogo();
    }
    
    public void ApagarFogo()
    {
        tempoAcumuladoEspuma += 1f;
        
        // Se acumulou tempo suficiente de espuma correta
        if (tempoAcumuladoEspuma >= tempoParaApagar)
        {
            if (estadoDoFogo == FireState.Normal)
            {
                estadoDoFogo = FireState.Min;
                tempoAcumuladoEspuma = 0f;
                Debug.Log("Fogo diminuiu para MIN");
            }
            else if (estadoDoFogo == FireState.Min)
            {
                estadoDoFogo = FireState.Apagado;
                tempoAcumuladoEspuma = 0f;
                Debug.Log("Fogo APAGADO!");
                TocarSom(somApagando);
            }
            else if (estadoDoFogo == FireState.Max)
            {
                estadoDoFogo = FireState.Normal;
                tempoAcumuladoEspuma = 0f;
                Debug.Log("Fogo voltou para NORMAL");
            }
            
            AtualizarVisualizacaoFogo();
        }
    }
    
    public void AumentarFogo()
    {
        contadorEspumaErrada++;
        
        // A cada 2 espumas erradas, o fogo aumenta
        if (contadorEspumaErrada >= 2)
        {
            if (estadoDoFogo == FireState.Min)
            {
                estadoDoFogo = FireState.Normal;
                Debug.Log("Fogo voltou para NORMAL");
            }
            else if (estadoDoFogo == FireState.Normal)
            {
                estadoDoFogo = FireState.Max;
                Debug.Log("Fogo aumentou para MAX!");
                TocarSom(somAumentando);
            }
            else if (estadoDoFogo == FireState.Max)
            {
                Debug.Log("Fogo já está no MÁXIMO! Cuidado!");
            }
            
            contadorEspumaErrada = 0;
            AtualizarVisualizacaoFogo();
        }
    }
    
    void AtualizarVisualizacaoFogo()
    {
        // Desativa todos os sistemas primeiro
        if (particulasFogoNormal != null) particulasFogoNormal.Stop();
        if (particulasFogoMin != null) particulasFogoMin.Stop();
        if (particulasFogoMax != null) particulasFogoMax.Stop();
        
        // Ativa o sistema correspondente ao estado atual
        switch (estadoDoFogo)
        {
            case FireState.Apagado:
                if (somFogo != null) somFogo.Stop();
                // Poderia ativar fumaça aqui
                Debug.Log("Fogo completamente apagado!");
                break;
                
            case FireState.Min:
                if (particulasFogoMin != null) particulasFogoMin.Play();
                if (somFogo != null) somFogo.volume = 0.3f;
                break;
                
            case FireState.Normal:
                if (particulasFogoNormal != null) particulasFogoNormal.Play();
                if (somFogo != null) somFogo.volume = 0.6f;
                break;
                
            case FireState.Max:
                if (particulasFogoMax != null) particulasFogoMax.Play();
                if (somFogo != null) somFogo.volume = 1f;
                break;
        }
    }
    
    void TocarSom(AudioClip clip)
    {
        if (somFogo != null && clip != null)
        {
            somFogo.PlayOneShot(clip);
        }
    }
    
    // Método para resetar o fogo (útil para testes)
    public void ResetarFogo()
    {
        estadoDoFogo = FireState.Normal;
        tempoAcumuladoEspuma = 0f;
        contadorEspumaErrada = 0;
        AtualizarVisualizacaoFogo();
    }
}
