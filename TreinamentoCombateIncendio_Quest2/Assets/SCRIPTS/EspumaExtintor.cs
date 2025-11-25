using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EspumaExtintor : MonoBehaviour
{
    public type tipoExtintor;
    
    void OnTriggerEnter(Collider other)
    {
        // Verifica se colidiu com fogo
        MasterFire fogo = other.GetComponent<MasterFire>();
        
        if (fogo != null)
        {
            // Verifica compatibilidade entre extintor e fogo
            if (tipoExtintor == (type)fogo.tipoDeFogo)
            {
                // Tipos compatíveis - apaga o fogo
                Debug.Log($"CORRETO! Extintor tipo {tipoExtintor} apagou fogo tipo {fogo.tipoDeFogo}");
                fogo.ApagarFogo();
            }
            else
            {
                // Tipos incompatíveis - fogo aumenta
                Debug.Log($"ERRADO! Extintor tipo {tipoExtintor} aumentou fogo tipo {fogo.tipoDeFogo}");
                fogo.AumentarFogo();
            }
            
            // Destrói a espuma após colidir
            Destroy(gameObject);
        }
    }
}
