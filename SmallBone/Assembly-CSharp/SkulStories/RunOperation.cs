using System;
using System.Collections;
using Characters.Operations;
using Runnables;
using UnityEditor;
using UnityEngine;

namespace SkulStories
{
	// Token: 0x02000116 RID: 278
	public class RunOperation : Sequence
	{
		// Token: 0x0600057E RID: 1406 RVA: 0x00010A29 File Offset: 0x0000EC29
		private void Awake()
		{
			this._operations.Initialize();
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x00010A36 File Offset: 0x0000EC36
		public override IEnumerator CRun()
		{
			yield return this._operations.CRun(this._owner.character);
			yield break;
		}

		// Token: 0x0400042C RID: 1068
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x0400042D RID: 1069
		[SerializeField]
		private Target _owner;
	}
}
