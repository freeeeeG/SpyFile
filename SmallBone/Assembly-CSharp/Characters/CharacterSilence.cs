using System;
using Characters.Actions;

namespace Characters
{
	// Token: 0x02000744 RID: 1860
	public sealed class CharacterSilence
	{
		// Token: 0x060025D9 RID: 9689 RVA: 0x000722D1 File Offset: 0x000704D1
		public CharacterSilence()
		{
			this._skill = new TrueOnlyLogicalSumList(false);
			this._dash = new TrueOnlyLogicalSumList(false);
			this._basicAttack = new TrueOnlyLogicalSumList(false);
			this._jump = new TrueOnlyLogicalSumList(false);
		}

		// Token: 0x060025DA RID: 9690 RVA: 0x0007230C File Offset: 0x0007050C
		public void Attach(Characters.Actions.Action.Type type, object key)
		{
			TrueOnlyLogicalSumList logicalSum = this.GetLogicalSum(type);
			if (logicalSum == null)
			{
				return;
			}
			logicalSum.Attach(key);
		}

		// Token: 0x060025DB RID: 9691 RVA: 0x0007232C File Offset: 0x0007052C
		public void Detach(Characters.Actions.Action.Type type, object key)
		{
			TrueOnlyLogicalSumList logicalSum = this.GetLogicalSum(type);
			if (logicalSum == null)
			{
				return;
			}
			logicalSum.Detach(key);
		}

		// Token: 0x060025DC RID: 9692 RVA: 0x00072350 File Offset: 0x00070550
		public bool CanUse(Characters.Actions.Action.Type type)
		{
			TrueOnlyLogicalSumList logicalSum = this.GetLogicalSum(type);
			if (logicalSum == null)
			{
				return true;
			}
			if (type == Characters.Actions.Action.Type.Dash)
			{
				this.Detach(Characters.Actions.Action.Type.Skill, this);
			}
			return !logicalSum.value;
		}

		// Token: 0x060025DD RID: 9693 RVA: 0x0007237E File Offset: 0x0007057E
		private TrueOnlyLogicalSumList GetLogicalSum(Characters.Actions.Action.Type type)
		{
			switch (type)
			{
			case Characters.Actions.Action.Type.Dash:
				return this._dash;
			case Characters.Actions.Action.Type.BasicAttack:
				return this._basicAttack;
			case Characters.Actions.Action.Type.Jump:
				return this._jump;
			case Characters.Actions.Action.Type.Skill:
				return this._skill;
			}
			return null;
		}

		// Token: 0x04002021 RID: 8225
		private TrueOnlyLogicalSumList _skill;

		// Token: 0x04002022 RID: 8226
		private TrueOnlyLogicalSumList _dash;

		// Token: 0x04002023 RID: 8227
		private TrueOnlyLogicalSumList _basicAttack;

		// Token: 0x04002024 RID: 8228
		private TrueOnlyLogicalSumList _jump;
	}
}
