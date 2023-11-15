using System;
using System.Collections.Generic;
using UnityEngine;

namespace flanne
{
	// Token: 0x02000063 RID: 99
	public class DamageBoostAgainstFrozen : MonoBehaviour
	{
		// Token: 0x0600046E RID: 1134 RVA: 0x00016D98 File Offset: 0x00014F98
		private void OnTweakDamge(object sender, object args)
		{
			GameObject gameObject = (sender as Health).gameObject;
			if (this.FS.IsFrozen(gameObject))
			{
				(args as List<ValueModifier>).Add(new MultValueModifier(0, 1f + this.damageBoost));
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00016DDC File Offset: 0x00014FDC
		private void Start()
		{
			this.AddObserver(new Action<object, object>(this.OnTweakDamge), Health.TweakDamageEvent);
			this.FS = FreezeSystem.SharedInstance;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00016E00 File Offset: 0x00015000
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnTweakDamge), Health.TweakDamageEvent);
		}

		// Token: 0x04000269 RID: 617
		[SerializeField]
		private float damageBoost;

		// Token: 0x0400026A RID: 618
		private FreezeSystem FS;
	}
}
