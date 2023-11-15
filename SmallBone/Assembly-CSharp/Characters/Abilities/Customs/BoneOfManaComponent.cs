using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D34 RID: 3380
	public class BoneOfManaComponent : AbilityComponent<BoneOfMana>
	{
		// Token: 0x06004434 RID: 17460 RVA: 0x000C6250 File Offset: 0x000C4450
		public override void Initialize()
		{
			this._ability.operationsByCount = new CharacterOperation[][]
			{
				this._operation0.components,
				this._operation1.components,
				this._operation2.components
			};
			base.Initialize();
		}

		// Token: 0x040033FB RID: 13307
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operation0;

		// Token: 0x040033FC RID: 13308
		[CharacterOperation.SubcomponentAttribute]
		[SerializeField]
		private CharacterOperation.Subcomponents _operation1;

		// Token: 0x040033FD RID: 13309
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operation2;
	}
}
