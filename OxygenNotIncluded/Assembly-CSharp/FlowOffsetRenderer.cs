using System;
using UnityEngine;

// Token: 0x020007C8 RID: 1992
[AddComponentMenu("KMonoBehaviour/scripts/FlowOffsetRenderer")]
public class FlowOffsetRenderer : KMonoBehaviour
{
	// Token: 0x06003758 RID: 14168 RVA: 0x0012B7DC File Offset: 0x001299DC
	protected override void OnSpawn()
	{
		this.FlowMaterial = new Material(Shader.Find("Klei/Flow"));
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
		this.OnResize();
		this.DoUpdate(0.1f);
	}

	// Token: 0x06003759 RID: 14169 RVA: 0x0012B838 File Offset: 0x00129A38
	private void OnResize()
	{
		for (int i = 0; i < this.OffsetTextures.Length; i++)
		{
			if (this.OffsetTextures[i] != null)
			{
				this.OffsetTextures[i].DestroyRenderTexture();
			}
			this.OffsetTextures[i] = new RenderTexture(Screen.width / 2, Screen.height / 2, 0, RenderTextureFormat.ARGBHalf);
			this.OffsetTextures[i].filterMode = FilterMode.Bilinear;
			this.OffsetTextures[i].name = "FlowOffsetTexture";
		}
	}

	// Token: 0x0600375A RID: 14170 RVA: 0x0012B8B4 File Offset: 0x00129AB4
	private void LateUpdate()
	{
		if ((Time.deltaTime > 0f && Time.timeScale > 0f) || this.forceUpdate)
		{
			float num = Time.deltaTime / Time.timeScale;
			this.DoUpdate(num * Time.timeScale / 4f + num * 0.5f);
		}
	}

	// Token: 0x0600375B RID: 14171 RVA: 0x0012B908 File Offset: 0x00129B08
	private void DoUpdate(float dt)
	{
		this.CurrentTime += dt;
		float num = this.CurrentTime * this.PhaseMultiplier;
		num -= (float)((int)num);
		float num2 = num - (float)((int)num);
		float y = 1f;
		if (num2 <= this.GasPhase0)
		{
			y = 0f;
		}
		this.GasPhase0 = num2;
		float z = 1f;
		float num3 = num + 0.5f - (float)((int)(num + 0.5f));
		if (num3 <= this.GasPhase1)
		{
			z = 0f;
		}
		this.GasPhase1 = num3;
		Shader.SetGlobalVector(this.ParametersName, new Vector4(this.GasPhase0, 0f, 0f, 0f));
		Shader.SetGlobalVector("_NoiseParameters", new Vector4(this.NoiseInfluence, this.NoiseScale, 0f, 0f));
		RenderTexture renderTexture = this.OffsetTextures[this.OffsetIdx];
		this.OffsetIdx = (this.OffsetIdx + 1) % 2;
		RenderTexture renderTexture2 = this.OffsetTextures[this.OffsetIdx];
		Material flowMaterial = this.FlowMaterial;
		flowMaterial.SetTexture("_PreviousOffsetTex", renderTexture);
		flowMaterial.SetVector("_FlowParameters", new Vector4(Time.deltaTime * this.OffsetSpeed, y, z, 0f));
		flowMaterial.SetVector("_MinFlow", new Vector4(this.MinFlow0.x, this.MinFlow0.y, this.MinFlow1.x, this.MinFlow1.y));
		flowMaterial.SetVector("_VisibleArea", new Vector4(0f, 0f, (float)Grid.WidthInCells, (float)Grid.HeightInCells));
		flowMaterial.SetVector("_LiquidGasMask", new Vector4(this.LiquidGasMask.x, this.LiquidGasMask.y, 0f, 0f));
		Graphics.Blit(renderTexture, renderTexture2, flowMaterial);
		Shader.SetGlobalTexture(this.OffsetTextureName, renderTexture2);
	}

	// Token: 0x0400220F RID: 8719
	private float GasPhase0;

	// Token: 0x04002210 RID: 8720
	private float GasPhase1;

	// Token: 0x04002211 RID: 8721
	public float PhaseMultiplier;

	// Token: 0x04002212 RID: 8722
	public float NoiseInfluence;

	// Token: 0x04002213 RID: 8723
	public float NoiseScale;

	// Token: 0x04002214 RID: 8724
	public float OffsetSpeed;

	// Token: 0x04002215 RID: 8725
	public string OffsetTextureName;

	// Token: 0x04002216 RID: 8726
	public string ParametersName;

	// Token: 0x04002217 RID: 8727
	public Vector2 MinFlow0;

	// Token: 0x04002218 RID: 8728
	public Vector2 MinFlow1;

	// Token: 0x04002219 RID: 8729
	public Vector2 LiquidGasMask;

	// Token: 0x0400221A RID: 8730
	[SerializeField]
	private Material FlowMaterial;

	// Token: 0x0400221B RID: 8731
	[SerializeField]
	private bool forceUpdate;

	// Token: 0x0400221C RID: 8732
	private TextureLerper FlowLerper;

	// Token: 0x0400221D RID: 8733
	public RenderTexture[] OffsetTextures = new RenderTexture[2];

	// Token: 0x0400221E RID: 8734
	private int OffsetIdx;

	// Token: 0x0400221F RID: 8735
	private float CurrentTime;
}
