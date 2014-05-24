using UnityEngine;
using System.Collections;

public class RunnerController : RunnerAnimationController {

	public float jumpHeight = 2f;
	public float jumpTime = 1f;
	public float maxSpeed = 6f;
	public float acceleration = 4f;
	public float turnSpeed = 1f;
	public float turnDistance = 1f;

	private Vector3 moveForce;
	private float jumpStartTime = 0f;
	private float currentSpeed = 0f;
	private float turnWay = 0f;



	public void StartRunning() {
		IsRunning = true;
	}



	public void TurnRight() {
		if(CanTurn){
			turnWay = turnDistance;
			IsTurningRight = true;
		}
	}



	public void TurnLeft() {
		if(CanTurn){
			turnWay = -turnDistance;
			IsTurningLeft = true;
		}
	}



	public void Update() {

		DoRun ();

		if(IsInJump)
			DoJump();

		if(IsInTurn){
			DoTurn();
		}
	}



	public void LateUpdate() {
		IsTurningLeft = false;
		IsTurningRight = false;
		IsJumping = false;
		IsSliding = false;

	}



	public void Jump() {
		if(CanJump){
			IsJumping = true;
			jumpStartTime = Time.time;
		}
	}


	private bool IsInJump {
		get {
			return Time.time - jumpStartTime < jumpTime && jumpStartTime != 0;
		}
	}



	private bool IsJumpLanding {
		get {
			return Time.time - jumpStartTime > jumpTime / 2f;
		}
	}



	private void DoJump() {
		if(IsJumpLanding){
			float delta = Mathf.Min(Time.deltaTime, Mathf.Abs(Time.time - jumpStartTime - jumpTime));
			Debug.Log(string.Format("Landing delta = {0} Time.deltaTime = {1} jumpLeftTime = {2}", delta, Time.deltaTime,
			                        Mathf.Abs(Time.time - jumpStartTime - jumpTime)));
			CachedTransform.Translate(Vector3.down * (jumpHeight / jumpTime) * delta);
		} else {
			float delta = Mathf.Min(Time.deltaTime, Mathf.Abs(Time.time - jumpStartTime - jumpTime / 2f));
			Debug.Log(string.Format("Rising delta = {0} Time.deltaTime = {1} jumpLeftTime = {2}", delta, Time.deltaTime,
			                        Mathf.Abs(Time.time - jumpStartTime - jumpTime / 2f)));
			CachedTransform.Translate(Vector3.up * (jumpHeight / jumpTime) * delta);
		}
	}



	private void DoRun() {
		currentSpeed += acceleration * Time.deltaTime;
		CachedTransform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
	}



	private bool IsInTurn {
		get{
			return turnWay != 0;
		}
	}



	private bool CanTurn {
		get{
			return !IsInJump && !IsInTurn && !IsInSlide;
		}
	}



	private bool CanJump {
		get {
			return !IsInJump && !IsInTurn && !IsInSlide;
		}
	}



	private void DoTurn() {
		float distance = Mathf.Min(turnSpeed * Time.deltaTime, Mathf.Abs(turnWay)) * Mathf.Sign(turnWay);
		CachedTransform.Translate(Vector3.right * distance);
		turnWay -= distance;
	}



	public void Slide(){
		if(CanSlide)
			IsSliding = true;
	}



	private bool CanSlide {
		get {
			return !IsInJump && !IsInTurn && !IsInSlide;
		}
	}



	private bool IsInSlide {
		get {
			return IsSliding;
		}
	}


	#region Collider
	
	private void OnCollisionEnter(Collision collisionInfo){
	}



	private void OnCollisionStay(Collision collisionInfo){
	}



	private void OnCollisionExit(Collision collisionInfo){
	}
	
	#endregion
}
