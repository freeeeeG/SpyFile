using System;
using Characters;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000351 RID: 849
	public class HealthCondition : Trigger
	{
		// Token: 0x06000FEA RID: 4074 RVA: 0x0002FAF0 File Offset: 0x0002DCF0
		protected override bool Check()
		{
			return this._character.health.percent >= (double)this._range.x && this._character.health.percent <= (double)this._range.y;
		}

		// Token: 0x04000D09 RID: 3337
		[SerializeField]
		private Character _character;

		// Token: 0x04000D0A RID: 3338
		[MinMaxSlider(0f, 1f)]
		[SerializeField]
		private Vector2 _range;
	}
}
