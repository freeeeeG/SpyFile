using System;
using Characters.Abilities.Constraints;
using UnityEngine;

namespace Characters.Abilities.Triggers
{
	// Token: 0x02000B69 RID: 2921
	public abstract class TriggerComponent<T> : TriggerComponent where T : Trigger
	{
		// Token: 0x14000099 RID: 153
		// (add) Token: 0x06003A7B RID: 14971 RVA: 0x000ACC96 File Offset: 0x000AAE96
		// (remove) Token: 0x06003A7C RID: 14972 RVA: 0x000ACCA9 File Offset: 0x000AAEA9
		public override event Action onTriggered
		{
			add
			{
				this._trigger.onTriggered += value;
			}
			remove
			{
				this._trigger.onTriggered -= value;
			}
		}

		// Token: 0x17000BE7 RID: 3047
		// (get) Token: 0x06003A7D RID: 14973 RVA: 0x000ACCBC File Offset: 0x000AAEBC
		public override float cooldownTime
		{
			get
			{
				return this._trigger.cooldownTime;
			}
		}

		// Token: 0x17000BE8 RID: 3048
		// (get) Token: 0x06003A7E RID: 14974 RVA: 0x000ACCCE File Offset: 0x000AAECE
		public override float remainCooldownTime
		{
			get
			{
				return this._trigger.remainCooldownTime;
			}
		}

		// Token: 0x06003A7F RID: 14975 RVA: 0x000ACCE0 File Offset: 0x000AAEE0
		public override void Attach(Character character)
		{
			this._trigger.Attach(character);
		}

		// Token: 0x06003A80 RID: 14976 RVA: 0x000ACCF3 File Offset: 0x000AAEF3
		public override void Detach()
		{
			this._trigger.Detach();
		}

		// Token: 0x06003A81 RID: 14977 RVA: 0x000ACD05 File Offset: 0x000AAF05
		public override void UpdateTime(float deltaTime)
		{
			if (!this._constraints.Pass())
			{
				return;
			}
			this._trigger.UpdateTime(deltaTime);
		}

		// Token: 0x06003A82 RID: 14978 RVA: 0x000ACD26 File Offset: 0x000AAF26
		public override void Refresh()
		{
			this._trigger.Refresh();
		}

		// Token: 0x04002E74 RID: 11892
		[SerializeField]
		private T _trigger;

		// Token: 0x04002E75 RID: 11893
		private Constraint[] _constraints = new Constraint[]
		{
			new LetterBox(),
			new Dialogue(),
			new Story(),
			new EndingCredit()
		};
	}
}
