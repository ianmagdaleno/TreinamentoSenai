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
        if (masterExtintor == null)
            masterExtintor = GetComponent<MasterExtintor>();
            
        if (grabInteractable == null)
            grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
            grabInteractable.activated.AddListener(OnActivated);
            grabInteractable.deactivated.AddListener(OnDeactivated);
        }
        
        if (activateAction.action != null)
        {
            activateAction.action.Enable();
        }
    }
    
    void Update()
    {
        // Tecla T para testar ativação manual no editor (opcional)
        if (Input.GetKeyDown(KeyCode.T) && estaSegurando)
        {
            masterExtintor.extintorAtivo = !masterExtintor.extintorAtivo;
        }
    }
    
    private void OnGrab(SelectEnterEventArgs args)
    {
        estaSegurando = true;
    }
    
    private void OnRelease(SelectExitEventArgs args)
    {
        estaSegurando = false;
        
        if (masterExtintor != null && masterExtintor.extintorAtivo)
        {
            masterExtintor.DesativarExtintor();
        }
    }
    
    private void OnActivated(ActivateEventArgs args)
    {
        if (masterExtintor != null)
        {
            masterExtintor.AtivarExtintor();
        }
    }
    
    private void OnDeactivated(DeactivateEventArgs args)
    {
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
