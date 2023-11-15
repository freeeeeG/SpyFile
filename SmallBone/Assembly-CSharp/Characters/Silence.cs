using System;
using Characters.Actions;

namespace Characters
{
	// Token: 0x0200069C RID: 1692
	public sealed class Silence
	{
		// Token: 0x060021C7 RID: 8647 RVA: 0x00065942 File Offset: 0x00063B42
		public Silence(Character owner)
		{
			this._owner = owner;
		}

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x060021C8 RID: 8648 RVA: 0x0006595D File Offset: 0x00063B5D
		public bool value
		{
			get
			{
				return this._trueOnlyLogicalSumList.value;
			}
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x0006596C File Offset: 0x00063B6C
		public void Attach(object key)
		{
			foreach (Characters.Actions.Action action in this._owner.actions)
			{
				if (action.type == Characters.Actions.Action.Type.Skill && action.running)
				{
					action.ForceEnd();
				}
			}
			this._trueOnlyLogicalSumList.Attach(key);
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x000659E0 File Offset: 0x00063BE0
		public bool Detach(object key)
		{
			return this._trueOnlyLogicalSumList.Detach(key);
		}

		// Token: 0x04001CCC RID: 7372
		private TrueOnlyLogicalSumList _trueOnlyLogicalSumList = new TrueOnlyLogicalSumList(false);

		// Token: 0x04001CCD RID: 7373
		private Character _owner;
	}
}
