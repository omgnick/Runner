using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndustrialObstaclesPreset4 : ObstaclesPresetBase {

	public override Dictionary<string, Vector2[]> ObstaclesPositions {
		get{
			return new Dictionary<string, Vector2[]> {
				{
					ObstaclesList.INDUSTRIAL_GOLD_COIN, new Vector2[] {
						new Vector2(0, 1),
						new Vector2(0, 2),
						new Vector2(0, 3),
					}
				},
			};
		}
	}
}
