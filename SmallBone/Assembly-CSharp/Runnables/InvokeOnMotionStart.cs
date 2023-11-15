using System;
using Characters.Actions;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C6 RID: 710
	public class InvokeOnMotionStart : MonoBehaviour
	{
		// Token: 0x06000E5F RID: 3679 RVA: 0x0002D38C File Offset: 0x0002B58C
		private void Awake()
		{
			if (this._motion == null)
			{
				return;
			}
			this._motion.onStart += this.ExecuteRunnable;
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0002D3B4 File Offset: 0x0002B5B4
		private void OnDestroy()
		{
			if (this._motion == null)
			{
				return;
			}
			this._motion.onStart -= this.ExecuteRunnable;
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0002D3DC File Offset: 0x0002B5DC
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

		// Token: 0x04000BF1 RID: 3057
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BF2 RID: 3058
		[SerializeField]
		private Characters.Actions.Motion _motion;

		// Token: 0x04000BF3 RID: 3059
		[SerializeField]
		private Runnable _execute;
	}
}
