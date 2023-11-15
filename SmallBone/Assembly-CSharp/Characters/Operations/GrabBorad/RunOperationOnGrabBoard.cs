using System;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Operations.GrabBorad
{
	// Token: 0x02000E92 RID: 3730
	public class RunOperationOnGrabBoard : CharacterOperation
	{
		// Token: 0x060049C4 RID: 18884 RVA: 0x000D77E4 File Offset: 0x000D59E4
		public override void Run(Character owner)
		{
			this._operations.Initialize();
			foreach (Target target in this._grabBoard.targets)
			{
				base.StartCoroutine(this._operations.CRun(target.character));
			}
		}

		// Token: 0x04003902 RID: 14594
		[SerializeField]
		private GrabBoard _grabBoard;

		// Token: 0x04003903 RID: 14595
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;
	}
}
