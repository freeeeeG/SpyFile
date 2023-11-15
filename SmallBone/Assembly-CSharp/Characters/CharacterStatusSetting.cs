using System;
using Characters.Operations;
using FX;
using FX.SpriteEffects;
using UnityEngine;

namespace Characters
{
	// Token: 0x020006D4 RID: 1748
	[CreateAssetMenu]
	public class CharacterStatusSetting : ScriptableObject
	{
		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x0006AA4E File Offset: 0x00068C4E
		public CharacterStatusSetting.Poison poison
		{
			get
			{
				return this._poison;
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002358 RID: 9048 RVA: 0x0006AA56 File Offset: 0x00068C56
		public CharacterStatusSetting.Bleed bleed
		{
			get
			{
				return this._bleed;
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x06002359 RID: 9049 RVA: 0x0006AA5E File Offset: 0x00068C5E
		public CharacterStatusSetting.Burn burn
		{
			get
			{
				return this._burn;
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x0600235A RID: 9050 RVA: 0x0006AA66 File Offset: 0x00068C66
		public CharacterStatusSetting.Freeze freeze
		{
			get
			{
				return this._freeze;
			}
		}

		// Token: 0x17000753 RID: 1875
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x0006AA6E File Offset: 0x00068C6E
		public CharacterStatusSetting.Stun stun
		{
			get
			{
				return this._stun;
			}
		}

		// Token: 0x17000754 RID: 1876
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x0006AA76 File Offset: 0x00068C76
		public static CharacterStatusSetting instance
		{
			get
			{
				if (CharacterStatusSetting._instance == null)
				{
					CharacterStatusSetting._instance = Resources.Load<CharacterStatusSetting>("CharacterStatusSetting");
				}
				return CharacterStatusSetting._instance;
			}
		}

		// Token: 0x04001E01 RID: 7681
		private static CharacterStatusSetting _instance;

		// Token: 0x04001E02 RID: 7682
		[SerializeField]
		private CharacterStatusSetting.Poison _poison;

		// Token: 0x04001E03 RID: 7683
		[SerializeField]
		private CharacterStatusSetting.Bleed _bleed;

		// Token: 0x04001E04 RID: 7684
		[SerializeField]
		private CharacterStatusSetting.Burn _burn;

		// Token: 0x04001E05 RID: 7685
		[SerializeField]
		private CharacterStatusSetting.Freeze _freeze;

		// Token: 0x04001E06 RID: 7686
		[SerializeField]
		private CharacterStatusSetting.Stun _stun;

		// Token: 0x020006D5 RID: 1749
		[Serializable]
		public class Poison
		{
			// Token: 0x17000755 RID: 1877
			// (get) Token: 0x0600235E RID: 9054 RVA: 0x0006AA99 File Offset: 0x00068C99
			public HitInfo hitInfo
			{
				get
				{
					return this._hitInfo;
				}
			}

			// Token: 0x17000756 RID: 1878
			// (get) Token: 0x0600235F RID: 9055 RVA: 0x0006AAA1 File Offset: 0x00068CA1
			public float baseTickDamage
			{
				get
				{
					return this._baseTickDamage;
				}
			}

			// Token: 0x17000757 RID: 1879
			// (get) Token: 0x06002360 RID: 9056 RVA: 0x0006AAA9 File Offset: 0x00068CA9
			public float duration
			{
				get
				{
					return this._duration;
				}
			}

			// Token: 0x17000758 RID: 1880
			// (get) Token: 0x06002361 RID: 9057 RVA: 0x0006AAB1 File Offset: 0x00068CB1
			public float tickFrequency
			{
				get
				{
					return this._tickFrequency;
				}
			}

			// Token: 0x17000759 RID: 1881
			// (get) Token: 0x06002362 RID: 9058 RVA: 0x0006AAB9 File Offset: 0x00068CB9
			public RuntimeAnimatorController loopEffectA
			{
				get
				{
					return this._loopEffectA;
				}
			}

			// Token: 0x1700075A RID: 1882
			// (get) Token: 0x06002363 RID: 9059 RVA: 0x0006AAC1 File Offset: 0x00068CC1
			public RuntimeAnimatorController loopEffectB
			{
				get
				{
					return this._loopEffectB;
				}
			}

			// Token: 0x1700075B RID: 1883
			// (get) Token: 0x06002364 RID: 9060 RVA: 0x0006AAC9 File Offset: 0x00068CC9
			public RuntimeAnimatorController loopEffectC
			{
				get
				{
					return this._loopEffectC;
				}
			}

			// Token: 0x1700075C RID: 1884
			// (get) Token: 0x06002365 RID: 9061 RVA: 0x0006AAD1 File Offset: 0x00068CD1
			public SoundInfo attachSound
			{
				get
				{
					return this._attachSound;
				}
			}

			// Token: 0x1700075D RID: 1885
			// (get) Token: 0x06002366 RID: 9062 RVA: 0x0006AAD9 File Offset: 0x00068CD9
			public GenericSpriteEffect.ColorOverlay colorOverlay
			{
				get
				{
					return this._colorOverlay;
				}
			}

			// Token: 0x1700075E RID: 1886
			// (get) Token: 0x06002367 RID: 9063 RVA: 0x0006AAE1 File Offset: 0x00068CE1
			public GenericSpriteEffect.ColorBlend colorBlend
			{
				get
				{
					return this._colorBlend;
				}
			}

			// Token: 0x1700075F RID: 1887
			// (get) Token: 0x06002368 RID: 9064 RVA: 0x0006AAE9 File Offset: 0x00068CE9
			public GenericSpriteEffect.Outline outline
			{
				get
				{
					return this._outline;
				}
			}

			// Token: 0x17000760 RID: 1888
			// (get) Token: 0x06002369 RID: 9065 RVA: 0x0006AAF1 File Offset: 0x00068CF1
			public GenericSpriteEffect.GrayScale grayScale
			{
				get
				{
					return this._grayScale;
				}
			}

			// Token: 0x04001E07 RID: 7687
			[SerializeField]
			internal HitInfo _hitInfo;

			// Token: 0x04001E08 RID: 7688
			[SerializeField]
			internal float _baseTickDamage;

			// Token: 0x04001E09 RID: 7689
			[SerializeField]
			private float _duration;

			// Token: 0x04001E0A RID: 7690
			[SerializeField]
			private float _tickFrequency;

			// Token: 0x04001E0B RID: 7691
			[SerializeField]
			private RuntimeAnimatorController _loopEffectA;

			// Token: 0x04001E0C RID: 7692
			[SerializeField]
			private RuntimeAnimatorController _loopEffectB;

			// Token: 0x04001E0D RID: 7693
			[SerializeField]
			private RuntimeAnimatorController _loopEffectC;

			// Token: 0x04001E0E RID: 7694
			[SerializeField]
			private SoundInfo _attachSound;

			// Token: 0x04001E0F RID: 7695
			[SerializeField]
			[Space(10f)]
			private GenericSpriteEffect.ColorOverlay _colorOverlay;

			// Token: 0x04001E10 RID: 7696
			[SerializeField]
			private GenericSpriteEffect.ColorBlend _colorBlend;

			// Token: 0x04001E11 RID: 7697
			[SerializeField]
			private GenericSpriteEffect.Outline _outline;

			// Token: 0x04001E12 RID: 7698
			[SerializeField]
			private GenericSpriteEffect.GrayScale _grayScale;
		}

		// Token: 0x020006D6 RID: 1750
		[Serializable]
		public class Bleed
		{
			// Token: 0x17000761 RID: 1889
			// (get) Token: 0x0600236B RID: 9067 RVA: 0x0006AAF9 File Offset: 0x00068CF9
			public HitInfo hitInfo
			{
				get
				{
					return this._hitInfo;
				}
			}

			// Token: 0x17000762 RID: 1890
			// (get) Token: 0x0600236C RID: 9068 RVA: 0x0006AB01 File Offset: 0x00068D01
			public float baseDamage
			{
				get
				{
					return this._baseDamage;
				}
			}

			// Token: 0x17000763 RID: 1891
			// (get) Token: 0x0600236D RID: 9069 RVA: 0x0006AB09 File Offset: 0x00068D09
			public RuntimeAnimatorController loopEffectA
			{
				get
				{
					return this._loopEffectA;
				}
			}

			// Token: 0x17000764 RID: 1892
			// (get) Token: 0x0600236E RID: 9070 RVA: 0x0006AB11 File Offset: 0x00068D11
			public RuntimeAnimatorController loopEffectB
			{
				get
				{
					return this._loopEffectB;
				}
			}

			// Token: 0x17000765 RID: 1893
			// (get) Token: 0x0600236F RID: 9071 RVA: 0x0006AB19 File Offset: 0x00068D19
			public RuntimeAnimatorController dripEffect
			{
				get
				{
					return this._dripEffect;
				}
			}

			// Token: 0x17000766 RID: 1894
			// (get) Token: 0x06002370 RID: 9072 RVA: 0x0006AB21 File Offset: 0x00068D21
			public RuntimeAnimatorController impactEffect
			{
				get
				{
					return this._impactEffect;
				}
			}

			// Token: 0x17000767 RID: 1895
			// (get) Token: 0x06002371 RID: 9073 RVA: 0x0006AB29 File Offset: 0x00068D29
			public SoundInfo attachSound
			{
				get
				{
					return this._attachSound;
				}
			}

			// Token: 0x17000768 RID: 1896
			// (get) Token: 0x06002372 RID: 9074 RVA: 0x0006AB31 File Offset: 0x00068D31
			public SoundInfo loopImpactSound
			{
				get
				{
					return this._loopSound;
				}
			}

			// Token: 0x17000769 RID: 1897
			// (get) Token: 0x06002373 RID: 9075 RVA: 0x0006AB39 File Offset: 0x00068D39
			public SoundInfo impactSound
			{
				get
				{
					return this._impactSound;
				}
			}

			// Token: 0x1700076A RID: 1898
			// (get) Token: 0x06002374 RID: 9076 RVA: 0x0006AB41 File Offset: 0x00068D41
			public GenericSpriteEffect.ColorOverlay colorOverlay
			{
				get
				{
					return this._colorOverlay;
				}
			}

			// Token: 0x1700076B RID: 1899
			// (get) Token: 0x06002375 RID: 9077 RVA: 0x0006AB49 File Offset: 0x00068D49
			public GenericSpriteEffect.ColorBlend colorBlend
			{
				get
				{
					return this._colorBlend;
				}
			}

			// Token: 0x1700076C RID: 1900
			// (get) Token: 0x06002376 RID: 9078 RVA: 0x0006AB51 File Offset: 0x00068D51
			public GenericSpriteEffect.Outline outline
			{
				get
				{
					return this._outline;
				}
			}

			// Token: 0x1700076D RID: 1901
			// (get) Token: 0x06002377 RID: 9079 RVA: 0x0006AB59 File Offset: 0x00068D59
			public GenericSpriteEffect.GrayScale grayScale
			{
				get
				{
					return this._grayScale;
				}
			}

			// Token: 0x1700076E RID: 1902
			// (get) Token: 0x06002378 RID: 9080 RVA: 0x0006AB61 File Offset: 0x00068D61
			public GenericSpriteEffect.ColorOverlay colorOverlayBleed
			{
				get
				{
					return this._colorOverlayBleed;
				}
			}

			// Token: 0x1700076F RID: 1903
			// (get) Token: 0x06002379 RID: 9081 RVA: 0x0006AB69 File Offset: 0x00068D69
			public GenericSpriteEffect.ColorBlend colorBlendBleed
			{
				get
				{
					return this._colorBlendBleed;
				}
			}

			// Token: 0x17000770 RID: 1904
			// (get) Token: 0x0600237A RID: 9082 RVA: 0x0006AB71 File Offset: 0x00068D71
			public GenericSpriteEffect.Outline outlineBleed
			{
				get
				{
					return this._outlineBleed;
				}
			}

			// Token: 0x17000771 RID: 1905
			// (get) Token: 0x0600237B RID: 9083 RVA: 0x0006AB79 File Offset: 0x00068D79
			public GenericSpriteEffect.GrayScale grayScaleBleed
			{
				get
				{
					return this._grayScaleBleed;
				}
			}

			// Token: 0x04001E13 RID: 7699
			[SerializeField]
			internal HitInfo _hitInfo;

			// Token: 0x04001E14 RID: 7700
			[SerializeField]
			private float _baseDamage;

			// Token: 0x04001E15 RID: 7701
			[SerializeField]
			private RuntimeAnimatorController _loopEffectA;

			// Token: 0x04001E16 RID: 7702
			[SerializeField]
			private RuntimeAnimatorController _loopEffectB;

			// Token: 0x04001E17 RID: 7703
			[SerializeField]
			private RuntimeAnimatorController _dripEffect;

			// Token: 0x04001E18 RID: 7704
			[SerializeField]
			private RuntimeAnimatorController _impactEffect;

			// Token: 0x04001E19 RID: 7705
			[SerializeField]
			private SoundInfo _attachSound;

			// Token: 0x04001E1A RID: 7706
			[SerializeField]
			private SoundInfo _loopSound;

			// Token: 0x04001E1B RID: 7707
			[SerializeField]
			private SoundInfo _impactSound;

			// Token: 0x04001E1C RID: 7708
			[SerializeField]
			[Space(10f)]
			private GenericSpriteEffect.ColorOverlay _colorOverlay;

			// Token: 0x04001E1D RID: 7709
			[SerializeField]
			private GenericSpriteEffect.ColorBlend _colorBlend;

			// Token: 0x04001E1E RID: 7710
			[SerializeField]
			private GenericSpriteEffect.Outline _outline;

			// Token: 0x04001E1F RID: 7711
			[SerializeField]
			private GenericSpriteEffect.GrayScale _grayScale;

			// Token: 0x04001E20 RID: 7712
			[Space(10f)]
			[SerializeField]
			private GenericSpriteEffect.ColorOverlay _colorOverlayBleed;

			// Token: 0x04001E21 RID: 7713
			[SerializeField]
			private GenericSpriteEffect.ColorBlend _colorBlendBleed;

			// Token: 0x04001E22 RID: 7714
			[SerializeField]
			private GenericSpriteEffect.Outline _outlineBleed;

			// Token: 0x04001E23 RID: 7715
			[SerializeField]
			private GenericSpriteEffect.GrayScale _grayScaleBleed;
		}

		// Token: 0x020006D7 RID: 1751
		[Serializable]
		public class Burn
		{
			// Token: 0x17000772 RID: 1906
			// (get) Token: 0x0600237D RID: 9085 RVA: 0x0006AB81 File Offset: 0x00068D81
			public HitInfo hitInfo
			{
				get
				{
					return this._hitInfo;
				}
			}

			// Token: 0x17000773 RID: 1907
			// (get) Token: 0x0600237E RID: 9086 RVA: 0x0006AB89 File Offset: 0x00068D89
			public HitInfo rangeHitInfo
			{
				get
				{
					return this._rangeHitInfo;
				}
			}

			// Token: 0x17000774 RID: 1908
			// (get) Token: 0x0600237F RID: 9087 RVA: 0x0006AB91 File Offset: 0x00068D91
			public float baseTargetTickDamage
			{
				get
				{
					return this._baseTargetTickDamage;
				}
			}

			// Token: 0x17000775 RID: 1909
			// (get) Token: 0x06002380 RID: 9088 RVA: 0x0006AB99 File Offset: 0x00068D99
			public float baseRangeTickDamage
			{
				get
				{
					return this._baseRangeTickDamage;
				}
			}

			// Token: 0x17000776 RID: 1910
			// (get) Token: 0x06002381 RID: 9089 RVA: 0x0006ABA1 File Offset: 0x00068DA1
			public float duration
			{
				get
				{
					return this._duration;
				}
			}

			// Token: 0x17000777 RID: 1911
			// (get) Token: 0x06002382 RID: 9090 RVA: 0x0006ABA9 File Offset: 0x00068DA9
			public float tickInterval
			{
				get
				{
					return this._tickInterval;
				}
			}

			// Token: 0x17000778 RID: 1912
			// (get) Token: 0x06002383 RID: 9091 RVA: 0x0006ABB1 File Offset: 0x00068DB1
			public float rangeRadius
			{
				get
				{
					return this._rangeRadius;
				}
			}

			// Token: 0x17000779 RID: 1913
			// (get) Token: 0x06002384 RID: 9092 RVA: 0x0006ABB9 File Offset: 0x00068DB9
			public RuntimeAnimatorController loopEffect
			{
				get
				{
					return this._loopEffect;
				}
			}

			// Token: 0x1700077A RID: 1914
			// (get) Token: 0x06002385 RID: 9093 RVA: 0x0006ABC1 File Offset: 0x00068DC1
			public SoundInfo attachSound
			{
				get
				{
					return this._attachSound;
				}
			}

			// Token: 0x1700077B RID: 1915
			// (get) Token: 0x06002386 RID: 9094 RVA: 0x0006ABC9 File Offset: 0x00068DC9
			public SoundInfo attackSound
			{
				get
				{
					return this._attackSound;
				}
			}

			// Token: 0x1700077C RID: 1916
			// (get) Token: 0x06002387 RID: 9095 RVA: 0x0006ABD1 File Offset: 0x00068DD1
			public GenericSpriteEffect.ColorOverlay colorOverlay
			{
				get
				{
					return this._colorOverlay;
				}
			}

			// Token: 0x1700077D RID: 1917
			// (get) Token: 0x06002388 RID: 9096 RVA: 0x0006ABD9 File Offset: 0x00068DD9
			public GenericSpriteEffect.ColorBlend colorBlend
			{
				get
				{
					return this._colorBlend;
				}
			}

			// Token: 0x1700077E RID: 1918
			// (get) Token: 0x06002389 RID: 9097 RVA: 0x0006ABE1 File Offset: 0x00068DE1
			public GenericSpriteEffect.Outline outline
			{
				get
				{
					return this._outline;
				}
			}

			// Token: 0x1700077F RID: 1919
			// (get) Token: 0x0600238A RID: 9098 RVA: 0x0006ABE9 File Offset: 0x00068DE9
			public GenericSpriteEffect.GrayScale grayScale
			{
				get
				{
					return this._grayScale;
				}
			}

			// Token: 0x04001E24 RID: 7716
			[SerializeField]
			internal HitInfo _hitInfo;

			// Token: 0x04001E25 RID: 7717
			[SerializeField]
			internal HitInfo _rangeHitInfo;

			// Token: 0x04001E26 RID: 7718
			[SerializeField]
			private float _baseTargetTickDamage;

			// Token: 0x04001E27 RID: 7719
			[SerializeField]
			private float _baseRangeTickDamage;

			// Token: 0x04001E28 RID: 7720
			[SerializeField]
			private float _duration;

			// Token: 0x04001E29 RID: 7721
			[SerializeField]
			private float _tickInterval;

			// Token: 0x04001E2A RID: 7722
			[SerializeField]
			private float _rangeRadius;

			// Token: 0x04001E2B RID: 7723
			[SerializeField]
			private RuntimeAnimatorController _loopEffect;

			// Token: 0x04001E2C RID: 7724
			[SerializeField]
			private SoundInfo _attachSound;

			// Token: 0x04001E2D RID: 7725
			[SerializeField]
			private SoundInfo _attackSound;

			// Token: 0x04001E2E RID: 7726
			[SerializeField]
			[Space(10f)]
			private GenericSpriteEffect.ColorOverlay _colorOverlay;

			// Token: 0x04001E2F RID: 7727
			[SerializeField]
			private GenericSpriteEffect.ColorBlend _colorBlend;

			// Token: 0x04001E30 RID: 7728
			[SerializeField]
			private GenericSpriteEffect.Outline _outline;

			// Token: 0x04001E31 RID: 7729
			[SerializeField]
			private GenericSpriteEffect.GrayScale _grayScale;
		}

		// Token: 0x020006D8 RID: 1752
		[Serializable]
		public class Freeze
		{
			// Token: 0x17000780 RID: 1920
			// (get) Token: 0x0600238C RID: 9100 RVA: 0x0006ABF1 File Offset: 0x00068DF1
			public float duration
			{
				get
				{
					return this._duration;
				}
			}

			// Token: 0x17000781 RID: 1921
			// (get) Token: 0x0600238D RID: 9101 RVA: 0x0006ABF9 File Offset: 0x00068DF9
			public float minimumTime
			{
				get
				{
					return this._minimumTime;
				}
			}

			// Token: 0x17000782 RID: 1922
			// (get) Token: 0x0600238E RID: 9102 RVA: 0x0006AC01 File Offset: 0x00068E01
			public float breakDuration
			{
				get
				{
					return this._breakDuration;
				}
			}

			// Token: 0x17000783 RID: 1923
			// (get) Token: 0x0600238F RID: 9103 RVA: 0x0006AC09 File Offset: 0x00068E09
			public SoundInfo attachSound
			{
				get
				{
					return this._attachSound;
				}
			}

			// Token: 0x17000784 RID: 1924
			// (get) Token: 0x06002390 RID: 9104 RVA: 0x0006AC11 File Offset: 0x00068E11
			public SoundInfo hitSound
			{
				get
				{
					return this._hitSound;
				}
			}

			// Token: 0x17000785 RID: 1925
			// (get) Token: 0x06002391 RID: 9105 RVA: 0x0006AC19 File Offset: 0x00068E19
			public SoundInfo detachSound
			{
				get
				{
					return this._detachSound;
				}
			}

			// Token: 0x17000786 RID: 1926
			// (get) Token: 0x06002392 RID: 9106 RVA: 0x0006AC21 File Offset: 0x00068E21
			public string[] nonHitCountAttackKeys
			{
				get
				{
					return this._nonHitCountAttackKeys;
				}
			}

			// Token: 0x04001E32 RID: 7730
			[SerializeField]
			private float _duration;

			// Token: 0x04001E33 RID: 7731
			[SerializeField]
			private float _minimumTime;

			// Token: 0x04001E34 RID: 7732
			[SerializeField]
			private float _breakDuration;

			// Token: 0x04001E35 RID: 7733
			[SerializeField]
			private SoundInfo _attachSound;

			// Token: 0x04001E36 RID: 7734
			[SerializeField]
			private SoundInfo _hitSound;

			// Token: 0x04001E37 RID: 7735
			[SerializeField]
			private SoundInfo _detachSound;

			// Token: 0x04001E38 RID: 7736
			[SerializeField]
			private string[] _nonHitCountAttackKeys;
		}

		// Token: 0x020006D9 RID: 1753
		[Serializable]
		public class Stun
		{
			// Token: 0x17000787 RID: 1927
			// (get) Token: 0x06002394 RID: 9108 RVA: 0x0006AC29 File Offset: 0x00068E29
			public float duration
			{
				get
				{
					return this._duration;
				}
			}

			// Token: 0x17000788 RID: 1928
			// (get) Token: 0x06002395 RID: 9109 RVA: 0x0006AC31 File Offset: 0x00068E31
			public SoundInfo attachSound
			{
				get
				{
					return this._attachSound;
				}
			}

			// Token: 0x04001E39 RID: 7737
			[SerializeField]
			private float _duration;

			// Token: 0x04001E3A RID: 7738
			[SerializeField]
			private SoundInfo _attachSound;
		}
	}
}
