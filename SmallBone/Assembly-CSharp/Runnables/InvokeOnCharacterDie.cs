using System;
using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002BF RID: 703
	public class InvokeOnCharacterDie : MonoBehaviour
	{
		// Token: 0x06000E41 RID: 3649 RVA: 0x0002CEDE File Offset: 0x0002B0DE
		private void Awake()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.health.onDie += this.ExecuteRunnable;
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0002CF0B File Offset: 0x0002B10B
		private void OnDestroy()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.health.onDie -= this.ExecuteRunnable;
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0002CF38 File Offset: 0x0002B138
		private void ExecuteRunnable()
		{
			if (this._character == null)
			{
				return;
			}
			if (this._trigger.IsSatisfied())
			{
				this._execute.Run();
			}
		}

		// Token: 0x04000BDC RID: 3036
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BDD RID: 3037
		[SerializeField]
		private Character _character;

		// Token: 0x04000BDE RID: 3038
		[SerializeField]
		private Runnable _execute;
	}
}
