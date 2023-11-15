using System;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs
{
	// Token: 0x02000D26 RID: 3366
	public class ArchlichSoulLootingPassiveComponent : AbilityComponent<ArchlichSoulLootingPassive>
	{
		// Token: 0x060043DC RID: 17372 RVA: 0x000C5650 File Offset: 0x000C3850
		public override void Initialize()
		{
			this._ability.operationsOnStacked = this._operations.components;
			base.Initialize();
		}

		// Token: 0x040033C8 RID: 13256
		[SerializeField]
		[CharacterOperation.SubcomponentAttribute]
		private CharacterOperation.Subcomponents _operations;
	}
}
