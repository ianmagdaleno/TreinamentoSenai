using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Manager de debug para testar o sistema de incêndio
/// Adicione em um GameObject vazio na cena
/// </summary>
public class FireTrainingDebugManager : MonoBehaviour
{
    [Header("Referências de Teste")]
    public MasterFire[] todosFogos;
    public MasterExtintor[] todosExtintores;
    
    [Header("Debug UI (Opcional)")]
    public TextMeshProUGUI debugText;
    public bool mostrarDebugNoConsole = true;
    public bool mostrarDebugNaTela = false;
    
    [Header("Teclas de Teste (Editor apenas)")]
    public KeyCode teclaResetarFogos = KeyCode.R;
    public KeyCode teclaListarStatus = KeyCode.L;
    public KeyCode teclaApagarTodos = KeyCode.K;
    
    private float tempoInicioMissao;
    private int fogosApagados = 0;
    private int tentativasErradas = 0;
    
    void Start()
    {
        tempoInicioMissao = Time.time;
        
        // Auto-encontra todos os fogos e extintores na cena
        if (todosFogos == null || todosFogos.Length == 0)
            todosFogos = FindObjectsOfType<MasterFire>();
        
        if (todosExtintores == null || todosExtintores.Length == 0)
            todosExtintores = FindObjectsOfType<MasterExtintor>();
        
        LogDebug($"Sistema iniciado: {todosFogos.Length} fogos e {todosExtintores.Length} extintores encontrados");
    }
    
    void Update()
    {
        // Atalhos de teclado apenas no editor
        #if UNITY_EDITOR
        if (Input.GetKeyDown(teclaResetarFogos))
        {
            ResetarTodosFogos();
        }
        
        if (Input.GetKeyDown(teclaListarStatus))
        {
            ListarStatusDeTodos();
        }
        
        if (Input.GetKeyDown(teclaApagarTodos))
        {
            ApagarTodosFogos();
        }
        #endif
        
        // Atualiza UI de debug
        if (mostrarDebugNaTela && debugText != null)
        {
            AtualizarDebugUI();
        }
    }
    
    public void ResetarTodosFogos()
    {
        foreach (MasterFire fogo in todosFogos)
        {
            if (fogo != null)
            {
                fogo.ResetarFogo();
            }
        }
        
        fogosApagados = 0;
        tentativasErradas = 0;
        tempoInicioMissao = Time.time;
        
        LogDebug("✓ Todos os fogos foram resetados!");
    }
    
    public void ApagarTodosFogos()
    {
        foreach (MasterFire fogo in todosFogos)
        {
            if (fogo != null)
            {
                fogo.estadoDoFogo = FireState.Apagado;
            }
        }
        
        LogDebug("✓ Todos os fogos foram apagados (debug)");
    }
    
    public void ListarStatusDeTodos()
    {
        LogDebug("=== STATUS DA MISSÃO ===");
        LogDebug($"Tempo decorrido: {(Time.time - tempoInicioMissao):F1}s");
        LogDebug($"Fogos apagados: {ContarFogosApagados()}/{todosFogos.Length}");
        LogDebug("");
        
        LogDebug("--- FOGOS ---");
        for (int i = 0; i < todosFogos.Length; i++)
        {
            if (todosFogos[i] != null)
            {
                string status = $"Fogo {i + 1}: Tipo {todosFogos[i].tipoDeFogo} | Estado: {todosFogos[i].estadoDoFogo}";
                LogDebug(status);
            }
        }
        
        LogDebug("");
        LogDebug("--- EXTINTORES ---");
        for (int i = 0; i < todosExtintores.Length; i++)
        {
            if (todosExtintores[i] != null)
            {
                string ativo = todosExtintores[i].extintorAtivo ? "ATIVO" : "Inativo";
                string status = $"Extintor {i + 1}: Tipo {todosExtintores[i].tipoDeExtintor} | {ativo}";
                LogDebug(status);
            }
        }
        
        LogDebug("=====================");
    }
    
    public int ContarFogosApagados()
    {
        int count = 0;
        foreach (MasterFire fogo in todosFogos)
        {
            if (fogo != null && fogo.estadoDoFogo == FireState.Apagado)
            {
                count++;
            }
        }
        return count;
    }
    
    public int ContarFogosAtivos()
    {
        int count = 0;
        foreach (MasterFire fogo in todosFogos)
        {
            if (fogo != null && fogo.estadoDoFogo != FireState.Apagado)
            {
                count++;
            }
        }
        return count;
    }
    
    public bool TodosFogosApagados()
    {
        return ContarFogosApagados() == todosFogos.Length;
    }
    
    void AtualizarDebugUI()
    {
        if (debugText == null) return;
        
        float tempo = Time.time - tempoInicioMissao;
        int fogosApagados = ContarFogosApagados();
        int fogosAtivos = ContarFogosAtivos();
        
        string texto = $"<b>TREINAMENTO DE COMBATE A INCÊNDIO</b>\n\n";
        texto += $"Tempo: {tempo:F1}s\n";
        texto += $"Fogos Apagados: {fogosApagados}/{todosFogos.Length}\n";
        texto += $"Fogos Ativos: {fogosAtivos}\n\n";
        
        if (TodosFogosApagados())
        {
            texto += "<color=green><b>✓ MISSÃO COMPLETA!</b></color>\n";
            texto += $"Tempo final: {tempo:F1}s";
        }
        else
        {
            texto += "<color=yellow>Continue apagando os fogos!</color>";
        }
        
        debugText.text = texto;
    }
    
    void LogDebug(string mensagem)
    {
        if (mostrarDebugNoConsole)
        {
            Debug.Log($"[FireTraining] {mensagem}");
        }
    }
    
    // Método público para ser chamado por outros scripts
    public void RegistrarFogoApagado()
    {
        fogosApagados++;
        LogDebug($"✓ Fogo apagado! Total: {fogosApagados}/{todosFogos.Length}");
        
        if (TodosFogosApagados())
        {
            float tempoFinal = Time.time - tempoInicioMissao;
            LogDebug($"🎉 PARABÉNS! Todos os fogos foram apagados em {tempoFinal:F1} segundos!");
        }
    }
    
    public void RegistrarTentativaErrada(type tipoExtintor, FireType tipoFogo)
    {
        tentativasErradas++;
        LogDebug($"❌ ERRO! Extintor tipo {tipoExtintor} usado em fogo tipo {tipoFogo}. Total de erros: {tentativasErradas}");
    }
    
    // Métodos de atalho para UI Buttons
    public void BotaoResetar() => ResetarTodosFogos();
    public void BotaoStatus() => ListarStatusDeTodos();
    public void BotaoApagarTodos() => ApagarTodosFogos();
}
