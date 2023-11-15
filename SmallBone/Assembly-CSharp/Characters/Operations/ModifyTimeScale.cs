using System;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000E25 RID: 3621
	public class ModifyTimeScale : CharacterOperation
	{
		// Token: 0x0600483D RID: 18493 RVA: 0x000D22B8 File Offset: 0x000D04B8
		public override void Run(Character target)
		{
			this._chronoToTarget.ApplyTo(target);
			this._chronoToGlobe.ApplyGlobe();
		}

		// Token: 0x04003758 RID: 14168
		[SerializeField]
		protected ChronoInfo _chronoToGlobe;

		// Token: 0x04003759 RID: 14169
		[SerializeField]
		protected ChronoInfo _chronoToTarget;
	}
}
