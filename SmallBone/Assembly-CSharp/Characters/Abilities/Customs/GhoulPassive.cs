using System;
using Level;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D51 RID: 3409
	[Serializable]
	public class GhoulPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x060044BF RID: 17599 RVA: 0x000C7A21 File Offset: 0x000C5C21
		// (set) Token: 0x060044C0 RID: 17600 RVA: 0x000C7A29 File Offset: 0x000C5C29
		public Character owner { get; set; }

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x060044C1 RID: 17601 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x060044C2 RID: 17602 RVA: 0x000C7A32 File Offset: 0x000C5C32
		// (set) Token: 0x060044C3 RID: 17603 RVA: 0x000C7A3A File Offset: 0x000C5C3A
		public float remainTime { get; set; }

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x060044C4 RID: 17604 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x060044C5 RID: 17605 RVA: 0x000C7A43 File Offset: 0x000C5C43
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

		// Token: 0x17000E4E RID: 3662
		// (get) Token: 0x060044C6 RID: 17606 RVA: 0x000C7A56 File Offset: 0x000C5C56
		public float iconFillAmount
		{
			get
			{
				return 1f - this.remainTime / base.duration;
			}
		}

		// Token: 0x17000E4F RID: 3663
		// (get) Token: 0x060044C7 RID: 17607 RVA: 0x000C7A6B File Offset: 0x000C5C6B
		public int iconStacks
		{
			get
			{
				return (int)((float)this._stacks * this._iconStacksPerStack);
			}
		}

		// Token: 0x17000E50 RID: 3664
		// (get) Token: 0x060044C8 RID: 17608 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x000C7A7C File Offset: 0x000C5C7C
		public override IAbilityInstance CreateInstance(Character owner)
		{
			this.owner = owner;
			Vector3 size = this._fleshPrefab.GetComponent<Collider2D>().bounds.size;
			this._spawnOffset.y = size.y * 0.6f;
			return this;
		}

		// Token: 0x060044CA RID: 17610 RVA: 0x000C7AC1 File Offset: 0x000C5CC1
		public void UpdateTime(float deltaTime)
		{
			if (this._stacks == 0)
			{
				return;
			}
			this.remainTime -= deltaTime;
			if (this.remainTime > 0f)
			{
				return;
			}
			this._stacks = 0;
			this.UpdateStack();
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x000C7AF8 File Offset: 0x000C5CF8
		private void OnOwnerKilled(ITarget target, ref Damage damage)
		{
			if (target.character == null || target.character.type == Character.Type.Dummy || target.character.type == Character.Type.Trap)
			{
				return;
			}
			if (damage.key.Equals(this._consumeKey, StringComparison.OrdinalIgnoreCase))
			{
				this.AddStack();
				if (this._healByConsumeKill > 0f)
				{
					this.owner.health.Heal((double)this._healByConsumeKill, true);
				}
				return;
			}
			if (!MMMaths.PercentChance(this._possibility))
			{
				return;
			}
			this._fleshPrefab.Spawn(damage.hitPoint + this._spawnOffset, this);
		}

		// Token: 0x060044CC RID: 17612 RVA: 0x000C7BA4 File Offset: 0x000C5DA4
		public void Attach()
		{
			this.remainTime = base.duration;
			Character owner = this.owner;
			owner.onKilled = (Character.OnKilledDelegate)Delegate.Combine(owner.onKilled, new Character.OnKilledDelegate(this.OnOwnerKilled));
			this._stat = this._statPerStack.Clone();
			this._stacks = 0;
			this.owner.stat.AttachValues(this._stat);
			this.UpdateStack();
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x000C7C18 File Offset: 0x000C5E18
		public void Detach()
		{
			Character owner = this.owner;
			owner.onKilled = (Character.OnKilledDelegate)Delegate.Remove(owner.onKilled, new Character.OnKilledDelegate(this.OnOwnerKilled));
			this.owner.stat.DetachValues(this._stat);
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x000C7C57 File Offset: 0x000C5E57
		public void Refresh()
		{
			this.AddStack();
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x000C7C5F File Offset: 0x000C5E5F
		public void AddStack()
		{
			if (this._refreshRemainTime)
			{
				this.remainTime = base.duration;
			}
			if (this._stacks < this._maxStack)
			{
				this._stacks++;
			}
			this.UpdateStack();
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x000C7C98 File Offset: 0x000C5E98
		private void UpdateStack()
		{
			for (int i = 0; i < this._stat.values.Length; i++)
			{
				this._stat.values[i].value = this._statPerStack.values[i].GetStackedValue((double)this._stacks);
			}
			this.owner.stat.SetNeedUpdate();
		}

		// Token: 0x04003465 RID: 13413
		[SerializeField]
		[Space]
		[Range(1f, 100f)]
		private int _possibility;

		// Token: 0x04003466 RID: 13414
		[SerializeField]
		private DroppedGhoulFlesh _fleshPrefab;

		// Token: 0x04003467 RID: 13415
		[Header("Consume")]
		[SerializeField]
		private string _consumeKey = "consume";

		// Token: 0x04003468 RID: 13416
		[SerializeField]
		[Tooltip("교대기로 킬할 경우 스택이 쌓이면서 힐할 양")]
		private float _healByConsumeKill;

		// Token: 0x04003469 RID: 13417
		[SerializeField]
		[Header("Stat")]
		private int _maxStack;

		// Token: 0x0400346A RID: 13418
		[Tooltip("스택이 쌓일 때마다 남은 시간을 초기화할지")]
		[SerializeField]
		private bool _refreshRemainTime = true;

		// Token: 0x0400346B RID: 13419
		[SerializeField]
		[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
		private float _iconStacksPerStack = 1f;

		// Token: 0x0400346C RID: 13420
		[SerializeField]
		private Stat.Values _statPerStack;

		// Token: 0x0400346D RID: 13421
		private int _stacks;

		// Token: 0x0400346E RID: 13422
		private Stat.Values _stat;

		// Token: 0x0400346F RID: 13423
		private Vector2 _spawnOffset;
	}
}
