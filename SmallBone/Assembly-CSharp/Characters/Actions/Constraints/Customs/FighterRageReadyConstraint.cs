using System;
using Characters.Abilities;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs
{
	// Token: 0x0200098C RID: 2444
	public class FighterRageReadyConstraint : Constraint
	{
		// Token: 0x06003457 RID: 13399 RVA: 0x0009AD76 File Offset: 0x00098F76
		public override bool Pass()
		{
			return this._fighterPassiveAttacher.rageReady;
		}

		// Token: 0x04002A57 RID: 10839
		[SerializeField]
		private FighterPassiveAttacher _fighterPassiveAttacher;
	}
}
