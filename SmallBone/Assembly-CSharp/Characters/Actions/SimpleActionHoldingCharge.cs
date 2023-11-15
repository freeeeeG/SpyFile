using System;
using System.Linq;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x0200095D RID: 2397
	public class SimpleActionHoldingCharge : SimpleAction
	{
		// Token: 0x060033A3 RID: 13219 RVA: 0x00099218 File Offset: 0x00097418
		public override bool TryStart()
		{
			if (!this.canUse || !base.ConsumeCooldownIfNeeded())
			{
				return false;
			}
			if (this._chargingMotions.Contains(base.owner.motion))
			{
				base.DoActionNonBlock(this._motion);
			}
			else
			{
				base.DoAction(this._motion);
			}
			return true;
		}

		// Token: 0x040029E2 RID: 10722
		[SerializeField]
		private Motion[] _chargingMotions;
	}
}
