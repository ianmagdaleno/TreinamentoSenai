# Sistema de Combate a Incêndio VR - Quest 2
## Guia de Implementação Completo

---

## 📋 Scripts Criados

### 1. **MasterExtintor.cs** - Script Principal do Extintor
- Gerencia o tipo de extintor (A, B, C, K)
- Controla ativação/desativação
- Spawna objetos de espuma com física
- Integra com sistema de partículas

### 2. **MasterFire.cs** - Script Principal do Fogo
- Controla estados do fogo (Apagado, Min, Normal, Max)
- Define tipo de fogo (A, B, C, K)
- Gerencia transições de estado
- Controla sistemas de partículas e áudio

### 3. **EspumaExtintor.cs** - Script da Espuma/Projétil
- Detecta colisão com fogo
- Verifica compatibilidade de tipos
- Comunica com MasterFire para aplicar efeitos

### 4. **ExtintorXRController.cs** - Controlador XR
- Integra XR Grab Interactable
- Gerencia input do gatilho do Quest 2
- Conecta interações VR com lógica do extintor

---

## 🎮 Configuração no Unity

### **EXTINTOR - Setup Completo**

#### GameObject do Extintor:
1. Crie um GameObject para o extintor (modelo 3D ou cilindro)
2. Adicione os seguintes componentes:

**Componentes Necessários:**
```
✓ XR Grab Interactable (XR Interaction Toolkit)
✓ Rigidbody (Use Gravity = false, Is Kinematic = true)
✓ Collider (para interação)
✓ MasterExtintor (script)
✓ ExtintorXRController (script)
```

**Configuração do MasterExtintor:**
- **Tipo De Extintor**: Escolha entre A, B, C ou K
- **Saida Da Espuma**: Crie um Empty GameObject como filho na ponta do extintor
- **Particulas Espuma**: Adicione um Particle System como filho
- **Prefab Espuma**: Crie um prefab (veja seção abaixo)
- **Forca Espuma**: 10-15 (ajuste conforme necessário)
- **Taxa De Spawn**: 0.1 segundos
- **Tempo De Vida Espuma**: 2 segundos

**Configuração do ExtintorXRController:**
- **Master Extintor**: Arraste o próprio objeto (auto-preenche)
- **Grab Interactable**: Arraste o XRGrabInteractable (auto-preenche)
- **Activate Action**: 
  - Clique no dropdown
  - Escolha: `XRI RightHand Interaction/Activate` ou `XRI LeftHand Interaction/Activate`

#### Particle System - Espuma Visual:
```
Configurações Recomendadas:
- Duration: 1.0
- Looping: true
- Start Lifetime: 1-2
- Start Speed: 5-10
- Start Size: 0.1-0.3
- Start Color: Branco/Creme
- Gravity Modifier: -0.5 (para simular espuma leve)
- Max Particles: 100
- Emission Rate: 50
- Shape: Cone (Angle: 15-20)
```

---

### **PREFAB ESPUMA - Projétil com Colisão**

#### Criar Prefab de Espuma:
1. Crie um GameObject chamado "EspumaPrefab"
2. Adicione os componentes:

**Componentes:**
```
✓ Sphere Collider (Radius: 0.05, Is Trigger = true)
✓ Rigidbody (Mass: 0.01, Use Gravity = true, Drag = 1)
✓ EspumaExtintor (script)
✓ Trail Renderer ou Particle System (opcional, para visual)
```

**Configuração do EspumaExtintor:**
- O campo `tipoExtintor` será preenchido automaticamente pelo MasterExtintor

3. Salve como Prefab na pasta Assets
4. Arraste o prefab para o campo "Prefab Espuma" do MasterExtintor

---

### **FOGO - Setup Completo**

#### GameObject do Fogo:
1. Crie um GameObject para cada fonte de fogo
2. Adicione os componentes:

**Componentes:**
```
✓ Box Collider ou Sphere Collider (Is Trigger = true)
✓ MasterFire (script)
```

**Configuração do MasterFire:**
- **Estado Do Fogo**: Normal (inicial)
- **Tipo De Fogo**: Escolha entre A, B, C ou K
- **Tempo Para Apagar**: 3 segundos (ajustável)
- **Tempo Para Diminuir**: 2 segundos

#### Particle Systems - Três Níveis de Fogo:
Crie 3 Particle Systems como filhos do GameObject do fogo:

**1. FogoMin (Fogo Baixo)**
```
- Start Lifetime: 0.5-1
- Start Speed: 1-2
- Start Size: 0.3-0.5
- Start Color: Laranja claro
- Emission Rate: 20
- Shape: Cone (Angle: 10)
```

**2. FogoNormal (Fogo Médio)**
```
- Start Lifetime: 1-1.5
- Start Speed: 2-4
- Start Size: 0.5-1
- Start Color: Laranja/Vermelho
- Emission Rate: 50
- Shape: Cone (Angle: 15)
```

**3. FogoMax (Fogo Alto)**
```
- Start Lifetime: 1.5-2
- Start Speed: 4-8
- Start Size: 1-2
- Start Color: Vermelho/Amarelo intenso
- Emission Rate: 100
- Shape: Cone (Angle: 20)
```

Arraste cada Particle System para os campos correspondentes no MasterFire.

#### Áudio (Opcional):
- Adicione um **Audio Source** ao GameObject do fogo
- Configure para Play On Awake = true, Loop = true
- Adicione um som de fogo crepitando
- Arraste clips de som para "Som Aumentando" e "Som Apagando"

---

## 🎯 Lógica de Compatibilidade

### Tipos de Fogo e Extintores:

| Tipo | Descrição | Extintor Correto |
|------|-----------|------------------|
| **A** | Combustíveis sólidos (madeira, papel) | Extintor tipo A |
| **B** | Combustíveis líquidos (gasolina, óleo) | Extintor tipo B |
| **C** | Equipamentos elétricos | Extintor tipo C |
| **K** | Gorduras e óleos de cozinha | Extintor tipo K |

### Sistema de Estados do Fogo:
```
Extintor CORRETO:
Max → Normal → Min → Apagado

Extintor ERRADO (a cada 2 espumas erradas):
Min → Normal → Max (PERIGO!)
```

---

## 🧪 Como Testar

### Teste 1: Pegar e Ativar Extintor
1. Inicie o Play Mode
2. Aproxime o controle do Quest do extintor
3. Aperte o grip/grab button para pegar
4. Pressione o gatilho (trigger) → Deve ver partículas de espuma
5. Solte o gatilho → Espuma deve parar

### Teste 2: Apagar Fogo Corretamente
1. Pegue um extintor do mesmo tipo que o fogo
2. Mire a espuma no fogo
3. Mantenha pressionado por ~3 segundos
4. Observe: Fogo Normal → Min → Apagado
5. Verifique os logs: "CORRETO! Extintor tipo X apagou fogo tipo X"

### Teste 3: Extintor Errado
1. Pegue um extintor de tipo DIFERENTE do fogo
2. Mire a espuma no fogo
3. Observe: Fogo aumenta após 2 espumas erradas
4. Verifique os logs: "ERRADO! Extintor tipo X aumentou fogo tipo Y"

### Teste 4: Build para Quest 2
1. File → Build Settings
2. Android platform
3. Conecte o Quest 2
4. Build and Run
5. Teste todos os controles no dispositivo

---

## 🔧 Troubleshooting

### Extintor não ativa ao pressionar gatilho:
- Verifique se o ExtintorXRController tem a Action configurada
- Confirme que o XR Interaction Toolkit está instalado
- Verifique se há um XR Origin na cena

### Espuma não colide com fogo:
- Confirme que o fogo tem um Collider com Is Trigger = true
- Verifique se o prefab da espuma tem Rigidbody e Collider
- Confirme que as camadas de colisão estão configuradas

### Partículas não aparecem:
- Verifique se os Particle Systems estão atribuídos no Inspector
- Confirme que "Play On Awake" está desmarcado nos sistemas de partículas
- Verifique a escala dos objetos

### Não consegue pegar o extintor:
- Confirme que há um XR Origin/XR Rig na cena
- Verifique se o XR Interaction Manager está presente
- Confirme que o XRGrabInteractable está configurado
- Verifique se há Colliders no extintor

---

## 📦 Próximos Passos / Melhorias

### Funcionalidades Adicionais:
- [ ] Sistema de pontuação
- [ ] Timer de missão
- [ ] Feedback háptico ao acertar/errar
- [ ] UI mostrando tipo de fogo e extintor
- [ ] Tutorial interativo
- [ ] Múltiplos focos de incêndio
- [ ] Sistema de recarga de extintores
- [ ] Efeitos de fumaça ao apagar
- [ ] Sons de sucesso/erro
- [ ] Sistema de progresso/níveis

### Otimizações:
- [ ] Object pooling para espuma
- [ ] LOD para partículas
- [ ] Reduzir draw calls
- [ ] Otimizar física

---

## 📱 Configurações do Quest 2

### Input Actions Padrão Quest 2:
- **Grip Button**: Pegar/Soltar objeto
- **Trigger (Gatilho)**: Ativar extintor
- **A/X Button**: Pode ser usado para UI ou reset
- **B/Y Button**: Pode ser usado para menu

### Performance Quest 2:
- Target: 72 Hz (ou 90/120 Hz se possível)
- Limite de partículas: ~1000 total na cena
- Resolução de texturas: 512x512 ou 1024x1024
- Evite muitos objetos com física ao mesmo tempo

---

## 📝 Notas Importantes

1. **Scripts dependem do XR Interaction Toolkit**: Certifique-se de que está instalado via Package Manager
2. **Input System**: Use o novo Input System do Unity (não o legado)
3. **Layers**: Considere criar layers separadas para "Fogo" e "Espuma" para otimizar colisões
4. **Testing**: Sempre teste no Quest 2 real, não apenas no editor

---

## 🎓 Conceitos Aprendidos

Esta implementação cobre:
- ✓ XR Interaction Toolkit
- ✓ Physics com Rigidbody e Colliders
- ✓ Particle Systems
- ✓ State Machines (FireState)
- ✓ Enums para tipos
- ✓ Collision Detection
- ✓ Input Actions do Quest 2
- ✓ Spawning e Object Lifecycle
- ✓ Audio básico
- ✓ Prefabs

---

**Desenvolvido para**: Unity com XR Interaction Toolkit  
**Target**: Meta Quest 2  
**Última atualização**: Novembro 2025
