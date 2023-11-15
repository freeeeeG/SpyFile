using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000ED0 RID: 3792
	public class WeightedRandom : CharacterOperation
	{
		// Token: 0x06004A7A RID: 19066 RVA: 0x000D94A9 File Offset: 0x000D76A9
		public override void Initialize()
		{
			this._toRandom.Initialize();
		}

		// Token: 0x06004A7B RID: 19067 RVA: 0x000D94B6 File Offset: 0x000D76B6
		public override void Run(Character owner)
		{
			this._toRandom.RunWeightedRandom(owner);
		}

		// Token: 0x06004A7C RID: 19068 RVA: 0x000D94C4 File Offset: 0x000D76C4
		public override void Stop()
		{
			this._toRandom.StopAll();
		}

		// Token: 0x0400399E RID: 14750
		[UnityEditor.Subcomponent(typeof(OperationWithWeight))]
		[SerializeField]
		private OperationWithWeight.Subcomponents _toRandom;
	}
}
