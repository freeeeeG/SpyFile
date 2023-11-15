using System;
using UnityEditor;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000557 RID: 1367
	public class Sequence : Decorator
	{
		// Token: 0x06001B05 RID: 6917 RVA: 0x000542AC File Offset: 0x000524AC
		protected override bool Check(EnemyWave wave)
		{
			SpawnConditionInfo[] components = this._conditions.components;
			for (int i = 0; i < components.Length; i++)
			{
				if (!components[i].IsSatisfied(wave))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04001741 RID: 5953
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(SpawnConditionInfo))]
		private SpawnConditionInfo.Subcomponents _conditions;
	}
}
