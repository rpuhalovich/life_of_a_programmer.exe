# life_of_a_programmer.exe

![](Docs/Images/title.png)

Game trailer (using a very early version of the game): https://www.youtube.com/watch?v=CzQxTDbbYWs

## Introduction

This is a game that was made as the second assignment for the COMP30019 subject at the University of Melbourne over the course of six weeks.

life_of_a_programmer.exe is a game set as the imaginary hyperbolic stereotype of what it is like to be an everyday programmer. The story is that you are a programmer running through code trying to catch a bug at the very end of the level (code) in the shortest time possible.

In accordance with the code running metaphor, the game has you parkour your way through obstacles over the course of several levels, each of which has a bug. In particular, you are able to double jump, dash, grapple, launch from boostpads and wall run to navigate.

Throughout the game you will see many references to common programming concepts. To name a few, each level is named after a common programming error (save for the tutorial, 0_hello_world.lvl), the tutorial controls are defined in array notation, the main menu is a terminal and of course, the end goal is to catch the bug.

## Getting Started

**_NOTE: The game is only to be played in a 16:9 aspect ratio in the game view. The UI is not designed for any other aspect ratio as is specified in the project settings!_**

A prebuilt binary is available in Releases.

After cloning the repository and opening the project in Unity, open `Assets/Scenes/Menu/Open Screen` to begin. The tutorial level is 0_hello_world.lvl and introduces each of the mechanics gradually.

## Contributions - Ryan

- Eight of the twelve participants for the evaluation were arranged by me.
- Post processing effects (chromatic aberration, colour correction etc.), including the use of bloom on most of the assets. This involved converting the current default project to one that uses the Universal Render Pipeline.
- Procedurally generated clouds (see Procedurally Generated Clouds section).
- The particle effects on the bugs and boost pads (see particle section).
- Open screen, and level selection screen.
- Animated the scene fade transitions, as well as scripting the ability to switch scenes.
- Scripting the pause menu, including the sensitivity and music sliders as well as the reset, level select and quit buttons. The PlayerPrefs functionality was used to persistently save the desired positions of sensitivity and music volume sliders (even after shutting down and restarting the game/device).
- Double jumping.
- Boost pads.
- Dash ability.
- Coming up with the story, cinematics (using the Unity timeline), and editing the gameplay video.
- All sound design of the various game elements using Reaper. Including the synthesis of some sounds with Serum.
- The choice of music and scripting the playback during gameplay and the level select screen, as well as the Open Screen sound.
- Levels 0_hello_world.lvl, 1_memory_leak.lvl and 2_null_pointer_exception.lvl were made by me. Seth however was the one to place all tutorial control signs in 0_hello_world.lvl.
- Implemented the best time feature for the timer. This was implemented using the PlayerPrefs functionality and allows for persistent records of a players best time on that particular device in between multiple play sessions.

## Contributions - Marvin

- Implemented FPS controller.
  - Mouse look control.
  - WASD keys for movement.
  - Space for jump, with gravity and ground check.
- Implemented checkpoint system.
  - Added scripts to manage individual checkpoints as well as level checkpoints.
  - Script to change the colour of checkpoints from red to green on trigger.
  - Script to ensure checkpoints are triggered in the correct order.
- Implemented respawn system.
  - Added respawn trigger ("death plane" below each level).
  - Added respawn point to reset player position upon death.
  - Script to trigger respawn plane.
  - Script to set respawn point to latest checkpoint triggered.
- Implemented stopwatch.
  - Stopwatch counter to the UI.
  - Script for stopwatch functionality.
  - Script to ensure stopwatch starts on the first checkpoint and stops on the final checkpoint (provided all previous checkpoints have been triggered).

## Contributions - Seth

- Three participants for evaluation were arranged by me.
- Wall running mechanics.
  - Wall running.
  - Tilting angle transition for Main Camera.
  - Jump refresh.
- Grappling mechanics.
  - Grappling Line.
  - Change of FOV when flying.
  - Momentum after reaching the end of the grapple.
- Implementing the grapple gun model and the grapple line coming out of it.
- Made the UI for the dash mechanic to indicate no. of stored dashes and its cooldown.
- Bug Glitch Shader (see Bug Glitch in Shader section).
- Fog Shader (see Fog in Shader section)
- Re-arranged and refined each level to make them more 'organised' (e.g. some child objects jotting out / not connecting to another object as it should).
- Tutorial guide.
- Levels 3_illegal_use_of_pointer.lvl and 4_segmentation_fault.lvl were done by me.
