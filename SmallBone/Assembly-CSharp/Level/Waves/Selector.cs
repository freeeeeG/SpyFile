using System;
using UnityEditor;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000556 RID: 1366
	public class Selector : Decorator
	{
		// Token: 0x06001B03 RID: 6915 RVA: 0x0005426C File Offset: 0x0005246C
		protected override bool Check(EnemyWave wave)
		{
			SpawnConditionInfo[] components = this._conditions.components;
			for (int i = 0; i < components.Length; i++)
			{
				if (components[i].IsSatisfied(wave))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04001740 RID: 5952
		[UnityEditor.Subcomponent(typeof(SpawnConditionInfo))]
		[SerializeField]
		private SpawnConditionInfo.Subcomponents _conditions;
	}
}
