using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

/*
 * When added to a GameObject, registers this object as "trackable". 
 */
public class UITargetTrackerTarget : MonoBehaviour {

	public float minimumDistanceToBeTracked = 100f;

    void Awake () {
		UITargetTrackerManager uiTargetTrackerManager = Object.FindObjectOfType<UITargetTrackerManager>();
		if (uiTargetTrackerManager == null)
			Debug.LogError("UITargetTrackerManager not found in scene. Can't register myself as tracker target, " + this.name);
		else
			uiTargetTrackerManager.RegisterAsTarget(this);
	}

}
