using System;
using UnityEngine;

namespace flanne.PowerupSystem
{
	// Token: 0x02000252 RID: 594
	public class SmiteHolyShieldCDR : MonoBehaviour
	{
		// Token: 0x06000CED RID: 3309 RVA: 0x0002F212 File Offset: 0x0002D412
		private void OnSmiteKill(object sender, object args)
		{
			this.holyShield.ReduceCDTimer(1f);
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0002F224 File Offset: 0x0002D424
		private void Start()
		{
			PlayerController componentInParent = base.GetComponentInParent<PlayerController>();
			this.holyShield = componentInParent.GetComponentInChildren<PreventDamage>();
			this.AddObserver(new Action<object, object>(this.OnSmiteKill), SmitePassive.SmiteKillNotification);
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0002F25B File Offset: 0x0002D45B
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnSmiteKill), SmitePassive.SmiteKillNotification);
		}

		// Token: 0x04000932 RID: 2354
		private PreventDamage holyShield;
	}
}
