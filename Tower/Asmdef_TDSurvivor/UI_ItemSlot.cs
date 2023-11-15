using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class UI_ItemSlot : MonoBehaviour
{
	// Token: 0x06000973 RID: 2419 RVA: 0x00023ACE File Offset: 0x00021CCE
	private void OnEnable()
	{
		EventMgr.Register<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Register(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00023B00 File Offset: 0x00021D00
	private void OnDisable()
	{
		EventMgr.Remove<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Remove(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00023B32 File Offset: 0x00021D32
	private void OnRoundStart(int index, int totalRound)
	{
		if (index == 1)
		{
			this.animator.SetBool("isOn", true);
		}
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x00023B49 File Offset: 0x00021D49
	private void OnPlayerVictory()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00023B5C File Offset: 0x00021D5C
	private void Start()
	{
		int itemCardLimit = GameDataManager.instance.GameplayData.ItemCardLimit;
		for (int i = 0; i < this.list_LockedItemSlot.Count; i++)
		{
			this.list_LockedItemSlot[i].ToggleLockIcon(i >= itemCardLimit);
		}
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00023BA7 File Offset: 0x00021DA7
	public Transform GetTransformForCardSlot(int index)
	{
		return this.list_LockedItemSlot[index].transform;
	}

	// Token: 0x04000776 RID: 1910
	[SerializeField]
	private Animator animator;

	// Token: 0x04000777 RID: 1911
	[SerializeField]
	private List<Obj_UI_LockIcon> list_LockedItemSlot;
}
