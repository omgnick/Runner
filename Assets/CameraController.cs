using UnityEngine;
using System.Collections;

public class CameraController : MonoSingleton<CameraController> {

	private Vector3 followMargin;
	private Transform followTarget;



	public Vector3 FollowMargin { 
		set{
			followMargin = value;
			ApplyMagrin();
		}
		get{
			return followMargin;
		}
	}



	public Transform FollowTarget {
		set{
			followTarget = value;
			ApplyMagrin();
		}

		get	{
			return followTarget;
		}
	}



	private void Update(){
		UpdatePosition();
	}



	private void UpdatePosition(){
		if(FollowTarget != null){
			Vector3 delta = Vector3.zero;
			delta.z = Mathf.Max(0, FollowTarget.position.z -
			                    CachedTransform.position.z + FollowMargin.z 
			                    );
			delta.x = FollowTarget.position.x - CachedTransform.position.x + FollowMargin.x;
			CachedTransform.Translate(delta, FollowTarget);
//			CachedTransform.LookAt(followTarget.position);
		}
	}



	private void ApplyMagrin(){
		if(FollowTarget != null){
			CachedTransform.position = followTarget.position;
			CachedTransform.Translate(FollowMargin, FollowTarget);
//			CachedTransform.LookAt(followTarget.position);	
		}
	}

}