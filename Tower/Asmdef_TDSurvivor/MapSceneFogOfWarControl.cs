using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000AC RID: 172
public class MapSceneFogOfWarControl : MonoBehaviour
{
	// Token: 0x0600038F RID: 911 RVA: 0x0000E431 File Offset: 0x0000C631
	private void Reset()
	{
		if (this.fogOfWar_Material == null)
		{
			this.fogOfWar_Material = base.GetComponent<Image>().material;
		}
	}

	// Token: 0x06000390 RID: 912 RVA: 0x0000E452 File Offset: 0x0000C652
	private void OnEnable()
	{
		EventMgr.Register<eSceneTimeType, float>(eGameEvents.StartSceneTimeChange, new Action<eSceneTimeType, float>(this.OnStartSceneTimeChange));
	}

	// Token: 0x06000391 RID: 913 RVA: 0x0000E46C File Offset: 0x0000C66C
	private void OnDisable()
	{
		EventMgr.Remove<eSceneTimeType, float>(eGameEvents.StartSceneTimeChange, new Action<eSceneTimeType, float>(this.OnStartSceneTimeChange));
	}

	// Token: 0x06000392 RID: 914 RVA: 0x0000E486 File Offset: 0x0000C686
	private void Awake()
	{
		this.runtimeMaterial = Object.Instantiate<Material>(this.fogOfWar_Material);
		base.GetComponent<Image>().material = this.runtimeMaterial;
		this.camStartPos = Singleton<CameraManager>.Instance.MainCamera.transform.position;
	}

	// Token: 0x06000393 RID: 915 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
	private void Update()
	{
		if (this.runtimeMaterial == null)
		{
			return;
		}
		this.delta = Singleton<CameraManager>.Instance.MainCamera.transform.InverseTransformPoint(this.camStartPos) * -1f;
		this.posVector.x = this.delta.x / (20f * this.uiCamera.aspect);
		this.posVector.y = this.delta.y / 20f;
		this.runtimeMaterial.SetVector("_WorldPosition", this.posVector);
	}

	// Token: 0x06000394 RID: 916 RVA: 0x0000E569 File Offset: 0x0000C769
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

	// Token: 0x06000395 RID: 917 RVA: 0x0000E5A4 File Offset: 0x0000C7A4
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

	// Token: 0x040003B7 RID: 951
	[SerializeField]
	private Camera uiCamera;

	// Token: 0x040003B8 RID: 952
	[SerializeField]
	private Image image_FogOfWarEffect;

	// Token: 0x040003B9 RID: 953
	[SerializeField]
	private Material fogOfWar_Material;

	// Token: 0x040003BA RID: 954
	private Material runtimeMaterial;

	// Token: 0x040003BB RID: 955
	private Vector3 posVector;

	// Token: 0x040003BC RID: 956
	private Vector3 delta;

	// Token: 0x040003BD RID: 957
	private Vector3 camStartPos;
}
