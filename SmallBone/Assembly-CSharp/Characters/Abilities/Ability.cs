using System;
using FX;
using FX.SpriteEffects;
using UnityEngine;

namespace Characters.Abilities
{
	// Token: 0x0200098F RID: 2447
	[Serializable]
	public abstract class Ability : IAbility
	{
		// Token: 0x17000B48 RID: 2888
		// (get) Token: 0x0600345D RID: 13405 RVA: 0x0009ADAD File Offset: 0x00098FAD
		// (set) Token: 0x0600345E RID: 13406 RVA: 0x0009ADB5 File Offset: 0x00098FB5
		public float duration
		{
			get
			{
				return this._duration;
			}
			set
			{
				this._duration = value;
			}
		}

		// Token: 0x17000B49 RID: 2889
		// (get) Token: 0x0600345F RID: 13407 RVA: 0x0009ADBE File Offset: 0x00098FBE
		// (set) Token: 0x06003460 RID: 13408 RVA: 0x0009ADC6 File Offset: 0x00098FC6
		public Sprite defaultIcon
		{
			get
			{
				return this._defaultIcon;
			}
			set
			{
				this._defaultIcon = value;
			}
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06003461 RID: 13409 RVA: 0x0009ADCF File Offset: 0x00098FCF
		public int iconPriority
		{
			get
			{
				return this._iconPriority;
			}
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06003462 RID: 13410 RVA: 0x0009ADD7 File Offset: 0x00098FD7
		// (set) Token: 0x06003463 RID: 13411 RVA: 0x0009ADDF File Offset: 0x00098FDF
		public bool iconFillInversed
		{
			get
			{
				return this._iconFillInversed;
			}
			set
			{
				this._iconFillInversed = value;
			}
		}

		// Token: 0x17000B4C RID: 2892
		// (get) Token: 0x06003464 RID: 13412 RVA: 0x0009ADE8 File Offset: 0x00098FE8
		// (set) Token: 0x06003465 RID: 13413 RVA: 0x0009ADF0 File Offset: 0x00098FF0
		public bool iconFillFlipped
		{
			get
			{
				return this._iconFillFlipped;
			}
			set
			{
				this._iconFillFlipped = value;
			}
		}

		// Token: 0x17000B4D RID: 2893
		// (get) Token: 0x06003466 RID: 13414 RVA: 0x0009ADF9 File Offset: 0x00098FF9
		public bool removeOnSwapWeapon
		{
			get
			{
				return this._removeOnSwapWeapon;
			}
		}

		// Token: 0x17000B4E RID: 2894
		// (get) Token: 0x06003467 RID: 13415 RVA: 0x0009AE01 File Offset: 0x00099001
		public EffectInfo loopEffect
		{
			get
			{
				return this._loopEffect;
			}
		}

		// Token: 0x17000B4F RID: 2895
		// (get) Token: 0x06003468 RID: 13416 RVA: 0x0009AE09 File Offset: 0x00099009
		public Ability.SpriteEffect spriteEffect
		{
			get
			{
				return this._spriteEffect;
			}
		}

		// Token: 0x17000B50 RID: 2896
		// (get) Token: 0x06003469 RID: 13417 RVA: 0x0009AE11 File Offset: 0x00099011
		public EffectInfo effectOnAttach
		{
			get
			{
				return this._effectOnAttach;
			}
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x0600346A RID: 13418 RVA: 0x0009AE19 File Offset: 0x00099019
		public EffectInfo effectOnDetach
		{
			get
			{
				return this._effectOnDetach;
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x0600346B RID: 13419 RVA: 0x0009AE21 File Offset: 0x00099021
		public SoundInfo soundOnAttach
		{
			get
			{
				return this._soundOnAttach;
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x0600346C RID: 13420 RVA: 0x0009AE29 File Offset: 0x00099029
		public SoundInfo soundOnDetach
		{
			get
			{
				return this._soundOnDetach;
			}
		}

		// Token: 0x0600346D RID: 13421 RVA: 0x0009AE34 File Offset: 0x00099034
		~Ability()
		{
			this._loopEffect = null;
			this._effectOnAttach = null;
			this._effectOnDetach = null;
		}

		// Token: 0x0600346E RID: 13422 RVA: 0x0009AE70 File Offset: 0x00099070
		public virtual void Initialize()
		{
			if (this._duration == 0f)
			{
				this._duration = float.PositiveInfinity;
			}
		}

		// Token: 0x0600346F RID: 13423
		public abstract IAbilityInstance CreateInstance(Character owner);

		// Token: 0x06003470 RID: 13424 RVA: 0x0001B058 File Offset: 0x00019258
		public override string ToString()
		{
			return this.GetAutoName();
		}

		// Token: 0x04002A5A RID: 10842
		[SerializeField]
		protected float _duration;

		// Token: 0x04002A5B RID: 10843
		[SerializeField]
		protected Sprite _defaultIcon;

		// Token: 0x04002A5C RID: 10844
		[SerializeField]
		protected int _iconPriority;

		// Token: 0x04002A5D RID: 10845
		[SerializeField]
		[Tooltip("아이콘 채우기 색깔 반전")]
		protected bool _iconFillInversed;

		// Token: 0x04002A5E RID: 10846
		[Tooltip("아이콘 채우기 방향 반전")]
		[SerializeField]
		protected bool _iconFillFlipped;

		// Token: 0x04002A5F RID: 10847
		[SerializeField]
		protected bool _removeOnSwapWeapon;

		// Token: 0x04002A60 RID: 10848
		[SerializeField]
		[Header("Effects")]
		private EffectInfo _loopEffect = new EffectInfo
		{
			subordinated = true
		};

		// Token: 0x04002A61 RID: 10849
		[SerializeField]
		private Ability.SpriteEffect _spriteEffect;

		// Token: 0x04002A62 RID: 10850
		[SerializeField]
		private EffectInfo _effectOnAttach;

		// Token: 0x04002A63 RID: 10851
		[SerializeField]
		private EffectInfo _effectOnDetach;

		// Token: 0x04002A64 RID: 10852
		[SerializeField]
		private SoundInfo _soundOnAttach;

		// Token: 0x04002A65 RID: 10853
		[SerializeField]
		private SoundInfo _soundOnDetach;

		// Token: 0x02000990 RID: 2448
		[Serializable]
		public class SpriteEffect
		{
			// Token: 0x17000B54 RID: 2900
			// (get) Token: 0x06003472 RID: 13426 RVA: 0x0009AEA4 File Offset: 0x000990A4
			public bool enabled
			{
				get
				{
					return this._colorOverlay.enabled || this._colorBlend.enabled || this._outline.enabled || this._grayScale.enabled;
				}
			}

			// Token: 0x06003473 RID: 13427 RVA: 0x0009AEDA File Offset: 0x000990DA
			public GenericSpriteEffect CreateInstance()
			{
				return new GenericSpriteEffect(this._priority, float.MaxValue, 1f, this._colorOverlay, this._colorBlend, this._outline, this._grayScale);
			}

			// Token: 0x04002A66 RID: 10854
			[SerializeField]
			private int _priority;

			// Token: 0x04002A67 RID: 10855
			[SerializeField]
			private GenericSpriteEffect.ColorOverlay _colorOverlay;

			// Token: 0x04002A68 RID: 10856
			[SerializeField]
			private GenericSpriteEffect.ColorBlend _colorBlend;

			// Token: 0x04002A69 RID: 10857
			[SerializeField]
			private GenericSpriteEffect.Outline _outline;

			// Token: 0x04002A6A RID: 10858
			[SerializeField]
			private GenericSpriteEffect.GrayScale _grayScale;
		}
	}
}
