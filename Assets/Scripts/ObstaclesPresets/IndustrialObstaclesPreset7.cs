using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndustrialObstaclesPreset7 : ObstaclesPresetBase {

	public override Dictionary<string, Vector2[]> ObstaclesPositions {
		get{
			return new Dictionary<string, Vector2[]> {
				{
					ObstaclesList.INDUSTRIAL_GOLD_COIN, new Vector2[] {
						new Vector2(-1, 1),
						new Vector2(-1, 2),
						new Vector2(-1, 3),
					}
				},
				{
					ObstaclesList.INDUSTRIAL_EXPLOSIVE_BARREL, new Vector2[] {
						new Vector2(-1, 0),
						new Vector2(-1, 4)
					}
				}
			};
		}
	}
}
