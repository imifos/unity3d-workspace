# Unity 3D - Simple Object Bouncing/Acceleration/Glue Behaviour

One single execution behaviour (script) on the player (red) and a property definition script on each obstacle (green) make the entire physics ping-pong happen. There is no additional code for movement in this scene other than the force applied each time the player hits the green obstacle. 

Currently, the script supports active bouncing (flipper), glue on contact with timer and sideways acceleration, defining the velocity amplification factor individually for each obstacle. The script also supports (inspector set) callbacks for all major events in order to adapt animation, sounds etc. All is based on physics and forces in order to make it "realistic" in the play flow.

Code in: 

* Assets/ObjectVelocityModifier2DController.cs - Added to the ball/player
* Assets/ObjectVelocityModifier2DProperties.cs - Added to each plattform/obstacle, and configured via Inspector

[![Demo Video](https://img.youtube.com/vi/fQ4BZjC4a6o/0.jpg)](https://www.youtube.com/watch?v=fQ4BZjC4a6o "Demo Video")
