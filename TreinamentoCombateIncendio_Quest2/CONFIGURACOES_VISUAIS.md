# 🎨 Guia Visual e Configurações Detalhadas

## 🎯 Configuração de Input Actions para Quest 2

### No ExtintorXRController:

1. **Localize o campo "Activate Action"** no Inspector
2. Clique no dropdown ao lado de "Action"
3. Navegue até: `XRI RightHand Interaction` → `Activate`
   - Ou `XRI LeftHand Interaction` → `Activate` para mão esquerda

**Path completo:**
```
XRI RightHand Interaction/Activate [InputActionProperty]
```

### Alternativa - Configurar manualmente:

Se não aparecer no dropdown:
1. Window → Package Manager
2. Instale/Atualize: **XR Interaction Toolkit**
3. Samples → Import "Starter Assets"
4. No Project, encontre: `XRI Default Input Actions`
5. Arraste para o campo de Input Action

---

## 🔥 Configuração Visual das Partículas

### Particle System - Espuma do Extintor

```
Main Module:
  ✓ Duration: 1.0
  ✓ Looping: TRUE
  ✓ Prewarm: FALSE
  ✓ Start Delay: 0
  ✓ Start Lifetime: 1.5
  ✓ Start Speed: 8
  ✓ Start Size: 0.2
  ✓ 3D Start Rotation: (0, 0, 0)
  ✓ Start Color: RGB(255, 255, 230) - Branco cremoso
  ✓ Gravity Modifier: -0.3 (espuma sobe levemente)
  ✓ Simulation Space: World
  ✓ Scaling Mode: Local
  ✓ Play On Awake: FALSE
  ✓ Max Particles: 100

Emission:
  ✓ Rate over Time: 50
  
Shape:
  ✓ Shape: Cone
  ✓ Angle: 15
  ✓ Radius: 0.1
  ✓ Arc: 360
  ✓ Emit from: Base

Color over Lifetime (opcional):
  ✓ Gradient: Branco → Transparente

Size over Lifetime (opcional):
  ✓ Curve: Aumenta levemente no meio, diminui no fim

Renderer:
  ✓ Render Mode: Billboard
  ✓ Material: Default-Particle ou criar material branco
```

---

### Particle System - Fogo Mínimo (FogoMin)

```
Main Module:
  ✓ Start Lifetime: 0.5 - 1.0
  ✓ Start Speed: 1 - 2
  ✓ Start Size: 0.3 - 0.5
  ✓ Start Color: RGB(255, 180, 80) - Laranja claro
  ✓ Gravity Modifier: -0.1
  ✓ Max Particles: 50

Emission:
  ✓ Rate over Time: 20

Shape:
  ✓ Shape: Cone
  ✓ Angle: 10
  ✓ Radius: 0.1

Color over Lifetime:
  ✓ 0%: Amarelo (255, 255, 100)
  ✓ 50%: Laranja (255, 150, 50)
  ✓ 100%: Vermelho escuro (150, 50, 0) com Alpha 0
```

---

### Particle System - Fogo Normal (FogoNormal)

```
Main Module:
  ✓ Start Lifetime: 1.0 - 1.5
  ✓ Start Speed: 2 - 4
  ✓ Start Size: 0.5 - 1.0
  ✓ Start Color: RGB(255, 120, 30) - Laranja intenso
  ✓ Gravity Modifier: -0.1
  ✓ Max Particles: 100

Emission:
  ✓ Rate over Time: 50

Shape:
  ✓ Shape: Cone
  ✓ Angle: 15
  ✓ Radius: 0.2

Color over Lifetime:
  ✓ 0%: Amarelo brilhante (255, 255, 150)
  ✓ 30%: Laranja (255, 130, 40)
  ✓ 70%: Vermelho (220, 50, 20)
  ✓ 100%: Vermelho escuro (100, 20, 0) com Alpha 0

Size over Lifetime:
  ✓ Curve começa em 1, aumenta para 1.2 no meio, termina em 0.3
```

---

### Particle System - Fogo Máximo (FogoMax)

```
Main Module:
  ✓ Start Lifetime: 1.5 - 2.0
  ✓ Start Speed: 4 - 8
  ✓ Start Size: 1.0 - 2.0
  ✓ Start Color: RGB(255, 80, 20) - Vermelho/Laranja intenso
  ✓ Gravity Modifier: -0.05
  ✓ Max Particles: 200

Emission:
  ✓ Rate over Time: 100
  ✓ Bursts: 1 burst de 20 partículas a cada 0.5s

Shape:
  ✓ Shape: Cone
  ✓ Angle: 20
  ✓ Radius: 0.3

Color over Lifetime:
  ✓ 0%: Amarelo muito brilhante (255, 255, 200)
  ✓ 20%: Amarelo (255, 220, 100)
  ✓ 50%: Laranja intenso (255, 100, 30)
  ✓ 80%: Vermelho (200, 40, 10)
  ✓ 100%: Vermelho escuro (80, 10, 0) com Alpha 0

Size over Lifetime:
  ✓ Começa em 1, cresce para 1.5, termina em 0.2

Velocity over Lifetime:
  ✓ Linear: Y = 0.5 (fogo sobe mais)
  
Noise (opcional para realismo):
  ✓ Strength: 0.5
  ✓ Frequency: 1
  ✓ Scroll Speed: 0.5
```

---

## 🎨 Materials Recomendados

### Material da Espuma:
```
Shader: Particles/Standard Unlit
Rendering Mode: Fade
Color: Branco (255, 255, 255)
Alpha: 200
Emission: Branco fraco (opcional)
```

### Material do Fogo:
```
Shader: Particles/Additive
Color: Usar color over lifetime do particle system
Emission: Ativado, cor amarela/laranja
```

---

## 🔊 Configuração de Áudio

### AudioSource no Fogo:

```
Audio Source Component:
  ✓ AudioClip: Som de fogo crepitando (loop)
  ✓ Play On Awake: TRUE
  ✓ Loop: TRUE
  ✓ Volume: 0.6 (ajustado dinamicamente pelo script)
  ✓ Pitch: 1
  ✓ Spatial Blend: 1 (3D)
  ✓ Doppler Level: 0
  ✓ Min Distance: 1
  ✓ Max Distance: 10
  ✓ Rolloff Mode: Logarithmic
```

### Clips Adicionais:
- **Som Aumentando**: Efeito de "whoosh" intenso
- **Som Apagando**: Efeito de vapor/gás

---

## 🎮 Configuração do XR Grab Interactable

### No GameObject do Extintor:

```
XR Grab Interactable Component:
  
  Interactable Settings:
    ✓ Interaction Manager: Auto (ou arraste XRInteractionManager)
    ✓ Interaction Layer Mask: Everything
    ✓ Colliders: (auto-detecta colliders no objeto)
  
  Selection:
    ✓ Select Mode: Single
    ✓ Allow Hover: TRUE
    ✓ Allow Select: TRUE
  
  Grab Configuration:
    ✓ Movement Type: Instantaneous ou Kinematic
    ✓ Track Position: TRUE
    ✓ Track Rotation: TRUE
    ✓ Throw on Detach: TRUE (opcional)
    ✓ Throw Smoothing Duration: 0.25
    
  Attach Configuration:
    ✓ Attach Transform: (vazio = ponto de origem do objeto)
    ✓ Attach Ease In Time: 0.15
    ✓ Use Dynamic Attach: FALSE
```

**Dica**: Se quiser que o extintor seja segurado em um ponto específico:
1. Crie um Empty GameObject filho chamado "AttachPoint"
2. Posicione onde a mão deve segurar
3. Arraste para "Attach Transform"

---

## 📦 Configuração do Prefab de Espuma

### GameObject: "EspumaPrefab"

```
Transform:
  ✓ Scale: (0.05, 0.05, 0.05)

Rigidbody:
  ✓ Mass: 0.01
  ✓ Drag: 1
  ✓ Angular Drag: 0.5
  ✓ Use Gravity: TRUE
  ✓ Is Kinematic: FALSE
  ✓ Interpolate: Interpolate
  ✓ Collision Detection: Continuous Dynamic
  ✓ Constraints: None

Sphere Collider:
  ✓ Is Trigger: TRUE
  ✓ Radius: 0.05
  ✓ Center: (0, 0, 0)

EspumaExtintor (Script):
  ✓ Tipo Extintor: (preenchido automaticamente)

Trail Renderer (Opcional para efeito visual):
  ✓ Time: 0.3
  ✓ Width: 0.02 → 0
  ✓ Color: Branco → Transparente
  ✓ Material: Default-Particle
```

---

## 🎯 Layers e Physics Matrix

### Recomendação de Layers:

Crie layers customizadas para otimizar colisões:

```
Layer 8: Fogo
Layer 9: Espuma
Layer 10: Extintor
```

### Physics Collision Matrix:
(Edit → Project Settings → Physics)

```
         Fogo  Espuma  Extintor
Fogo     [X]   [✓]     [ ]
Espuma   [✓]   [ ]     [ ]
Extintor [ ]   [ ]     [ ]
```

- Fogo colide com Espuma ✓
- Espuma NÃO colide com Espuma
- Extintor NÃO colide com nada (apenas XR Interaction)

### Aplicar Layers:
1. Selecione GameObject do fogo → Layer: "Fogo"
2. Prefab da espuma → Layer: "Espuma"
3. Extintor → Layer: "Extintor"

---

## 🏗️ Hierarquia Completa da Cena

```
Scene: TreinamentoIncendio
│
├── XR Origin (XR Rig)
│   ├── Camera Offset
│   │   ├── Main Camera
│   │   ├── LeftHand Controller
│   │   └── RightHand Controller
│   └── XR Interaction Manager
│
├── Environment
│   ├── Ground
│   ├── Walls
│   └── Props
│
├── Extintores
│   ├── Extintor_TipoA
│   │   ├── Modelo3D
│   │   ├── SaidaEspuma
│   │   └── ParticulasEspuma
│   │
│   ├── Extintor_TipoB
│   ├── Extintor_TipoC
│   └── Extintor_TipoK
│
├── Focos de Incendio
│   ├── Fogo_TipoA (Madeira)
│   │   ├── FogoMin
│   │   ├── FogoNormal
│   │   └── FogoMax
│   │
│   ├── Fogo_TipoB (Líquido)
│   ├── Fogo_TipoC (Elétrico)
│   └── Fogo_TipoK (Gordura)
│
├── UI
│   ├── Canvas (World Space)
│   └── Instruções
│
└── Managers
    ├── GameManager (vazio para futuro)
    └── AudioManager (opcional)
```

---

## ⚡ Otimizações para Quest 2

### Performance Tips:

1. **Particle Limits:**
   ```
   Total de partículas na cena: < 1000
   Por sistema: < 200
   ```

2. **Draw Calls:**
   ```
   Use atlas de texturas
   Combine meshes quando possível
   Batching: Ativado
   ```

3. **Physics:**
   ```
   Fixed Timestep: 0.02 (50 Hz)
   Max particles with physics: < 50 simultaneamente
   Use layers para evitar cálculos desnecessários
   ```

4. **Textures:**
   ```
   Resolução máxima: 1024x1024
   Compressão: ASTC 6x6 (Android)
   Mipmaps: Ativado
   ```

5. **Lighting:**
   ```
   Baked lighting sempre que possível
   Max 2-3 realtime lights
   Shadows: Medium/Low resolution
   ```

---

## 🧪 Checklist de Teste

### Antes de Build:

- [ ] XR Interaction Toolkit instalado
- [ ] Input System (novo) ativado
- [ ] Android Build Support instalado
- [ ] Oculus XR Plugin ativado
- [ ] Todos os prefabs criados
- [ ] Input Actions configuradas
- [ ] Layers criadas e atribuídas
- [ ] Physics Matrix configurada
- [ ] Audio clips adicionados
- [ ] Particle systems configurados
- [ ] Testar no editor com Device Simulator

### Durante Teste no Quest:

- [ ] Framerate estável (72+ FPS)
- [ ] Controles responsivos
- [ ] Grab/Release funcionando
- [ ] Gatilho ativa/desativa corretamente
- [ ] Espuma colide com fogo
- [ ] Transições de estado corretas
- [ ] Partículas visíveis e performáticas
- [ ] Áudio funcionando (se implementado)
- [ ] Sem crashes ou bugs visuais

---

## 📱 Build Settings para Quest 2

```
File → Build Settings:

Platform: Android

Player Settings:
  Company Name: [Seu Nome/SENAI]
  Product Name: Treinamento Combate Incendio
  
  Other Settings:
    ✓ Color Space: Linear
    ✓ Auto Graphics API: FALSE
    ✓ Graphics APIs: Vulkan, OpenGLES3
    ✓ Minimum API Level: Android 10 (API 29)
    ✓ Target API Level: Android 12 (API 31)
    ✓ Scripting Backend: IL2CPP
    ✓ Target Architectures: ARM64 ✓
    
  XR Plugin Management:
    ✓ Android → Oculus ✓
    
  Quality:
    ✓ VSync Count: Don't Sync
    ✓ Texture Quality: Medium
    ✓ Anti Aliasing: 2x Multi Sampling
```

---

**Dica Final:** Teste SEMPRE no Quest 2 real antes de considerar finalizado. O simulador não representa fielmente a performance e experiência real do dispositivo.
