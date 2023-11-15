using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200029B RID: 667
public class UnlockBonusTips : IUserInterface
{
	// Token: 0x0600105F RID: 4191 RVA: 0x0002D082 File Offset: 0x0002B282
	public override void Initialize()
	{
		base.Initialize();
		this.anim = base.GetComponent<Animator>();
	}

	// Token: 0x06001060 RID: 4192 RVA: 0x0002D096 File Offset: 0x0002B296
	public override void Show()
	{
		base.Show();
		this.anim.SetBool("isOpen", true);
	}

	// Token: 0x06001061 RID: 4193 RVA: 0x0002D0B0 File Offset: 0x0002B2B0
	public void SetBouns(GameLevelInfo info)
	{
		foreach (ItemSlot itemSlot in this.m_SlotList)
		{
			Object.Destroy(itemSlot.gameObject);
		}
		this.m_SlotList.Clear();
		for (int i = 0; i < 3; i++)
		{
			if (i < info.UnlockItems.Length)
			{
				ItemSlot itemSlot2 = null;
				AttType attType = info.UnlockItems[i].AttType;
				if (attType != AttType.Turret)
				{
					if (attType == AttType.Mark)
					{
						itemSlot2 = Object.Instantiate<TrapItemSlot>(this.trapSlotPrefab, this.parentObj);
						itemSlot2.SetContent(info.UnlockItems[i], this.m_ToggleGroup);
					}
				}
				else
				{
					itemSlot2 = Object.Instantiate<TurretItemSlot>(this.turretSlotPrefab, this.parentObj);
					itemSlot2.SetContent(info.UnlockItems[i], this.m_ToggleGroup);
				}
				this.m_SlotList.Add(itemSlot2);
			}
		}
	}

	// Token: 0x06001062 RID: 4194 RVA: 0x0002D1A0 File Offset: 0x0002B3A0
	public void CloseTips()
	{
		this.anim.SetBool("isOpen", false);
	}

	// Token: 0x040008B8 RID: 2232
	private Animator anim;

	// Token: 0x040008B9 RID: 2233
	[SerializeField]
	private ToggleGroup m_ToggleGroup;

	// Token: 0x040008BA RID: 2234
	[SerializeField]
	private Transform parentObj;

	// Token: 0x040008BB RID: 2235
	[SerializeField]
	private TurretItemSlot turretSlotPrefab;

	// Token: 0x040008BC RID: 2236
	[SerializeField]
	private TrapItemSlot trapSlotPrefab;

	// Token: 0x040008BD RID: 2237
	private List<ItemSlot> m_SlotList = new List<ItemSlot>();
}
