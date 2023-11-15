using System;
using Characters.Operations;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt
{
	// Token: 0x0200114D RID: 4429
	public class ElderEntEventReceiver : MonoBehaviour
	{
		// Token: 0x0600564B RID: 22091 RVA: 0x00100CE8 File Offset: 0x000FEEE8
		private void Awake()
		{
			this._onAppearance_Appearance.Initialize();
			this._onAppearance_Impact_Left.Initialize();
			this._onAppearance_Impact_Right.Initialize();
			this._onAppearance_Roar.Initialize();
			this._onAppearance_Sign.Initialize();
			this._onDead_DarkQuartz_Explosion.Initialize();
			this._onDead_DarkQuartz_Intro.Initialize();
			this._onDead_DarkQuartz_Spark.Initialize();
			this._onDead_Intro.Initialize();
			this._onDead_Normalize.Initialize();
			this._onEnergyBomb_Fire.Initialize();
			this._onFistSlam_Impact.Initialize();
			this._onFistSlam_Intro.Initialize();
			this._onFistSlam_Outro.Initialize();
			this._onFistSlam_Recovery.Initialize();
			this._onFistSlam_Recovery_Sign.Initialize();
			this._onFistSlam_Sign.Initialize();
			this._onGroggy_Groggy.Initialize();
			this._onGroggy_Impact.Initialize();
			this._onGroggy_Intro.Initialize();
			this._onGroggy_Recovery.Initialize();
			this._onLaser_Impact.Initialize();
			this._onLaser_Intro_Impact.Initialize();
			this._onLaser_Laser.Initialize();
			this._onLaser_Laser_End.Initialize();
			this._onLaser_Sign.Initialize();
			this._onSweeping_Intro.Initialize();
			this._onSweeping_Outro.Initialize();
			this._onSweeping_Sign.Initialize();
			this._onSweeping_Sweeping.Initialize();
			this._onSweeping_Sweeping_End.Initialize();
			this._onP2_Awakening_Intro.Initialize();
			this._onP2_Awakening_Awakening.Initialize();
			this._onP2_EnergyCorps_Emerge.Initialize();
			this._onP2_EnergyCorps_Intro_Impact.Initialize();
			this._onP2_EnergyCorps_Sign.Initialize();
			this._onP2_FistPowerSlam_Impact.Initialize();
			this._onP2_FistPowerSlam_Intro.Initialize();
			this._onP2_FistPowerSlam_Outro.Initialize();
			this._onP2_FistPowerSlam_Recovery.Initialize();
			this._onP2_FistPowerSlam_Recovery_Sign.Initialize();
			this._onP2_FistPowerSlam_Sign.Initialize();
			this._onP2_Groggy_Groggy.Initialize();
			this._onP2_Groggy_Impact.Initialize();
			this._onP2_Groggy_Intro.Initialize();
			this._onP2_Groggy_Recovery.Initialize();
			this._onP2_SweepingCombo_End.Initialize();
			this._onP2_SweepingCombo_Intro.Initialize();
			this._onP2_SweepingCombo_Outro.Initialize();
			this._onP2_SweepingCombo_Ready.Initialize();
			this._onP2_SweepingCombo_Sign.Initialize();
			this._onP2_SweepingCombo_Sweeping.Initialize();
			this._onP2_BothFistPowerSlam_Intro.Initialize();
			this._onP2_BothFistPowerSlam_Sign.Initialize();
			this._onP2_BothFistPowerSlam_Impact.Initialize();
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._onP2_BothFistPowerSlam_Impact_InHardmode.Initialize();
			}
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x00100F69 File Offset: 0x000FF169
		private void RunOperation(OperationInfos operationInfos)
		{
			operationInfos.gameObject.SetActive(true);
			operationInfos.Run(this._owner);
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x00100F83 File Offset: 0x000FF183
		public void Platform_On()
		{
			this.RunOperation(this._onPlatform_On);
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x00100F91 File Offset: 0x000FF191
		public void Platform_Off()
		{
			this.RunOperation(this._onPlatform_Off);
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x00100F9F File Offset: 0x000FF19F
		public void Appearance_Appearance()
		{
			this.RunOperation(this._onAppearance_Appearance);
		}

		// Token: 0x06005650 RID: 22096 RVA: 0x00100FAD File Offset: 0x000FF1AD
		public void Appearance_Impact_Left()
		{
			this.RunOperation(this._onAppearance_Impact_Left);
		}

		// Token: 0x06005651 RID: 22097 RVA: 0x00100FBB File Offset: 0x000FF1BB
		public void Appearance_Impact_Right()
		{
			this.RunOperation(this._onAppearance_Impact_Right);
		}

		// Token: 0x06005652 RID: 22098 RVA: 0x00100FC9 File Offset: 0x000FF1C9
		public void Appearance_Roar()
		{
			this.RunOperation(this._onAppearance_Roar);
		}

		// Token: 0x06005653 RID: 22099 RVA: 0x00100FD7 File Offset: 0x000FF1D7
		public void Appearance_Sign()
		{
			this.RunOperation(this._onAppearance_Sign);
		}

		// Token: 0x06005654 RID: 22100 RVA: 0x00100FE5 File Offset: 0x000FF1E5
		public void FistSlam_Intro()
		{
			this.RunOperation(this._onFistSlam_Intro);
		}

		// Token: 0x06005655 RID: 22101 RVA: 0x00100FF3 File Offset: 0x000FF1F3
		public void FistSlam_Impact()
		{
			this.RunOperation(this._onFistSlam_Impact);
		}

		// Token: 0x06005656 RID: 22102 RVA: 0x00101001 File Offset: 0x000FF201
		public void FistSlam_Recovery()
		{
			this.RunOperation(this._onFistSlam_Recovery);
		}

		// Token: 0x06005657 RID: 22103 RVA: 0x0010100F File Offset: 0x000FF20F
		public void FistSlam_Recovery_Sign()
		{
			this.RunOperation(this._onFistSlam_Recovery_Sign);
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x0010101D File Offset: 0x000FF21D
		public void FistSlam_Sign()
		{
			this.RunOperation(this._onFistSlam_Sign);
		}

		// Token: 0x06005659 RID: 22105 RVA: 0x0010102B File Offset: 0x000FF22B
		public void FistSlam_Slam()
		{
			this.RunOperation(this._onFistSlam_Slam);
		}

		// Token: 0x0600565A RID: 22106 RVA: 0x00101039 File Offset: 0x000FF239
		public void FistSlam_Outro()
		{
			this.RunOperation(this._onFistSlam_Outro);
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x00101047 File Offset: 0x000FF247
		public void Sweeping_Intro()
		{
			this.RunOperation(this._onSweeping_Intro);
		}

		// Token: 0x0600565C RID: 22108 RVA: 0x00101055 File Offset: 0x000FF255
		public void Sweeping_Sign()
		{
			this.RunOperation(this._onSweeping_Sign);
		}

		// Token: 0x0600565D RID: 22109 RVA: 0x00101063 File Offset: 0x000FF263
		public void Sweeping_Sweeping()
		{
			this.RunOperation(this._onSweeping_Sweeping);
		}

		// Token: 0x0600565E RID: 22110 RVA: 0x00101071 File Offset: 0x000FF271
		public void Sweeping_Sweeping_End()
		{
			this.RunOperation(this._onSweeping_Sweeping_End);
		}

		// Token: 0x0600565F RID: 22111 RVA: 0x0010107F File Offset: 0x000FF27F
		public void Sweeping_Outro()
		{
			this.RunOperation(this._onSweeping_Outro);
		}

		// Token: 0x06005660 RID: 22112 RVA: 0x0010108D File Offset: 0x000FF28D
		public void Laser_Intro_Impact()
		{
			this.RunOperation(this._onLaser_Intro_Impact);
		}

		// Token: 0x06005661 RID: 22113 RVA: 0x0010109B File Offset: 0x000FF29B
		public void Laser_Sign()
		{
			this.RunOperation(this._onLaser_Sign);
		}

		// Token: 0x06005662 RID: 22114 RVA: 0x001010A9 File Offset: 0x000FF2A9
		public void Laser_Impact()
		{
			this.RunOperation(this._onLaser_Impact);
		}

		// Token: 0x06005663 RID: 22115 RVA: 0x001010B7 File Offset: 0x000FF2B7
		public void Laser_Laser()
		{
			this.RunOperation(this._onLaser_Laser);
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x001010C5 File Offset: 0x000FF2C5
		public void Laser_Laser_End()
		{
			this.RunOperation(this._onLaser_Laser_End);
		}

		// Token: 0x06005665 RID: 22117 RVA: 0x001010D3 File Offset: 0x000FF2D3
		public void EnergyBomb_Fire()
		{
			this.RunOperation(this._onEnergyBomb_Fire);
		}

		// Token: 0x06005666 RID: 22118 RVA: 0x001010E1 File Offset: 0x000FF2E1
		public void Groggy_Intro()
		{
			this.RunOperation(this._onGroggy_Intro);
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x001010EF File Offset: 0x000FF2EF
		public void Groggy_Groggy()
		{
			this.RunOperation(this._onGroggy_Groggy);
		}

		// Token: 0x06005668 RID: 22120 RVA: 0x001010FD File Offset: 0x000FF2FD
		public void Groggy_Impact()
		{
			this.RunOperation(this._onGroggy_Impact);
		}

		// Token: 0x06005669 RID: 22121 RVA: 0x0010110B File Offset: 0x000FF30B
		public void Groggy_Recovery()
		{
			this.RunOperation(this._onGroggy_Recovery);
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x00101119 File Offset: 0x000FF319
		public void Dead_Intro()
		{
			this.RunOperation(this._onDead_Intro);
		}

		// Token: 0x0600566B RID: 22123 RVA: 0x00101127 File Offset: 0x000FF327
		public void Dead_DarkQuartz_Intro()
		{
			this.RunOperation(this._onDead_DarkQuartz_Intro);
		}

		// Token: 0x0600566C RID: 22124 RVA: 0x00101135 File Offset: 0x000FF335
		public void Dead_DarkQuartz_Spark()
		{
			this.RunOperation(this._onDead_DarkQuartz_Spark);
		}

		// Token: 0x0600566D RID: 22125 RVA: 0x00101143 File Offset: 0x000FF343
		public void Dead_DarkQuartz_Explosion()
		{
			this.RunOperation(this._onDead_DarkQuartz_Explosion);
		}

		// Token: 0x0600566E RID: 22126 RVA: 0x00101151 File Offset: 0x000FF351
		public void Dead_Normalize()
		{
			this.RunOperation(this._onDead_Normalize);
		}

		// Token: 0x0600566F RID: 22127 RVA: 0x0010115F File Offset: 0x000FF35F
		public void P2_Awakening_Intro()
		{
			this.RunOperation(this._onP2_Awakening_Intro);
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x0010116D File Offset: 0x000FF36D
		public void P2_Awakening_Awakening()
		{
			this.RunOperation(this._onP2_Awakening_Awakening);
		}

		// Token: 0x06005671 RID: 22129 RVA: 0x0010117B File Offset: 0x000FF37B
		public void P2_EnergyCorps_Intro_Impact()
		{
			this.RunOperation(this._onP2_EnergyCorps_Intro_Impact);
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x00101189 File Offset: 0x000FF389
		public void P2_EnergyCorps_Sign()
		{
			this.RunOperation(this._onP2_EnergyCorps_Sign);
		}

		// Token: 0x06005673 RID: 22131 RVA: 0x00101197 File Offset: 0x000FF397
		public void P2_EnergyCorps_Emerge()
		{
			this.RunOperation(this._onP2_EnergyCorps_Emerge);
		}

		// Token: 0x06005674 RID: 22132 RVA: 0x001011A5 File Offset: 0x000FF3A5
		public void P2_Groggy_Intro()
		{
			this.RunOperation(this._onP2_Groggy_Intro);
		}

		// Token: 0x06005675 RID: 22133 RVA: 0x001011B3 File Offset: 0x000FF3B3
		public void P2_Groggy_Groggy()
		{
			this.RunOperation(this._onP2_Groggy_Groggy);
		}

		// Token: 0x06005676 RID: 22134 RVA: 0x001011C1 File Offset: 0x000FF3C1
		public void P2_Groggy_Impact()
		{
			this.RunOperation(this._onP2_Groggy_Impact);
		}

		// Token: 0x06005677 RID: 22135 RVA: 0x001011CF File Offset: 0x000FF3CF
		public void P2_Groggy_Recovery()
		{
			this.RunOperation(this._onP2_Groggy_Recovery);
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x001011DD File Offset: 0x000FF3DD
		public void P2_FistPowerSlam_Intro()
		{
			this.RunOperation(this._onP2_FistPowerSlam_Intro);
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x001011EB File Offset: 0x000FF3EB
		public void P2_FistPowerSlam_Sign()
		{
			this.RunOperation(this._onP2_FistPowerSlam_Sign);
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x001011F9 File Offset: 0x000FF3F9
		public void P2_FistPowerSlam_Sign2()
		{
			this.RunOperation(this._onP2_FistPowerSlam_Sign2);
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x00101207 File Offset: 0x000FF407
		public void P2_FistPowerSlam_Slam()
		{
			this.RunOperation(this._onP2_FistPowerSlam_Slam);
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x00101215 File Offset: 0x000FF415
		public void P2_FistPowerSlam_Impact()
		{
			this.RunOperation(this._onP2_FistPowerSlam_Impact);
		}

		// Token: 0x0600567D RID: 22141 RVA: 0x00101223 File Offset: 0x000FF423
		public void P2_FistPowerSlam_Recovery_Sign()
		{
			this.RunOperation(this._onP2_FistPowerSlam_Recovery_Sign);
		}

		// Token: 0x0600567E RID: 22142 RVA: 0x00101231 File Offset: 0x000FF431
		public void P2_FistPowerSlam_Recovery()
		{
			this.RunOperation(this._onP2_FistPowerSlam_Recovery);
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x0010123F File Offset: 0x000FF43F
		public void P2_FistPowerSlam_Outro()
		{
			this.RunOperation(this._onP2_FistPowerSlam_Outro);
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x0010124D File Offset: 0x000FF44D
		public void P2_BothFistPowerSlam_Intro()
		{
			this.RunOperation(this._onP2_BothFistPowerSlam_Intro);
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x0010125B File Offset: 0x000FF45B
		public void P2_BothFistPowerSlam_Sign()
		{
			this.RunOperation(this._onP2_BothFistPowerSlam_Sign);
		}

		// Token: 0x06005682 RID: 22146 RVA: 0x00101269 File Offset: 0x000FF469
		public void P2_BothFistPowerSlam_Slam()
		{
			this.RunOperation(this._onP2_BothFistPowerSlam_Slam);
		}

		// Token: 0x06005683 RID: 22147 RVA: 0x00101278 File Offset: 0x000FF478
		public void P2_BothFistPowerSlam_Impact()
		{
			this.RunOperation(this._onP2_BothFistPowerSlam_Impact);
			if (!Singleton<HardmodeManager>.Instance.hardmode)
			{
				return;
			}
			this._fistSlamCount++;
			if (this._fistSlamCount == 3)
			{
				this._fistSlamCount = 0;
				this.RunOperation(this._onP2_BothFistPowerSlam_Impact_InHardmode);
			}
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x001012C8 File Offset: 0x000FF4C8
		public void P2_SweepingCombo_Intro()
		{
			this.RunOperation(this._onP2_SweepingCombo_Intro);
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x001012D6 File Offset: 0x000FF4D6
		public void P2_SweepingCombo_Ready()
		{
			this.RunOperation(this._onP2_SweepingCombo_Ready);
		}

		// Token: 0x06005686 RID: 22150 RVA: 0x001012E4 File Offset: 0x000FF4E4
		public void P2_SweepingCombo_Sign()
		{
			this.RunOperation(this._onP2_SweepingCombo_Sign);
		}

		// Token: 0x06005687 RID: 22151 RVA: 0x001012F2 File Offset: 0x000FF4F2
		public void P2_SweepingCombo_Sweeping()
		{
			this.RunOperation(this._onP2_SweepingCombo_Sweeping);
		}

		// Token: 0x06005688 RID: 22152 RVA: 0x00101300 File Offset: 0x000FF500
		public void P2_SweepingCombo_Sweeping_End()
		{
			this.RunOperation(this._onP2_SweepingCombo_End);
		}

		// Token: 0x06005689 RID: 22153 RVA: 0x0010130E File Offset: 0x000FF50E
		public void P2_SweepingCombo_Outro()
		{
			this.RunOperation(this._onP2_SweepingCombo_Outro);
		}

		// Token: 0x04004550 RID: 17744
		[SerializeField]
		private Character _owner;

		// Token: 0x04004551 RID: 17745
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		[Header("Platform")]
		[Space]
		private OperationInfos _onPlatform_On;

		// Token: 0x04004552 RID: 17746
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onPlatform_Off;

		// Token: 0x04004553 RID: 17747
		[Header("1 Phase")]
		[Space]
		[Header("Appearance")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onAppearance_Appearance;

		// Token: 0x04004554 RID: 17748
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onAppearance_Impact_Left;

		// Token: 0x04004555 RID: 17749
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onAppearance_Impact_Right;

		// Token: 0x04004556 RID: 17750
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onAppearance_Roar;

		// Token: 0x04004557 RID: 17751
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onAppearance_Sign;

		// Token: 0x04004558 RID: 17752
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		[Header("Fist Slam")]
		private OperationInfos _onFistSlam_Intro;

		// Token: 0x04004559 RID: 17753
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onFistSlam_Impact;

		// Token: 0x0400455A RID: 17754
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onFistSlam_Recovery;

		// Token: 0x0400455B RID: 17755
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onFistSlam_Recovery_Sign;

		// Token: 0x0400455C RID: 17756
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onFistSlam_Sign;

		// Token: 0x0400455D RID: 17757
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onFistSlam_Slam;

		// Token: 0x0400455E RID: 17758
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onFistSlam_Outro;

		// Token: 0x0400455F RID: 17759
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		[Header("Sweeping")]
		private OperationInfos _onSweeping_Intro;

		// Token: 0x04004560 RID: 17760
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onSweeping_Sign;

		// Token: 0x04004561 RID: 17761
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onSweeping_Sweeping;

		// Token: 0x04004562 RID: 17762
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onSweeping_Sweeping_End;

		// Token: 0x04004563 RID: 17763
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onSweeping_Outro;

		// Token: 0x04004564 RID: 17764
		[Header("Energy Bomb")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onLaser_Intro_Impact;

		// Token: 0x04004565 RID: 17765
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onLaser_Sign;

		// Token: 0x04004566 RID: 17766
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onLaser_Impact;

		// Token: 0x04004567 RID: 17767
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onLaser_Laser;

		// Token: 0x04004568 RID: 17768
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onLaser_Laser_End;

		// Token: 0x04004569 RID: 17769
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onEnergyBomb_Fire;

		// Token: 0x0400456A RID: 17770
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		[Header("Groggy")]
		private OperationInfos _onGroggy_Intro;

		// Token: 0x0400456B RID: 17771
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onGroggy_Groggy;

		// Token: 0x0400456C RID: 17772
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onGroggy_Impact;

		// Token: 0x0400456D RID: 17773
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onGroggy_Recovery;

		// Token: 0x0400456E RID: 17774
		[Header("Dead")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onDead_Intro;

		// Token: 0x0400456F RID: 17775
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onDead_DarkQuartz_Intro;

		// Token: 0x04004570 RID: 17776
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onDead_DarkQuartz_Spark;

		// Token: 0x04004571 RID: 17777
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onDead_DarkQuartz_Explosion;

		// Token: 0x04004572 RID: 17778
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onDead_Normalize;

		// Token: 0x04004573 RID: 17779
		[Space]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		[Header("2 Phase")]
		[Header("Awakening")]
		private OperationInfos _onP2_Awakening_Intro;

		// Token: 0x04004574 RID: 17780
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_Awakening_Awakening;

		// Token: 0x04004575 RID: 17781
		[Header("EnergyCorps")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_EnergyCorps_Intro_Impact;

		// Token: 0x04004576 RID: 17782
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_EnergyCorps_Sign;

		// Token: 0x04004577 RID: 17783
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_EnergyCorps_Emerge;

		// Token: 0x04004578 RID: 17784
		[Subcomponent(typeof(OperationInfos))]
		[Header("Groggy")]
		[SerializeField]
		private OperationInfos _onP2_Groggy_Intro;

		// Token: 0x04004579 RID: 17785
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_Groggy_Groggy;

		// Token: 0x0400457A RID: 17786
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_Groggy_Impact;

		// Token: 0x0400457B RID: 17787
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_Groggy_Recovery;

		// Token: 0x0400457C RID: 17788
		[Header("Fist Power Slam")]
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_FistPowerSlam_Intro;

		// Token: 0x0400457D RID: 17789
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_FistPowerSlam_Sign;

		// Token: 0x0400457E RID: 17790
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_FistPowerSlam_Sign2;

		// Token: 0x0400457F RID: 17791
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_FistPowerSlam_Slam;

		// Token: 0x04004580 RID: 17792
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_FistPowerSlam_Impact;

		// Token: 0x04004581 RID: 17793
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_FistPowerSlam_Recovery_Sign;

		// Token: 0x04004582 RID: 17794
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_FistPowerSlam_Recovery;

		// Token: 0x04004583 RID: 17795
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_FistPowerSlam_Outro;

		// Token: 0x04004584 RID: 17796
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		[Header("Both Fist Power Slam")]
		private OperationInfos _onP2_BothFistPowerSlam_Intro;

		// Token: 0x04004585 RID: 17797
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_BothFistPowerSlam_Sign;

		// Token: 0x04004586 RID: 17798
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_BothFistPowerSlam_Slam;

		// Token: 0x04004587 RID: 17799
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_BothFistPowerSlam_Impact;

		// Token: 0x04004588 RID: 17800
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_BothFistPowerSlam_Impact_InHardmode;

		// Token: 0x04004589 RID: 17801
		private int _fistSlamCount;

		// Token: 0x0400458A RID: 17802
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		[Header("Sweeping Combo")]
		private OperationInfos _onP2_SweepingCombo_Intro;

		// Token: 0x0400458B RID: 17803
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_SweepingCombo_Ready;

		// Token: 0x0400458C RID: 17804
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_SweepingCombo_Sign;

		// Token: 0x0400458D RID: 17805
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_SweepingCombo_Sweeping;

		// Token: 0x0400458E RID: 17806
		[SerializeField]
		[Subcomponent(typeof(OperationInfos))]
		private OperationInfos _onP2_SweepingCombo_End;

		// Token: 0x0400458F RID: 17807
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos _onP2_SweepingCombo_Outro;
	}
}
