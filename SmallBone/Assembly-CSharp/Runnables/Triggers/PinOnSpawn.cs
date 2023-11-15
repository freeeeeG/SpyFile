using System;
using Characters;
using Level.Waves;
using UnityEngine;

namespace Runnables.Triggers
{
	// Token: 0x02000353 RID: 851
	public class PinOnSpawn : Trigger
	{
		// Token: 0x06000FEF RID: 4079 RVA: 0x0002FBC0 File Offset: 0x0002DDC0
		protected override bool Check()
		{
			bool flag = false;
			if (this._pin.characters == null)
			{
				return flag;
			}
			foreach (Character character in this._pin.characters)
			{
				flag |= character.gameObject.activeInHierarchy;
			}
			return flag;
		}

		// Token: 0x04000D0C RID: 3340
		[SerializeField]
		private Pin _pin;
	}
}
