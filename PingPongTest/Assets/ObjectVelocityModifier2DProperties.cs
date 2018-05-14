
using UnityEngine;
using UnityEngine.Events;

/*
Programmed by 2018 Tasha CARL / @imifos

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

public class ObjectVelocityModifier2DProperties : MonoBehaviour {

	[System.Serializable]
	public enum Direction
	{
		LEFT, RIGHT, AWAY, TOFRONT
	}

	[System.Serializable]
	public class Property
    {
		// General
        // -------
		[Header("General")]

        // Addressed specific object tag or none for default
		[Tooltip("Target object type by tag, or empty to use as default. Specific tag has priority over default.")]
		public string tagNameOrEmpty="";

        // Ignore Behaviour
        [Tooltip("FALSE to use the bouncing properties, TRUE to apply standard physics and 'neutralise' the bouncing. Same as factor 0.")]
		public bool ignoreTheseModifiers=false;
                
        // Bouncing Behaviour
        // ------------------
		[Header("Bouncing Behaviour")]
		public bool doBounce = false;

		[Tooltip("Multiplication of incoming velocity to calculate outgoing bouncing velocity.")]
        public float bounceVelocityFactor=2f;

        // Glue Behaviour
        // --------------
		[Header("Glue Behaviour")]
        public bool doGlue = false;
        public float glueTime = 2f;

		[Tooltip("TRUE to preserve velocity from before glue full-stop and restore it afterwards.")]
        public bool gluePreserveVelocity = false;

		// Acceleration Behaviour
        // ----------------------
		[Header("Accelerate Behaviour")]
		public bool doAcceleration = false;

		public Direction accelerationDirection=Direction.LEFT;

		[Tooltip("Multiplication of incoming velocity to calculate outgoing acceleration velocity.")]
        public float accelerationVelocityFactor = 2f;

        // Callbacks
        // ---------
		[Header("Event Callbacks")]
        public UnityEvent onIsHit;
        public UnityEvent onIsGlued;
        public UnityEvent onGlueReleased;
        public UnityEvent onHasBounced;
        public UnityEvent onIsAccelerated;
    }

	[Header("Object Behaviour Properties")]
    public Property[] properties;
   


    public Property ForTag(string name)
    {
		int defaultIndex = -1;
		int foundIndex = -1;

        // Search properties
		for (int i = 0; i < properties.Length; i++)
		{
			if (properties[i].tagNameOrEmpty == name)
				foundIndex = i;
			if (properties[i].tagNameOrEmpty == "")
				defaultIndex = i;
		}

        // Found a definition for tag
		if (foundIndex != -1)
		{
			if (properties[foundIndex].ignoreTheseModifiers)
				return null; // This tag is set to not to apply modifiers
			else
				return properties[foundIndex];
		}

		// Found a default definition
		if (defaultIndex != -1)
		{
			if (properties[defaultIndex].ignoreTheseModifiers)
				return null;
			else
				return properties[defaultIndex];
		}

		// Found nothing, caller should ignore.
		return null; 
    }
}
