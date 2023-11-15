using System;
using UnityEngine;

// Token: 0x020000DB RID: 219
public class AlwaysFaceCamera : MonoBehaviour
{
	// Token: 0x0600053B RID: 1339 RVA: 0x00015024 File Offset: 0x00013224
	private void Awake()
	{
		this.camera = Singleton<CameraManager>.Instance.MainCamera;
		this.rotationOffsetQuat = Quaternion.Euler(this.rotationOffset);
		if (this.camera.orthographic)
		{
			base.transform.forward = this.rotationOffsetQuat * (this.camera.transform.forward * -1f);
		}
	}

	// Token: 0x0600053C RID: 1340 RVA: 0x00015090 File Offset: 0x00013290
	private void Update()
	{
		if (!this.camera.orthographic)
		{
			base.transform.forward = this.rotationOffsetQuat * (this.camera.transform.position - base.transform.position);
			return;
		}
		base.transform.forward = this.rotationOffsetQuat * (this.camera.transform.forward * -1f);
	}

	// Token: 0x040004DE RID: 1246
	[SerializeField]
	private Vector3 rotationOffset;

	// Token: 0x040004DF RID: 1247
	private Camera camera;

	// Token: 0x040004E0 RID: 1248
	private Quaternion rotationOffsetQuat;
}
