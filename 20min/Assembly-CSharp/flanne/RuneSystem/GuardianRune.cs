using System;
using System.Collections;
using flanne.Core;
using UnityEngine;
using UnityEngine.Events;

namespace flanne.RuneSystem
{
	// Token: 0x02000150 RID: 336
	public class GuardianRune : Rune
	{
		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060008AB RID: 2219 RVA: 0x000245E7 File Offset: 0x000227E7
		[SerializeField]
		private int cap
		{
			get
			{
				return this.level;
			}
		}

		// Token: 0x060008AC RID: 2220 RVA: 0x000245F0 File Offset: 0x000227F0
		private void OnPreventDamage()
		{
			if (!this.active)
			{
				return;
			}
			Summon[] array = Object.FindObjectsOfType<Summon>();
			this.counter++;
			if (this.counter < this.cap && array.Length != 0)
			{
				int num = Random.Range(0, array.Length);
				base.StartCoroutine(this.KillSummonCR(array[num]));
				this.player.playerHealth.isProtected = true;
			}
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00024658 File Offset: 0x00022858
		private void OnHealthChange(int hp)
		{
			if (this.counter >= this.cap)
			{
				return;
			}
			if (hp == 1)
			{
				if (Object.FindObjectsOfType<Summon>().Length != 0)
				{
					this.active = true;
					base.StartCoroutine(this.WaitToActivateCR());
					return;
				}
			}
			else if (this.active)
			{
				this.player.playerHealth.isProtected = false;
				this.active = false;
			}
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x000246B8 File Offset: 0x000228B8
		protected override void Init()
		{
			this.player.playerHealth.onHealthChangedTo.AddListener(new UnityAction<int>(this.OnHealthChange));
			this.player.playerHealth.onDamagePrevented.AddListener(new UnityAction(this.OnPreventDamage));
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x00024708 File Offset: 0x00022908
		private void OnDestroy()
		{
			this.player.playerHealth.onHealthChangedTo.RemoveListener(new UnityAction<int>(this.OnHealthChange));
			this.player.playerHealth.onDamagePrevented.RemoveListener(new UnityAction(this.OnPreventDamage));
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x00024757 File Offset: 0x00022957
		private IEnumerator WaitToActivateCR()
		{
			yield return new WaitForSeconds(0.1f);
			this.player.playerHealth.isProtected = true;
			yield break;
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x00024766 File Offset: 0x00022966
		private IEnumerator KillSummonCR(Summon summon)
		{
			PauseController.SharedInstance.Pause();
			yield return new WaitForSecondsRealtime(0.3f);
			this.summonDeathFXObj.transform.position = summon.transform.position;
			this.summonDeathFXObj.SetActive(true);
			Object.Destroy(summon.gameObject);
			SoundEffectSO soundEffectSO = this.soundFX;
			if (soundEffectSO != null)
			{
				soundEffectSO.Play(null);
			}
			yield return new WaitForSecondsRealtime(0.4f);
			this.summonDeathFXObj.SetActive(false);
			yield return new WaitForSecondsRealtime(0.3f);
			this.knockbackObj.SetActive(true);
			PauseController.SharedInstance.UnPause();
			this.PostNotification(GuardianRune.SummonDestroyedNotification, null);
			yield break;
		}

		// Token: 0x0400066C RID: 1644
		public static string SummonDestroyedNotification = "GuardianRune.SummonDestroyedNotification";

		// Token: 0x0400066D RID: 1645
		[SerializeField]
		private GameObject summonDeathFXObj;

		// Token: 0x0400066E RID: 1646
		[SerializeField]
		private GameObject knockbackObj;

		// Token: 0x0400066F RID: 1647
		[SerializeField]
		private SoundEffectSO soundFX;

		// Token: 0x04000670 RID: 1648
		private bool active;

		// Token: 0x04000671 RID: 1649
		private int counter;
	}
}
