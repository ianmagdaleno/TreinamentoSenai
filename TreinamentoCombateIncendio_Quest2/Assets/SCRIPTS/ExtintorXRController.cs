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
        }
        
        // Registra o input action do gatilho
        if (activateAction.action != null)
        {
            activateAction.action.Enable();
        }
    }
    
    void Update()
    {
        // Verifica se está segurando e se o gatilho está pressionado
        if (estaSegurando && masterExtintor != null)
        {
            float activateValue = activateAction.action.ReadValue<float>();
            
            // Considera pressionado se o valor for maior que 0.5
            if (activateValue > 0.5f)
            {
                if (!masterExtintor.extintorAtivo)
                {
                    masterExtintor.AtivarExtintor();
                }
            }
            else
            {
                if (masterExtintor.extintorAtivo)
                {
                    masterExtintor.DesativarExtintor();
                }
            }
        }
    }
    
    private void OnGrab(SelectEnterEventArgs args)
    {
        estaSegurando = true;
        Debug.Log("Extintor pegado!");
    }
    
    private void OnRelease(SelectExitEventArgs args)
    {
        estaSegurando = false;
        
        // Garante que o extintor seja desativado ao soltar
        if (masterExtintor != null && masterExtintor.extintorAtivo)
        {
            masterExtintor.DesativarExtintor();
        }
        
        Debug.Log("Extintor solto!");
    }
    
    void OnDestroy()
    {
        // Remove os listeners ao destruir
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
        
        if (activateAction.action != null)
        {
            activateAction.action.Disable();
        }
    }
}
