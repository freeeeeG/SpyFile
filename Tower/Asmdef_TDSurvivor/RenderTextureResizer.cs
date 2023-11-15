using System;
using UnityEngine;

// Token: 0x020001A3 RID: 419
[RequireComponent(typeof(Camera))]
public class RenderTextureResizer : MonoBehaviour
{
	// Token: 0x06000B2D RID: 2861 RVA: 0x0002B4E6 File Offset: 0x000296E6
	private void Start()
	{
		this.UpdateRenderTexture();
		this.lastResolution = new Vector2Int(Screen.width, Screen.height);
	}

	// Token: 0x06000B2E RID: 2862 RVA: 0x0002B504 File Offset: 0x00029704
	private void Update()
	{
		if (Screen.width != this.lastResolution.x || Screen.height != this.lastResolution.y)
		{
			this.UpdateRenderTexture();
			this.lastResolution = new Vector2Int(Screen.width, Screen.height);
		}
	}

	// Token: 0x06000B2F RID: 2863 RVA: 0x0002B550 File Offset: 0x00029750
	private void UpdateRenderTexture()
	{
		if (this.targetRenderTexture != null)
		{
			this.targetRenderTexture.Release();
			this.targetRenderTexture.width = Screen.width;
			this.targetRenderTexture.height = Screen.height;
			Camera component = base.GetComponent<Camera>();
			if (component != null)
			{
				component.targetTexture = this.targetRenderTexture;
				component.enabled = false;
				component.enabled = true;
				return;
			}
		}
		else
		{
			Debug.LogError("Target RenderTexture is not assigned!");
		}
	}

	// Token: 0x040008FE RID: 2302
	public RenderTexture targetRenderTexture;

	// Token: 0x040008FF RID: 2303
	private Vector2Int lastResolution;
}
