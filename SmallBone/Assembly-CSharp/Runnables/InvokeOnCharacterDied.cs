using System;
using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C0 RID: 704
	public class InvokeOnCharacterDied : MonoBehaviour
	{
		// Token: 0x06000E45 RID: 3653 RVA: 0x0002CF61 File Offset: 0x0002B161
		private void Awake()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.health.onDied += this.OnDied;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0002CF90 File Offset: 0x0002B190
		private void OnDied()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.health.onDied -= this.OnDied;
			if (this._trigger.IsSatisfied())
			{
				this._execute.Run();
			}
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x0002CFE0 File Offset: 0x0002B1E0
		private void OnDestroy()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.health.onDied -= this.OnDied;
		}

		// Token: 0x04000BDF RID: 3039
		[SerializeField]
		[Trigger.SubcomponentAttribute]
		private Trigger _trigger;

		// Token: 0x04000BE0 RID: 3040
		[SerializeField]
		private Runnable _execute;

		// Token: 0x04000BE1 RID: 3041
		[SerializeField]
		private Character _character;
	}
}
