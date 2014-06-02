using UnityEngine;
using System.Collections;

public class RunTouchUserInput : MonoSingleton<RunTouchUserInput>, ICharacterControllerInput {

	private RunnerController runnerController;

	private int fingerId;
	private Vector2 touchMove;

	
	
	private void Start(){
		runnerController.StartRunning();
	}



	public void Init(RunnerController runner){
		runnerController = runner;
  		
	}
	
	
	
	private void Update() {

		Touch touch = Input.GetTouch(0);

		if(fingerId != touch.fingerId)
			ClearTouchData();

		fingerId = touch.fingerId;
		touchMove += touch.deltaPosition;

		if(touch.phase == TouchPhase.Ended){
			CreateActionFromTouchData();
		}
	}



	private void ClearTouchData(){
		touchMove = Vector2.zero;
	}



	private void CreateActionFromTouchData(){
		if(IsTurningRight)
			runnerController.TurnRight();
		else if (IsTurningLeft)
			runnerController.TurnLeft();
		else if (IsJumping)
			runnerController.Jump();

		ClearTouchData();
	}



	private bool IsTurningRight{
		get{
			return touchMove.x > 0 && touchMove.x > Mathf.Abs(touchMove.y);
		}
	}



	private bool IsTurningLeft {
		get{
			return touchMove.x < 0 && Mathf.Abs(touchMove.x) > Mathf.Abs(touchMove.y);
		}
	}



	private bool IsJumping {
		get{
			return touchMove.y > 0 && touchMove.y > Mathf.Abs(touchMove.x);
		}
	}
}


public interface ICharacterControllerInput {
	void Init(RunnerController runner);
}
