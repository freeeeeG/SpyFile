using System;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F23 RID: 3875
	public class Vibration : CharacterOperation
	{
		// Token: 0x06004B92 RID: 19346 RVA: 0x000DE76D File Offset: 0x000DC96D
		public override void Run(Character owner)
		{
			Singleton<Service>.Instance.controllerVibation.vibration.Attach(this, this._curve);
		}

		// Token: 0x04003AD4 RID: 15060
		[SerializeField]
		private Curve _curve;
	}
}
