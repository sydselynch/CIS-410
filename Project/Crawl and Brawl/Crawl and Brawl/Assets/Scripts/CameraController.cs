using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public bool shakeOnStart = false;//Test-run/Call ShakeCamera() on start
	public float initShakeAmount;
	public float initShakeDuration;
	private float shakeAmount;//The amount to shake this frame.
	private float shakeDuration;//The duration this frame.
	private int count;
	//Readonly values...
	float shakePercentage;//A percentage (0-1) representing the amount of shake to be applied when setting rotation.
	float startAmount;//The initial shake amount (to determine percentage), set when ShakeCamera is called.
	float startDuration;//The initial shake duration, set when ShakeCamera is called.
	public Vector3 endPoint;
	bool isRunning = false;	//Is the coroutine running right now?

	// cam movement;
	public float planeSize;
	private float endTime;
	private bool isMoving;
	public float cameraDelayTime;
	public float cameraSpeed;

	void Start() {
		isMoving = false;
		if(shakeOnStart) ShakeCamera ();
		endTime = Time.time;
	}
	private void Update(){
		if (Time.time - endTime >= cameraDelayTime && !isMoving) {
			endPoint = transform.position + new Vector3 (planeSize, 0f, 0f);
			isMoving = true;
		}
		moveCamera ();
	}

	private void FixedUpdate(){
		
		}

		
	public void ShakeCamera() {
		shakeAmount = initShakeAmount;
		shakeDuration = initShakeDuration;
		startAmount = shakeAmount;//Set default (start) values
		startDuration = shakeDuration;//Set default (start) values

		if (!isRunning) StartCoroutine (Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
	
	}
	public void moveCamera(){
		if (isMoving) {
			if (!Vector3.Equals (endPoint, transform.position)) {
				transform.position = Vector3.MoveTowards (transform.position, endPoint, cameraSpeed);
			} else {
				endTime = Time.time;
				isMoving = false;
			}
		}
	}

	public void ShakeCamera (float amount, float duration) {

		shakeAmount += amount;//Add to the current amount.
		startAmount = shakeAmount;//Reset the start amount, to determine percentage.
		shakeDuration += duration;//Add to the current time.
		startDuration = shakeDuration;//Reset the start time.
		if(!isRunning) StartCoroutine (Shake());//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
	}
		
	IEnumerator Shake() {
		isRunning = true;
		while (shakeDuration > 0.01f) {
			Vector2 rotationAmount = Random.insideUnitSphere * shakeAmount;//A Vector3 to add to the Local Rotation

			shakePercentage = shakeDuration / startDuration;//Used to set the amount of shake (% * startAmount).

			shakeAmount = startAmount * shakePercentage;//Set the amount of shake (% * startAmount).
			shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime);//Lerp the time, so it is less and tapers off towards the end.

			transform.localRotation = Quaternion.Euler (rotationAmount);//Set the local rotation the be the rotation amount.

			yield return null;
		}
		transform.localRotation = Quaternion.identity;//Set the local rotation to 0 when done, just to get rid of any fudging stuff.
		isRunning = false;
	}

}