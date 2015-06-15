using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RunEngine : MonoSingleton<RunEngine> {

	virtual protected void Start () {
		InitializeLevelGenerator();

		CharacterStats stats = Config.player.Stats;
		stats.turnDistance = LevelGenerator.Instance.TrackLength;
		stats.maxTurnsNumber = LevelGenerator.Instance.TracksNumber / 2;

		InitializeCharacterController(stats);
		InitializeCameraController();
		InitializeHudPanel(stats);
		InitializeLevelObstaclesGenerator();
	}



	protected void DestroyOldGenerators(){
		Destroy(LevelGenerator.Instance.CachedGameObject);
		Destroy(CharacterGenerator.Instance.CachedGameObject);
		Destroy (LevelObstaclesGenerator.Instance.CachedGameObject);
	}



	protected void InitializeLevelGenerator(){
		List<string> prefabs = new List<string>() {
			"Prefabs/Levels/Industrial/SemiWalls",
			"Prefabs/Levels/Industrial/SemiWalls2",
			"Prefabs/Levels/Industrial/SemiWalls3",
		};
		
		LevelGenerator.Instance.LoadPrefabs(prefabs);
		LevelGenerator.Instance.MaxPrefabsNumber = 10;
		LevelGenerator.Instance.CreateLevel();
		LevelGenerator.Instance.RunBuildingLevelProcess();
	}



	protected void InitializeCharacterController(CharacterStats stats){
		#if UNITY_EDITOR
		ICharacterControllerInput input = RunUserInput.Instance;
		#elif UNITY_ANDROID
		ICharacterControllerInput input = RunTouchUserInput.Instance;
		#endif
		
		CharacterGenerator.Instance.CreateCharacter(stats, input);
	}



	protected void InitializeCameraController(){
		CameraController.Instance.FollowMargin = new Vector3(0, 3, -5);
		CameraController.Instance.FollowTarget = CharacterGenerator.Instance.MainCharachter.CachedTransform;
	}



	protected void InitializeHudPanel(CharacterStats stats){
		HudPanel.Instance.SetCoinsNumber(stats.gold);
		HudPanel.Instance.SetLifesNumber(stats.hitpoints);
	}



	protected void InitializeLevelObstaclesGenerator(){
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



	public void OnRunnerDied(RunnerController runner){
		HudPanel.Instance.ShowResults(runner.Coins);
	}



	virtual public void EndTheRun(){
		RunnerController runner = CharacterGenerator.Instance.MainCharachter.GetComponent<RunnerController>();

		Config.player.Gold += runner.Coins;
		Config.player.Save();

		HTTPRequestManager.Instance.AddRequest("regular_run_ended", new Hashtable(){
			{"gold", runner.Coins}
		});

		Invoke("LoadMainMenu", 2f);
	}



	protected void LoadMainMenu(){
		DestroyOldGenerators();
		Application.LoadLevel("MainMenu");
	}
}
