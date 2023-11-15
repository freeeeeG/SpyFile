using System;
using Level;
using UnityEngine;

namespace Runnables
{
	// Token: 0x020002D1 RID: 721
	public sealed class ChangeCameraZone : Runnable
	{
		// Token: 0x06000E85 RID: 3717 RVA: 0x0002D652 File Offset: 0x0002B852
		public override void Run()
		{
			Map.Instance.cameraZone = this._cameraZone;
			Map.Instance.SetCameraZoneOrDefault();
		}

		// Token: 0x04000C0D RID: 3085
		[SerializeField]
		private CameraZone _cameraZone;
	}
}
