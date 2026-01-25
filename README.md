# 🦆 AudioDuck
### Redução automática e inteligente de áudio para Windows.

[![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)]()
[![Platform](https://img.shields.io/badge/Windows-10-0078D6)]()
[![License: GPL v3](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://www.gnu.org/licenses/gpl-3.0)
[![Status](https://img.shields.io/badge/status-active-success)]()

> Reduz automaticamente o volume do Spotify (ou qualquer app) quando outro aplicativo começa a reproduzir áudio e restaura quando para.
---

## ✨ Sobre o projeto

O **Ducker** é um utilitário leve para Windows que resolve um problema comum:

🎵 Você está ouvindo música  
▶️ Começa um vídeo no YouTube/Netflix/VLC  
🔊 O som mistura tudo  

Com o Ducker:

✔ O vídeo começa → a música abaixa automaticamente  
✔ O vídeo pausa → o volume volta ao normal  
✔ Tudo suave, sem cliques manuais

Funciona com **qualquer app que use áudio no Windows**, pois opera diretamente nas **Audio Sessions (WASAPI)**.

---

## 🖼 Preview

![Preview](audioduck/tree/master/fontes/imagem.jpg)

---

## 🚀 Funcionalidades

- 🦆 Ducking automático de volume
- 🎚 Controle de volume
- 🔄 Restauração automática do volume original
- 🎧 Compatível com Spotify, Chrome, Edge, VLC, Discord, etc.

---

## 🧠 Como funciona

O Ducker usa:

- **WASAPI (Core Audio API do Windows)**
- **NAudio**
- Monitoramento de **Audio Sessions**

Fluxo simplificado:

```text
Detecta áudio em outro app
        ↓
app tocando?
        ↓
Sim → reduz volume
Não → restaura volume
```