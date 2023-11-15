using System;
using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C3 RID: 707
	public class InvokeOnCharacterMovementOnGrounded : MonoBehaviour
	{
		// Token: 0x06000E51 RID: 3665 RVA: 0x0002D196 File Offset: 0x0002B396
		private void Awake()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.movement.onGrounded += this.ExecuteRunnable;
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0002D1C3 File Offset: 0x0002B3C3
		private void OnDestroy()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.movement.onGrounded -= this.ExecuteRunnable;
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0002D1F0 File Offset: 0x0002B3F0
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

		// Token: 0x04000BE8 RID: 3048
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BE9 RID: 3049
		[SerializeField]
		private Character _character;

		// Token: 0x04000BEA RID: 3050
		[SerializeField]
		private Runnable _execute;
	}
}
