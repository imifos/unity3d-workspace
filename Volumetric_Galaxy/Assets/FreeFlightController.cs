using UnityEngine;

/*
Very simple script to allow flying around the scene.
RotateHorizontal (qd) / RotateVertical (zx) / Advance (aw) are no defaults and have to be added in Project Settings.
Cursor keys for up/down/left/right.

@Imifos
License: Public domain, use on your own risk.
*/
public class FreeFlightController : MonoBehaviour {

    public float lookSpeed = 15.0f;
    public float moveSpeed = 150.0f;

    float rotationY = 0.0f;
    float rotationX = 90.0f;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(130f, 0, 0);
        transform.position = new Vector3(140f, 650f /* above galaxy plane */, 400f /* towards the other galaxy edge */);

        //transform.position = new Vector3(0f, -4.262524f, -193.387f );
        //transform.LookAt(new Vector3(0, 0, 0));
    }

    void Update()
    {
        transform.position -= transform.up * moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
        transform.position += transform.right * moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.position += transform.forward * moveSpeed * Input.GetAxis("Advance") * Time.deltaTime;

        rotationY += lookSpeed * Input.GetAxis("RotateHorizontal") * Time.deltaTime;
        rotationX += lookSpeed * Input.GetAxis("RotateVertical") * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
    }

}

