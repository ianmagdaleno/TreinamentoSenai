using UnityEngine;

/// <summary>
/// Script helper para configurar automaticamente um GameObject de Extintor
/// Arraste este script para um GameObject vazio e clique em "Setup Extintor" no Inspector
/// </summary>
public class ExtintorAutoSetup : MonoBehaviour
{
    [Header("Configurações")]
    public type tipoExtintor = type.A;
    public Material materialEspuma;
    
    [ContextMenu("Setup Extintor Completo")]
    void SetupExtintor()
    {
        Debug.Log("Configurando Extintor...");
        
        // 1. Criar ponto de saída da espuma
        GameObject saidaEspuma = new GameObject("SaidaEspuma");
        saidaEspuma.transform.parent = transform;
        saidaEspuma.transform.localPosition = new Vector3(0, 0, 0.5f); // Ajuste conforme modelo
        saidaEspuma.transform.localRotation = Quaternion.identity;
        
        // 2. Criar Particle System para espuma
        GameObject particulasObj = new GameObject("ParticulasEspuma");
        particulasObj.transform.parent = transform;
        particulasObj.transform.localPosition = Vector3.zero;
        
        ParticleSystem ps = particulasObj.AddComponent<ParticleSystem>();
        var main = ps.main;
        main.duration = 1f;
        main.loop = true;
        main.startLifetime = 1.5f;
        main.startSpeed = 8f;
        main.startSize = 0.2f;
        main.startColor = new Color(1f, 1f, 0.9f, 0.8f);
        main.gravityModifier = -0.3f;
        main.maxParticles = 100;
        main.playOnAwake = false;
        
        var emission = ps.emission;
        emission.rateOverTime = 50;
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 15f;
        
        // 3. Adicionar componentes necessários
        if (GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        
        if (GetComponent<Collider>() == null)
        {
            CapsuleCollider col = gameObject.AddComponent<CapsuleCollider>();
            col.radius = 0.05f;
            col.height = 0.3f;
            col.direction = 1; // Y-axis
        }
        
        // 4. Adicionar scripts
        MasterExtintor masterExt = gameObject.GetComponent<MasterExtintor>();
        if (masterExt == null)
            masterExt = gameObject.AddComponent<MasterExtintor>();
        
        masterExt.tipoDeExtintor = tipoExtintor;
        masterExt.saidaDaEspuma = saidaEspuma.transform;
        masterExt.particulasEspuma = ps;
        
        ExtintorXRController xrController = gameObject.GetComponent<ExtintorXRController>();
        if (xrController == null)
            xrController = gameObject.AddComponent<ExtintorXRController>();
        
        Debug.Log("✓ Extintor configurado! Adicione manualmente:");
        Debug.Log("  1. XR Grab Interactable component");
        Debug.Log("  2. Modelo 3D do extintor");
        Debug.Log("  3. Prefab da espuma no MasterExtintor");
        Debug.Log("  4. Configure o Input Action no ExtintorXRController");
    }
}
