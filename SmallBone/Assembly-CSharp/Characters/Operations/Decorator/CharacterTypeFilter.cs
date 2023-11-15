using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EBB RID: 3771
	public sealed class CharacterTypeFilter : CharacterOperation
	{
		// Token: 0x06004A21 RID: 18977 RVA: 0x000D8936 File Offset: 0x000D6B36
		public override void Initialize()
		{
			this._operations.Initialize();
		}

		// Token: 0x06004A22 RID: 18978 RVA: 0x000D8943 File Offset: 0x000D6B43
		public override void Run(Character owner)
		{
			if (!this._characterType[owner.type])
			{
				return;
			}
			this.Run(owner, owner);
		}

		// Token: 0x06004A23 RID: 18979 RVA: 0x000D8961 File Offset: 0x000D6B61
		public override void Run(Character owner, Character target)
		{
			if (!this._characterType[target.type])
			{
				return;
			}
			base.StartCoroutine(this._operations.CRun(owner, target));
		}

		// Token: 0x06004A24 RID: 18980 RVA: 0x00048973 File Offset: 0x00046B73
		public override void Stop()
		{
			base.StopAllCoroutines();
		}

		// Token: 0x0400395A RID: 14682
		[SerializeField]
		private CharacterTypeBoolArray _characterType;

		// Token: 0x0400395B RID: 14683
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _operations;
	}
}
