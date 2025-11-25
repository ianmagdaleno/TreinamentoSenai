using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public enum type
{
    A,
    B,
    C,
    K
}

public class MasterExtintor : MonoBehaviour
{
    [Header("Configurações do Extintor")]
    public type tipoDeExtintor;
    public Transform saidaDaEspuma;
    public ParticleSystem particulasEspuma;
    
    [Header("Configurações da Espuma")]
    public GameObject prefabEspuma;
    public float forcaEspuma = 10f;
    public float taxaDeSpawn = 0.1f;
    public float tempoDeVidaEspuma = 2f;
    
    [Header("Controle")]
    public bool extintorAtivo = false;
    
    private XRGrabInteractable grabInteractable;
    private bool estaSegurando = false;
    private float proximoSpawn = 0f;
    
    void Start()
    {
        Debug.Log($"[Extintor] Inicializando extintor tipo {tipoDeExtintor}");
        
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
            Debug.Log("[Extintor] XRGrabInteractable encontrado e configurado");
        }
        else
        {
            Debug.LogWarning("[Extintor] XRGrabInteractable NÃO encontrado! Adicione o componente.");
        }
        
        if (particulasEspuma != null)
        {
            particulasEspuma.Stop();
            Debug.Log("[Extintor] Particle System encontrado");
        }
        else
        {
            Debug.LogWarning("[Extintor] Particle System NÃO atribuído!");
        }
        
        if (saidaDaEspuma == null)
        {
            Debug.LogWarning("[Extintor] Saída Da Espuma NÃO atribuída!");
        }
        
        if (prefabEspuma == null)
        {
            Debug.LogWarning("[Extintor] Prefab Espuma NÃO atribuído!");
        }
    }

    void Update()
    {
        // TESTE: Permite ativar manualmente no Inspector mesmo sem segurar
        // Remova o "|| true" depois de testar
        if ((estaSegurando && extintorAtivo) || extintorAtivo)
        {
            // Ativa o sistema de partículas
            if (particulasEspuma != null && !particulasEspuma.isPlaying)
            {
                particulasEspuma.Play();
                Debug.Log($"[Extintor] Partículas ATIVADAS - Tipo: {tipoDeExtintor}");
            }
            
            // Spawna objetos de espuma com física para colisão
            if (Time.time >= proximoSpawn && prefabEspuma != null && saidaDaEspuma != null)
            {
                SpawnarEspuma();
                proximoSpawn = Time.time + taxaDeSpawn;
            }
            else if (prefabEspuma == null)
            {
                Debug.LogWarning("[Extintor] Prefab Espuma não atribuído!");
            }
            else if (saidaDaEspuma == null)
            {
                Debug.LogWarning("[Extintor] Saída Da Espuma não atribuída!");
            }
        }
        else
        {
            // Para o sistema de partículas
            if (particulasEspuma != null && particulasEspuma.isPlaying)
            {
                particulasEspuma.Stop();
                Debug.Log("[Extintor] Partículas DESATIVADAS");
            }
        }
    }
    
    private void OnGrab(SelectEnterEventArgs args)
    {
        estaSegurando = true;
    }
    
    private void OnRelease(SelectExitEventArgs args)
    {
        estaSegurando = false;
        extintorAtivo = false;
    }
    
    // Método público para ser chamado por Input Action ou Button no Inspector
    public void AtivarExtintor()
    {
        if (estaSegurando)
        {
            extintorAtivo = true;
            Debug.Log($"Extintor tipo {tipoDeExtintor} ativado!");
        }
    }
    
    public void DesativarExtintor()
    {
        extintorAtivo = false;
        Debug.Log($"Extintor tipo {tipoDeExtintor} desativado!");
    }
    
    void SpawnarEspuma()
    {
        if (prefabEspuma != null && saidaDaEspuma != null)
        {
            GameObject espuma = Instantiate(prefabEspuma, saidaDaEspuma.position, saidaDaEspuma.rotation);
            
            // Adiciona força na direção da saída
            Rigidbody rb = espuma.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = saidaDaEspuma.forward * forcaEspuma;
            }
            
            // Passa o tipo do extintor para a espuma
            EspumaExtintor espumaScript = espuma.GetComponent<EspumaExtintor>();
            if (espumaScript != null)
            {
                espumaScript.tipoExtintor = tipoDeExtintor;
            }
            
            // Destroi após um tempo
            Destroy(espuma, tempoDeVidaEspuma);
        }
    }
    
    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
}
