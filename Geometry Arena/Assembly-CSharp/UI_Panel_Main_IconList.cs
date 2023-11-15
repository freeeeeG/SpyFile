using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public abstract class UI_Panel_Main_IconList : MonoBehaviour
{
	// Token: 0x060006B3 RID: 1715 RVA: 0x00025DBD File Offset: 0x00023FBD
	private void Awake()
	{
		this.listIcons = new List<GameObject>();
	}

	// Token: 0x060006B4 RID: 1716 RVA: 0x00025DCC File Offset: 0x00023FCC
	public virtual void InitIcons(Transform transformParent = null)
	{
		UI_ToolTip.inst.TryClose();
		foreach (GameObject gameObject in this.listIcons)
		{
			Object.Destroy(gameObject.gameObject);
		}
		this.listIcons = new List<GameObject>();
		int num = 0;
		for (int i = 0; i < this.IconNum(); i++)
		{
			if (this.IfAvailable(i))
			{
				int num2 = num % this.columnNum;
				int num3 = num / this.columnNum;
				Vector2 b = new Vector2(this.distX * (float)num2, this.distY * (float)num3);
				Vector2 v = this.startPos + b;
				GameObject gameObject2 = Object.Instantiate<GameObject>(this.prefab_Icon);
				this.listIcons.Add(gameObject2.gameObject);
				gameObject2.transform.SetParent((transformParent == null) ? base.transform : transformParent);
				gameObject2.transform.localPosition = v;
				this.InitSingleIcon(gameObject2, i);
				num++;
			}
		}
	}

	// Token: 0x060006B5 RID: 1717 RVA: 0x00025EF4 File Offset: 0x000240F4
	public void ClearAll()
	{
		foreach (GameObject gameObject in this.listIcons)
		{
			Object.Destroy(gameObject.gameObject);
		}
		this.listIcons = new List<GameObject>();
	}

	// Token: 0x060006B6 RID: 1718
	protected abstract bool IfAvailable(int ID);

	// Token: 0x060006B7 RID: 1719
	protected abstract void InitSingleIcon(GameObject obj, int ID);

	// Token: 0x060006B8 RID: 1720
	protected abstract int IconNum();

	// Token: 0x0400058C RID: 1420
	[SerializeField]
	protected GameObject prefab_Icon;

	// Token: 0x0400058D RID: 1421
	[SerializeField]
	protected Vector2 startPos = Vector2.zero;

	// Token: 0x0400058E RID: 1422
	[SerializeField]
	protected int columnNum = 5;

	// Token: 0x0400058F RID: 1423
	[SerializeField]
	protected float distX = 50f;

	// Token: 0x04000590 RID: 1424
	[SerializeField]
	protected float distY = -50f;

	// Token: 0x04000591 RID: 1425
	[SerializeField]
	protected float iconScale = 1f;

	// Token: 0x04000592 RID: 1426
	[SerializeField]
	protected List<GameObject> listIcons = new List<GameObject>();
}
