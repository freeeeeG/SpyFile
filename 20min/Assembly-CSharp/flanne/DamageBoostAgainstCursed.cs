using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000DB RID: 219
	public class DamageBoostAgainstCursed : MonoBehaviour
	{
		// Token: 0x0600069A RID: 1690 RVA: 0x0001DCA0 File Offset: 0x0001BEA0
		private void OnTweakDamge(object sender, object args)
		{
			GameObject gameObject = (sender as Health).gameObject;
			if (this.CS.IsCursed(gameObject))
			{
				(args as List<ValueModifier>).Add(new MultValueModifier(0, 1f + this.damageBoost));
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0001DCE4 File Offset: 0x0001BEE4
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnTweakDamge), Health.TweakDamageEvent);
			this.CS = CurseSystem.Instance;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0001DD08 File Offset: 0x0001BF08
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnTweakDamge), Health.TweakDamageEvent);
		}

		// Token: 0x0400046A RID: 1130
		[SerializeField]
		private float damageBoost;

		// Token: 0x0400046B RID: 1131
		private CurseSystem CS;
	}
}
