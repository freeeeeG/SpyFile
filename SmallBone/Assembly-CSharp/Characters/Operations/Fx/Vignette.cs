using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F24 RID: 3876
	public class Vignette : CharacterOperation
	{
		// Token: 0x06004B94 RID: 19348 RVA: 0x000DE78A File Offset: 0x000DC98A
		public override void Run(Character owner)
		{
			Singleton<VignetteSpawner>.Instance.Spawn(this._startColor, this._endColor, this._curve);
		}

		// Token: 0x04003AD5 RID: 15061
		[SerializeField]
		private Color _startColor;

		// Token: 0x04003AD6 RID: 15062
		[SerializeField]
		private Color _endColor;

		// Token: 0x04003AD7 RID: 15063
		[SerializeField]
		private Curve _curve;
	}
}
