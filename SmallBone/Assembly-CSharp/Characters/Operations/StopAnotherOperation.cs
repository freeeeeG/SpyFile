using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E3F RID: 3647
	public class StopAnotherOperation : Operation
	{
		// Token: 0x0600489E RID: 18590 RVA: 0x000D37CC File Offset: 0x000D19CC
		public override void Run()
		{
			for (int i = 0; i < this._operationsToStop.values.Length; i++)
			{
				this._operationsToStop.values[i].Stop();
			}
		}

		// Token: 0x040037B6 RID: 14262
		[SerializeField]
		private StopAnotherOperation.OperationsToStop _operationsToStop;

		// Token: 0x02000E40 RID: 3648
		[Serializable]
		private class OperationsToStop : ReorderableArray<CharacterOperation>
		{
		}
	}
}
