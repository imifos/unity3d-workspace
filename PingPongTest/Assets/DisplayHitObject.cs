using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHitObject : MonoBehaviour {

	public Text text;
	private GameObject lastCollidedObject = null;
	private int counter = 0;
    
	private void OnCollisionEnter(Collision c)
	{
		if (c.gameObject == lastCollidedObject)
            return;
        lastCollidedObject = c.gameObject;

		counter++;
		if (counter>30) {
			counter = 0;
			text.text = "";
		}
        
		text.text+=" > "+c.gameObject.name;
	}
}
