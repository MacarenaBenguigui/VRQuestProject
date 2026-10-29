# VR Quest Prototype

**Unity 6.0 (6000.0.58f2) · Meta Quest 2 / 3 · XR Interaction Toolkit · C#**

A short VR experience for Meta Quest built around hand interaction and a guided flow.

## What it does
- **Object interaction:** grab objects and drop them into their sockets; the scene checks when all pieces are placed.
- **Progression across scenes:** three stages driven by a single `SceneManager`, with screen fades between them.
- **Final quiz:** world-space quiz panel with answer feedback and a full restart of the experience.
- **UI feedback:** animated panels and buttons (scale in/out with DOTween) and an emissive blink material (`FadeMaterial`) to point the player to the next object.

## Where to look
- `Assets/Scripts/SceneManager.cs`: interaction checks, scene flow, quiz and restart
- `Assets/Scripts/FadeMaterial.cs`: emissive blink to guide the player
- `Assets/Scenes/VRScene.unity`: main scene

## How to run
1. Open the project in Unity 6.0 (6000.0.58f2).
2. Load `Assets/Scenes/VRScene`.
3. Build for **Android (Meta Quest)** to play on the headset.

---

Developed by **Macarena Benguigui** · [Portfolio](https://macarenabenguigui.github.io/portfolio/)
