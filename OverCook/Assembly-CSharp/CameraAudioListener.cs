using System;
using UnityEngine;

// Token: 0x02000128 RID: 296
[RequireComponent(typeof(AudioListener))]
[RequireComponent(typeof(Camera))]
public class CameraAudioListener : MonoBehaviour
{
	// Token: 0x0600056B RID: 1387 RVA: 0x0002A2A6 File Offset: 0x000286A6
	private void OnEnable()
	{
		this.m_audiolistener.enabled = (Camera.main == this.m_camera);
	}

	// Token: 0x0600056C RID: 1388 RVA: 0x0002A2C4 File Offset: 0x000286C4
	private void LateUpdate()
	{
		Camera main = Camera.main;
		if (main != null && main.enabled)
		{
			this.m_audiolistener.enabled = (Camera.main == this.m_camera);
		}
		else
		{
			this.m_audiolistener.enabled = this.m_camera.enabled;
		}
	}

	// Token: 0x0600056D RID: 1389 RVA: 0x0002A324 File Offset: 0x00028724
	private void OnDisable()
	{
		this.m_audiolistener.enabled = false;
	}

	// Token: 0x040004A3 RID: 1187
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private AudioListener m_audiolistener;

	// Token: 0x040004A4 RID: 1188
	[SerializeField]
	[AssignComponent(Editorbility.NonEditable)]
	private Camera m_camera;
}
