using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001B1 RID: 433
public class EditModeGridEffect : MonoBehaviour
{
	// Token: 0x06000B86 RID: 2950 RVA: 0x0002D3A0 File Offset: 0x0002B5A0
	private void Awake()
	{
		if (this.gridRenderer != null)
		{
			this.runtimeMaterial = Object.Instantiate<Material>(this.gridRenderer.sharedMaterial);
			this.gridRenderer.material = this.runtimeMaterial;
		}
		this.gridRenderer.enabled = false;
	}

	// Token: 0x06000B87 RID: 2951 RVA: 0x0002D3EE File Offset: 0x0002B5EE
	private void OnEnable()
	{
		EventMgr.Register<eGameState>(eGameEvents.GameStateChanged, new Action<eGameState>(this.OnGameStateChanged));
	}

	// Token: 0x06000B88 RID: 2952 RVA: 0x0002D407 File Offset: 0x0002B607
	private void OnDisable()
	{
		EventMgr.Remove<eGameState>(eGameEvents.GameStateChanged, new Action<eGameState>(this.OnGameStateChanged));
	}

	// Token: 0x06000B89 RID: 2953 RVA: 0x0002D420 File Offset: 0x0002B620
	private void Update()
	{
		if (this.isActivated)
		{
			Vector3 zero = Vector3.zero;
			Ray ray = Singleton<CameraManager>.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);
			float distance;
			if (this.yZeroPlane.Raycast(ray, out distance))
			{
				this.mousePos3D = ray.GetPoint(distance);
			}
			Debug.DrawLine(this.mousePos3D, this.mousePos3D + Vector3.up * 50f, Color.red);
			zero.x = this.mousePos3D.x;
			zero.y = this.mousePos3D.z;
			this.runtimeMaterial.SetVector("_MousePos", zero);
		}
	}

	// Token: 0x06000B8A RID: 2954 RVA: 0x0002D4D8 File Offset: 0x0002B6D8
	private void OnGameStateChanged(eGameState state)
	{
		if (state == eGameState.EDIT_MODE)
		{
			if (this.coroutine != null)
			{
				base.StopCoroutine(this.coroutine);
			}
			this.coroutine = base.StartCoroutine(this.CR_SwitchMode(0f, 0.5f));
			this.isActivated = true;
		}
		else if (this.curState == eGameState.EDIT_MODE)
		{
			if (this.coroutine != null)
			{
				base.StopCoroutine(this.coroutine);
			}
			this.coroutine = base.StartCoroutine(this.CR_SwitchMode(0.5f, -0.2f));
			this.isActivated = false;
		}
		this.curState = state;
	}

	// Token: 0x06000B8B RID: 2955 RVA: 0x0002D569 File Offset: 0x0002B769
	private IEnumerator CR_SwitchMode(float from, float to)
	{
		float time = 0f;
		float duration = 0.33f;
		this.gridRenderer.enabled = true;
		while (time <= duration)
		{
			float t = this.curve_Dissolve.Evaluate(time / duration);
			this.runtimeMaterial.SetFloat("_Dissolve", Mathf.Lerp(from, to, t));
			yield return null;
			time += Time.deltaTime;
		}
		this.runtimeMaterial.SetFloat("_Dissolve", to);
		if (to < 0f)
		{
			this.gridRenderer.enabled = false;
		}
		this.coroutine = null;
		yield break;
	}

	// Token: 0x04000931 RID: 2353
	[SerializeField]
	private Renderer gridRenderer;

	// Token: 0x04000932 RID: 2354
	[SerializeField]
	private AnimationCurve curve_Dissolve;

	// Token: 0x04000933 RID: 2355
	private Coroutine coroutine;

	// Token: 0x04000934 RID: 2356
	private Material runtimeMaterial;

	// Token: 0x04000935 RID: 2357
	private eGameState curState;

	// Token: 0x04000936 RID: 2358
	private bool isActivated;

	// Token: 0x04000937 RID: 2359
	private Plane yZeroPlane = new Plane(Vector3.up, 0f);

	// Token: 0x04000938 RID: 2360
	private Vector3 mousePos3D = Vector3.zero;
}
