using System;
using UnityEngine;

// Token: 0x02000363 RID: 867
[RequireComponent(typeof(Canvas))]
public class CanvasCameraAssignment : MonoBehaviour
{
	// Token: 0x06001091 RID: 4241 RVA: 0x0005F8D9 File Offset: 0x0005DCD9
	private void Awake()
	{
		this.SetCamera(this.m_camera);
	}

	// Token: 0x06001092 RID: 4242 RVA: 0x0005F8E7 File Offset: 0x0005DCE7
	private void OnEnable()
	{
		this.SetCameraEnabled(true);
	}

	// Token: 0x06001093 RID: 4243 RVA: 0x0005F8F0 File Offset: 0x0005DCF0
	private void OnDisable()
	{
		this.SetCameraEnabled(false);
	}

	// Token: 0x06001094 RID: 4244 RVA: 0x0005F8F9 File Offset: 0x0005DCF9
	private void SetCameraEnabled(bool _enabled)
	{
	}

	// Token: 0x06001095 RID: 4245 RVA: 0x0005F8FC File Offset: 0x0005DCFC
	public void SetCamera(string _camera)
	{
		this.m_camera = _camera;
		GameObject gameObject = GameObject.Find(this.m_camera);
		if (gameObject != null)
		{
			Camera worldCamera = gameObject.RequireComponent<Camera>();
			this.m_canvas.worldCamera = worldCamera;
			this.SetCameraEnabled(true);
		}
	}

	// Token: 0x04000CC3 RID: 3267
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Canvas m_canvas;

	// Token: 0x04000CC4 RID: 3268
	[SerializeField]
	private string m_camera;
}
