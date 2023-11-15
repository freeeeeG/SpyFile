using System;
using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Actions.Constraints.Customs
{
	// Token: 0x0200098D RID: 2445
	public class GraveDiggerGraveConstraint : Constraint
	{
		// Token: 0x06003459 RID: 13401 RVA: 0x0009AD83 File Offset: 0x00098F83
		public override bool Pass()
		{
			return this._container.hasActivatedGrave;
		}

		// Token: 0x04002A58 RID: 10840
		[SerializeField]
		private GraveDiggerGraveContainer _container;
	}
}
