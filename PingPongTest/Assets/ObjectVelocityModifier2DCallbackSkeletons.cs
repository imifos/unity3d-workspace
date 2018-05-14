
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

public class ObjectVelocityModifier2DCallbackSkeletons : MonoBehaviour
{
	public GameObject player;

    public void onIsHit()
	{
		Debug.Log("Reset position to Start");
		/* Optional as the test setting turns out to be a closed cirtuit :)
		player.transform.position = new Vector3(-3.98f,6.36f,0);
		Rigidbody rb = player.GetComponent<Rigidbody>();
		rb.velocity = Vector3.zero;
		*/
    }

	public void onIsGlued()
	{
		Debug.Log("event");
	}
	public void onGlueReleased()
	{
		Debug.Log("event");
	}
	public void onHasBounced()
	{
		Debug.Log("event");
	}
	public void onIsAccelerated()
	{
		Debug.Log("event");
	}

}
