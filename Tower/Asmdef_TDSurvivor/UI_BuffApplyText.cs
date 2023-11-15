using System;
using System.Collections;
using Febucci.UI;
using UnityEngine;

// Token: 0x02000147 RID: 327
public class UI_BuffApplyText : MonoBehaviour
{
	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x0600088E RID: 2190 RVA: 0x00020CAF File Offset: 0x0001EEAF
	public float Width
	{
		get
		{
			return this.width;
		}
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x00020CB8 File Offset: 0x0001EEB8
	public void Trigger(Vector3 worldPos, string content)
	{
		this.worldPosition = worldPos;
		this.curCameraPos = Singleton<CameraManager>.Instance.MainCamera.transform.position;
		this.text_Content.ShowText(content + "!");
		base.StartCoroutine(this.EffectProc());
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x00020D0C File Offset: 0x0001EF0C
	private void Update()
	{
		if (this.curCameraPos != Singleton<CameraManager>.Instance.MainCamera.transform.position)
		{
			Vector3 position = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(this.worldPosition);
			base.transform.position = position;
		}
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00020D57 File Offset: 0x0001EF57
	private IEnumerator EffectProc()
	{
		while (this.text_Content.isShowingText)
		{
			yield return null;
		}
		yield return new WaitForSeconds(this.waitTimeAfterShowText);
		this.text_Content.StartDisappearingText();
		yield return new WaitForSeconds(0.5f);
		while (this.text_Content.isHidingText)
		{
			yield return null;
		}
		Singleton<PrefabManager>.Instance.DespawnPrefab(base.gameObject, 0f);
		yield break;
	}

	// Token: 0x040006EC RID: 1772
	[SerializeField]
	private TypewriterByCharacter text_Content;

	// Token: 0x040006ED RID: 1773
	[SerializeField]
	private float waitTimeAfterShowText;

	// Token: 0x040006EE RID: 1774
	[SerializeField]
	private float width = 1f;

	// Token: 0x040006EF RID: 1775
	private Vector3 worldPosition;

	// Token: 0x040006F0 RID: 1776
	private Vector3 curCameraPos;
}
