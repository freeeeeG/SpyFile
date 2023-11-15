using System;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002D5 RID: 725
	public sealed class DestroyObject : Runnable
	{
		// Token: 0x06000E8D RID: 3725 RVA: 0x0002D6AE File Offset: 0x0002B8AE
		public override void Run()
		{
			UnityEngine.Object.Destroy(this._object);
		}

		// Token: 0x04000C11 RID: 3089
		[SerializeField]
		private GameObject _object;
	}
}
