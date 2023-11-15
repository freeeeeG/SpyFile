using System;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E52 RID: 3666
	public class DualKnockback : TargetedCharacterOperation
	{
		// Token: 0x060048DC RID: 18652 RVA: 0x00002191 File Offset: 0x00000391
		public override void Run(Character owner, Character target)
		{
		}

		// Token: 0x040037E4 RID: 14308
		[Information("Obsolete", InformationAttribute.InformationType.Warning, true)]
		[SerializeField]
		private PushForce _pushForce1;

		// Token: 0x040037E5 RID: 14309
		[SerializeField]
		private Curve _curve1;

		// Token: 0x040037E6 RID: 14310
		[SerializeField]
		private PushForce _pushForce2;

		// Token: 0x040037E7 RID: 14311
		[SerializeField]
		private Curve _curve2;
	}
}
