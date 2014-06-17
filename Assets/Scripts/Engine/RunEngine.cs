using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunEngine : MonoSingleton<RunEngine> {

	private void Start () {
		InitializeLevelGenerator();

		CharacterStats stats = new CharacterStats();
		stats.turnDistance = LevelGenerator.Instance.TrackLength;
		stats.maxTurnsNumber = LevelGenerator.Instance.TracksNumber / 2;

		InitializeCharacterController(stats);
		InitializeCameraController();
		InitializeHudPanel(stats);
		InitializeLevelObstaclesGenerator();
	}



	private void InitializeLevelGenerator(){
		List<string> prefabs = new List<string>() {"Prefabs/Levels/Industrial/SemiWalls"};
		
		LevelGenerator.Instance.LoadPrefabs(prefabs);
		LevelGenerator.Instance.MaxPrefabsNumber = 10;
		LevelGenerator.Instance.CreateLevel();
		LevelGenerator.Instance.RunBuildingLevelProcess();
	}



	private void InitializeCharacterController(CharacterStats stats){
		#if UNITY_EDITOR
		ICharacterControllerInput input = RunUserInput.Instance;
		#elif UNITY_ANDROID
		ICharacterControllerInput input = RunTouchUserInput.Instance;
		#endif
		
		CharacterGenerator.Instance.CreateCharacter(stats, input);
	}



	private void InitializeCameraController(){
		CameraController.Instance.FollowMargin = new Vector3(0, 3, -5);
		CameraController.Instance.FollowTarget = CharacterGenerator.Instance.MainCharachter.CachedTransform;
	}



	private void InitializeHudPanel(CharacterStats stats){
		HudPanel.Instance.SetCoinsNumber(stats.gold);
		HudPanel.Instance.SetLifesNumber(stats.hitpoints);
	}



	private void InitializeLevelObstaclesGenerator(){
		LevelObstaclesGenerator.Instance.ObstaclePresets = new List<ObstaclesPresetBase>(){
			new IndustrialObstaclesPreset1(),
			new IndustrialObstaclesPreset2(),
			new IndustrialObstaclesPreset3(),
			new IndustrialObstaclesPreset4(),
			new IndustrialObstaclesPreset5(),
			new IndustrialObstaclesPreset6(),
			new IndustrialObstaclesPreset7(),
			new IndustrialObstaclesPreset8(),
			new IndustrialObstaclesPreset9(),
			new IndustrialObstaclesPreset10(),
		};
		LevelObstaclesGenerator.Instance.CreateObstaclesPool();
		LevelObstaclesGenerator.Instance.FillLevelPrefabs();
	}



	public void EndTheRun(){
		RunnerController runner = CharacterGenerator.Instance.MainCharachter.GetComponent<RunnerController>();

		HTTPRequestManager.Instance.AddRequest("regular_run_ended", new Hashtable(){
			{"gold", runner.Coins}
		});

		Invoke("LoadMainMenu", 2f);
	}



	private void LoadMainMenu(){
		Application.LoadLevel("MainMenu");
	}
}
