using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Movement
{
	// Token: 0x02000E55 RID: 3669
	public class DualSmash : TargetedCharacterOperation
	{
		// Token: 0x060048E2 RID: 18658 RVA: 0x00002191 File Offset: 0x00000391
		public override void Run(Character owner, Character target)
		{
		}

		// Token: 0x040037F5 RID: 14325
		[Information("Obsolete", InformationAttribute.InformationType.Warning, true)]
		[SerializeField]
		private PushForce _pushForce1;

		// Token: 0x040037F6 RID: 14326
		[SerializeField]
		private Curve _curve1;

		// Token: 0x040037F7 RID: 14327
		[SerializeField]
		private PushForce _pushForce2;

		// Token: 0x040037F8 RID: 14328
		[SerializeField]
		private Curve _curve2;

		// Token: 0x040037F9 RID: 14329
		[SerializeField]
		[UnityEditor.Subcomponent(typeof(TargetedOperationInfo))]
		private TargetedOperationInfo.Subcomponents _onCollide;
	}
}
