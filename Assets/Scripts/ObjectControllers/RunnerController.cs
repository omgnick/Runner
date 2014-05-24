using UnityEngine;
using System.Collections;

public class RunnerController : RunnerAnimationController {

	public float jumpHeight = 2f;
	public float jumpSpeed = 1f;
	public float maxSpeed = 6f;
	public float acceleration = 4f;
	public float turnSpeed = 1f;
	public float turnDistance = 1f;

	private Vector3 moveDistance;
	private Vector3 moveSpeed;



	public RunnerController(){
		moveDistance = Vector3.zero;
		moveSpeed = Vector3.zero;
	}



	public void StartRunning() {
		IsRunning = true;
	}



	public void TurnRight() {
		if(CanTurn){
			moveDistance.x = turnDistance;
			moveSpeed.x = turnSpeed;
			IsTurningRight = true;
		}
	}



	public void TurnLeft() {
		if(CanTurn){
			moveDistance.x = -turnDistance;
			moveSpeed.x = -turnSpeed;
			IsTurningLeft = true;
		}
	}



	public void FixedUpdate() {
		ApplyForce();
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
			moveDistance.y += jumpHeight;
			moveSpeed.y += jumpSpeed;
		}
	}


	private bool IsInAir {
		get {
			return moveSpeed.y != 0;
		}
	}



	private bool IsJumpLanding {
		get {
			return moveSpeed.y < 0;
		}
	}



	private bool IsJumpTopPoint{
		get{
			return moveDistance.y == 0 && moveSpeed.y > 0;
		}
	}


	private void ApplyForce(){
		Vector3 move = Time.deltaTime * moveSpeed;

		//OX
		move.x = Mathf.Min(Mathf.Abs(move.x), Mathf.Abs(moveDistance.x)) * Mathf.Sign(moveDistance.x);
		moveDistance.x -= move.x;

		if(moveDistance.x == 0)
			moveSpeed.x = 0;


		//OY
		move.y = Mathf.Min (move.y, moveDistance.y);
		moveDistance.y -= move.y;

		if(IsJumpTopPoint) {
			moveSpeed.y = Config.world_gravity;
			moveDistance.y = 0;
		}

		//OZ
		moveSpeed.z = Mathf.Min(moveSpeed.z + acceleration * Time.deltaTime, maxSpeed);
			

		CachedTransform.Translate(move);
	}



	private bool IsInTurn {
		get{
			return moveSpeed.x != 0;
		}
	}



	private bool CanTurn {
		get{
			return !IsInAir && !IsInTurn && !IsInSlide;
		}
	}



	private bool CanJump {
		get {
			return !IsInAir && !IsInTurn && !IsInSlide;
		}
	}



	public void Slide(){
		if(CanSlide)
			IsSliding = true;
	}



	private bool CanSlide {
		get {
			return !IsInAir && !IsInTurn && !IsInSlide;
		}
	}



	private bool IsInSlide {
		get {
			return IsSliding;
		}
	}


	#region Collider
	
	private void OnCollisionEnter(Collision collisionInfo){

		foreach(ContactPoint point in collisionInfo.contacts){

			//AIR
			if(IsJumpLanding && point.normal.y > 0){
				moveSpeed.y = 0;
				moveDistance.y = 0;
				CachedTransform.position = point.point;
			} else if(IsInAir && !IsJumpLanding && point.normal.y < 0){
				moveSpeed.y = -Config.world_gravity;
				moveDistance.y = 0;
			}

		}
	}



	private void OnCollisionStay(Collision collisionInfo){
	}



	private void OnCollisionExit(Collision collisionInfo){
	}
	
	#endregion
}
