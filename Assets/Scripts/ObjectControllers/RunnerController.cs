using UnityEngine;
using System.Collections;

public class RunnerController : RunnerAnimationController {

	private CharacterStats stats;

	private Vector3 moveDistance;
	private Vector3 moveSpeed;
	private int turnNumber = 0;



	public RunnerController(){
		moveDistance = Vector3.zero;
		moveSpeed = Vector3.zero;
	}



	public void Init(CharacterStats stats){
		this.stats = stats;
	}



	public void StartRunning() {
		IsRunning = true;
	}



	public void TurnRight() {
		if(CanTurnRight){
			moveDistance.x = stats.turnDistance;
			moveSpeed.x = stats.turnSpeed;
			IsTurningRight = true;
			turnNumber++;
		}
	}



	public void TurnLeft() {
		if(CanTurnLeft){
			moveDistance.x = -stats.turnDistance;
			moveSpeed.x = -stats.turnSpeed;
			turnNumber--;
			IsTurningLeft = true;
		}
	}



	override protected void FixedUpdate() {
		base.FixedUpdate();

		if(IsDead)
			return;

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
			moveDistance.y += stats.jumpHeight;
			moveSpeed.y += stats.jumpSpeed;
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

		//OX Changing Tracks
		move.x = Mathf.Min(Mathf.Abs(move.x), Mathf.Abs(moveDistance.x)) * Mathf.Sign(moveDistance.x);
		moveDistance.x -= move.x;

		if(moveDistance.x == 0)
			moveSpeed.x = 0;


		//OY Jumping
		move.y = Mathf.Min (move.y, moveDistance.y);
		moveDistance.y -= move.y;

		if(IsJumpTopPoint) {
			moveSpeed.y = Config.world_gravity;
			moveDistance.y = 0;
		}

		//OZ Running Forward
		moveSpeed.z = Mathf.Min(moveSpeed.z + stats.acceleration * Time.deltaTime, stats.maxSpeed);
			

		CachedTransform.Translate(move);
	}



	private bool IsInTurn {
		get{
			return moveSpeed.x != 0 || IsTurningLeft || IsTurningRight;
		}
	}



	private bool CanTurn {
		get{
			return !IsInAir && !IsInTurn && !IsInSlide;
		}
	}



	private bool CanTurnLeft {
		get{
			return CanTurn && -stats.maxTurnsNumber < turnNumber; 
		}
	}



	private bool CanTurnRight {
		get{
			return CanTurn && stats.maxTurnsNumber > turnNumber;
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



	public void TakeDamage(int damage){
		stats.hitpoints -= damage;

		if(ShouldDie)
			Die();
	}



	private bool ShouldDie {
		get{
			return stats.hitpoints <= 0;
		}
	}



	private void Die(){
		IsDead = true;
	}



	public int Coins {
		get{
			return stats.coins;
		}
		set{
			stats.coins = value;
		}
	}



	public int Hitpoins {
		get{
			return stats.hitpoints;
		}
	}
}
