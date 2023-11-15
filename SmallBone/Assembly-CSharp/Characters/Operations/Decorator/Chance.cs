using System;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EBA RID: 3770
	public class Chance : CharacterOperation
	{
		// Token: 0x06004A1D RID: 18973 RVA: 0x000D8853 File Offset: 0x000D6A53
		public override void Initialize()
		{
			if (this._onSuccess != null)
			{
				this._onSuccess.Initialize();
			}
			if (this._onFail != null)
			{
				this._onFail.Initialize();
			}
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x000D8888 File Offset: 0x000D6A88
		public override void Run(Character owner)
		{
			if (MMMaths.Chance(this._successChance))
			{
				if (this._onSuccess == null)
				{
					return;
				}
				this._onSuccess.Stop();
				this._onSuccess.Run(owner);
				return;
			}
			else
			{
				if (this._onFail == null)
				{
					return;
				}
				this._onFail.Stop();
				this._onFail.Run(owner);
				return;
			}
		}

		// Token: 0x06004A1F RID: 18975 RVA: 0x000D88EF File Offset: 0x000D6AEF
		public override void Stop()
		{
			if (this._onSuccess != null)
			{
				this._onSuccess.Stop();
			}
			if (this._onFail != null)
			{
				this._onFail.Stop();
			}
		}

		// Token: 0x04003957 RID: 14679
		[SerializeField]
		[Range(0f, 1f)]
		private float _successChance = 0.5f;

		// Token: 0x04003958 RID: 14680
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation _onSuccess;

		// Token: 0x04003959 RID: 14681
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation _onFail;
	}
}
