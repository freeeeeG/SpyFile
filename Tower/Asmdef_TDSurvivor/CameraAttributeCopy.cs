using System;
using UnityEngine;

// Token: 0x02000054 RID: 84
[ExecuteInEditMode]
public class CameraAttributeCopy : MonoBehaviour
{
	// Token: 0x060001C8 RID: 456 RVA: 0x00007BB1 File Offset: 0x00005DB1
	private void Reset()
	{
		if (!this.myCamera)
		{
			this.myCamera = base.GetComponent<Camera>();
		}
	}

	// Token: 0x060001C9 RID: 457 RVA: 0x00007BCC File Offset: 0x00005DCC
	private void Start()
	{
		this.tarCamera = Singleton<CameraManager>.Instance.MainCamera;
		this.myCamera.orthographic = this.tarCamera.orthographic;
	}

	// Token: 0x060001CA RID: 458 RVA: 0x00007BF4 File Offset: 0x00005DF4
	private void Update()
	{
		if (this.tarCamera.enabled)
		{
			base.transform.CopyPosAndRot(this.tarCamera.gameObject.transform);
			if (this.tarCamera.orthographic)
			{
				this.myCamera.orthographicSize = this.tarCamera.orthographicSize;
				return;
			}
			this.myCamera.fieldOfView = this.tarCamera.fieldOfView;
		}
	}

	// Token: 0x04000156 RID: 342
	[SerializeField]
	private Camera myCamera;

	// Token: 0x04000157 RID: 343
	private Camera tarCamera;
}
