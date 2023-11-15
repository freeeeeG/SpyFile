using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002DA RID: 730
	public sealed class RunOperations : Runnable
	{
		// Token: 0x06000E98 RID: 3736 RVA: 0x0002D77B File Offset: 0x0002B97B
		private void Awake()
		{
			this._operations.Initialize();
		}

		// Token: 0x06000E99 RID: 3737 RVA: 0x0002D788 File Offset: 0x0002B988
		public override void Run()
		{
			if (!this._owner.character.gameObject.activeSelf)
			{
				return;
			}
			base.StartCoroutine(this._operations.CRun(this._owner.character));
		}

		// Token: 0x04000C17 RID: 3095
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04000C18 RID: 3096
		[SerializeField]
		private Target _owner;
	}
}
