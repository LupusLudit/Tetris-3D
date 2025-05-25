# Tetris 3D
Tetris 3D is a modern reimagining of the classic Tetris game, extended into three dimensions.
This project challenges players to think spatially by rotating and positioning blocks not just on a 2D grid, but within a full 3D space.

The game features many gameplay mechanics introduced in the original Tetris, such as block spawning, rotation, movement, collision detection, and line clearing.
However, it also introduces brand-new concepts that could not be achieved in two dimensions. For example, players can rotate the entire game board to better visualize the playing field.

The project also features custom-made UI elements and animations to enhance the visual experience.

## Introduction and basic file structure description
This game is programed in C#, using the unity game engine.
The scripts containing the actual game code can be found in `Tetris-3D > Assets > Scripts`.
Other game elements used (like the 3D tile models, images, etc.) can be found in the Assets folder.
If you want to test the game, I have created a set of tests for the main game logic. You can find them in `Tetris-3D > Assets > Tests`. These tests can be run directly in Unity.
The entire project is commented to make the code easier to navigate. Each class includes a description in the ProjectDocs.xml file, located in `Tetris-3D > Assets > Docs`.

## How to play
This project now includes a ready-to-play executables. To play the game, simply visit the [latest release](https://github.com/LupusLudit/Tetris-3D/releases/tag/v1.1.0)
and download the provided files. The program allows you to change the game settings. It will automatically create a directory for these settings, so the user doesn't need to do anything manually. Please note, however, that any changes made to the settings will only be saved locally.

## Software used
The following software was used during development and can also be used if you wish to inspect or modify the code:

- **The Unity Game Engine**: [https://unity.com](https://unity.com/download)
- **Visual Studio** (you can use any other text editor for inspecting the code): [https://visualstudio.microsoft.com](https://visualstudio.microsoft.com/)

All 3D models used in the project were custom-made using **Blender**. You can download Blender from the [official website](https://www.blender.org/) if you'd like to modify or inspect the models.

## Notes
This project has reached its initial release version.

It is important to note that this game was developed as part of a **school project**.

The reason the first commit contained so many elements is because
until the day of the first commit (29.09.2024), I had not made significant changes to the project and stored it only locally.
