using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Weapons.Yaksha
{
	// Token: 0x02000C1E RID: 3102
	[Serializable]
	public class YakshaHomePassive : Ability, IAbilityInstance
	{
		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06003FB3 RID: 16307 RVA: 0x000B8D7E File Offset: 0x000B6F7E
		// (set) Token: 0x06003FB4 RID: 16308 RVA: 0x000B8D86 File Offset: 0x000B6F86
		public Character owner { get; set; }

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06003FB5 RID: 16309 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06003FB6 RID: 16310 RVA: 0x00071719 File Offset: 0x0006F919
		// (set) Token: 0x06003FB7 RID: 16311 RVA: 0x00002191 File Offset: 0x00000391
		public float remainTime
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06003FB8 RID: 16312 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06003FB9 RID: 16313 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06003FBA RID: 16314 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06003FBB RID: 16315 RVA: 0x000B8D8F File Offset: 0x000B6F8F
		// (set) Token: 0x06003FBC RID: 16316 RVA: 0x000B8D97 File Offset: 0x000B6F97
		public int iconStacks { get; protected set; }

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06003FBD RID: 16317 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003FBE RID: 16318 RVA: 0x000B8DA0 File Offset: 0x000B6FA0
		public override void Initialize()
		{
			base.Initialize();
			this._chargedBaseColor = this._gauge.defaultBarGaugeColor.chargedColor;
		}

		// Token: 0x06003FBF RID: 16319 RVA: 0x000716FD File Offset: 0x0006F8FD
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x06003FC0 RID: 16320 RVA: 0x000B8DC0 File Offset: 0x000B6FC0
		public void UpdateTime(float deltaTime)
		{
			if (this._gauge.gaugePercent < 1f)
			{
				return;
			}
			this._gaugeAnimationTime += deltaTime * 2f;
			if (this._gaugeAnimationTime > 2f)
			{
				this._gaugeAnimationTime = 0f;
			}
			this._gauge.defaultBarGaugeColor.chargedColor = Color.LerpUnclamped(this._chargedBaseColor, this._chargedBuffColor, (this._gaugeAnimationTime < 1f) ? this._gaugeAnimationTime : (2f - this._gaugeAnimationTime));
		}

		// Token: 0x06003FC1 RID: 16321 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003FC2 RID: 16322 RVA: 0x00002191 File Offset: 0x00000391
		public void Attach()
		{
		}

		// Token: 0x06003FC3 RID: 16323 RVA: 0x00002191 File Offset: 0x00000391
		public void Detach()
		{
		}

		// Token: 0x0400310A RID: 12554
		[SerializeField]
		[Header("게이지 바 설정")]
		private Color _chargedBuffColor;

		// Token: 0x0400310B RID: 12555
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x0400310E RID: 12558
		private Color _chargedBaseColor;

		// Token: 0x0400310F RID: 12559
		private float _gaugeAnimationTime;
	}
}
