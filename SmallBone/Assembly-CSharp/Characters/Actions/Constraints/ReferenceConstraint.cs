using System;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x02000984 RID: 2436
	public class ReferenceConstraint : Constraint
	{
		// Token: 0x06003443 RID: 13379 RVA: 0x0009ABCF File Offset: 0x00098DCF
		public override bool Pass()
		{
			return this._constraint.Pass();
		}

		// Token: 0x06003444 RID: 13380 RVA: 0x0009ABDC File Offset: 0x00098DDC
		public override void Consume()
		{
			this._constraint.Consume();
		}

		// Token: 0x04002A4D RID: 10829
		[SerializeField]
		private Constraint _constraint;
	}
}
