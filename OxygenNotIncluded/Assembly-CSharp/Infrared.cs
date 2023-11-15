using System;
using UnityEngine;

// Token: 0x02000A46 RID: 2630
public class Infrared : MonoBehaviour
{
	// Token: 0x06004F25 RID: 20261 RVA: 0x001C0056 File Offset: 0x001BE256
	public static void DestroyInstance()
	{
		Infrared.Instance = null;
	}

	// Token: 0x06004F26 RID: 20262 RVA: 0x001C005E File Offset: 0x001BE25E
	private void Awake()
	{
		Infrared.temperatureParametersId = Shader.PropertyToID("_TemperatureParameters");
		Infrared.Instance = this;
		this.OnResize();
		this.UpdateState();
	}

	// Token: 0x06004F27 RID: 20263 RVA: 0x001C0081 File Offset: 0x001BE281
	private void OnRenderImage(RenderTexture source, RenderTexture dest)
	{
		Graphics.Blit(source, this.minionTexture);
		Graphics.Blit(source, dest);
	}

	// Token: 0x06004F28 RID: 20264 RVA: 0x001C0098 File Offset: 0x001BE298
	private void OnResize()
	{
		if (this.minionTexture != null)
		{
			this.minionTexture.DestroyRenderTexture();
		}
		if (this.cameraTexture != null)
		{
			this.cameraTexture.DestroyRenderTexture();
		}
		int num = 2;
		this.minionTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		this.cameraTexture = new RenderTexture(Screen.width / num, Screen.height / num, 0, RenderTextureFormat.ARGB32);
		base.GetComponent<Camera>().targetTexture = this.cameraTexture;
	}

	// Token: 0x06004F29 RID: 20265 RVA: 0x001C0120 File Offset: 0x001BE320
	public void SetMode(Infrared.Mode mode)
	{
		Vector4 zero;
		if (mode != Infrared.Mode.Disabled)
		{
			if (mode != Infrared.Mode.Disease)
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
			}
			else
			{
				zero = new Vector4(1f, 0f, 0f, 0f);
				GameComps.InfraredVisualizers.ClearOverlayColour();
			}
		}
		else
		{
			zero = Vector4.zero;
		}
		Shader.SetGlobalVector("_ColouredOverlayParameters", zero);
		this.mode = mode;
		this.UpdateState();
	}

	// Token: 0x06004F2A RID: 20266 RVA: 0x001C0198 File Offset: 0x001BE398
	private void UpdateState()
	{
		base.gameObject.SetActive(this.mode > Infrared.Mode.Disabled);
		if (base.gameObject.activeSelf)
		{
			this.Update();
		}
	}

	// Token: 0x06004F2B RID: 20267 RVA: 0x001C01C4 File Offset: 0x001BE3C4
	private void Update()
	{
		switch (this.mode)
		{
		case Infrared.Mode.Disabled:
			break;
		case Infrared.Mode.Infrared:
			GameComps.InfraredVisualizers.UpdateTemperature();
			return;
		case Infrared.Mode.Disease:
			GameComps.DiseaseContainers.UpdateOverlayColours();
			break;
		default:
			return;
		}
	}

	// Token: 0x04003383 RID: 13187
	private RenderTexture minionTexture;

	// Token: 0x04003384 RID: 13188
	private RenderTexture cameraTexture;

	// Token: 0x04003385 RID: 13189
	private Infrared.Mode mode;

	// Token: 0x04003386 RID: 13190
	public static int temperatureParametersId;

	// Token: 0x04003387 RID: 13191
	public static Infrared Instance;

	// Token: 0x020018D9 RID: 6361
	public enum Mode
	{
		// Token: 0x04007325 RID: 29477
		Disabled,
		// Token: 0x04007326 RID: 29478
		Infrared,
		// Token: 0x04007327 RID: 29479
		Disease
	}
}
