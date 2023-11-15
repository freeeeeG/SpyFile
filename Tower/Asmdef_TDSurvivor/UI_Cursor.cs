using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000159 RID: 345
public class UI_Cursor : MonoBehaviour
{
	// Token: 0x06000906 RID: 2310 RVA: 0x00022224 File Offset: 0x00020424
	private void Start()
	{
		Cursor.visible = false;
		if (this.uiCamera == null)
		{
			this.uiCamera = Singleton<CameraManager>.Instance.UICamera;
		}
	}

	// Token: 0x06000907 RID: 2311 RVA: 0x0002224C File Offset: 0x0002044C
	private void Update()
	{
		this.image_Cursor.transform.position = this.uiCamera.ScreenToWorldPoint(Input.mousePosition);
		this.image_Cursor.transform.localPosition = this.image_Cursor.transform.localPosition.WithZ(0f);
	}

	// Token: 0x04000721 RID: 1825
	[SerializeField]
	private Image image_Cursor;

	// Token: 0x04000722 RID: 1826
	[SerializeField]
	private Camera uiCamera;
}
