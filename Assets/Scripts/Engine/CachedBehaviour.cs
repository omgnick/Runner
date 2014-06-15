using UnityEngine;
using System.Collections;

public class CachedBehaviour : MonoBehaviour {

	private Animator cachedAnimator;
	private Animation cachedAnimation;
	private Rigidbody cachedRigidbody;
	private Collider cachedCollider;
	private Transform cachedTransform;
	private GameObject cachedGameObject;
	private SphereCollider cachedSphereCollider;
	private CapsuleCollider cachedCapsuleCollider;


	public Animator CachedAnimator {
		get {
			if(cachedAnimator == null)
				cachedAnimator = GetComponent<Animator>();

			return cachedAnimator;
		}
	}



	public Rigidbody CachedRigidbody {
		get {
			if(cachedRigidbody == null)
				cachedRigidbody = rigidbody;

			return cachedRigidbody;
		}
	}



	public Collider CachedCollider {
		get {
			if(cachedCollider == null)
				cachedCollider = collider;

			return cachedCollider;
		}
	}



	public Transform CachedTransform {
		get {
			if(cachedTransform == null)
				cachedTransform = transform;

			return cachedTransform;
		}
	}



	public GameObject CachedGameObject {
		get {
			if(cachedGameObject == null)
				cachedGameObject = gameObject;

			return cachedGameObject;
		}
	}



	public Animation CachedAnimation {
		get {
			if(cachedAnimation == null)
				cachedAnimation = GetComponent<Animation>();
			
			return cachedAnimation;
		}
	}



	public SphereCollider CachedSphereCollider{
		get{
			if(cachedSphereCollider == null)
				cachedSphereCollider = GetComponent<SphereCollider>();

			return cachedSphereCollider;
		}
	}



	public CapsuleCollider CachedCapsuleCollider{
		get{
			if(cachedCapsuleCollider == null)
				cachedCapsuleCollider = GetComponent<CapsuleCollider>();
			
			return cachedCapsuleCollider;
		}
	}
}
