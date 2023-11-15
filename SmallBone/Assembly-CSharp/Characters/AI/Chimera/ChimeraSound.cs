using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.AI.Chimera
{
	// Token: 0x0200124F RID: 4687
	public class ChimeraSound : MonoBehaviour
	{
		// Token: 0x06005CE7 RID: 23783 RVA: 0x001116C7 File Offset: 0x0010F8C7
		public void PlayRoarSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._roarSound, base.transform.position);
		}

		// Token: 0x06005CE8 RID: 23784 RVA: 0x001116E5 File Offset: 0x0010F8E5
		public void PlayImpactSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._impactSound, base.transform.position);
		}

		// Token: 0x06005CE9 RID: 23785 RVA: 0x00111703 File Offset: 0x0010F903
		public void PlaySlamReadySound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlamReadySound, base.transform.position);
		}

		// Token: 0x06005CEA RID: 23786 RVA: 0x00111721 File Offset: 0x0010F921
		public void PlaySlamSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlamSound, base.transform.position);
		}

		// Token: 0x06005CEB RID: 23787 RVA: 0x0011173F File Offset: 0x0010F93F
		public void PlaySweepReadySound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sweepingReadySound, base.transform.position);
		}

		// Token: 0x06005CEC RID: 23788 RVA: 0x0011175D File Offset: 0x0010F95D
		public void PlaySweepAttackSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sweepingSound, base.transform.position);
		}

		// Token: 0x06005CED RID: 23789 RVA: 0x0011177B File Offset: 0x0010F97B
		public void PlayEnergyBombSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._energyBombSound, base.transform.position);
		}

		// Token: 0x06005CEE RID: 23790 RVA: 0x00111799 File Offset: 0x0010F999
		public void PlayEnergyBombReadySound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._energyBombReadySound, base.transform.position);
		}

		// Token: 0x06005CEF RID: 23791 RVA: 0x001117B7 File Offset: 0x0010F9B7
		public void PlayGroggyStartSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._groggySound, base.transform.position);
		}

		// Token: 0x06005CF0 RID: 23792 RVA: 0x001117D5 File Offset: 0x0010F9D5
		public void PlayGroggyOnGroundSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._groundImpactSound, base.transform.position);
		}

		// Token: 0x06005CF1 RID: 23793 RVA: 0x001117F3 File Offset: 0x0010F9F3
		public void PlayDieSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dieSound, base.transform.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dieShoutSound, base.transform.position);
		}

		// Token: 0x04004A5D RID: 19037
		[Header("Intro")]
		[Space]
		[SerializeField]
		private SoundInfo _impactSound;

		// Token: 0x04004A5E RID: 19038
		[Header("Stomp")]
		[Space]
		[SerializeField]
		private SoundInfo _fistSlamSound;

		// Token: 0x04004A5F RID: 19039
		[SerializeField]
		private SoundInfo _fistSlamReadySound;

		// Token: 0x04004A60 RID: 19040
		[SerializeField]
		[Space]
		[Header("Bite")]
		private SoundInfo _sweepingReadySound;

		// Token: 0x04004A61 RID: 19041
		[SerializeField]
		private SoundInfo _sweepingSound;

		// Token: 0x04004A62 RID: 19042
		[Space]
		[SerializeField]
		[Header("VenomBall")]
		private SoundInfo _energyBombReadySound;

		// Token: 0x04004A63 RID: 19043
		[SerializeField]
		private SoundInfo _energyBombSound;

		// Token: 0x04004A64 RID: 19044
		[Space]
		[SerializeField]
		[Header("VenomCannon")]
		private SoundInfo _groggySound;

		// Token: 0x04004A65 RID: 19045
		[SerializeField]
		private SoundInfo _groundImpactSound;

		// Token: 0x04004A66 RID: 19046
		[Header("VenomBreath")]
		[Space]
		[SerializeField]
		private SoundInfo _venomBreathSound;

		// Token: 0x04004A67 RID: 19047
		[Space]
		[Header("Roar")]
		[SerializeField]
		private SoundInfo _roarSound;

		// Token: 0x04004A68 RID: 19048
		[Space]
		[SerializeField]
		[Header("In")]
		private SoundInfo _inSound;

		// Token: 0x04004A69 RID: 19049
		[SerializeField]
		[Header("Out")]
		[Space]
		private SoundInfo _outSound;

		// Token: 0x04004A6A RID: 19050
		[SerializeField]
		[Header("BigStomp")]
		[Space]
		private SoundInfo _bicStompSound;

		// Token: 0x04004A6B RID: 19051
		[Header("Outro")]
		[Space]
		[SerializeField]
		private SoundInfo _dieSound;

		// Token: 0x04004A6C RID: 19052
		[SerializeField]
		private SoundInfo _dieShoutSound;
	}
}
