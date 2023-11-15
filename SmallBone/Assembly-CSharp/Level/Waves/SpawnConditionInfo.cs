using System;
using UnityEngine;

namespace Level.Waves
{
	// Token: 0x0200055A RID: 1370
	public sealed class SpawnConditionInfo : MonoBehaviour
	{
		// Token: 0x06001B0C RID: 6924 RVA: 0x0005438E File Offset: 0x0005258E
		public bool IsSatisfied(EnemyWave wave)
		{
			return this._condition.IsSatisfied(wave);
		}

		// Token: 0x06001B0D RID: 6925 RVA: 0x0005439C File Offset: 0x0005259C
		public override string ToString()
		{
			if (this._tag != null && this._tag.Length != 0)
			{
				return this._tag;
			}
			return this.GetAutoName();
		}

		// Token: 0x04001744 RID: 5956
		[SpawnCondition.SubcomponentAttribute(true)]
		[SerializeField]
		private SpawnCondition _condition;

		// Token: 0x04001745 RID: 5957
		[SerializeField]
		private string _tag;

		// Token: 0x0200055B RID: 1371
		[Serializable]
		internal class Subcomponents : SubcomponentArray<SpawnConditionInfo>
		{
		}
	}
}
