using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameModes.Horde
{
	// Token: 0x020007B6 RID: 1974
	[CreateAssetMenu(fileName = "horde_level_config", menuName = "Team17/Game Mode/Horde/Level Config")]
	[Serializable]
	public class HordeLevelConfig : LevelConfigBase
	{
		// Token: 0x060025DD RID: 9693 RVA: 0x000B32B0 File Offset: 0x000B16B0
		public override List<OrderDefinitionNode> GetAllRecipes()
		{
			List<OrderDefinitionNode> list = new List<OrderDefinitionNode>();
			for (int i = 0; i < this.m_waves.Count; i++)
			{
				HordeWaveData hordeWaveData = this.m_waves[i];
				for (int j = 0; j < hordeWaveData.m_recipes.m_recipes.Length; j++)
				{
					list.Add(hordeWaveData.m_recipes.m_recipes[j].m_order);
				}
			}
			return list;
		}

		// Token: 0x04001DB7 RID: 7607
		[SerializeField]
		public float m_plateReturnTime = 10f;

		// Token: 0x04001DB8 RID: 7608
		[Header("Horde Target Data")]
		[SerializeField]
		public int m_targetHealth = 100;

		// Token: 0x04001DB9 RID: 7609
		[SerializeField]
		public float m_targetRepairSpeed = 0.5f;

		// Token: 0x04001DBA RID: 7610
		[SerializeField]
		public float m_targetRepairThreshold = 10f;

		// Token: 0x04001DBB RID: 7611
		[SerializeField]
		public int m_targetRepairCostMax = 200;

		// Token: 0x04001DBC RID: 7612
		[Header("Horde Level Data")]
		[SerializeField]
		public int m_health = 100;

		// Token: 0x04001DBD RID: 7613
		[SerializeField]
		public RecipeMoneyData m_recipeMoney;

		// Token: 0x04001DBE RID: 7614
		[SerializeField]
		public HordeOutroFlowroutineData m_flowroutineData;

		// Token: 0x04001DBF RID: 7615
		[SerializeField]
		public HordeWavesData m_waves;
	}
}
