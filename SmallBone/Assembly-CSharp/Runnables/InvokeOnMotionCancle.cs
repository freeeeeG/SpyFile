using System;
using Characters.Actions;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C5 RID: 709
	public class InvokeOnMotionCancle : MonoBehaviour
	{
		// Token: 0x06000E5B RID: 3675 RVA: 0x0002D313 File Offset: 0x0002B513
		private void Awake()
		{
			if (this._motion == null)
			{
				return;
			}
			this._motion.onCancel += this.ExecuteRunnable;
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0002D33B File Offset: 0x0002B53B
		private void OnDestroy()
		{
			if (this._motion == null)
			{
				return;
			}
			this._motion.onCancel -= this.ExecuteRunnable;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0002D363 File Offset: 0x0002B563
		private void ExecuteRunnable()
		{
			if (this._motion == null)
			{
				return;
			}
			if (this._trigger.IsSatisfied())
			{
				this._execute.Run();
			}
		}

		// Token: 0x04000BEE RID: 3054
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BEF RID: 3055
		[SerializeField]
		private Characters.Actions.Motion _motion;

		// Token: 0x04000BF0 RID: 3056
		[SerializeField]
		private Runnable _execute;
	}
}
