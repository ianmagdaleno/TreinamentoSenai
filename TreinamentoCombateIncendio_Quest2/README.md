# 🔥 Sistema de Combate a Incêndio VR - Quest 2

Simulação de treinamento de combate a incêndio em Realidade Virtual para Meta Quest 2.

## 🎯 Funcionalidades

- ✅ **Pegar extintor** com controles do Quest 2
- ✅ **Ativar extintor** com gatilho do controle
- ✅ **Sistema de espuma** com física e partículas
- ✅ **4 tipos de fogo** (A, B, C, K)
- ✅ **4 tipos de extintor** (A, B, C, K)
- ✅ **Compatibilidade**: Extintor correto apaga, errado aumenta o fogo
- ✅ **Estados do fogo**: Apagado → Min → Normal → Max
- ✅ **Feedback visual** com particle systems
- ✅ **Feedback sonoro** (opcional)

## 📁 Scripts Criados

| Script | Função |
|--------|--------|
| `MasterExtintor.cs` | Gerencia extintor, ativação e spawn de espuma |
| `MasterFire.cs` | Gerencia estados e tipos de fogo |
| `EspumaExtintor.cs` | Detecta colisão da espuma com fogo |
| `ExtintorXRController.cs` | Integra XR Interaction com o extintor |
| `ExtintorAutoSetup.cs` | Helper para setup automático de extintor |
| `FogoAutoSetup.cs` | Helper para setup automático de fogo |

## 🚀 Quick Start

### 1. Setup Rápido com Scripts Helper

#### Criar Extintor:
1. GameObject → Create Empty → Renomeie para "Extintor"
2. Adicione o script `ExtintorAutoSetup`
3. No Inspector, clique em `⋮` → `Setup Extintor Completo`
4. Adicione manualmente:
   - Component → XR → XR Grab Interactable
   - Modelo 3D do extintor como filho
   - Configure Input Action no `ExtintorXRController`

#### Criar Fogo:
1. GameObject → Create Empty → Renomeie para "Fogo"
2. Adicione o script `FogoAutoSetup`
3. No Inspector, clique em `⋮` → `Setup Fogo Completo`
4. Adicione clips de áudio (opcional)

### 2. Criar Prefab de Espuma

1. GameObject → 3D Object → Sphere
2. Escala: (0.05, 0.05, 0.05)
3. Adicione:
   - Rigidbody (Mass: 0.01, Use Gravity: true)
   - Sphere Collider (Is Trigger: true, Radius: 0.05)
   - Script `EspumaExtintor`
4. Salve como Prefab
5. Arraste para o campo "Prefab Espuma" do `MasterExtintor`

## 🎮 Controles Quest 2

- **Grip Button**: Pegar/Soltar extintor
- **Trigger (Gatilho)**: Ativar/Desativar extintor

## 📖 Documentação Completa

Veja `GUIA_IMPLEMENTACAO.md` para:
- Configuração detalhada de cada componente
- Configurações de Particle Systems
- Troubleshooting
- Otimizações
- Próximos passos

## 🧪 Como Testar

1. **Play Mode no Editor**:
   - Use o XR Device Simulator para simular controles
   - Teste pegar e ativar extintor

2. **Build para Quest 2**:
   - File → Build Settings → Android
   - Player Settings → XR Plugin Management → Oculus
   - Build and Run

## 🔧 Requisitos

- Unity 2021.3+ ou superior
- XR Interaction Toolkit (instalado via Package Manager)
- Input System (novo, não o legado)
- Android Build Support
- Oculus XR Plugin

## 📊 Lógica de Compatibilidade

```
EXTINTOR CORRETO (mesmo tipo):
Max → Normal → Min → Apagado
(~3 segundos de espuma por transição)

EXTINTOR ERRADO (tipo diferente):
Min → Normal → Max
(a cada 2 espumas erradas)
```

## 🎓 Tipos de Fogo

| Tipo | Descrição | Exemplo |
|------|-----------|---------|
| **A** | Sólidos combustíveis | Madeira, papel, tecido |
| **B** | Líquidos inflamáveis | Gasolina, óleo, tinta |
| **C** | Equipamentos elétricos | Computadores, painéis |
| **K** | Gorduras/óleos cozinha | Fritadeiras, fogões |

## 🐛 Problemas Comuns

**Extintor não ativa:**
- Verifique Input Action no `ExtintorXRController`
- Confirme que XR Origin está na cena

**Espuma não colide:**
- Verifique Colliders (Is Trigger = true no fogo)
- Confirme que prefab tem Rigidbody

**Não consegue pegar:**
- Adicione XR Grab Interactable
- Verifique XR Interaction Manager na cena

## 📝 Estrutura de Componentes

### Extintor GameObject:
```
Extintor
├── XRGrabInteractable
├── Rigidbody
├── Collider
├── MasterExtintor
├── ExtintorXRController
├── SaidaEspuma (Transform vazio)
└── ParticulasEspuma (Particle System)
```

### Fogo GameObject:
```
Fogo
├── BoxCollider (Is Trigger)
├── MasterFire
├── AudioSource
├── FogoMin (Particle System)
├── FogoNormal (Particle System)
└── FogoMax (Particle System)
```

## 🎨 Próximas Melhorias

- [ ] Sistema de pontuação
- [ ] Timer de missão
- [ ] UI com instruções
- [ ] Tutorial interativo
- [ ] Múltiplos focos de incêndio
- [ ] Feedback háptico
- [ ] Efeitos de fumaça
- [ ] Sistema de níveis

## 📄 Licença

Projeto educacional - SENAI

---

**Desenvolvido para**: Meta Quest 2  
**Engine**: Unity + XR Interaction Toolkit  
**Data**: Novembro 2025
