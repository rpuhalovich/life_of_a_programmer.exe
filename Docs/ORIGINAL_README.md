[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-f059dc9a6f8d3a56e377f745f24479a46679e63a5d9fe6f495e02850cd0d8118.svg)](https://classroom.github.com/online_ide?assignment_repo_id=446148&assignment_repo_type=GroupAssignmentRepo)

**The University of Melbourne**

# COMP30019 – Graphics and Interaction

Final Electronic Submission (project): **4pm, November 1**

Do not forget **One member** of your group must submit a text file to the LMS (Canvas) by the due date which includes the commit ID of your final submission.

You can add a link to your Gameplay Video here but you must have already submit it by **4pm, October 17**

# Project-2 README

You must modify this `README.md` that describes your application, specifically what it does, how to use it, and how you evaluated and improved it.

Remember that _"this document"_ should be `well written` and formatted **appropriately**. This is just an example of different formating tools available for you. For help with the format you can find a guide [here](https://docs.github.com/en/github/writing-on-github).

**Get ready to complete all the tasks:**

- [x] Read the handout for Project-2 carefully.

- [ ] Brief explanation of the game.

- [ ] How to use it (especially the user interface aspects).

- [ ] How you designed objects and entities.

- [ ] How you handled the graphics pipeline and camera motion.

- [ ] The procedural generation technique and/or algorithm used, including a high level description of the implementation details.

- [ ] Descriptions of how the custom shaders work (and which two should be marked).

- [ ] A description of the particle system you wish to be marked and how to locate it in your Unity project.

- [ ] Description of the querying and observational methods used, including a description of the participants (how many, demographics), description of the methodology (which techniques did you use, what did you have participants do, how did you record the data), and feedback gathered.

- [ ] Document the changes made to your game based on the information collected during the evaluation.

- [ ] References and external resources that you used.

- [ ] A description of the contributions made by each member of the group.

## Table of contents

- [Team Members](#team-members)
- [Explanation of the game](#explanation-of-the-game)
- [Technologies](#technologies)
- [Using Images](#using-images)
- [Code Snipets ](#code-snippets)

## Team Members

| Name           |     Task      |    State |
| :------------- | :-----------: | -------: |
| Student Name 1 |   MainScene   |     Done |
| Student Name 2 |    Shader     |  Testing |
| Student Name 3 | README Format | Amazing! |

## Explanation of the game

Our game is a first person shooter (FPS) that....

You can use emojis :+1: but do not over use it, we are looking for professional work. If you would not add them in your job, do not use them here! :shipit:

## Technologies

Project is created with:

- Unity 2021.1.13f1
- Ipsum version: 2.33
- Ament library version: 999

## Using Images

You can use images/gif by adding them to a folder in your repo:

<p align="center">
  <img src="Gifs/Q1-1.gif"  width="300" >
</p>

To create a gif from a video you can follow this [link](https://ezgif.com/video-to-gif/ezgif-6-55f4b3b086d4.mov).

## Code Snippets

You can include a code snippet here, but make sure to explain it!
Do not just copy all your code, only explain the important parts.

```c#
public class firstPersonController : MonoBehaviour
{
    //This function run once when Unity is in Play
     void Start ()
    {
      standMotion();
    }
}
```

## Using the auto formatter

Install astyle using Chocolatey (https://chocolatey.org/install) using `choco install -y astyle`.

Then run `.\Scripts\astyle.bat`.

## Resources

Creating A Dash Ability: https://www.youtube.com/watch?v=QyqSoz2ivOk

Kenney Prototype Textures: https://www.kenney.nl/assets/prototype-textures

Lens Dirt: https://gitlab.labranet.jamk.fi/K8721/unity/-/tree/6dd55de75c1b9f1a7239dab5ec6ffc2badfa09d7/SurvivalShooter/Assets/PostProcessing/Textures/Lens%20Dirt

Footsteps: https://freesound.org/people/Disagree/sounds/433725/
