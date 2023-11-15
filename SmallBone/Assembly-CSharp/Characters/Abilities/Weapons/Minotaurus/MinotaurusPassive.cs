using System;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Weapons.Minotaurus
{
	// Token: 0x02000C08 RID: 3080
	[Serializable]
	public sealed class MinotaurusPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000D56 RID: 3414
		// (get) Token: 0x06003F26 RID: 16166 RVA: 0x000B75BD File Offset: 0x000B57BD
		// (set) Token: 0x06003F27 RID: 16167 RVA: 0x000B75C5 File Offset: 0x000B57C5
		public Character owner { get; set; }

		// Token: 0x17000D57 RID: 3415
		// (get) Token: 0x06003F28 RID: 16168 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x06003F29 RID: 16169 RVA: 0x000B75CE File Offset: 0x000B57CE
		// (set) Token: 0x06003F2A RID: 16170 RVA: 0x000B75D6 File Offset: 0x000B57D6
		public float remainTime { get; set; }

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x06003F2B RID: 16171 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x06003F2C RID: 16172 RVA: 0x000B75DF File Offset: 0x000B57DF
		public Sprite icon
		{
			get
			{
				if (this._gaugeWithValue.currentValue <= 0f || this.remainTime <= 0f)
				{
					return null;
				}
				return this._defaultIcon;
			}
		}

		// Token: 0x17000D5B RID: 3419
		// (get) Token: 0x06003F2D RID: 16173 RVA: 0x000B7608 File Offset: 0x000B5808
		public float iconFillAmount
		{
			get
			{
				return 1f - this.remainTime / base.duration;
			}
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x06003F2E RID: 16174 RVA: 0x00018EC5 File Offset: 0x000170C5
		public int iconStacks
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x06003F2F RID: 16175 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x000B761D File Offset: 0x000B581D
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x000B7627 File Offset: 0x000B5827
		public void Attach()
		{
			this.remainTime = base.duration;
			this._attacked = false;
			this.ResetStack();
		}

		// Token: 0x06003F32 RID: 16178 RVA: 0x000B7642 File Offset: 0x000B5842
		public void Detach()
		{
			this.remainTime = 0f;
			this._attacked = false;
			this.ResetStack();
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x000B765C File Offset: 0x000B585C
		public void UpdateTime(float deltaTime)
		{
			if (this._gaugeWithValue.currentValue > 0f)
			{
				this.remainTime -= deltaTime;
				if (this.remainTime <= 0f)
				{
					this.ResetStack();
				}
			}
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x000B7691 File Offset: 0x000B5891
		private void AddStack()
		{
			this._gaugeWithValue.Add(1f);
			this.remainTime = base.duration;
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x000B76B0 File Offset: 0x000B58B0
		public void StartRecordingAttacks()
		{
			this._attacked = false;
			Character owner = this.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			Character owner2 = this.owner;
			owner2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner2.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x000B7712 File Offset: 0x000B5912
		public void StopRecodingAttacks()
		{
			Character owner = this.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			if (!this._attacked)
			{
				this.ResetStack();
				return;
			}
			this.AddStack();
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x000B7750 File Offset: 0x000B5950
		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (gaveDamage.key != this._passiveAttackKey)
			{
				return;
			}
			this._attacked = true;
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x000B776D File Offset: 0x000B596D
		private void ResetStack()
		{
			if (this._gaugeWithValue.currentValue >= this._gaugeWithValue.maxValue)
			{
				return;
			}
			this._gaugeWithValue.Clear();
		}

		// Token: 0x040030AE RID: 12462
		[SerializeField]
		private string _passiveAttackKey;

		// Token: 0x040030AF RID: 12463
		[SerializeField]
		private ValueGauge _gaugeWithValue;

		// Token: 0x040030B0 RID: 12464
		private bool _attacked;
	}
}
