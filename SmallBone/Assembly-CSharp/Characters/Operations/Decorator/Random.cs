using System;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EC4 RID: 3780
	public class Random : CharacterOperation
	{
		// Token: 0x06004A3D RID: 19005 RVA: 0x000D8CEA File Offset: 0x000D6EEA
		public override void Initialize()
		{
			this._toRandom.Initialize();
		}

		// Token: 0x06004A3E RID: 19006 RVA: 0x000D8CF8 File Offset: 0x000D6EF8
		public override void Run(Character owner)
		{
			CharacterOperation characterOperation = this._toRandom.components.Random<CharacterOperation>();
			if (characterOperation == null)
			{
				return;
			}
			characterOperation.Run(owner);
		}

		// Token: 0x06004A3F RID: 19007 RVA: 0x000D8D27 File Offset: 0x000D6F27
		public override void Stop()
		{
			this._toRandom.Stop();
		}

		// Token: 0x0400396F RID: 14703
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _toRandom;
	}
}
