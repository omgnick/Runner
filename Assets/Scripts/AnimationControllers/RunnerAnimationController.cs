using UnityEngine;
using System.Collections;

public class RunnerAnimationController : CachedBehaviour {

	protected const string IS_RUNNING = "IsRunning";
	protected const string IS_TURNING_LEFT = "IsTurningLeft";
	protected const string IS_TURNING_RIGHT = "IsTurningRight";
	protected const string IS_JUMPING = "IsJumping";
	protected const string IS_SLIDING = "IsSliding";



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
			return CachedAnimator.GetBool(IS_JUMPING);
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
}
