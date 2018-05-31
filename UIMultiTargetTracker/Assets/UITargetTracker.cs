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
 * An active tracker on an object. 
 */
public class UITargetTracker : MonoBehaviour {
    
    // Defines how much % the tracker rectangle is enlarged compared to the 3D screen object bounds
	public float rectanglePaddingMultiplier=0.1f;

	// The object to track
	public GameObject objectToTrack;

    /*
     * Called every frame.
     * As long as the instance exist, we supposed here that the display must happen. 
     * The various conditions on visibility et al are calculated in the manager.
     */
    private void Update() {
        
		if (!objectToTrack)
			return; 
		
		// New tracker rectangle size
		Rect visualRect = TargetObjectBoundsToScreenSpace(objectToTrack.GetComponentInChildren<Renderer>());

		// Parent UI component position on the target object visual 2D UI position (anchor is 0,0)
		RectTransform rt = GetComponent<RectTransform>();
		rt.position = new Vector2(visualRect.xMax, visualRect.yMin);

		// Adjust all child elements. The tracker rectangle image to the size of the visual 2D bounds and
		// the other object statically on the side, attacked to the rectangle
		foreach (Transform child in transform) {
            
            // Make all child elements visible
			// The parent UI components itself is always active, otherwise it does not receive update events
			child.gameObject.SetActive(true);

			float xPadding = visualRect.width * rectanglePaddingMultiplier;
			float yPadding = visualRect.height * rectanglePaddingMultiplier;

			if (child.name == "RectangleImage")	{
				// Resize the tracker sprite rectangle
				rt = child.GetComponent<RectTransform>();
				rt.position = new Vector2(visualRect.xMin - xPadding, visualRect.yMin - yPadding);
				rt.sizeDelta = new Vector2(visualRect.width + xPadding * 2, visualRect.height + yPadding * 2);
			}
			else {
				// Reposition the other objects (texts and sprites) beside the tracker rectangle
				rt = child.GetComponent<RectTransform>();
				rt.position = new Vector2(visualRect.xMin + visualRect.width + xPadding, rt.position.y);
			}
		}

	}
		
        
    /*
     * Project the 3D scene object towards the 2D UI canvas, then take the most left, the most right, the most up and the most 
     * down 2D points of all to determine the outer visual bounds on the UI overlay of the object. This is the size of the 
     * rectangle "visually" surrounding the object when drawn on the UI plane.
     */
	private Rect TargetObjectBoundsToScreenSpace(Renderer r) {
        
        // Object visual rectangle in world space
        Bounds b = r.bounds;
        
        // Calculate the screen space rectangle surrounding the 3D object
        Camera c = Camera.main;
		Rect rect = Rect.zero;
        Vector3 screenSpacePoint;

		screenSpacePoint = c.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
		AdjustRect(ref rect, screenSpacePoint, true);
        screenSpacePoint = c.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
		AdjustRect(ref rect, screenSpacePoint);
		screenSpacePoint = c.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
		AdjustRect(ref rect, screenSpacePoint);
		screenSpacePoint = c.WorldToScreenPoint(new Vector3(b.center.x + b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
		AdjustRect(ref rect, screenSpacePoint);
		screenSpacePoint = c.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z + b.extents.z));
		AdjustRect(ref rect, screenSpacePoint);
		screenSpacePoint = c.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y + b.extents.y, b.center.z - b.extents.z));
		AdjustRect(ref rect, screenSpacePoint);
		screenSpacePoint = c.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z + b.extents.z));
		AdjustRect(ref rect, screenSpacePoint);
		screenSpacePoint = c.WorldToScreenPoint(new Vector3(b.center.x - b.extents.x, b.center.y - b.extents.y, b.center.z - b.extents.z));
		AdjustRect(ref rect, screenSpacePoint);

		return rect;
    }
    
	private void AdjustRect(ref Rect rect,Vector3 pnt,bool firstCall=false) {
		if (firstCall) {
			rect.xMin=pnt.x;
            rect.yMin = pnt.y;
			rect.xMax = pnt.x;
			rect.yMax = pnt.y;
		}
		else {
			rect.xMin = Mathf.Min(rect.xMin, pnt.x);
			rect.yMin = Mathf.Min(rect.yMin, pnt.y);
			rect.xMax = Mathf.Max(rect.xMax, pnt.x);
			rect.yMax = Mathf.Max(rect.yMax, pnt.y);
        }
	}

    /*
     * Resets the instance, generally invoked when put back in pool or when taken out of pool.
     */
	public void ResetState(GameObject trackerTarget=null) {
        
		// Set anchor and pivot points of main and child objects to overwrite bad configuration the user made in the inspector.
        // The Y size of sub-elements other than the rectangle is taken from the inspector and not modified.
        RectTransform rt = this.GetComponent<RectTransform>();
        rt.position = Vector2.zero; // left bottom corner
        rt.anchorMax = Vector2.zero;
        rt.anchorMin = Vector2.zero;
        rt.pivot = Vector2.zero;

        foreach (Transform child in transform) {
            
            child.gameObject.SetActive(false);

            rt = child.GetComponent<RectTransform>();
            rt.anchorMax = Vector2.zero; // left bottom 
            rt.anchorMin = Vector2.zero;
            rt.pivot = Vector2.zero;
        }

		objectToTrack = trackerTarget;
	}

}
