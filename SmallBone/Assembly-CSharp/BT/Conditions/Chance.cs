using System;
using UnityEngine;

namespace BT.Conditions
{
	// Token: 0x0200142B RID: 5163
	public class Chance : Condition
	{
		// Token: 0x0600655A RID: 25946 RVA: 0x00125712 File Offset: 0x00123912
		protected override bool Check(Context context)
		{
			return MMMaths.Chance(this._successChance);
		}

		// Token: 0x040051A4 RID: 20900
		[SerializeField]
		[Range(0f, 1f)]
		private float _successChance;
	}
}
