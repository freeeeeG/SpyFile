using System;
using System.Linq;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200094E RID: 2382
	public class MultipleActionHoldingCharge : MultipleAction
	{
		// Token: 0x06003343 RID: 13123 RVA: 0x00098468 File Offset: 0x00096668
		public override bool TryStart()
		{
			if (!base.cooldown.canUse)
			{
				return false;
			}
			for (int i = 0; i < this._motions.components.Length; i++)
			{
				if (base.PassAllConstraints(this._motions.components[i]) && base.ConsumeCooldownIfNeeded())
				{
					if (this._chargingMotions.Contains(base.owner.motion))
					{
						base.DoActionNonBlock(this._motions.components[i]);
					}
					else
					{
						base.DoAction(this._motions.components[i]);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x040029BA RID: 10682
		[SerializeField]
		private Motion[] _chargingMotions;
	}
}
