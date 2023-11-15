using System;
using UnityEngine;

namespace Characters.Operations.Decorator.Deprecated
{
	// Token: 0x02000ED1 RID: 3793
	public class RandomlyRunningOperation : CharacterOperation
	{
		// Token: 0x06004A7E RID: 19070 RVA: 0x000D94D1 File Offset: 0x000D76D1
		public override void Initialize()
		{
			this._operation.Initialize();
		}

		// Token: 0x06004A7F RID: 19071 RVA: 0x000D94DE File Offset: 0x000D76DE
		public override void Run(Character owner)
		{
			if (this._actuationrate >= UnityEngine.Random.Range(1, 100))
			{
				this._operation.Run(owner);
			}
		}

		// Token: 0x06004A80 RID: 19072 RVA: 0x000D9501 File Offset: 0x000D7701
		public override void Stop()
		{
			this._operation.Stop();
		}

		// Token: 0x0400399F RID: 14751
		[SerializeField]
		[Range(1f, 100f)]
		private int _actuationrate;

		// Token: 0x040039A0 RID: 14752
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation _operation;
	}
}
