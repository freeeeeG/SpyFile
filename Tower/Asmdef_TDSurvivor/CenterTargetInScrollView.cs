using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000132 RID: 306
[RequireComponent(typeof(ScrollRect))]
public class CenterTargetInScrollView : MonoBehaviour
{
	// Token: 0x060007F4 RID: 2036 RVA: 0x0001E5D0 File Offset: 0x0001C7D0
	private void Start()
	{
		this.mat_Map = this.mapImage.material;
		this.startScreenPos = RectTransformUtility.WorldToScreenPoint(this.canvas.worldCamera, this.scrollRect.content.position);
		this.startScreenPos.x = this.startScreenPos.x / (float)Screen.width;
		this.startScreenPos.y = this.startScreenPos.y / (float)Screen.height;
	}

	// Token: 0x060007F5 RID: 2037 RVA: 0x0001E644 File Offset: 0x0001C844
	private void Update()
	{
		this.curScreenPos = RectTransformUtility.WorldToScreenPoint(this.canvas.worldCamera, this.scrollRect.content.position);
		this.curScreenPos.x = this.curScreenPos.x / (float)Screen.width;
		this.curScreenPos.y = this.curScreenPos.y / (float)Screen.height;
		this.mat_Map.SetVector("_MapOffset", -1f * (this.curScreenPos - this.startScreenPos));
	}

	// Token: 0x060007F6 RID: 2038 RVA: 0x0001E6D8 File Offset: 0x0001C8D8
	public void CenterToTarget(RectTransform target, Vector2 desiredScreenPosition)
	{
		if (this.canvas == null)
		{
			Debug.LogError("Canvas is null", base.gameObject);
			return;
		}
		Vector2 vector = -1f * target.anchoredPosition;
		vector.y = this.scrollRect.content.anchoredPosition.y;
		vector.x += (float)Screen.width / 2f;
		vector.x = Mathf.Clamp(vector.x, this.scrollRect.content.sizeDelta.x * -1f + (float)Screen.width, 0f);
		vector.y = target.anchoredPosition.y * -1f;
		this.scrollRect.content.DOAnchorPos(vector, 0.5f, false).SetEase(Ease.OutCubic);
	}

	// Token: 0x04000672 RID: 1650
	[SerializeField]
	private ScrollRect scrollRect;

	// Token: 0x04000673 RID: 1651
	[SerializeField]
	private Canvas canvas;

	// Token: 0x04000674 RID: 1652
	[SerializeField]
	private RawImage mapImage;

	// Token: 0x04000675 RID: 1653
	private Material mat_Map;

	// Token: 0x04000676 RID: 1654
	private Vector3 startScreenPos;

	// Token: 0x04000677 RID: 1655
	private Vector3 curScreenPos;
}
