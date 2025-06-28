# DremFaceMentalHealthApp
This repository contains the code for the interview assessment for DreamFace Technologies LLC.

# Getting Started 
## Unity Editor
1. Download the [Unity Editor](https://unity.com/download) (6000.0.44f1 or above)
2. Download this repo (*.zip or *.git). **NOTE**: Always pull from the [main branch](https://github.com/varun404/DremFaceMentalHealthApp/tree/main) of this repo if using version control.
3. [Open this repo](https://docs.unity3d.com/2019.1/Documentation/Manual/GettingStartedOpeningProjects.html) in the Unity Editor.
4. Enter all secret keys in the Secrets script located under Assets/Scripts/Auth

## APK
1. Download the apk available [here](https://drive.google.com/drive/folders/1Ifu_0g_ycQrVQPjKkz23o_qsXBBfAmkk?usp=drive_link)

# Scenes
The release scenes are present under /Assets/Scenes/Release
The following are included in the release build:
1. /Assets/Scenes/Release/AppMain

## Scene Description
### /Assets/Scenes/Release/AppMain
1. All Managers in the scene are children of the "--------- MANAGERS --------" GameObject
2. The entire app ui is under the "MainUICanvas" GameObject, subdivided into sub-canvases based on app state.


# Architecture and Design
The app is designed with modularity and clarity in mind. It is structured into 5 primary app states, each representing a core section of the application.

## App States:
Each state controls what is displayed on screen:
1. **None** – Default state with no UI visible.
2. **Home** – Landing screen of the app.
3. **Activity** – Interactive and assessment-based experiences.
4. **Chat** – Conversational interface powered by GPT.
5. **Analytics** – Tracks user activity insights.

Each state has:
1. A dedicated (except None, Home) **"Manager"** script to handle core logic and backend flow.
2. A matching **"UIHandler"** that manages frontend rendering and updates.

## Other Core Managers
In addition to state-specific managers, the following system-wide managers orchestrate key functionality:
1. **AppManager** – Central control unit; tracks app state, session status, and lifecycle events.
2. **UIManager** – Activates UI based on app state.
3. **TTSManager** – Manages Text-to-Speech responses.
4. **STTManager** – Handles Speech-to-Text for voice input.
5. **ChatGPTManager** – Sends and receives data from the OpenAI GPT API.
 

All requests, responses, and feature interactions are routed through these managers.

This approach was chosen intentionally to:
1. Ensure a clear and easy-to-follow control flow per feature.
2. Maintain access control over core data and functionality.


# Prefabs & UI Modularity
Each App State is implemented as a separate UI Canvas prefab, making it easy for UI/UX designers to make visual changes without affecting unrelated features. 
Each canvas is self-contained and linked with its own UI Handler for clean separation.

Additional prefabs include:
1. **AIMessage & UserMessage** – Visually distinct message bubbles in the chat interface.
2. **User Input** – A custom scrollable input field  with integrated voice input and submit buttons for user-friendly messaging.

This prefab-based approach ensures modularity, rapid iteration, and bug containment across features.
