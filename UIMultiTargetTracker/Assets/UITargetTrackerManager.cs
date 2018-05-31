using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
 * Manages the tracker UI object pool, the registry of objects allowing themselves
 * to be targeted and everything around tracking.
 */
public class UITargetTrackerManager : MonoBehaviour
{

	private class TargetDescriptor
	{
		public UITargetTrackerTarget target;
		public GameObject tracker; // ...or null if no tracker is attached. See UpdateTargetStatesTicker()
	}

	// To be assigned in the inspector
	public GameObject UITargetTrackerPrefab;

	// Static singleton instance reference
	public static UITargetTrackerManager Instance { get; private set; }

	// List of ALL registered target objects, with tracker attached or not.
	private List<TargetDescriptor> targetObjectRegistry = new List<TargetDescriptor>();

	// Tracker UI instances pool
	private Stack<GameObject> trackerInstancePool = new Stack<GameObject>(20);

	/*
     * 
     */
	private void Awake() {
		// Handle single-tone aspect
        if (UITargetTrackerManager.Instance != null && UITargetTrackerManager.Instance != this)
			throw new Exception("This script can only exist once in the scene.");
        UITargetTrackerManager.Instance = this;
	}

	/*
     * 
     */
	private void Start() {
		// Calculate xxx of potential targets every N seconds.
		// As the targets are supposed to be far away, there is no need to do this every frame.
		// The required frequency depends on the speed of the observer and the require
        // reactivity of the trackers. Call to the ticker method can also by placed in Update().
		InvokeRepeating("UpdateTargetStatesTicker", 0, 0.5f);
	}

	/*
     * Goes of all registered targets and verifies if they are elligitable for having a target tracker attached.
     */
	private void UpdateTargetStatesTicker() {
		
        // All potential target objects we know of
        foreach (TargetDescriptor descr in targetObjectRegistry) {

			bool needTracker = false;

			// Yes, is it closer than the minimum distance required to be tracked?
			if (Vector3.Distance(descr.target.transform.position, Camera.main.transform.position) < descr.target.minimumDistanceToBeTracked) {
				// Yes, is it on front of camera?
				Vector3 targetScreenPoint = Camera.main.WorldToViewportPoint(descr.target.transform.position);
				if (targetScreenPoint.z > 0 && targetScreenPoint.x > 0 && targetScreenPoint.x < 1 && targetScreenPoint.y > 0 && targetScreenPoint.y < 1)
                    // Yes, add a tracker, otherwhise remove
					needTracker = true;
			}

			if (needTracker && descr.tracker == null) {
				descr.tracker = AllocateInstance(descr.target.gameObject); // from pool
			}
			else if (!needTracker && descr.tracker != null) {
				ReleaseInstance(descr.tracker); // back to pool
				descr.tracker = null;
			}
		}
	}

	/*
     * Takes a tracker instance from the pool. If the pool is empty, a new instance is created, which will then later
     * be put back into the pool. By this, the pool builds up to the maximum amount of trackers simultaneously displayed at one moment.
     */
	private GameObject AllocateInstance(GameObject target) {

		GameObject go;
		if (trackerInstancePool.Count > 0)
			go = trackerInstancePool.Pop();
		else
			go = Instantiate(UITargetTrackerPrefab, this.gameObject.transform);

		go.GetComponent<UITargetTracker>().ResetState(target);

		return go;
	}

	/*
     * Reset a tracker instance and places it back in the pool.
     */
	private void ReleaseInstance(GameObject tracker) {
		tracker.GetComponent<UITargetTracker>().ResetState();
		trackerInstancePool.Push(tracker);
	}

	/*
     * Removes all objects from the pool that built-up over time. The objects will then be garbage collected.
     * There is no need to call the method if the pool remains in a reasonable size.
     */
	private void ClearPool() {
		trackerInstancePool.Clear();
	}


	/*
     * Invoked by a game object to mark itself as target. 
     * By doing this, as soon as it satisfies the "show target tracker" condition,
     * the tracker is displayed in the UI canvas.
     * The simplest way to have objects invoking this method is be assigning the 
     * "UITargetTrackerTarget" behaviour, ie. adding the script as component.
     */
	public void RegisterAsTarget(UITargetTrackerTarget target) {
		TargetDescriptor td = new TargetDescriptor();
		td.target = target;
		td.tracker = null; // tracker gets assigned when conditions are met, see UpdateTargetStates()

		targetObjectRegistry.Add(td);
	}

	public void UnregisterAsTarget(UITargetTrackerTarget target) {
		foreach (TargetDescriptor descr in targetObjectRegistry)
			if (descr.target == target) {
				targetObjectRegistry.Remove(descr);
				return;
			}
	}

}
