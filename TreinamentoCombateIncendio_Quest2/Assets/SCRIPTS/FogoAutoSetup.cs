using UnityEngine;

/// <summary>
/// Script helper para configurar automaticamente um GameObject de Fogo
/// Arraste este script para um GameObject vazio e clique em "Setup Fogo" no Inspector
/// </summary>
public class FogoAutoSetup : MonoBehaviour
{
    [Header("Configurações")]
    public FireType tipoFogo = FireType.A;
    public Color corFogo = new Color(1f, 0.5f, 0f, 1f); // Laranja
    
    [ContextMenu("Setup Fogo Completo")]
    void SetupFogo()
    {
        Debug.Log("Configurando Fogo...");
        
        // 1. Adicionar collider trigger
        if (GetComponent<Collider>() == null)
        {
            BoxCollider col = gameObject.AddComponent<BoxCollider>();
            col.isTrigger = true;
            col.size = new Vector3(1f, 2f, 1f);
        }
        
        // 2. Criar particle systems para cada nível
        ParticleSystem psMin = CriarParticulasFogo("FogoMin", 0.3f, 1f, 20);
        ParticleSystem psNormal = CriarParticulasFogo("FogoNormal", 0.6f, 2f, 50);
        ParticleSystem psMax = CriarParticulasFogo("FogoMax", 1f, 4f, 100);
        
        // 3. Adicionar script MasterFire
        MasterFire masterFire = gameObject.GetComponent<MasterFire>();
        if (masterFire == null)
            masterFire = gameObject.AddComponent<MasterFire>();
        
        masterFire.tipoDeFogo = tipoFogo;
        masterFire.estadoDoFogo = FireState.Normal;
        masterFire.particulasFogoMin = psMin;
        masterFire.particulasFogoNormal = psNormal;
        masterFire.particulasFogoMax = psMax;
        
        // 4. Adicionar audio source
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = true;
            audioSource.loop = true;
            audioSource.spatialBlend = 1f; // 3D sound
            audioSource.volume = 0.6f;
        }
        
        masterFire.somFogo = audioSource;
        
        Debug.Log("✓ Fogo configurado! Adicione manualmente:");
        Debug.Log("  1. Modelo 3D ou mesh do fogo (opcional)");
        Debug.Log("  2. AudioClip de fogo no AudioSource");
        Debug.Log("  3. AudioClips para 'Som Aumentando' e 'Som Apagando'");
    }
    
    ParticleSystem CriarParticulasFogo(string nome, float tamanho, float velocidade, int emissao)
    {
        GameObject psObj = new GameObject(nome);
        psObj.transform.parent = transform;
        psObj.transform.localPosition = Vector3.zero;
        
        ParticleSystem ps = psObj.AddComponent<ParticleSystem>();
        
        var main = ps.main;
        main.duration = 1f;
        main.loop = true;
        main.startLifetime = 1f;
        main.startSpeed = velocidade;
        main.startSize = tamanho;
        main.startColor = new ParticleSystem.MinMaxGradient(
            new Color(1f, 0.8f, 0f, 1f), // Amarelo
            new Color(1f, 0.3f, 0f, 1f)  // Laranja/Vermelho
        );
        main.gravityModifier = -0.1f;
        main.maxParticles = emissao * 2;
        main.playOnAwake = false;
        
        var emission = ps.emission;
        emission.rateOverTime = emissao;
        
        var shape = ps.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 10f + (emissao / 10f); // Ângulo aumenta com emissão
        shape.radius = 0.1f;
        
        var colorOverLifetime = ps.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(new Color(1f, 1f, 0f, 1f), 0f),
                new GradientColorKey(new Color(1f, 0.5f, 0f, 1f), 0.5f),
                new GradientColorKey(new Color(0.5f, 0.1f, 0f, 1f), 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(0.5f, 0.5f),
                new GradientAlphaKey(0f, 1f)
            }
        );
        colorOverLifetime.color = gradient;
        
        return ps;
    }
}
