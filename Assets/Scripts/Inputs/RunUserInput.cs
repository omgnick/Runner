using UnityEngine;
using System.Collections;

public class RunUserInput : MonoSingleton<RunUserInput>, ICharacterControllerInput {

	public RunnerController runnerController;



	public void Init(RunnerController runner){
		runnerController = runner;
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
