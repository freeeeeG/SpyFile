using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E2E RID: 3630
	public class PrintDebugLog : Operation
	{
		// Token: 0x0600486D RID: 18541 RVA: 0x000D289A File Offset: 0x000D0A9A
		public override void Run()
		{
			Debug.Log(this._message);
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x000D28A7 File Offset: 0x000D0AA7
		public override string ToString()
		{
			return "PrintDebugLog (" + this._message + ")";
		}

		// Token: 0x04003775 RID: 14197
		[SerializeField]
		private string _message = "Run";
	}
}
