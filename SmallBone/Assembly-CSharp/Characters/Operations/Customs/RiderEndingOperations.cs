using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Customs
{
	// Token: 0x02000FE5 RID: 4069
	public class RiderEndingOperations : MonoBehaviour
	{
		// Token: 0x06004EA0 RID: 20128 RVA: 0x000EBB8B File Offset: 0x000E9D8B
		public void Initialize()
		{
			this._operations.Initialize();
		}

		// Token: 0x06004EA1 RID: 20129 RVA: 0x000EBB98 File Offset: 0x000E9D98
		public void Run(Character owner)
		{
			this._operations.Run(owner);
		}

		// Token: 0x04003EB4 RID: 16052
		[SerializeField]
		[Subcomponent(typeof(Operation))]
		private CharacterOperation.Subcomponents _operations;
	}
}
