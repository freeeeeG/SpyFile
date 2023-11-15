using System;
using UnityEngine;

// Token: 0x02000137 RID: 311
public class TiltUIWithMousePosition : MonoBehaviour
{
	// Token: 0x06000805 RID: 2053 RVA: 0x0001E9F0 File Offset: 0x0001CBF0
	private void Awake()
	{
		this.startEulerAngles = base.transform.localRotation.eulerAngles;
	}

	// Token: 0x06000806 RID: 2054 RVA: 0x0001EA18 File Offset: 0x0001CC18
	private void Update()
	{
		if (this.isEnabled)
		{
			Vector3 mousePosition = Input.mousePosition;
			float x = (Mathf.Clamp01(mousePosition.y / (float)Screen.height) - 0.5f) * 2f * this.maxRotationAngles.x;
			float num = (Mathf.Clamp01(mousePosition.x / (float)Screen.width) - 0.5f) * 2f * this.maxRotationAngles.y;
			base.transform.localEulerAngles = this.startEulerAngles + new Vector3(x, -num, 0f);
		}
	}

	// Token: 0x06000807 RID: 2055 RVA: 0x0001EAAA File Offset: 0x0001CCAA
	public void Toggle(bool isOn)
	{
		this.isEnabled = isOn;
		if (!this.isEnabled)
		{
			base.transform.localEulerAngles = this.startEulerAngles;
		}
	}

	// Token: 0x04000685 RID: 1669
	public Vector3 maxRotationAngles;

	// Token: 0x04000686 RID: 1670
	[SerializeField]
	private bool isEnabled = true;

	// Token: 0x04000687 RID: 1671
	private Vector3 startEulerAngles = Vector3.zero;
}
