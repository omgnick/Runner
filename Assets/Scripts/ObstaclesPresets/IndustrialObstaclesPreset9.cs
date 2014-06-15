using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndustrialObstaclesPreset10 : ObstaclesPresetBase {

	public override Dictionary<string, Vector2[]> ObstaclesPositions {
		get{
			return new Dictionary<string, Vector2[]> {
				{
					ObstaclesList.INDUSTRIAL_GOLD_COIN, new Vector2[] {
						new Vector2(2, 0),
						new Vector2(2, 1),
						new Vector2(2, 4),
					}
				},
				{
					ObstaclesList.INDUSTRIAL_EXPLOSIVE_BARREL, new Vector2[] {
						new Vector2(2, 3),
						new Vector2(2, 2),
						new Vector2(0, 0),
						new Vector2(-2, 4),
					}
				}
			};
		}
	}
}
