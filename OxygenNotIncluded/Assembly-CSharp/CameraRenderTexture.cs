using System;
using UnityEngine;

// Token: 0x0200090A RID: 2314
public class CameraRenderTexture : MonoBehaviour
{
	// Token: 0x0600431D RID: 17181 RVA: 0x001773E0 File Offset: 0x001755E0
	private void Awake()
	{
		this.material = new Material(Shader.Find("Klei/PostFX/CameraRenderTexture"));
	}

	// Token: 0x0600431E RID: 17182 RVA: 0x001773F7 File Offset: 0x001755F7
	private void Start()
	{
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
		this.OnResize();
	}

	// Token: 0x0600431F RID: 17183 RVA: 0x00177434 File Offset: 0x00175634
	private void OnResize()
	{
		if (this.resultTexture != null)
		{
			this.resultTexture.DestroyRenderTexture();
		}
		this.resultTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
		this.resultTexture.name = base.name;
		this.resultTexture.filterMode = FilterMode.Point;
		this.resultTexture.autoGenerateMips = false;
		if (this.TextureName != "")
		{
			Shader.SetGlobalTexture(this.TextureName, this.resultTexture);
		}
	}

	// Token: 0x06004320 RID: 17184 RVA: 0x001774BD File Offset: 0x001756BD
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.resultTexture, this.material);
	}

	// Token: 0x06004321 RID: 17185 RVA: 0x001774D1 File Offset: 0x001756D1
	public RenderTexture GetTexture()
	{
		return this.resultTexture;
	}

	// Token: 0x06004322 RID: 17186 RVA: 0x001774D9 File Offset: 0x001756D9
	public bool ShouldFlip()
	{
		return false;
	}

	// Token: 0x04002BC2 RID: 11202
	public string TextureName;

	// Token: 0x04002BC3 RID: 11203
	private RenderTexture resultTexture;

	// Token: 0x04002BC4 RID: 11204
	private Material material;
}
