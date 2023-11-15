using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D25 RID: 3365
	[Serializable]
	public class ArchlichSoulLootingPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E0E RID: 3598
		// (get) Token: 0x060043C8 RID: 17352 RVA: 0x000C53C4 File Offset: 0x000C35C4
		// (set) Token: 0x060043C9 RID: 17353 RVA: 0x000C53CC File Offset: 0x000C35CC
		public Character owner { get; set; }

		// Token: 0x17000E0F RID: 3599
		// (get) Token: 0x060043CA RID: 17354 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x060043CB RID: 17355 RVA: 0x00071719 File Offset: 0x0006F919
		// (set) Token: 0x060043CC RID: 17356 RVA: 0x00002191 File Offset: 0x00000391
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

		// Token: 0x17000E11 RID: 3601
		// (get) Token: 0x060043CD RID: 17357 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E12 RID: 3602
		// (get) Token: 0x060043CE RID: 17358 RVA: 0x000C53D5 File Offset: 0x000C35D5
		public Sprite icon
		{
			get
			{
				if (this._stacks <= 0)
				{
					return null;
				}
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E13 RID: 3603
		// (get) Token: 0x060043CF RID: 17359 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000E14 RID: 3604
		// (get) Token: 0x060043D0 RID: 17360 RVA: 0x000C53E8 File Offset: 0x000C35E8
		public int iconStacks
		{
			get
			{
				return this._stacks;
			}
		}

		// Token: 0x17000E15 RID: 3605
		// (get) Token: 0x060043D1 RID: 17361 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x000C53F0 File Offset: 0x000C35F0
		public override void Initialize()
		{
			base.Initialize();
			this._stat = this._statPerStack.Clone();
			for (int i = 0; i < this.operationsOnStacked.Length; i++)
			{
				this.operationsOnStacked[i].Initialize();
			}
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x000C5434 File Offset: 0x000C3634
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x000C5440 File Offset: 0x000C3640
		public void Attach()
		{
			Character owner = this.owner;
			owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.OnKilled));
			this.owner.health.onTookDamage += new TookDamageDelegate(this.OnTookDamage);
			this.UpdateStack();
			this.owner.stat.AttachValues(this._stat);
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x000C54AC File Offset: 0x000C36AC
		public void Detach()
		{
			Character owner = this.owner;
			owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.OnKilled));
			this.owner.health.onTookDamage -= new TookDamageDelegate(this.OnTookDamage);
			this.owner.stat.DetachValues(this._stat);
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x000C5514 File Offset: 0x000C3714
		private void OnKilled(ITarget target, ref Damage damage)
		{
			if (target.character == null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
			{
				return;
			}
			if (!damage.key.Equals(this._skillKey, StringComparison.CurrentCultureIgnoreCase))
			{
				return;
			}
			if (this._stacks == this._maxStack)
			{
				return;
			}
			this._stacks++;
			this.UpdateStack();
			for (int i = 0; i < this.operationsOnStacked.Length; i++)
			{
				this.operationsOnStacked[i].Run(this.owner);
			}
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x000C55AC File Offset: 0x000C37AC
		private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			if (tookDamage.attackType != Damage.AttackType.None)
			{
				Damage damage = tookDamage;
				if (damage.amount != 0.0)
				{
					this._stacks /= 2;
					this.UpdateStack();
					return;
				}
			}
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x000C55F0 File Offset: 0x000C37F0
		private void UpdateStack()
		{
			for (int i = 0; i < this._stat.values.Length; i++)
			{
				this._stat.values[i].value = this._statPerStack.values[i].GetStackedValue((double)this._stacks);
			}
			this.owner.stat.SetNeedUpdate();
		}

		// Token: 0x040033C1 RID: 13249
		[NonSerialized]
		public CharacterOperation[] operationsOnStacked;

		// Token: 0x040033C2 RID: 13250
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x040033C3 RID: 13251
		[SerializeField]
		private int _maxStack;

		// Token: 0x040033C4 RID: 13252
		[SerializeField]
		private string _skillKey;

		// Token: 0x040033C5 RID: 13253
		private Stat.Values _stat;

		// Token: 0x040033C6 RID: 13254
		private int _stacks;
	}
}
