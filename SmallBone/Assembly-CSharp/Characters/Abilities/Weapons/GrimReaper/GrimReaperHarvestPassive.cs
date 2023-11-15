using System;
using UnityEngine;

namespace Characters.Abilities.Weapons.GrimReaper
{
	// Token: 0x02000C0A RID: 3082
	[Serializable]
	public class GrimReaperHarvestPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000D5E RID: 3422
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x000B77B5 File Offset: 0x000B59B5
		// (set) Token: 0x06003F3F RID: 16191 RVA: 0x000B77BD File Offset: 0x000B59BD
		public Character owner { get; set; }

		// Token: 0x17000D5F RID: 3423
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000D60 RID: 3424
		// (get) Token: 0x06003F41 RID: 16193 RVA: 0x000B77C6 File Offset: 0x000B59C6
		// (set) Token: 0x06003F42 RID: 16194 RVA: 0x000B77CE File Offset: 0x000B59CE
		public float remainTime { get; set; }

		// Token: 0x17000D61 RID: 3425
		// (get) Token: 0x06003F43 RID: 16195 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000D62 RID: 3426
		// (get) Token: 0x06003F44 RID: 16196 RVA: 0x000B77D7 File Offset: 0x000B59D7
		public Sprite icon
		{
			get
			{
				if (this._stack <= 0)
				{
					return null;
				}
				return this._defaultIcon;
			}
		}

		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06003F45 RID: 16197 RVA: 0x000B77EA File Offset: 0x000B59EA
		public float iconFillAmount
		{
			get
			{
				return this.remainTime / base.duration;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06003F46 RID: 16198 RVA: 0x000B77F9 File Offset: 0x000B59F9
		public int iconStacks
		{
			get
			{
				return this._stack;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06003F47 RID: 16199 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06003F48 RID: 16200 RVA: 0x000B7801 File Offset: 0x000B5A01
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x000B780C File Offset: 0x000B5A0C
		public void Attach()
		{
			this._stack = 0;
			this._stat = this._statPerStack.Clone();
			this.owner.stat.AttachValues(this._stat);
			Character owner = this.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
			this.remainTime = this._duration;
			this.UpdateStack();
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x000B7880 File Offset: 0x000B5A80
		public void Detach()
		{
			this.owner.stat.DetachValues(this._stat);
			Character owner = this.owner;
			owner.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(owner.onGaveDamage, new GaveDamageDelegate(this.OnOwnerGaveDamage));
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x000B78C0 File Offset: 0x000B5AC0
		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (target.character == null)
			{
				return;
			}
			if (!this._characterType[target.character.type])
			{
				return;
			}
			if (!gaveDamage.key.Equals(this._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return;
			}
			if (!MMMaths.PercentChance(this._possibility))
			{
				return;
			}
			this._grimReaperSoul.Spawn(target.character.collider.bounds.center, this);
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x000B793C File Offset: 0x000B5B3C
		public void UpdateTime(float deltaTime)
		{
			this.remainTime -= deltaTime;
			if (this.remainTime <= 0f)
			{
				this.Reset();
			}
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x000B795F File Offset: 0x000B5B5F
		public void Reset()
		{
			this._stack = 0;
			this.UpdateStack();
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x000B796E File Offset: 0x000B5B6E
		public void AddStack()
		{
			this.remainTime = this._duration;
			this._stack = Mathf.Clamp(this._stack + 1, 0, this._maxStack);
			this.UpdateStack();
		}

		// Token: 0x06003F50 RID: 16208 RVA: 0x000B799C File Offset: 0x000B5B9C
		private void UpdateStack()
		{
			for (int i = 0; i < this._stat.values.Length; i++)
			{
				this._stat.values[i].value = this._statPerStack.values[i].GetStackedValue((double)this._stack);
			}
			this.owner.stat.SetNeedUpdate();
		}

		// Token: 0x040030B3 RID: 12467
		[SerializeField]
		[Header("영혼 생성")]
		private GrimReaperSoul _grimReaperSoul;

		// Token: 0x040030B4 RID: 12468
		[SerializeField]
		private string _attackKey;

		// Token: 0x040030B5 RID: 12469
		[SerializeField]
		[Range(1f, 100f)]
		private int _possibility;

		// Token: 0x040030B6 RID: 12470
		[SerializeField]
		private CharacterTypeBoolArray _characterType;

		// Token: 0x040030B7 RID: 12471
		[SerializeField]
		[Header("영혼 스텟")]
		private int _maxStack;

		// Token: 0x040030B8 RID: 12472
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x040030B9 RID: 12473
		private Stat.Values _stat;

		// Token: 0x040030BA RID: 12474
		private int _stack;
	}
}
