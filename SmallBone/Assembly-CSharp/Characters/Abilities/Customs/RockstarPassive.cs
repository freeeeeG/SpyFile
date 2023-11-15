using System;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations.Summon;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D89 RID: 3465
	[Serializable]
	public class RockstarPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E79 RID: 3705
		// (get) Token: 0x060045B5 RID: 17845 RVA: 0x000CA373 File Offset: 0x000C8573
		// (set) Token: 0x060045B6 RID: 17846 RVA: 0x000CA37B File Offset: 0x000C857B
		public Character owner { get; set; }

		// Token: 0x17000E7A RID: 3706
		// (get) Token: 0x060045B7 RID: 17847 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E7B RID: 3707
		// (get) Token: 0x060045B8 RID: 17848 RVA: 0x000CA384 File Offset: 0x000C8584
		// (set) Token: 0x060045B9 RID: 17849 RVA: 0x000CA38C File Offset: 0x000C858C
		public float remainTime { get; set; }

		// Token: 0x17000E7C RID: 3708
		// (get) Token: 0x060045BA RID: 17850 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E7D RID: 3709
		// (get) Token: 0x060045BB RID: 17851 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E7E RID: 3710
		// (get) Token: 0x060045BC RID: 17852 RVA: 0x000CA395 File Offset: 0x000C8595
		public float iconFillAmount
		{
			get
			{
				return this._summonRemainCooldown / this._summonCooldown;
			}
		}

		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x060045BD RID: 17853 RVA: 0x000CA3A4 File Offset: 0x000C85A4
		public int iconStacks
		{
			get
			{
				return (int)(this._gauge.currentValue * this._iconStacksPerStack);
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x060045BE RID: 17854 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x000CA3B9 File Offset: 0x000C85B9
		public override void Initialize()
		{
			base.Initialize();
			this._stat = this._statPerStack.Clone();
			this._summonOperationRunner.Initialize();
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x000CA3DD File Offset: 0x000C85DD
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			return this;
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x000CA3E8 File Offset: 0x000C85E8
		public void UpdateTime(float deltaTime)
		{
			this._buffRemaintime -= deltaTime;
			if (this._buffRemaintime < 0f)
			{
				this._buffRemaintime = this._buffDuration;
				this._gauge.Clear();
				this.UpdateStack();
			}
			this._summonRemainCooldown -= deltaTime;
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x000CA43B File Offset: 0x000C863B
		public void Attach()
		{
			this._buffRemaintime = this._buffDuration;
			this.owner.stat.AttachValues(this._stat);
			this.UpdateStack();
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x000CA465 File Offset: 0x000C8665
		public void Detach()
		{
			this.owner.stat.DetachValues(this._stat);
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x000CA480 File Offset: 0x000C8680
		public void AddStack(int amount)
		{
			this._buffRemaintime = this._buffDuration;
			this._gauge.Add((float)amount);
			this.UpdateStack();
			if (this._gauge.currentValue < this._gauge.maxValue || this._summonRemainCooldown > 0f)
			{
				return;
			}
			this._summonRemainCooldown = this._summonCooldown;
			this._summonOperationRunner.Run(this.owner);
			Action action = this.onSummon;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x000CA500 File Offset: 0x000C8700
		private void UpdateStack()
		{
			for (int i = 0; i < this._stat.values.Length; i++)
			{
				this._stat.values[i].value = this._statPerStack.values[i].GetStackedValue((double)((int)this._gauge.currentValue));
			}
			this.owner.stat.SetNeedUpdate();
		}

		// Token: 0x040034F7 RID: 13559
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x040034F8 RID: 13560
		[SerializeField]
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		private float _iconStacksPerStack = 1f;

		// Token: 0x040034F9 RID: 13561
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x040034FA RID: 13562
		private Stat.Values _stat;

		// Token: 0x040034FB RID: 13563
		[SerializeField]
		private float _buffDuration;

		// Token: 0x040034FC RID: 13564
		private float _buffRemaintime;

		// Token: 0x040034FD RID: 13565
		[SerializeField]
		private float _summonCooldown;

		// Token: 0x040034FE RID: 13566
		private float _summonRemainCooldown;

		// Token: 0x040034FF RID: 13567
		[Tooltip("게이지가 꽉찼을 때 실행할 SummonOperationRunner")]
		[SerializeField]
		private SummonOperationRunner _summonOperationRunner;

		// Token: 0x04003502 RID: 13570
		public Action onSummon;
	}
}
