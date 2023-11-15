using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Upgrades
{
	// Token: 0x0200084E RID: 2126
	public sealed class RunOperations : UpgradeAbility
	{
		// Token: 0x06002C3F RID: 11327 RVA: 0x00087822 File Offset: 0x00085A22
		public override void Attach(Character target)
		{
			if (target == null)
			{
				Debug.LogError("Player is null");
				return;
			}
			base.StartCoroutine(this._operations.CRun(target));
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x00002191 File Offset: 0x00000391
		public override void Detach()
		{
		}

		// Token: 0x04002567 RID: 9575
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;
	}
}
