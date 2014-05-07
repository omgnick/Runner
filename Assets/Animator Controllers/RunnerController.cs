using UnityEngine;
using System.Collections;

public class RunnerController : MonoBehaviour {

	private Animator animator;

	void Start () {
		animator = GetComponent<Animator>();

		if(animator == null){
			Debug.LogWarning("Animator not found. Destroying myself");
			Destroy(this);
		}

	}
	

	void Update () {

		animator.SetFloat("Speed", Input.GetAxis("Vertical"));
	}
}
