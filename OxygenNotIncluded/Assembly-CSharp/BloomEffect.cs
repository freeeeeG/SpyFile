using System;
using UnityEngine;

// Token: 0x02000A43 RID: 2627
public class BloomEffect : MonoBehaviour
{
	// Token: 0x170005DC RID: 1500
	// (get) Token: 0x06004F18 RID: 20248 RVA: 0x001BFD97 File Offset: 0x001BDF97
	protected Material material
	{
		get
		{
			if (this.m_Material == null)
			{
				this.m_Material = new Material(this.blurShader);
				this.m_Material.hideFlags = HideFlags.DontSave;
			}
			return this.m_Material;
		}
	}

	// Token: 0x06004F19 RID: 20249 RVA: 0x001BFDCB File Offset: 0x001BDFCB
	protected void OnDisable()
	{
		if (this.m_Material)
		{
			UnityEngine.Object.DestroyImmediate(this.m_Material);
		}
	}

	// Token: 0x06004F1A RID: 20250 RVA: 0x001BFDE8 File Offset: 0x001BDFE8
	protected void Start()
	{
		if (!this.blurShader || !this.material.shader.isSupported)
		{
			base.enabled = false;
			return;
		}
		this.BloomMaskMaterial = new Material(Shader.Find("Klei/PostFX/BloomMask"));
		this.BloomCompositeMaterial = new Material(Shader.Find("Klei/PostFX/BloomComposite"));
	}

	// Token: 0x06004F1B RID: 20251 RVA: 0x001BFE48 File Offset: 0x001BE048
	public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		float num = 0.5f + (float)iteration * this.blurSpread;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06004F1C RID: 20252 RVA: 0x001BFEB4 File Offset: 0x001BE0B4
	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		float num = 1f;
		Graphics.BlitMultiTap(source, dest, this.material, new Vector2[]
		{
			new Vector2(-num, -num),
			new Vector2(-num, num),
			new Vector2(num, num),
			new Vector2(num, -num)
		});
	}

	// Token: 0x06004F1D RID: 20253 RVA: 0x001BFF18 File Offset: 0x001BE118
	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
		temporary.name = "bloom_source";
		Graphics.Blit(source, temporary, this.BloomMaskMaterial);
		int width = Math.Max(source.width / 4, 4);
		int height = Math.Max(source.height / 4, 4);
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0);
		renderTexture.name = "bloom_downsampled";
		this.DownSample4x(temporary, renderTexture);
		RenderTexture.ReleaseTemporary(temporary);
		for (int i = 0; i < this.iterations; i++)
		{
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0);
			temporary2.name = "bloom_blurred";
			this.FourTapCone(renderTexture, temporary2, i);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary2;
		}
		this.BloomCompositeMaterial.SetTexture("_BloomTex", renderTexture);
		Graphics.Blit(source, destination, this.BloomCompositeMaterial);
		RenderTexture.ReleaseTemporary(renderTexture);
	}

	// Token: 0x0400337C RID: 13180
	private Material BloomMaskMaterial;

	// Token: 0x0400337D RID: 13181
	private Material BloomCompositeMaterial;

	// Token: 0x0400337E RID: 13182
	public int iterations = 3;

	// Token: 0x0400337F RID: 13183
	public float blurSpread = 0.6f;

	// Token: 0x04003380 RID: 13184
	public Shader blurShader;

	// Token: 0x04003381 RID: 13185
	private Material m_Material;
}
