using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPointerToObjectSelectionMapper : MonoBehaviour {

	public GameObject selectedObject;

    void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            selectedObject=hitInfo.transform.root.gameObject;
        }
        else
        {
			selectedObject = null;
        }
	}
}
