# DirectedMemories
It's a maze, it's a museum, there are ghosts, there are memories

##Motivation & Background
This is being developed as part of the final project for **CS 498sl - Virtual Reality** at the **University of Illinois at Urbana - Champaign** in the Spring 2016 Semester. This is the pilot offering of this class, and is instructed by Steve LaValle.

We have two main motivations behind this project. The first of these is the technical problem of spacial audio. The implementation of audio in virtual reality has been attempted but not polished. Current audio engines are built for games with short, nearby sound effects (e.g, gunfire or splashing) and background music. To have a richer virtual reality experience, it must be possible to create richer audio that mimics real-world audio effects in real time. Publicly available research in spatial stereo audio is easy to find, however the publicly available implementations of spacial audio are unremarkable. The best available implementation we’ve found so far is RealSpace3D Audio, a plug in for Unity 3D; however, we did not find this sufficiently robust for our project. We therefore plan to use this as reference material while implementing our own algorithm for spatial audio.

The second is that of lingering horror. We often find people trying to convey horrific memories, such as those of wars, and similar tragic events. However this sentiment is rarely felt by the person receiving this communication as the magnitude of horror involved is often far greater than what the revier imagines. Usually, the feeling of horror only sets in when a more power stimulus is offered. This is usually the case with well crafted books, movies, and video games. We plan on exploring this effect in VR by providing similar stimuli.

##Description
Let’s start with a standard hedge maze. These mazes are usually either quite small or are large and have clues to help solve them. This is usually the case to help ensure that the maze can be completed in a reasonable amount of time. Our maze will similarly be quite large, and will have an invisible point source of audio that the user must try to navigate towards. This audio is a narration of some event by an invisible ghost. Users will use the three dimensional nature of the audio to help navigate towards the ghost.

Our maze is three dimensional one that users fly through and in conjunction with the ghost’s narrative, the user can also look to his sides to view some stunning visuals that compliment what the ghost is saying. This setup is akin to the tesseract room from the movie - Interstellar (img: http://i.imgur.com/ci47vkl.png).
We plan to use either Unity 3D or Unreal Engine 4 for this project, depending on what we find provides our intended aesthetics. Furthermore, we plan on using RealSpace3D Audio as reference material for implementing our spatial sound algorithm.

##Deliverables
The main product is a set of scripts or plugin that creates richer audio. Specifically, we aim to mimic echoes, diffusion, direction, reverberation, and other real-world audio effects in real time. To demonstrate this product, we are developing the user experience described above in either Unity 3D or Unreal Engine 4..
All items to be delivered will be software.

##Human Factors
There is only a short, half-page section on audio in the Oculus Best Practices guide. The length of the section shows that audio in VR is not being exploited to its full potential. The only concrete recommendation is to use stereo sound which is a rather obvious consideration for VR. Our project will go above and beyond this.

Most of the motion will take place on a one-person boat that flies through the air. The rules of VR acceleration are therefore applicable. Namely, no motion at all is preferable to constant velocity, constant velocity preferable to instantaneous acceleration, and instantaneous acceleration to constant acceleration. 

There is one convention we will deliberately break. We plan to put picture frames along the walls, but instead of a picture, or perhaps a window into another physical room, it is an opening to another location or world, so that two or more picture frames next to each other seem to fill each other's space. However, this is a somewhat common trope in cinematography, so we do not expect much discomfort.

##Collaborators

|Name|Github|Website|
|----|--------|-----|
| Abhishek Modi | [modi95](https://www.github.com/modi95) |http://www.akmodi.com|
|Eric Ahn|[wchill](https://www.github.com/wchill)|http://www.intense.io|
|Mark Miller| [MrMallIronMaker](https://www.github.com/MrMallIronmaker)| |

Printable document at http://goo.gl/RMRTz8
