using System;
using System.Collections;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002CD RID: 717
	[Obsolete("삭제될 예정입니다. UpdateInvoker를 사용해주세요.")]
	public class OnceInvoke : MonoBehaviour
	{
		// Token: 0x06000E7A RID: 3706 RVA: 0x0002D59D File Offset: 0x0002B79D
		private void Start()
		{
			base.StartCoroutine(this.CRun());
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x0002D5AC File Offset: 0x0002B7AC
		public IEnumerator CRun()
		{
			while (!this._trigger.IsSatisfied())
			{
				yield return null;
			}
			this._execute.Run();
			yield break;
		}

		// Token: 0x04000C03 RID: 3075
		[Trigger.SubcomponentAttribute]
		[SerializeField]
		private Trigger _trigger;

		// Token: 0x04000C04 RID: 3076
		[SerializeField]
		private Runnable _execute;
	}
}
