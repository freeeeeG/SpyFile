using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B4E RID: 2894
public class KScrollbarVisibility : MonoBehaviour
{
	// Token: 0x06005954 RID: 22868 RVA: 0x0020AD4E File Offset: 0x00208F4E
	private void Start()
	{
		this.Update();
	}

	// Token: 0x06005955 RID: 22869 RVA: 0x0020AD58 File Offset: 0x00208F58
	private void Update()
	{
		if (this.content.content == null)
		{
			return;
		}
		bool flag = false;
		Vector2 vector = new Vector2(this.parent.rect.width, this.parent.rect.height);
		Vector2 sizeDelta = this.content.content.GetComponent<RectTransform>().sizeDelta;
		if ((sizeDelta.x >= vector.x && this.checkWidth) || (sizeDelta.y >= vector.y && this.checkHeight))
		{
			flag = true;
		}
		if (this.scrollbar.gameObject.activeSelf != flag)
		{
			this.scrollbar.gameObject.SetActive(flag);
			if (this.others != null)
			{
				GameObject[] array = this.others;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].SetActive(flag);
				}
			}
		}
	}

	// Token: 0x04003C5E RID: 15454
	[SerializeField]
	private ScrollRect content;

	// Token: 0x04003C5F RID: 15455
	[SerializeField]
	private RectTransform parent;

	// Token: 0x04003C60 RID: 15456
	[SerializeField]
	private bool checkWidth = true;

	// Token: 0x04003C61 RID: 15457
	[SerializeField]
	private bool checkHeight = true;

	// Token: 0x04003C62 RID: 15458
	[SerializeField]
	private Scrollbar scrollbar;

	// Token: 0x04003C63 RID: 15459
	[SerializeField]
	private GameObject[] others;
}
