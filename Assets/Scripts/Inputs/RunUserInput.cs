using UnityEngine;
using System.Collections;

public class RunUserInput : CachedBehaviour {

	public RunnerController runnerController;



	private void Start(){
		runnerController.StartRunning();
	}



	private void Update() {
		float horizontal = Input.GetAxis("Horizontal");

		if(horizontal > 0)
			runnerController.TurnRight();
		else if(horizontal < 0)
			runnerController.TurnLeft();

		float jump = Input.GetAxis("Jump");

		if(jump > 0)
			runnerController.Jump();

		float slide = Input.GetAxis("Slide");

		if(slide > 0)
			runnerController.Slide();
	}

}
