using System;
using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002C1 RID: 705
	public class InvokeOnCharacterDiedTryCatch : MonoBehaviour
	{
		// Token: 0x06000E49 RID: 3657 RVA: 0x0002D00D File Offset: 0x0002B20D
		private void Awake()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.health.onDiedTryCatch += this.OnDiedTryCatch;
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x0002D03C File Offset: 0x0002B23C
		private void OnDiedTryCatch()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.health.onDiedTryCatch -= this.OnDiedTryCatch;
			if (this._trigger.IsSatisfied())
			{
				this._execute.Run();
			}
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x0002D08C File Offset: 0x0002B28C
		private void OnDestroy()
		{
			if (this._character == null)
			{
				return;
			}
			this._character.health.onDiedTryCatch -= this.OnDiedTryCatch;
		}

		// Token: 0x04000BE2 RID: 3042
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _trigger;

		// Token: 0x04000BE3 RID: 3043
		[SerializeField]
		private Runnable _execute;

		// Token: 0x04000BE4 RID: 3044
		[SerializeField]
		private Character _character;
	}
}
