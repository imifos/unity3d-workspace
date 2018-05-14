using System.Collections;
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

public class ObjectVelocityModifier2DController : MonoBehaviour {
    
	private Rigidbody myBody;
	private Vector3 velocityBeforeImpact;
	private Vector3 velocityBeforeGlue;
	private GameObject lastCollidedObject=null;
            

    void Start()
    {
        myBody = GetComponent<Rigidbody>();
    }
        
    void FixedUpdate()
    {
		// If we need the original velocity, we need to keep the velocity because 
		// the physics engine re-sets the velocity on impact according to physics, 
		// so in "OnCollisionEnter()", we don't have access to this 'pure' velocity anymore.
		velocityBeforeImpact = myBody.velocity;
    }

    private void OnCollisionEnter(Collision c)
    {
		// The glue effect (coroutine) provoques multiple calls on same object, 
        // so we need to block this.
		if (c.gameObject==lastCollidedObject) {
			return;
		}
		lastCollidedObject = c.gameObject;
              
        // Get object bouncing properties from object hit
        // If there are no reflection properties, just let the physics engine do it thing
        ObjectVelocityModifier2DProperties properties = c.gameObject.GetComponent<ObjectVelocityModifier2DProperties>();
        if (properties == null)
            return;
        
		ObjectVelocityModifier2DProperties.Property p = properties.ForTag(gameObject.tag);
		if (p == null)
            return;
        
		if (p.onIsHit != null)
			p.onIsHit.Invoke();
		
        // Apply effects
		DoBounce(c,p);
		DoGlue(c,p);
		DoAcceleration(c, p);
    }

    /*
     * Applies the acceleration effect.
     */
	private void DoAcceleration(Collision c, ObjectVelocityModifier2DProperties.Property appliedProperty)
	{
		if (!appliedProperty.doAcceleration)
            return;

		Vector3 direction=Vector3.zero;
		switch(appliedProperty.accelerationDirection) {
			case ObjectVelocityModifier2DProperties.Direction.AWAY:
				direction = Vector3.right;
				break;
			case ObjectVelocityModifier2DProperties.Direction.LEFT:
				direction = Vector3.forward;
                break;
			case ObjectVelocityModifier2DProperties.Direction.RIGHT:
				direction = Vector3.back;
                break;
			case ObjectVelocityModifier2DProperties.Direction.TOFRONT:
				direction = Vector3.left;
                break;
        }
		myBody.AddForce(Quaternion.AngleAxis(90, direction) * c.contacts[0].normal * appliedProperty.accelerationVelocityFactor, ForceMode.Impulse);
		          
		if (appliedProperty.onIsAccelerated != null)
			appliedProperty.onIsAccelerated.Invoke();
	}


	/*
     * Applies the glue effect.
     */
	private void DoGlue(Collision c, ObjectVelocityModifier2DProperties.Property appliedProperty)
	{
		if (!appliedProperty.doGlue)
            return;
        
		IEnumerator coroutine = UnglueAfter(appliedProperty);
        StartCoroutine(coroutine);
	}

	private IEnumerator UnglueAfter(ObjectVelocityModifier2DProperties.Property appliedProperty)
    {
        // ** Start-up/Initialisation

		// Keep original physics velocity
        velocityBeforeGlue = myBody.velocity;

        // Documentaion: If isKinematic is enabled, Forces, collisions or joints will not affect the rigidbody anymore. 
        // The rigidbody will be under full control of animation or script control by changing transform.position. 
        //Kinematic bodies also affect the motion of other rigidbodies through collisions or joints. 
        myBody.isKinematic = true;
        myBody.velocity = Vector3.zero;

		//if (p.onIsGlued!=null)
		//	p.onIsGlued.Invoke();

        // ** Yield flow
		yield return new WaitForSeconds(appliedProperty.glueTime);

        // ** Delayed invocation
		myBody.isKinematic = false;
		if (appliedProperty.gluePreserveVelocity)
		  myBody.velocity = velocityBeforeGlue;

		if (appliedProperty.onGlueReleased!=null)
			appliedProperty.onGlueReleased.Invoke();
    }

	/*
     * Applies the bouncing effect.
     * https://answers.unity.com/questions/580867/issue-using-vector3reflect-to-bounce-a-ball.html
     */
	private void DoBounce(Collision c,ObjectVelocityModifier2DProperties.Property appliedProperty)
	{
		ContactPoint cp = c.contacts[0];

		if (!appliedProperty.doBounce)
			return;
		
		// Method 1:
        // Add velocity in direction of hit objects normale multiplied by factor. The object flies then according to physics.
        // This is the same than method 3.
        //myBody.velocity = velocityBeforeImpact + cp.normal * p.velocityFactor * velocityBeforeImpact.magnitude;

        // Method 2:
        // - Calculate the reflection angle (incoming velocity to touched object normale)
        // - Add an applification
        // (not used) myBody.velocity = Vector3.Reflect(velocityBeforeImpact, cp.normal);
        //myBody.velocity += cp.normal * bounceForce;

        // Method 3:
        // Using the physics engine.
		myBody.AddForce(c.contacts[0].normal * appliedProperty.bounceVelocityFactor, ForceMode.Impulse);
	}
}
