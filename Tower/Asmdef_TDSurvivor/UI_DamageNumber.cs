using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x0200015B RID: 347
public class UI_DamageNumber : MonoBehaviour
{
	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06000915 RID: 2325 RVA: 0x0002276C File Offset: 0x0002096C
	public float Width
	{
		get
		{
			return this.width;
		}
	}

	// Token: 0x06000916 RID: 2326 RVA: 0x00022774 File Offset: 0x00020974
	public void Trigger(Vector3 worldPos, int value, bool isCrit, eDamageType damageType)
	{
		this.worldPosition = worldPos;
		this.curCameraPos = Singleton<CameraManager>.Instance.MainCamera.transform.position;
		this.text_Value.enabled = false;
		this.text_Value.enabled = true;
		this.text_Value.text = value.ToString();
		this.text_Value.color = damageType.GetColor();
		this.animator.SetBool("isCrit", isCrit);
		this.isCrit = isCrit;
		if (!isCrit && value < this.shrinkSizeDamageThreshold)
		{
			float t = (float)value / (float)this.shrinkSizeDamageThreshold;
			base.transform.localScale = Vector3.one * Mathf.Lerp(this.shrinkSizeScale, 1f, t);
		}
		else
		{
			base.transform.localScale = Vector3.one;
		}
		base.StartCoroutine(this.EffectProc());
	}

	// Token: 0x06000917 RID: 2327 RVA: 0x00022854 File Offset: 0x00020A54
	private void Update()
	{
		if (!this.isCrit && this.curCameraPos != Singleton<CameraManager>.Instance.MainCamera.transform.position)
		{
			Vector3 position = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(this.worldPosition);
			base.transform.position = position;
		}
	}

	// Token: 0x06000918 RID: 2328 RVA: 0x000228A7 File Offset: 0x00020AA7
	private IEnumerator EffectProc()
	{
		this.animator.SetBool("isOn", true);
		this.node_RandomOffset.transform.localPosition = Vector3.zero;
		float time = 0f;
		float rndOffset = this.isCrit ? 0f : Random.Range(-1f * this.randomOffsetWidth, this.randomOffsetWidth);
		while (time <= this.duration)
		{
			float t = time / this.duration;
			this.node_RandomOffset.transform.localPosition = new Vector3(Mathf.Lerp(0f, rndOffset, t), 0f, 0f);
			yield return null;
			time += Time.deltaTime;
		}
		this.animator.SetBool("isOn", false);
		yield return new WaitForSeconds(0.5f);
		Singleton<PrefabManager>.Instance.DespawnPrefab(base.gameObject, 0f);
		yield break;
	}

	// Token: 0x04000731 RID: 1841
	[SerializeField]
	private Animator animator;

	// Token: 0x04000732 RID: 1842
	[SerializeField]
	private TMP_Text text_Value;

	// Token: 0x04000733 RID: 1843
	[SerializeField]
	private Transform node_RandomOffset;

	// Token: 0x04000734 RID: 1844
	[SerializeField]
	private float duration;

	// Token: 0x04000735 RID: 1845
	[SerializeField]
	private float width = 1f;

	// Token: 0x04000736 RID: 1846
	[SerializeField]
	private float randomOffsetWidth = 1f;

	// Token: 0x04000737 RID: 1847
	private Vector3 worldPosition;

	// Token: 0x04000738 RID: 1848
	private Vector3 curCameraPos;

	// Token: 0x04000739 RID: 1849
	private bool isCrit;

	// Token: 0x0400073A RID: 1850
	private readonly int shrinkSizeDamageThreshold = 10;

	// Token: 0x0400073B RID: 1851
	private readonly float shrinkSizeScale = 0.5f;
}
