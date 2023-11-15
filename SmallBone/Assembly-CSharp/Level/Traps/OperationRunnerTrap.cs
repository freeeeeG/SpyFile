using System;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps
{
	// Token: 0x0200066E RID: 1646
	public class OperationRunnerTrap : Trap
	{
		// Token: 0x060020F8 RID: 8440 RVA: 0x00063709 File Offset: 0x00061909
		private void Awake()
		{
			this._operations.Initialize();
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x00063716 File Offset: 0x00061916
		private void OnEnable()
		{
			base.StartCoroutine(this._operations.CRun(this._character));
		}

		// Token: 0x04001C10 RID: 7184
		[SerializeField]
		private Character _character;

		// Token: 0x04001C11 RID: 7185
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;
	}
}
