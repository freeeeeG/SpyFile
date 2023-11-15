using System;
using Scenes;
using UnityEngine;

namespace Characters.Operations.Fx
{
	// Token: 0x02000F0C RID: 3852
	public class CameraShakeCurve : CharacterOperation
	{
		// Token: 0x06004B40 RID: 19264 RVA: 0x000DD89E File Offset: 0x000DBA9E
		public override void Run(Character owner)
		{
			Scene<GameBase>.instance.cameraController.shake.Attach(this, this._curve);
		}

		// Token: 0x04003A6D RID: 14957
		[SerializeField]
		private Curve _curve;
	}
}
