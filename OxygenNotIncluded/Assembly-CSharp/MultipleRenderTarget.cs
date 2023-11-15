using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000919 RID: 2329
public class MultipleRenderTarget : MonoBehaviour
{
	// Token: 0x1400001D RID: 29
	// (add) Token: 0x06004382 RID: 17282 RVA: 0x0017A694 File Offset: 0x00178894
	// (remove) Token: 0x06004383 RID: 17283 RVA: 0x0017A6CC File Offset: 0x001788CC
	public event Action<Camera> onSetupComplete;

	// Token: 0x06004384 RID: 17284 RVA: 0x0017A701 File Offset: 0x00178901
	private void Start()
	{
		base.StartCoroutine(this.SetupProxy());
	}

	// Token: 0x06004385 RID: 17285 RVA: 0x0017A710 File Offset: 0x00178910
	private IEnumerator SetupProxy()
	{
		yield return null;
		Camera component = base.GetComponent<Camera>();
		Camera camera = new GameObject().AddComponent<Camera>();
		camera.CopyFrom(component);
		this.renderProxy = camera.gameObject.AddComponent<MultipleRenderTargetProxy>();
		camera.name = component.name + " MRT";
		camera.transform.parent = component.transform;
		camera.transform.SetLocalPosition(Vector3.zero);
		camera.depth = component.depth - 1f;
		component.cullingMask = 0;
		component.clearFlags = CameraClearFlags.Color;
		this.quad = new FullScreenQuad("MultipleRenderTarget", component, true);
		if (this.onSetupComplete != null)
		{
			this.onSetupComplete(camera);
		}
		yield break;
	}

	// Token: 0x06004386 RID: 17286 RVA: 0x0017A71F File Offset: 0x0017891F
	private void OnPreCull()
	{
		if (this.renderProxy != null)
		{
			this.quad.Draw(this.renderProxy.Textures[0]);
		}
	}

	// Token: 0x06004387 RID: 17287 RVA: 0x0017A747 File Offset: 0x00178947
	public void ToggleColouredOverlayView(bool enabled)
	{
		if (this.renderProxy != null)
		{
			this.renderProxy.ToggleColouredOverlayView(enabled);
		}
	}

	// Token: 0x04002CB7 RID: 11447
	private MultipleRenderTargetProxy renderProxy;

	// Token: 0x04002CB8 RID: 11448
	private FullScreenQuad quad;

	// Token: 0x04002CBA RID: 11450
	public bool isFrontEnd;
}
