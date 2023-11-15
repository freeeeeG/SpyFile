using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000AF6 RID: 2806
[ExecuteInEditMode]
public class ProgressGaugeUI : BaseProgressBarUI
{
	// Token: 0x060038CE RID: 14542 RVA: 0x0010C080 File Offset: 0x0010A480
	protected override void UpdateFill()
	{
		Image image = this.m_images[1];
		if (image != null)
		{
			RectTransform rectTransform = image.rectTransform;
			rectTransform.localRotation = this.ProgressToRotation(base.Value);
		}
	}

	// Token: 0x060038CF RID: 14543 RVA: 0x0010C0BC File Offset: 0x0010A4BC
	private Quaternion ProgressToRotation(float _progress)
	{
		float z = MathUtils.Remap(_progress, 0f, 1f, this.m_emptyAngle, this.m_fullAngle);
		return Quaternion.Euler(0f, 0f, z);
	}

	// Token: 0x060038D0 RID: 14544 RVA: 0x0010C0F8 File Offset: 0x0010A4F8
	protected override Image CreateFillImage(GameObject _rect)
	{
		RectTransform component = _rect.GetComponent<RectTransform>();
		component.pivot = new Vector2(0.5f, 0f);
		component.sizeDelta = new Vector2(0f, 0f);
		component.anchorMin = new Vector2(0.4f, 0.15f);
		component.anchorMax = new Vector2(0.6f, 0.85f);
		return _rect.GetComponent<Image>();
	}

	// Token: 0x060038D1 RID: 14545 RVA: 0x0010C168 File Offset: 0x0010A568
	protected override void PositionNotch(Image _notch, float _position)
	{
		RectTransform rectTransform = _notch.rectTransform;
		rectTransform.pivot = new Vector2(0.5f, 0f);
		rectTransform.sizeDelta = new Vector2(5f, 0f);
		rectTransform.anchorMin = new Vector2(_position, Mathf.Sin(_position * 3.1415927f));
		rectTransform.anchorMax = new Vector2(_position, Mathf.Sin(_position * 3.1415927f));
		rectTransform.localRotation = this.ProgressToRotation(_position);
	}

	// Token: 0x04002D7A RID: 11642
	[SerializeField]
	private float m_emptyAngle = 90f;

	// Token: 0x04002D7B RID: 11643
	[SerializeField]
	private float m_fullAngle = -90f;
}
