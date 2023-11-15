using System;
using System.Collections;
using UnityEngine;

namespace flanne
{
	// Token: 0x020000EF RID: 239
	public class TripleNextPowerup : MonoBehaviour
	{
		// Token: 0x060006EE RID: 1774 RVA: 0x0001EA5C File Offset: 0x0001CC5C
		private void OnPowerupApplied(object sender, object args)
		{
			if (this.used)
			{
				return;
			}
			this.used = true;
			Powerup perk = sender as Powerup;
			PlayerController.Instance.playerPerks.Equip(perk);
			PlayerController.Instance.playerPerks.Equip(perk);
			Object.Destroy(base.gameObject);
		}

		// Token: 0x060006EF RID: 1775 RVA: 0x0001EAAB File Offset: 0x0001CCAB
		private void Start()
		{
			base.StartCoroutine(this.WaitToObservePowerupCR());
		}

		// Token: 0x060006F0 RID: 1776 RVA: 0x0001EABA File Offset: 0x0001CCBA
		private void OnDestroy()
		{
			this.RemoveObserver(new Action<object, object>(this.OnPowerupApplied), Powerup.AppliedNotifcation);
		}

		// Token: 0x060006F1 RID: 1777 RVA: 0x0001EAD3 File Offset: 0x0001CCD3
		private IEnumerator WaitToObservePowerupCR()
		{
			yield return null;
			this.AddObserver(new Action<object, object>(this.OnPowerupApplied), Powerup.AppliedNotifcation);
			yield break;
		}

		// Token: 0x040004B9 RID: 1209
		private bool used;
	}
}
