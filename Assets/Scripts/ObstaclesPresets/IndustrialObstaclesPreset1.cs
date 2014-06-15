using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IndustrialObstaclesPreset1 : ObstaclesPresetBase {

	public override Dictionary<string, Vector2[]> ObstaclesPositions {
		get{
			return new Dictionary<string, Vector2[]> {
				{
					ObstaclesList.INDUSTRIAL_EXPLOSIVE_BARREL, new Vector2[] {
						new Vector2(-1,0),
						new Vector2(0, 2),
						new Vector2(0, 4),
					}
				},
			};
		}
	}
}
