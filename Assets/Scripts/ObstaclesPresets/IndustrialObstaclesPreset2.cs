using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndustrialObstaclesPreset2 : ObstaclesPresetBase {

	public override Dictionary<string, Vector2[]> ObstaclesPositions {
		get{
			return new Dictionary<string, Vector2[]> {
				{
					ObstaclesList.INDUSTRIAL_EXPLOSIVE_BARREL, new Vector2[] {
						new Vector2(0,0),
						new Vector2(1, 1),
						new Vector2(-2, 3),
					}
				},
			};
		}
	}
}
