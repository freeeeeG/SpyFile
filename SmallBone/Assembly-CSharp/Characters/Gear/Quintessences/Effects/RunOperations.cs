using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects
{
	// Token: 0x020008EE RID: 2286
	public sealed class RunOperations : QuintessenceEffect
	{
		// Token: 0x060030E2 RID: 12514 RVA: 0x000923A0 File Offset: 0x000905A0
		private void Awake()
		{
			this._operations.Initialize();
		}

		// Token: 0x060030E3 RID: 12515 RVA: 0x000923B0 File Offset: 0x000905B0
		protected override void OnInvoke(Quintessence quintessence)
		{
			Character owner = quintessence.owner;
			if (owner == null)
			{
				return;
			}
			this._operations.StopAll();
			base.StartCoroutine(this._operations.CRun(owner));
		}

		// Token: 0x0400284A RID: 10314
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;
	}
}
