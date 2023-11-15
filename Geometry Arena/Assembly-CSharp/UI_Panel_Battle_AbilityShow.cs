using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000A5 RID: 165
public class UI_Panel_Battle_AbilityShow : UI_Panel_Main_IconList
{
	// Token: 0x060005B6 RID: 1462 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Awake()
	{
	}

	// Token: 0x060005B7 RID: 1463 RVA: 0x000051D0 File Offset: 0x000033D0
	private void Start()
	{
	}

	// Token: 0x060005B8 RID: 1464 RVA: 0x00020ABB File Offset: 0x0001ECBB
	private void Update()
	{
		if (Player.inst != null)
		{
			this.UpdateIcons();
		}
	}

	// Token: 0x060005B9 RID: 1465 RVA: 0x00020AD0 File Offset: 0x0001ECD0
	protected override int IconNum()
	{
		return 16;
	}

	// Token: 0x060005BA RID: 1466 RVA: 0x00020AD4 File Offset: 0x0001ECD4
	protected override bool IfAvailable(int ID)
	{
		return ID != 0 && ID != 5 && ID != 12;
	}

	// Token: 0x060005BB RID: 1467 RVA: 0x00020AE5 File Offset: 0x0001ECE5
	protected override void InitSingleIcon(GameObject obj, int ID)
	{
		obj.GetComponent<UI_Icon_AbilityBattle>().Init(ID);
	}

	// Token: 0x060005BC RID: 1468 RVA: 0x00020AF4 File Offset: 0x0001ECF4
	public void UpdateIcons()
	{
		if (this.inited)
		{
			using (List<GameObject>.Enumerator enumerator = this.listIcons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					gameObject.GetComponent<UI_Icon_AbilityBattle>().UpdateNum();
				}
				return;
			}
		}
		this.InitIcons(null);
		this.inited = true;
	}

	// Token: 0x040004A3 RID: 1187
	private bool inited;
}
