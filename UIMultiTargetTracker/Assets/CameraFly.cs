using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Just a very simple script to allow flying around the scene.
 * RotateHorizontal/RotateVertical are not defaults and have to be added manually.
 */
public class CameraFly : MonoBehaviour
{

	public float lookSpeed = 15.0f;
	public float moveSpeed = 15.0f;

	float rotationX = 0.0f;
	float rotationY = 0.0f;

	private void Start() {
		transform.rotation = Quaternion.Euler(0, 0, 0);
		transform.position = new Vector3(0, 20, -50);
	}

	void Update() {
        
        transform.position += transform.forward * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
		transform.position += transform.right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;

		rotationX += lookSpeed * Input.GetAxis("RotateHorizontal") * Time.deltaTime;
		rotationY += lookSpeed * Input.GetAxis("RotateVertical") * Time.deltaTime;
		transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
	}

}
