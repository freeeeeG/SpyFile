using System;
using Hardmode;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Decorator
{
	// Token: 0x02000EBF RID: 3775
	public sealed class InHardmode : CharacterOperation
	{
		// Token: 0x06004A2B RID: 18987 RVA: 0x000D8A6D File Offset: 0x000D6C6D
		public override void Initialize()
		{
			this._inHardmode.Initialize();
			this._inNormal.Initialize();
		}

		// Token: 0x06004A2C RID: 18988 RVA: 0x000D8A85 File Offset: 0x000D6C85
		public override void Run(Character owner)
		{
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._inHardmode.Run(owner);
				return;
			}
			this._inNormal.Run(owner);
		}

		// Token: 0x06004A2D RID: 18989 RVA: 0x000D8A85 File Offset: 0x000D6C85
		public override void Run(Character owner, Character target)
		{
			if (Singleton<HardmodeManager>.Instance.hardmode)
			{
				this._inHardmode.Run(owner);
				return;
			}
			this._inNormal.Run(owner);
		}

		// Token: 0x06004A2E RID: 18990 RVA: 0x000D8AAC File Offset: 0x000D6CAC
		public override void Stop()
		{
			this._inHardmode.Stop();
			this._inNormal.Stop();
		}

		// Token: 0x04003966 RID: 14694
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _inHardmode;

		// Token: 0x04003967 RID: 14695
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _inNormal;
	}
}
