using System;
using UnityEngine;

// Token: 0x0200091A RID: 2330
public class MultipleRenderTargetProxy : MonoBehaviour
{
	// Token: 0x06004389 RID: 17289 RVA: 0x0017A76C File Offset: 0x0017896C
	private void Start()
	{
		if (ScreenResize.Instance != null)
		{
			ScreenResize instance = ScreenResize.Instance;
			instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		}
		this.CreateRenderTarget();
		ShaderReloader.Register(new System.Action(this.OnShadersReloaded));
	}

	// Token: 0x0600438A RID: 17290 RVA: 0x0017A7C3 File Offset: 0x001789C3
	public void ToggleColouredOverlayView(bool enabled)
	{
		this.colouredOverlayBufferEnabled = enabled;
		this.CreateRenderTarget();
	}

	// Token: 0x0600438B RID: 17291 RVA: 0x0017A7D4 File Offset: 0x001789D4
	private void CreateRenderTarget()
	{
		RenderBuffer[] array = new RenderBuffer[this.colouredOverlayBufferEnabled ? 3 : 2];
		this.Textures[0] = this.RecreateRT(this.Textures[0], 24, RenderTextureFormat.ARGB32);
		this.Textures[0].filterMode = FilterMode.Point;
		this.Textures[0].name = "MRT0";
		this.Textures[1] = this.RecreateRT(this.Textures[1], 0, RenderTextureFormat.R8);
		this.Textures[1].filterMode = FilterMode.Point;
		this.Textures[1].name = "MRT1";
		array[0] = this.Textures[0].colorBuffer;
		array[1] = this.Textures[1].colorBuffer;
		if (this.colouredOverlayBufferEnabled)
		{
			this.Textures[2] = this.RecreateRT(this.Textures[2], 0, RenderTextureFormat.ARGB32);
			this.Textures[2].filterMode = FilterMode.Bilinear;
			this.Textures[2].name = "MRT2";
			array[2] = this.Textures[2].colorBuffer;
		}
		base.GetComponent<Camera>().SetTargetBuffers(array, this.Textures[0].depthBuffer);
		this.OnShadersReloaded();
	}

	// Token: 0x0600438C RID: 17292 RVA: 0x0017A900 File Offset: 0x00178B00
	private RenderTexture RecreateRT(RenderTexture rt, int depth, RenderTextureFormat format)
	{
		RenderTexture result = rt;
		if (rt == null || rt.width != Screen.width || rt.height != Screen.height || rt.format != format)
		{
			if (rt != null)
			{
				rt.DestroyRenderTexture();
			}
			result = new RenderTexture(Screen.width, Screen.height, depth, format);
		}
		return result;
	}

	// Token: 0x0600438D RID: 17293 RVA: 0x0017A95D File Offset: 0x00178B5D
	private void OnResize()
	{
		this.CreateRenderTarget();
	}

	// Token: 0x0600438E RID: 17294 RVA: 0x0017A965 File Offset: 0x00178B65
	private void Update()
	{
		if (!this.Textures[0].IsCreated())
		{
			this.CreateRenderTarget();
		}
	}

	// Token: 0x0600438F RID: 17295 RVA: 0x0017A97C File Offset: 0x00178B7C
	private void OnShadersReloaded()
	{
		Shader.SetGlobalTexture("_MRT0", this.Textures[0]);
		Shader.SetGlobalTexture("_MRT1", this.Textures[1]);
		if (this.colouredOverlayBufferEnabled)
		{
			Shader.SetGlobalTexture("_MRT2", this.Textures[2]);
		}
	}

	// Token: 0x04002CBB RID: 11451
	public RenderTexture[] Textures = new RenderTexture[3];

	// Token: 0x04002CBC RID: 11452
	private bool colouredOverlayBufferEnabled;
}
