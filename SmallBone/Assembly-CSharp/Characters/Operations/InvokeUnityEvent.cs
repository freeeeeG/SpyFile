using System;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Operations
{
	// Token: 0x02000E1F RID: 3615
	public sealed class InvokeUnityEvent : Operation
	{
		// Token: 0x06004825 RID: 18469 RVA: 0x000D1DEA File Offset: 0x000CFFEA
		public override void Run()
		{
			this._unityEvent.Invoke();
		}

		// Token: 0x04003739 RID: 14137
		[SerializeField]
		private UnityEvent _unityEvent;
	}
}
