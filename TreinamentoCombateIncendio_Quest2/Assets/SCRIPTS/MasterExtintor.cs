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
    D,
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
        grabInteractable = GetComponent<XRGrabInteractable>();
        
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
        
        if (particulasEspuma != null)
        {
            particulasEspuma.Stop();
        }
    }

    void Update()
    {
        if (extintorAtivo)
        {
            // Ativa o sistema de partículas
            if (particulasEspuma != null && !particulasEspuma.isPlaying)
            {
                particulasEspuma.Play();
            }
            
            // Spawna objetos de espuma com física para colisão
            if (Time.time >= proximoSpawn && prefabEspuma != null && saidaDaEspuma != null)
            {
                SpawnarEspuma();
                proximoSpawn = Time.time + taxaDeSpawn;
            }
        }
        else
        {
            // Para o sistema de partículas
            if (particulasEspuma != null && particulasEspuma.isPlaying)
            {
                particulasEspuma.Stop();
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
    
    public void AtivarExtintor()
    {
        extintorAtivo = true;
    }
    
    public void DesativarExtintor()
    {
        extintorAtivo = false;
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
