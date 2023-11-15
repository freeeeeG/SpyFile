using System;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace BT
{
	// Token: 0x0200141D RID: 5149
	public class RunOperations : Node
	{
		// Token: 0x06006538 RID: 25912 RVA: 0x001252D9 File Offset: 0x001234D9
		private void Awake()
		{
			this._operations.Initialize();
		}

		// Token: 0x06006539 RID: 25913 RVA: 0x001252E8 File Offset: 0x001234E8
		protected override NodeState UpdateDeltatime(Context context)
		{
			Character target = context.Get<Character>(Key.OwnerCharacter);
			this._operations.StopAll();
			base.StartCoroutine(this._operations.CRun(target));
			return NodeState.Success;
		}

		// Token: 0x0400518A RID: 20874
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;
	}
}
