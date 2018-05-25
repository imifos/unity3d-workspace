# Unity 3D - Simple Object Bouncing/Acceleration/Glue Behaviour

The behaviour is realised by setting a simple execution behaviour (script) on the player (red) and a property definition script on each obstacle (green) make the entire physics-based ping-pong happen. There is no additional code for movement in this scene other than the forces applied each time the player hits the green obstacles. 

Currently, the script supports active bouncing (flipper effect), a glue on contact with timer and a sideways acceleration, defining the velocity amplification factor individually for each obstacle. The script also supports (set in inspector) callbacks for all major events in order to adapt animation, sounds etc. Everything is based on physics and forces in order to make it "realistic" in the play flow. 

Code in: 

* Assets/ObjectVelocityModifier2DController.cs - Added to the ball/player
* Assets/ObjectVelocityModifier2DProperties.cs - Added to each plattform/obstacle, and configured via Inspector

[![Demo Video](https://img.youtube.com/vi/fQ4BZjC4a6o/0.jpg)](https://www.youtube.com/watch?v=fQ4BZjC4a6o "Demo Video")
