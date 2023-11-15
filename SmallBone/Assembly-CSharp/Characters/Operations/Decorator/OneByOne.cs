using System;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EC0 RID: 3776
	public class OneByOne : CharacterOperation
	{
		// Token: 0x06004A30 RID: 18992 RVA: 0x000D8AC4 File Offset: 0x000D6CC4
		public override void Initialize()
		{
			this._operations.Initialize();
			if (this._randomizeEntry)
			{
				this._index = this._operations.components.RandomIndex<CharacterOperation>();
			}
		}

		// Token: 0x06004A31 RID: 18993 RVA: 0x000D8AF0 File Offset: 0x000D6CF0
		public override void Run(Character owner)
		{
			CharacterOperation characterOperation = this._operations.components[this._index];
			this._index = (this._index + 1) % this._operations.components.Length;
			if (characterOperation == null)
			{
				return;
			}
			characterOperation.Run(owner);
		}

		// Token: 0x06004A32 RID: 18994 RVA: 0x000D8B3D File Offset: 0x000D6D3D
		public override void Stop()
		{
			this._operations.Stop();
		}

		// Token: 0x04003968 RID: 14696
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operations;

		// Token: 0x04003969 RID: 14697
		[Tooltip("제일 먼저 실행될 위치를 임의로 지정")]
		[SerializeField]
		private bool _randomizeEntry = true;

		// Token: 0x0400396A RID: 14698
		private int _index;
	}
}
