using System;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200032B RID: 811
	public class PrintDebugLog : Runnable
	{
		// Token: 0x06000F8C RID: 3980 RVA: 0x0002F215 File Offset: 0x0002D415
		public override void Run()
		{
			Debug.Log(this._log);
		}

		// Token: 0x04000CC4 RID: 3268
		[SerializeField]
		private string _log;
	}
}
