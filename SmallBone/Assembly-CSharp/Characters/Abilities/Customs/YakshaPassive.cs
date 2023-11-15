using System;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D9F RID: 3487
	[Serializable]
	public class YakshaPassive : Ability, IAbilityInstance
	{
		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06004629 RID: 17961 RVA: 0x000CB0DF File Offset: 0x000C92DF
		// (set) Token: 0x0600462A RID: 17962 RVA: 0x000CB0E7 File Offset: 0x000C92E7
		public Character owner { get; set; }

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x0600462B RID: 17963 RVA: 0x000716FD File Offset: 0x0006F8FD
		public IAbility ability
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x0600462C RID: 17964 RVA: 0x00071719 File Offset: 0x0006F919
		// (set) Token: 0x0600462D RID: 17965 RVA: 0x00002191 File Offset: 0x00000391
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

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x0600462E RID: 17966 RVA: 0x000076D4 File Offset: 0x000058D4
		public bool attached
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x0600462F RID: 17967 RVA: 0x0009ADBE File Offset: 0x00098FBE
		public Sprite icon
		{
			get
			{
				return this._defaultIcon;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06004630 RID: 17968 RVA: 0x00071719 File Offset: 0x0006F919
		public float iconFillAmount
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06004631 RID: 17969 RVA: 0x000CB0F0 File Offset: 0x000C92F0
		// (set) Token: 0x06004632 RID: 17970 RVA: 0x000CB0F8 File Offset: 0x000C92F8
		public int iconStacks { get; protected set; }

		// Token: 0x17000E9F RID: 3743
		// (get) Token: 0x06004633 RID: 17971 RVA: 0x00018EC5 File Offset: 0x000170C5
		public bool expired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x000CB101 File Offset: 0x000C9301
		public override void Initialize()
		{
			base.Initialize();
			this._operations.Initialize();
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x000716FD File Offset: 0x0006F8FD
		public override IAbilityInstance CreateInstance(Character owner)
		{
			return this;
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x00002191 File Offset: 0x00000391
		public void UpdateTime(float deltaTime)
		{
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x00002191 File Offset: 0x00000391
		public void Refresh()
		{
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x00002191 File Offset: 0x00000391
		public void Attach()
		{
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x000CB114 File Offset: 0x000C9314
		public void Detach()
		{
			this._operationRunner.Stop();
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x000CB124 File Offset: 0x000C9324
		public void AddStack()
		{
			this._gauge.Add(1f);
			int iconStacks = this.iconStacks;
			this.iconStacks = iconStacks + 1;
			if (this.iconStacks < this._stacksToAttack)
			{
				return;
			}
			this._gauge.Clear();
			this.iconStacks = 0;
			this._operationRunner.Stop();
			this._operationRunner = this.owner.StartCoroutineWithReference(this._operations.CRun(this.owner));
		}

		// Token: 0x04003533 RID: 13619
		[SerializeField]
		private ValueGauge _gauge;

		// Token: 0x04003534 RID: 13620
		[SerializeField]
		private int _stacksToAttack;

		// Token: 0x04003535 RID: 13621
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04003536 RID: 13622
		private CoroutineReference _operationRunner;
	}
}
