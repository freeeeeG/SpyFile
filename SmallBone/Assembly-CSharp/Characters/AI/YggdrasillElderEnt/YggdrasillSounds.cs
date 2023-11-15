using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x0200114C RID: 4428
	public class YggdrasillSounds : MonoBehaviour
	{
		// Token: 0x06005631 RID: 22065 RVA: 0x00100989 File Offset: 0x000FEB89
		public void PlaySignSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._appearance_Sign, base.transform.position);
		}

		// Token: 0x06005632 RID: 22066 RVA: 0x001009A7 File Offset: 0x000FEBA7
		public void PlayImpactSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._appearance_Impact, base.transform.position);
		}

		// Token: 0x06005633 RID: 22067 RVA: 0x001009C5 File Offset: 0x000FEBC5
		public void PlayAppearanceSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._appearance_Appearance, base.transform.position);
		}

		// Token: 0x06005634 RID: 22068 RVA: 0x001009E3 File Offset: 0x000FEBE3
		public void PlayRoarSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._appearance_Roar, base.transform.position);
		}

		// Token: 0x06005635 RID: 22069 RVA: 0x00100A01 File Offset: 0x000FEC01
		public void PlaySlamIntroSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlam_Intro1, base.transform.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlam_Intro2, base.transform.position);
		}

		// Token: 0x06005636 RID: 22070 RVA: 0x00100A3B File Offset: 0x000FEC3B
		public void PlaySlamSignSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlam_Sign, base.transform.position);
		}

		// Token: 0x06005637 RID: 22071 RVA: 0x00100A59 File Offset: 0x000FEC59
		public void PlaySlamImpactSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlam_Impact1, base.transform.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlam_Impact2, base.transform.position);
		}

		// Token: 0x06005638 RID: 22072 RVA: 0x00100A93 File Offset: 0x000FEC93
		public void PlaySlamRecoverySignSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlam_Recovery_Sign, base.transform.position);
		}

		// Token: 0x06005639 RID: 22073 RVA: 0x00100AB1 File Offset: 0x000FECB1
		public void PlaySlamRecoverySound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlam_Recovery, base.transform.position);
		}

		// Token: 0x0600563A RID: 22074 RVA: 0x00100ACF File Offset: 0x000FECCF
		public void PlaySlamOutroSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._fistSlam_Outro, base.transform.position);
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x00100AED File Offset: 0x000FECED
		public void PlaySweepingIntroSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sweeping_Intro1, base.transform.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sweeping_Intro2, base.transform.position);
		}

		// Token: 0x0600563C RID: 22076 RVA: 0x00100B27 File Offset: 0x000FED27
		public void PlaySweepingReadySound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sweeping_Ready, base.transform.position);
		}

		// Token: 0x0600563D RID: 22077 RVA: 0x00100B45 File Offset: 0x000FED45
		public void PlaySweepingSweepingSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sweeping_Sweeping1, base.transform.position);
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sweeping_Sweeping2, base.transform.position);
		}

		// Token: 0x0600563E RID: 22078 RVA: 0x00100B7F File Offset: 0x000FED7F
		public void PlaySweepingOutroSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._sweeping_Outro, base.transform.position);
		}

		// Token: 0x0600563F RID: 22079 RVA: 0x00100B9D File Offset: 0x000FED9D
		public void PlayEnergyBombImpactSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._laser_Intro_Impact, base.transform.position);
		}

		// Token: 0x06005640 RID: 22080 RVA: 0x00100BBB File Offset: 0x000FEDBB
		public void PlayEnergyBombSignSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._laser_Sign, base.transform.position);
		}

		// Token: 0x06005641 RID: 22081 RVA: 0x00100BD9 File Offset: 0x000FEDD9
		public void PlayEnergyBombFireSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._energyBomb_Fire, base.transform.position);
		}

		// Token: 0x06005642 RID: 22082 RVA: 0x00100BF7 File Offset: 0x000FEDF7
		public void PlayGroggyIntroSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._groggy_Intro, base.transform.position);
		}

		// Token: 0x06005643 RID: 22083 RVA: 0x00100C15 File Offset: 0x000FEE15
		public void PlayGroggyGroggySound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._groggy_groggy, base.transform.position);
		}

		// Token: 0x06005644 RID: 22084 RVA: 0x00100C33 File Offset: 0x000FEE33
		public void PlayGroggyImpctSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._groggy_impact, base.transform.position);
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x00100C51 File Offset: 0x000FEE51
		public void PlayGroggyRecoverySound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._groggy_recovery, base.transform.position);
		}

		// Token: 0x06005646 RID: 22086 RVA: 0x00100C6F File Offset: 0x000FEE6F
		public void PlayDeadIntroSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dead_Intro, base.transform.position);
		}

		// Token: 0x06005647 RID: 22087 RVA: 0x00100C8D File Offset: 0x000FEE8D
		public void PlayDeadDarkQuartzIntroSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dead_DarkQuartz_Intro, base.transform.position);
		}

		// Token: 0x06005648 RID: 22088 RVA: 0x00100CAB File Offset: 0x000FEEAB
		public void PlayDeadDarkQuartzExplosionSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dead_DarkQuartz_Explosion, base.transform.position);
		}

		// Token: 0x06005649 RID: 22089 RVA: 0x00100CC9 File Offset: 0x000FEEC9
		public void PlayDeadNormalizeSound()
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(this._dead_Normalize, base.transform.position);
		}

		// Token: 0x04004533 RID: 17715
		[Space]
		[SerializeField]
		[Header("Intro")]
		private SoundInfo _appearance_Sign;

		// Token: 0x04004534 RID: 17716
		[SerializeField]
		private SoundInfo _appearance_Impact;

		// Token: 0x04004535 RID: 17717
		[SerializeField]
		private SoundInfo _appearance_Appearance;

		// Token: 0x04004536 RID: 17718
		[SerializeField]
		private SoundInfo _appearance_Roar;

		// Token: 0x04004537 RID: 17719
		[SerializeField]
		[Space]
		[Header("Fist Slam")]
		private SoundInfo _fistSlam_Intro1;

		// Token: 0x04004538 RID: 17720
		[SerializeField]
		private SoundInfo _fistSlam_Intro2;

		// Token: 0x04004539 RID: 17721
		[SerializeField]
		private SoundInfo _fistSlam_Sign;

		// Token: 0x0400453A RID: 17722
		[SerializeField]
		private SoundInfo _fistSlam_Impact1;

		// Token: 0x0400453B RID: 17723
		[SerializeField]
		private SoundInfo _fistSlam_Impact2;

		// Token: 0x0400453C RID: 17724
		[SerializeField]
		private SoundInfo _fistSlam_Recovery_Sign;

		// Token: 0x0400453D RID: 17725
		[SerializeField]
		private SoundInfo _fistSlam_Recovery;

		// Token: 0x0400453E RID: 17726
		[SerializeField]
		private SoundInfo _fistSlam_Outro;

		// Token: 0x0400453F RID: 17727
		[Header("Sweep")]
		[Space]
		[SerializeField]
		private SoundInfo _sweeping_Intro1;

		// Token: 0x04004540 RID: 17728
		[SerializeField]
		private SoundInfo _sweeping_Intro2;

		// Token: 0x04004541 RID: 17729
		[SerializeField]
		private SoundInfo _sweeping_Ready;

		// Token: 0x04004542 RID: 17730
		[SerializeField]
		private SoundInfo _sweeping_Sweeping1;

		// Token: 0x04004543 RID: 17731
		[SerializeField]
		private SoundInfo _sweeping_Sweeping2;

		// Token: 0x04004544 RID: 17732
		[SerializeField]
		private SoundInfo _sweeping_Outro;

		// Token: 0x04004545 RID: 17733
		[Header("EnergyBomb")]
		[Space]
		[SerializeField]
		private SoundInfo _laser_Intro_Impact;

		// Token: 0x04004546 RID: 17734
		[SerializeField]
		private SoundInfo _laser_Sign;

		// Token: 0x04004547 RID: 17735
		[SerializeField]
		private SoundInfo _energyBomb_Fire;

		// Token: 0x04004548 RID: 17736
		[Header("Groggy")]
		[Space]
		[SerializeField]
		private SoundInfo _groggy_Intro;

		// Token: 0x04004549 RID: 17737
		[SerializeField]
		private SoundInfo _groggy_groggy;

		// Token: 0x0400454A RID: 17738
		[SerializeField]
		private SoundInfo _groggy_impact;

		// Token: 0x0400454B RID: 17739
		[SerializeField]
		private SoundInfo _groggy_recovery;

		// Token: 0x0400454C RID: 17740
		[SerializeField]
		[Space]
		[Header("Dead")]
		private SoundInfo _dead_Intro;

		// Token: 0x0400454D RID: 17741
		[SerializeField]
		private SoundInfo _dead_DarkQuartz_Intro;

		// Token: 0x0400454E RID: 17742
		[SerializeField]
		private SoundInfo _dead_DarkQuartz_Explosion;

		// Token: 0x0400454F RID: 17743
		[SerializeField]
		private SoundInfo _dead_Normalize;
	}
}
