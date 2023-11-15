using System;
using Characters.Actions;
using UnityEngine;

namespace Runnables.Customs
{
	// Token: 0x0200037B RID: 891
	public class ActionEventAttacher : MonoBehaviour
	{
		// Token: 0x06001054 RID: 4180 RVA: 0x000305A0 File Offset: 0x0002E7A0
		private void OnEnable()
		{
			ActionEventAttacher.Type type = this._type;
			if (type == ActionEventAttacher.Type.OnStart)
			{
				this._action.onStart += this.Run;
				return;
			}
			if (type != ActionEventAttacher.Type.OnEnd)
			{
				return;
			}
			this._action.onEnd += this.Run;
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x000305EC File Offset: 0x0002E7EC
		private void OnDisable()
		{
			if (this._action == null)
			{
				return;
			}
			ActionEventAttacher.Type type = this._type;
			if (type == ActionEventAttacher.Type.OnStart)
			{
				this._action.onStart -= this.Run;
				return;
			}
			if (type != ActionEventAttacher.Type.OnEnd)
			{
				return;
			}
			this._action.onEnd -= this.Run;
		}

		// Token: 0x06001056 RID: 4182 RVA: 0x00030646 File Offset: 0x0002E846
		private void Run()
		{
			if (this._once && this._executed)
			{
				return;
			}
			this._execute.Run();
			this._executed = true;
		}

		// Token: 0x04000D57 RID: 3415
		[SerializeField]
		private Characters.Actions.Action _action;

		// Token: 0x04000D58 RID: 3416
		[SerializeField]
		private ActionEventAttacher.Type _type;

		// Token: 0x04000D59 RID: 3417
		[SerializeField]
		private Runnable _execute;

		// Token: 0x04000D5A RID: 3418
		[SerializeField]
		private bool _once;

		// Token: 0x04000D5B RID: 3419
		private bool _executed;

		// Token: 0x0200037C RID: 892
		private enum Type
		{
			// Token: 0x04000D5D RID: 3421
			OnStart,
			// Token: 0x04000D5E RID: 3422
			OnEnd
		}
	}
}
