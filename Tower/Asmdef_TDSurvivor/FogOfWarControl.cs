using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020001B2 RID: 434
public class FogOfWarControl : MonoBehaviour
{
	// Token: 0x06000B8D RID: 2957 RVA: 0x0002D5AE File Offset: 0x0002B7AE
	private void Reset()
	{
		if (this.fogOfWar_Material == null)
		{
			this.fogOfWar_Material = base.GetComponent<RawImage>().material;
		}
	}

	// Token: 0x06000B8E RID: 2958 RVA: 0x0002D5CF File Offset: 0x0002B7CF
	private void OnEnable()
	{
		EventMgr.Register<eSceneTimeType, float>(eGameEvents.StartSceneTimeChange, new Action<eSceneTimeType, float>(this.OnStartSceneTimeChange));
	}

	// Token: 0x06000B8F RID: 2959 RVA: 0x0002D5E9 File Offset: 0x0002B7E9
	private void OnDisable()
	{
		EventMgr.Remove<eSceneTimeType, float>(eGameEvents.StartSceneTimeChange, new Action<eSceneTimeType, float>(this.OnStartSceneTimeChange));
	}

	// Token: 0x06000B90 RID: 2960 RVA: 0x0002D603 File Offset: 0x0002B803
	private void Awake()
	{
		this.runtimeMaterial = Object.Instantiate<Material>(this.fogOfWar_Material);
		base.GetComponent<RawImage>().material = this.runtimeMaterial;
		this.camStartPos = Singleton<CameraManager>.Instance.MainCamera.transform.position;
	}

	// Token: 0x06000B91 RID: 2961 RVA: 0x0002D644 File Offset: 0x0002B844
	private void Update()
	{
		if (this.runtimeMaterial == null)
		{
			return;
		}
		this.delta = Singleton<CameraManager>.Instance.MainCamera.transform.position - this.camStartPos;
		this.posVector.x = this.delta.x / (20f * Singleton<CameraManager>.Instance.MainCamera.aspect);
		this.posVector.z = this.delta.z / 20f;
		this.runtimeMaterial.SetVector("_WorldPosition", this.posVector);
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x0002D6E8 File Offset: 0x0002B8E8
	private void OnStartSceneTimeChange(eSceneTimeType type, float duration)
	{
		if (type == eSceneTimeType.DAYTIME)
		{
			base.StartCoroutine(this.CR_SceneTimeChangeEffect(1f, 0f, duration));
			return;
		}
		if (type == eSceneTimeType.NIGHT)
		{
			base.StartCoroutine(this.CR_SceneTimeChangeEffect(0f, 1f, duration));
		}
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x0002D723 File Offset: 0x0002B923
	private IEnumerator CR_SceneTimeChangeEffect(float from, float to, float duration)
	{
		float time = 0f;
		Color color = this.image_FogOfWarEffect.color;
		while (time <= duration)
		{
			float t = time / duration;
			this.runtimeMaterial.SetFloat("_ScreenDistAlphaLerp", Mathf.Lerp(from, to, Easing.GetEasingFunction(Easing.Type.EaseOutCubic, t)));
			yield return null;
			time += Time.deltaTime;
		}
		this.runtimeMaterial.SetFloat("_ScreenDistAlphaLerp", Mathf.Lerp(from, to, 1f));
		this.image_FogOfWarEffect.color = color;
		yield break;
	}

	// Token: 0x04000939 RID: 2361
	[SerializeField]
	private RawImage image_FogOfWarEffect;

	// Token: 0x0400093A RID: 2362
	[SerializeField]
	private Material fogOfWar_Material;

	// Token: 0x0400093B RID: 2363
	private Material runtimeMaterial;

	// Token: 0x0400093C RID: 2364
	private Vector3 posVector;

	// Token: 0x0400093D RID: 2365
	private Vector3 delta;

	// Token: 0x0400093E RID: 2366
	private Vector3 camStartPos;
}
