using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C91 RID: 3217
public class VirtualCursorOverlayFix : MonoBehaviour
{
	// Token: 0x0600667E RID: 26238 RVA: 0x00263888 File Offset: 0x00261A88
	private void Awake()
	{
		int width = Screen.currentResolution.width;
		int height = Screen.currentResolution.height;
		this.cursorRendTex = new RenderTexture(width, height, 0);
		this.screenSpaceCamera.enabled = true;
		this.screenSpaceCamera.targetTexture = this.cursorRendTex;
		this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
		base.StartCoroutine(this.RenderVirtualCursor());
	}

	// Token: 0x0600667F RID: 26239 RVA: 0x00263904 File Offset: 0x00261B04
	private IEnumerator RenderVirtualCursor()
	{
		bool ShowCursor = KInputManager.currentControllerIsGamepad;
		while (Application.isPlaying)
		{
			ShowCursor = KInputManager.currentControllerIsGamepad;
			if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.C))
			{
				ShowCursor = true;
			}
			this.screenSpaceCamera.enabled = true;
			if (!this.screenSpaceOverlayImage.enabled && ShowCursor)
			{
				yield return SequenceUtil.WaitForSecondsRealtime(0.1f);
			}
			this.actualCursor.enabled = ShowCursor;
			this.screenSpaceOverlayImage.enabled = ShowCursor;
			this.screenSpaceOverlayImage.material.SetTexture("_MainTex", this.cursorRendTex);
			yield return null;
		}
		yield break;
	}

	// Token: 0x040046A4 RID: 18084
	private RenderTexture cursorRendTex;

	// Token: 0x040046A5 RID: 18085
	public Camera screenSpaceCamera;

	// Token: 0x040046A6 RID: 18086
	public Image screenSpaceOverlayImage;

	// Token: 0x040046A7 RID: 18087
	public RawImage actualCursor;
}
