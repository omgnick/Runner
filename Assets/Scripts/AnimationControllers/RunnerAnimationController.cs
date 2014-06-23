using UnityEngine;
using System.Collections;

public class RunnerAnimationController : CachedBehaviour {

	protected float turnAnimationLength = 2.067f;

	private const string IS_RUNNING = "IsRunning";
	private const string IS_TURNING_LEFT = "IsTurningLeft";
	private const string IS_TURNING_RIGHT = "IsTurningRight";
	private const string IS_JUMPING = "IsJumping";
	private const string IS_SLIDING = "IsSliding";
	private const string IS_DEAD = "IsDead";
	private const string COLLIDER_HEIGHT = "ColliderHeight";

	private const string ANIMATION_STATE_TURNING_RIGHT = "Base Layer.TurningRight";
	private const string ANIMATION_STATE_TURNING_LEFT = "Base Layer.TurningLeft";
	private const string ANIMATION_STATE_JUMPING = "Jump.Jump";



	virtual protected void Start () {
		if(CachedAnimator == null) {
			Debug.LogWarning("Animator not found. Destroying myself");
			Destroy(this);
		}
	}



	protected bool IsRunning {
		set {
			CachedAnimator.SetBool(IS_RUNNING, value);
		}

		get{
			return CachedAnimator.GetBool(IS_RUNNING);
		}
	}



	protected bool IsTurningLeft {
		set {
			CachedAnimator.SetBool(IS_TURNING_LEFT, value);
		}
		
		get{
			return CachedAnimator.GetBool(IS_TURNING_LEFT);
		}
	}



	protected bool IsTurningRight {
		set {
			CachedAnimator.SetBool(IS_TURNING_RIGHT, value);
		}
		
		get{
			return CachedAnimator.GetBool(IS_TURNING_RIGHT);
		}
	}



	protected bool IsJumping {
		set {
			CachedAnimator.SetBool(IS_JUMPING, value);
		}
		
		get{
			return CachedAnimator.GetBool(IS_JUMPING) || IsCurrentAnimationState(ANIMATION_STATE_JUMPING);
		}
	}



	protected bool IsSliding {
		set {
			CachedAnimator.SetBool(IS_SLIDING, value);
		}
		
		get{
			return CachedAnimator.GetBool(IS_SLIDING);
		}
	}



	protected bool IsDead {
		set {
			CachedAnimator.SetBool(IS_DEAD, value);
		}
		
		get{
			return CachedAnimator.GetBool(IS_DEAD);
		}
	}



	protected bool IsCurrentAnimationState(string name) {

		return CachedAnimator.GetCurrentAnimatorStateInfo(0).nameHash == Animator.StringToHash(name);
	}



	private void ResetAnimatorSpeed(){
		CachedAnimator.speed = 1f;
	}



	private void UpdateCollider(){
		if(IsJumping){
			CachedCapsuleCollider.height = CachedAnimator.GetFloat(COLLIDER_HEIGHT);
		}
	}



	virtual protected void FixedUpdate(){
		UpdateCollider();
	}
}
