# Unity 3D - UI Multi-Target Tracker 

A simple behaviour implementing an UI multi-target tracker on 3D objects approaching to a given distance. A frame is drawn using a 9-slice sprite and 2 texts and 2 images are added to simulate kind of indicators. The tracker pops-in and disappears with a slight delay because it's calculated every 0.5 second. This is fine when flying over landscapes and having tracked objects in distance. But this behaviour is optional.

In Action:

https://vimeo.com/272832220

Code in: 
* Assets/UITargetTrackerTarget.cs - Simple behaviour that makes a GameObject "trackable" by registering it in the manager.
* Assets/UITargetTrackerManager.cs - Manages UI obejct pool, objects to track and when a tracker is added o removed.
* Assets/UITargetTracker.cs - Tracker UI element


