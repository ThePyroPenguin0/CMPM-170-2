# Gravitas

## Concept
Gravitas is a 2D platforming game prototype which is designed to invoke a sense of annoyance within the player by "glitching" gravity at random points as the player tries to complete a platforming level. The actual sense of annoyance comes from two mechanics. First, the reorientation of the level as gravity "glitches" can lead to a previously safe jump or move being suddenly lethal. This is an intended mechanic and there is probably some Art<sup>TM</sup> thing that can be made out of that. The second (and actually interesting) mechanic is the "locking" of the controls to the orientation the player character starts at.

## What the fuck does that mean
### Gravity glitches
"Glitching" gravity means that at (pseudo)random points in the game, the orientation of gravity (to be modelled by rotating the level) will switch without warning. This will not be planned or scheduled, and as a result, objects that were previously walls and ceilings could at any moment become floors instead.
### Control locking
Locking the controls to the player's original orientation means that changes in the player's perspective of the game are only represented on the game's *screen*. Suppose gravity "rotates" 90 degrees clockwise. The player will now find themselves visually perceiving the level as though the left wall was the floor, with the former floor being the right wall of the level. However, the control scheme (either WASD or arrow keys) will not change to reflect this shift in perspective, and so the controls (from the player's perspective) will also rotate 90 degrees - up will now be left, down is right, left is duck, and right is jump.
This mechanic is to follow all changes in gravity and the player will be expected to keep up in real time. Is it hard? Probably. Is that our problem? Only if we're playing it.

## Credits
Ayush Bandopadhyay - Artwork  
Joseph Coulson - Mechanical design  
Luc Harnist - Level Design  
Gabriel Lipow - Mechanical design

## Assets used:
- "Irritating" song: "Microchip" by Oliver Buckland
- "Serene" song: "Path to Lake Land" by Alexandr Zhelanov
- Arrow keys sprite: https://www.kindpng.com/imgv/hRxibhJ_pixel-arrow-keys-png-transparent-png/
- Drawn pixel art: Drawn on pixil.com
