using System;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x02001140 RID: 4416
	public class YggdrasillAnimation : MonoBehaviour
	{
		// Token: 0x1700110C RID: 4364
		// (get) Token: 0x060055EF RID: 21999 RVA: 0x00100089 File Offset: 0x000FE289
		public YggdrasillAnimation.Tag tag
		{
			get
			{
				return this._tag;
			}
		}

		// Token: 0x1700110D RID: 4365
		// (get) Token: 0x060055F0 RID: 22000 RVA: 0x00100091 File Offset: 0x000FE291
		public CharacterAnimationController.AnimationInfo info
		{
			get
			{
				return this._info;
			}
		}

		// Token: 0x040044E0 RID: 17632
		[SerializeField]
		private YggdrasillAnimation.Tag _tag;

		// Token: 0x040044E1 RID: 17633
		[SerializeField]
		private CharacterAnimationController.AnimationInfo _info;

		// Token: 0x02001141 RID: 4417
		public enum Tag
		{
			// Token: 0x040044E3 RID: 17635
			P1_Appearance,
			// Token: 0x040044E4 RID: 17636
			P1_Dead,
			// Token: 0x040044E5 RID: 17637
			P1_EnergyBomb,
			// Token: 0x040044E6 RID: 17638
			P1_FistSlam,
			// Token: 0x040044E7 RID: 17639
			P1_FistSlam_Intro,
			// Token: 0x040044E8 RID: 17640
			P1_FistSlam_Outro,
			// Token: 0x040044E9 RID: 17641
			P1_Groggy,
			// Token: 0x040044EA RID: 17642
			P1_Idle,
			// Token: 0x040044EB RID: 17643
			P1_Idle_CutScene,
			// Token: 0x040044EC RID: 17644
			P1_Laser,
			// Token: 0x040044ED RID: 17645
			P1_Laser_Intro,
			// Token: 0x040044EE RID: 17646
			P1_Sleep,
			// Token: 0x040044EF RID: 17647
			P1_Sweeping_Intro,
			// Token: 0x040044F0 RID: 17648
			P1_Sweeping_Left,
			// Token: 0x040044F1 RID: 17649
			P1_Sweeping_Right,
			// Token: 0x040044F2 RID: 17650
			P1_Sweeping_Outro,
			// Token: 0x040044F3 RID: 17651
			P2_Awakening = 100,
			// Token: 0x040044F4 RID: 17652
			P2_BothFistPowerSlam,
			// Token: 0x040044F5 RID: 17653
			P2_BothFistPowerSlam_Intro,
			// Token: 0x040044F6 RID: 17654
			P2_BothFistPowerSlam_Outro,
			// Token: 0x040044F7 RID: 17655
			P2_EnergyCorps,
			// Token: 0x040044F8 RID: 17656
			P2_EnergyCorps_Intro,
			// Token: 0x040044F9 RID: 17657
			P2_FistPowerSlam,
			// Token: 0x040044FA RID: 17658
			P2_FistPowerSlam_Intro,
			// Token: 0x040044FB RID: 17659
			P2_FistPowerSlam_Outro,
			// Token: 0x040044FC RID: 17660
			P2_Groggy,
			// Token: 0x040044FD RID: 17661
			P2_Idle,
			// Token: 0x040044FE RID: 17662
			P2_SweepingCombo_Intro,
			// Token: 0x040044FF RID: 17663
			P2_SweepingComob_Left,
			// Token: 0x04004500 RID: 17664
			P2_SweepingComob_Right,
			// Token: 0x04004501 RID: 17665
			P2_SweepingComob_Outro
		}
	}
}
