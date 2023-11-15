using System;
using Level;
using UnityEngine;

namespace Runnables
{
	// Token: 0x0200032A RID: 810
	public class OpenGate : Runnable
	{
		// Token: 0x06000F89 RID: 3977 RVA: 0x0002F1F5 File Offset: 0x0002D3F5
		private void Start()
		{
			this._gate = this._map.GetComponentInChildren<Gate>();
		}

		// Token: 0x06000F8A RID: 3978 RVA: 0x0002F208 File Offset: 0x0002D408
		public override void Run()
		{
			this._gate.Activate();
		}

		// Token: 0x04000CC2 RID: 3266
		[SerializeField]
		[GetComponentInParent(false)]
		private Map _map;

		// Token: 0x04000CC3 RID: 3267
		private Gate _gate;
	}
}
