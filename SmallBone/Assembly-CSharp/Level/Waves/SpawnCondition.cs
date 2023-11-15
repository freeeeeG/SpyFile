using System;
using UnityEditor;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x02000558 RID: 1368
	public abstract class SpawnCondition : MonoBehaviour
	{
		// Token: 0x06001B07 RID: 6919
		protected abstract bool Check(EnemyWave wave);

		// Token: 0x06001B08 RID: 6920 RVA: 0x000542E1 File Offset: 0x000524E1
		public bool IsSatisfied(EnemyWave wave)
		{
			return this._inverter ^ this.Check(wave);
		}

		// Token: 0x04001742 RID: 5954
		[SerializeField]
		private bool _inverter;

		// Token: 0x02000559 RID: 1369
		[AttributeUsage(AttributeTargets.Field)]
		public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
		{
			// Token: 0x06001B0A RID: 6922 RVA: 0x000542F1 File Offset: 0x000524F1
			public SubcomponentAttribute(bool allowCustom = true) : base(allowCustom, SpawnCondition.SubcomponentAttribute.types)
			{
			}

			// Token: 0x04001743 RID: 5955
			public new static readonly Type[] types = new Type[]
			{
				typeof(Always),
				typeof(EnterZone),
				typeof(HardModeLevel),
				typeof(RemainEnemies),
				typeof(ReturnFail),
				typeof(Sequence),
				typeof(Selector),
				typeof(TimeOut),
				typeof(TimeRemain)
			};
		}
	}
}
