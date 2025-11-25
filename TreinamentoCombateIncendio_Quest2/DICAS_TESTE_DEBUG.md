# 🧪 Dicas de Teste e Debug

## Scripts Disponíveis para Teste

### 1. FireTrainingDebugManager.cs

Script auxiliar para facilitar testes durante o desenvolvimento.

#### Instalação:
1. Crie um GameObject vazio: `DebugManager`
2. Adicione o script `FireTrainingDebugManager`
3. O script encontra automaticamente todos os fogos e extintores

#### Atalhos de Teclado (Editor):
- **R** - Resetar todos os fogos
- **L** - Listar status de todos os fogos e extintores
- **K** - Apagar todos os fogos (debug rápido)

#### Uso:
```csharp
// Os atalhos funcionam automaticamente no Play Mode do Editor
// Pressione L para ver o status completo no Console
```

---

## 🔍 Debug Visual em Cena

### Adicionar UI de Debug:

1. **Criar Canvas:**
   ```
   Hierarchy → UI → Canvas
   - Render Mode: World Space
   - Position: Acima da área de jogo
   - Scale: (0.01, 0.01, 0.01)
   ```

2. **Adicionar Text:**
   ```
   Canvas → Right Click → UI → Text - TextMeshPro
   - Configure tamanho e posição
   - Arraste para o campo "Debug Text" do FireTrainingDebugManager
   ```

3. **Ativar Debug na Tela:**
   ```
   No FireTrainingDebugManager:
   ✓ Mostrar Debug Na Tela = TRUE
   ```

Agora verá informações em tempo real sobre fogos e tempo da missão!

---

## 📊 Logs Úteis

### Logs Automáticos dos Scripts:

#### MasterExtintor:
```
"Extintor tipo A ativado!"
"Extintor tipo A desativado!"
```

#### EspumaExtintor:
```
"CORRETO! Extintor tipo A apagou fogo tipo A"
"ERRADO! Extintor tipo B aumentou fogo tipo A"
```

#### MasterFire:
```
"Fogo diminuiu para MIN"
"Fogo APAGADO!"
"Fogo voltou para NORMAL"
"Fogo aumentou para MAX!"
"Fogo já está no MÁXIMO! Cuidado!"
```

---

## 🎮 Teste de Fluxo Completo

### Cenário 1: Uso Correto

1. Play Mode
2. Pegue Extintor tipo A (grip button)
3. Mire em Fogo tipo A
4. Pressione gatilho por 3 segundos
5. **Esperado:**
   - Partículas de espuma aparecem
   - Fogo diminui: Normal → Min
   - Continue por mais 3s
   - Fogo apaga: Min → Apagado
   - Log: "CORRETO! Extintor tipo A apagou fogo tipo A"

### Cenário 2: Uso Incorreto

1. Play Mode
2. Pegue Extintor tipo B
3. Mire em Fogo tipo A
4. Pressione gatilho
5. **Esperado:**
   - Espuma sai
   - Após 2 espumas erradas: Fogo aumenta
   - Fogo cresce: Normal → Max
   - Log: "ERRADO! Extintor tipo B aumentou fogo tipo A"

### Cenário 3: Recuperação

1. Fogo está em Max (do teste anterior)
2. Pegue Extintor tipo A (correto)
3. Pressione gatilho por ~3s
4. **Esperado:**
   - Fogo diminui: Max → Normal
   - Continue: Normal → Min → Apagado

---

## 🐛 Troubleshooting Detalhado

### Problema: "Extintor não pega"

**Checklist:**
```
□ XR Origin está na cena?
□ XR Interaction Manager presente?
□ Extintor tem XRGrabInteractable?
□ Extintor tem Collider?
□ Layers de interação corretas?
```

**Teste:**
1. Hierarchy → XR Origin → XR Interaction Manager
2. Verifique se "Interaction Layer Mask" inclui layer do extintor
3. No extintor, verifique "Interaction Layer Mask" do XRGrabInteractable

**Solução Rápida:**
```
Selecione extintor:
- Layer: Default
XRGrabInteractable:
- Interaction Layer Mask: Everything
```

---

### Problema: "Gatilho não ativa extintor"

**Checklist:**
```
□ ExtintorXRController adicionado?
□ Input Action configurada?
□ XR Interaction Toolkit instalado?
□ Input System (novo) ativado?
```

**Diagnóstico:**
1. Play Mode
2. Pegue o extintor (sem pressionar gatilho)
3. Console deve mostrar: "Extintor pegado!"
4. Pressione gatilho
5. Console deve mostrar: "Extintor tipo X ativado!"

**Se não mostrar:**
```
ExtintorXRController:
1. Clique no campo "Activate Action"
2. Dropdown → XRI RightHand Interaction → Activate
3. Teste novamente
```

**Alternativa Manual:**
```csharp
// No Inspector do ExtintorXRController
// Use Path: <XRController>{RightHand}/activatePressed
```

---

### Problema: "Espuma não colide com fogo"

**Checklist:**
```
□ Fogo tem Collider com Is Trigger = true?
□ Prefab espuma tem Collider + Rigidbody?
□ Script EspumaExtintor no prefab?
□ Prefab atribuído no MasterExtintor?
```

**Teste de Colisão:**
1. Pause Play Mode
2. Scene View → Show → Physics Debugger
3. Verifique se colliders estão verdes/ativos
4. Verifique se espuma e fogo se sobrepõem

**Script de Teste:**
```csharp
// Adicione temporariamente no EspumaExtintor.cs
void OnTriggerEnter(Collider other)
{
    Debug.Log($"Espuma colidiu com: {other.gameObject.name}");
    // ... resto do código
}
```

**Solução de Layers:**
```
Edit → Project Settings → Physics
Physics Collision Matrix:
- "Espuma" deve colidir com "Fogo" ✓
- Verifique se não está desmarcado [ ]
```

---

### Problema: "Partículas não aparecem"

**Checklist - Extintor:**
```
□ Particle System atribuído em MasterExtintor?
□ Play On Awake = FALSE?
□ Particle System é filho do extintor?
□ Material de partícula existe?
```

**Teste:**
1. Select extintor → Hierarchy
2. Encontre filho "ParticulasEspuma"
3. Inspector → Particle System → Play (botão)
4. Se não aparecer:
   - Verifique "Renderer" → Material
   - Verifique Max Particles > 0
   - Verifique Start Size > 0

**Checklist - Fogo:**
```
□ 3 Particle Systems criados?
□ Atribuídos em MasterFire?
□ Script MasterFire ativo?
□ Estado inicial é Normal?
```

**Forçar Visualização:**
```csharp
// No Inspector do MasterFire, clique em ⋮
// Context Menu → "Resetar Fogo"
// Ou pressione R no editor (com DebugManager)
```

---

### Problema: "Fogo não muda de estado"

**Diagnóstico:**
1. Ative Console: Window → General → Console
2. Filtre por "Fogo" ou "Extintor"
3. Pressione gatilho no extintor
4. Observe logs

**Se ver "CORRETO!" mas fogo não muda:**
```
Verifique no MasterFire:
- Tempo Para Apagar não está muito alto (use 3)
- Particle Systems estão atribuídos
- Estados estão corretos
```

**Forçar Mudança Manual:**
```csharp
// No Inspector do MasterFire durante Play Mode
// Mude "Estado Do Fogo" manualmente para testar transições
```

---

### Problema: "Build não funciona no Quest 2"

**Checklist:**
```
□ Android Build Support instalado?
□ Oculus XR Plugin ativado?
□ Input System configurado?
□ Permissions configuradas?
```

**Configuração Quest 2:**
```
File → Build Settings → Player Settings:

Other Settings:
  ✓ Color Space: Linear
  ✓ Auto Graphics API: FALSE
  ✓ Graphics APIs: Vulkan primeiro, OpenGLES3 segundo
  ✓ Minimum API Level: 29 (Android 10)
  ✓ Scripting Backend: IL2CPP
  ✓ ARM64: ✓

XR Plugin Management:
  ✓ Android Tab → Oculus ✓
  ✓ Oculus Settings → Stereo Rendering Mode: Multiview
```

**Quest 2 deve estar em Developer Mode:**
```
1. Instale Oculus App no celular
2. Conecte Quest 2 à conta
3. Settings → Developer Mode → ON
4. Conecte Quest ao PC via USB
5. No Quest, aceite "Allow USB Debugging"
```

---

## 📝 Checklist de Teste Completo

### Antes de Testar:
- [ ] Todos os scripts sem erros
- [ ] Prefab de espuma criado e atribuído
- [ ] Particle systems configurados
- [ ] Input actions configuradas
- [ ] XR Origin na cena
- [ ] XR Interaction Manager presente

### Durante Teste no Editor:
- [ ] Consegue pegar extintor
- [ ] Gatilho ativa espuma
- [ ] Espuma voa na direção correta
- [ ] Espuma colide com fogo
- [ ] Fogo correto: diminui e apaga
- [ ] Fogo errado: aumenta
- [ ] Logs aparecem no Console
- [ ] Partículas mudam com estados

### Antes de Build para Quest:
- [ ] Testado em Editor sem erros
- [ ] Performance aceitável (>60 FPS)
- [ ] Sem warnings graves no Console
- [ ] Build Settings configurado
- [ ] Quest 2 em Developer Mode
- [ ] USB Debugging permitido

### Depois de Build no Quest:
- [ ] Aplicativo abre sem crash
- [ ] Controles responsivos
- [ ] Framerate estável (72+ FPS)
- [ ] Interações funcionam igual ao editor
- [ ] Partículas visíveis
- [ ] Áudio funciona (se implementado)

---

## 🎓 Comandos Úteis no Console

### Filtros no Console:

```
Filtrar por sucesso:
  CORRETO!

Filtrar por erros:
  ERRADO!

Filtrar ativações:
  ativado

Ver mudanças de estado:
  Fogo
```

### Limpar Console:
```
Ctrl + Shift + C (Windows)
Cmd + Shift + C (Mac)
```

---

## 💡 Dicas de Desenvolvimento

### Teste Iterativo:
1. Implemente uma feature de cada vez
2. Teste imediatamente
3. Use DebugManager para validar
4. Só passe para próxima quando funcionar

### Ordem Recomendada:
1. ✓ Pegar extintor (XR Grab)
2. ✓ Ativar com gatilho (Input)
3. ✓ Spawnar espuma (Prefab)
4. ✓ Colisão básica (Trigger)
5. ✓ Lógica de compatibilidade
6. ✓ Estados do fogo
7. ✓ Partículas visuais
8. ✓ Áudio
9. ✓ Polish e otimização

### Use Gizmos para Debug Visual:
```csharp
// Adicione em MasterFire.cs
void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, 1f);
}

// Adicione em MasterExtintor.cs (saidaDaEspuma)
void OnDrawGizmos()
{
    if (saidaDaEspuma != null)
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(saidaDaEspuma.position, 
                       saidaDaEspuma.position + saidaDaEspuma.forward * 2f);
    }
}
```

---

**Boa sorte com o desenvolvimento! 🔥🧯**
