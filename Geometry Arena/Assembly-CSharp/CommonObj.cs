using System;
using UnityEngine;

// Token: 0x020000F3 RID: 243
public class CommonObj : MonoBehaviour
{
	// Token: 0x170000F9 RID: 249
	// (get) Token: 0x060008AE RID: 2222 RVA: 0x000326A7 File Offset: 0x000308A7
	// (set) Token: 0x060008AF RID: 2223 RVA: 0x000326BE File Offset: 0x000308BE
	public float TransScale
	{
		get
		{
			return base.gameObject.transform.lossyScale.y;
		}
		set
		{
			base.gameObject.transform.localScale = Vector2.one * value;
		}
	}

	// Token: 0x04000722 RID: 1826
	[SerializeField]
	public EnumShapeType shapeType = EnumShapeType.UNINITED;

	// Token: 0x04000723 RID: 1827
	[SerializeField]
	public EnumObjType objType = EnumObjType.UNINITED;

	// Token: 0x04000724 RID: 1828
	public Color mainColor;
}
