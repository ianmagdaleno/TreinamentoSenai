using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

/// <summary>
/// Script auxiliar para conectar os inputs do XR Toolkit com o MasterExtintor
/// Este script deve estar no mesmo GameObject que o XRGrabInteractable e MasterExtintor
/// </summary>
public class ExtintorXRController : MonoBehaviour
{
    [Header("Referências")]
    public MasterExtintor masterExtintor;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    
    [Header("Input Actions")]
    public InputActionProperty activateAction;
    
    private bool estaSegurando = false;
    
    void Start()
    {
        // Pega as referências automaticamente se não foram definidas
        if (masterExtintor == null)
            masterExtintor = GetComponent<MasterExtintor>();
            
        if (grabInteractable == null)
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        
        // Registra os eventos de pegar e soltar
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
            grabInteractable.activated.AddListener(OnActivated);
            grabInteractable.deactivated.AddListener(OnDeactivated);
            Debug.Log("[ExtintorXR] Eventos de grab E activate registrados");
        }
        else
        {
            Debug.LogError("[ExtintorXR] XRGrabInteractable NÃO encontrado!");
        }
        
        // Registra o input action do gatilho
        if (activateAction.action != null)
        {
            activateAction.action.Enable();
            Debug.Log($"[ExtintorXR] Input Action configurado: {activateAction.action.name}");
        }
        else
        {
            Debug.LogWarning("[ExtintorXR] Input Action NÃO configurado! Configure 'Activate Action' no Inspector.");
        }
    }
    
    void Update()
    {
        // TESTE: Tecla T para testar ativação no editor
        if (Input.GetKeyDown(KeyCode.T) && estaSegurando)
        {
            Debug.Log("[ExtintorXR] Tecla T pressionada - Testando ativação");
            masterExtintor.extintorAtivo = !masterExtintor.extintorAtivo;
        }
        
        // Verifica se está segurando
        if (!estaSegurando || masterExtintor == null)
            return;
            
        // Os eventos activated/deactivated já cuidam da ativação
        // Não precisa fazer nada no Update quando os eventos estão funcionando
    }
    
    private void OnGrab(SelectEnterEventArgs args)
    {
        estaSegurando = true;
        Debug.Log($"[ExtintorXR] ✓ Extintor PEGADO! estaSegurando = {estaSegurando}");
        Debug.Log($"[ExtintorXR] Interactor: {args.interactorObject}");
    }
    
    private void OnRelease(SelectExitEventArgs args)
    {
        estaSegurando = false;
        Debug.Log($"[ExtintorXR] Extintor SOLTO! estaSegurando = {estaSegurando}");
        
        // Garante que o extintor seja desativado ao soltar
        if (masterExtintor != null && masterExtintor.extintorAtivo)
        {
            masterExtintor.DesativarExtintor();
        }
    }
    
    // Eventos de ativação (gatilho)
    private void OnActivated(ActivateEventArgs args)
    {
        Debug.Log("[ExtintorXR] 🔥 GATILHO PRESSIONADO!");
        if (masterExtintor != null)
        {
            masterExtintor.AtivarExtintor();
        }
    }
    
    private void OnDeactivated(DeactivateEventArgs args)
    {
        Debug.Log("[ExtintorXR] GATILHO SOLTO!");
        if (masterExtintor != null)
        {
            masterExtintor.DesativarExtintor();
        }
    }
    
    void OnDestroy()
    {
        // Remove os listeners ao destruir
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
            grabInteractable.activated.RemoveListener(OnActivated);
            grabInteractable.deactivated.RemoveListener(OnDeactivated);
        }
        
        if (activateAction.action != null)
        {
            activateAction.action.Disable();
        }
    }
}
