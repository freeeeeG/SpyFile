using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades
{
	// Token: 0x02000AE5 RID: 2789
	public sealed class KingSlayerComponent : AbilityComponent<KingSlayer>
	{
		// Token: 0x17000BD5 RID: 3029
		// (get) Token: 0x06003913 RID: 14611 RVA: 0x000A8407 File Offset: 0x000A6607
		public int triggerPercent
		{
			get
			{
				return this._triggerPercent;
			}
		}

		// Token: 0x04002D4F RID: 11599
		[Range(0f, 100f)]
		[SerializeField]
		private int _triggerPercent;
	}
}
