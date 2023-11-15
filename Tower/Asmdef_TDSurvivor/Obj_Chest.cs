using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

// Token: 0x020000C2 RID: 194
public class Obj_Chest : MonoBehaviour
{
	// Token: 0x06000479 RID: 1145 RVA: 0x00012124 File Offset: 0x00010324
	private void OnMouseDown()
	{
		if (LeanTouch.PointOverGui(Input.mousePosition))
		{
			return;
		}
		if (Singleton<GameStateController>.Instance.IsCurrentState(eGameState.NORMAL_MODE))
		{
			this.OpenChest();
		}
	}

	// Token: 0x0600047A RID: 1146 RVA: 0x0001214C File Offset: 0x0001034C
	public void Initialize()
	{
		base.transform.rotation = ((Random.Range(0, 2) == 0) ? Quaternion.Euler(0f, -90f, 0f) : Quaternion.Euler(0f, -180f, 0f));
		this.animator.SetBool("isOn", true);
		this.isClickable = true;
	}

	// Token: 0x0600047B RID: 1147 RVA: 0x000121AF File Offset: 0x000103AF
	public void OpenChest()
	{
		if (!this.isClickable)
		{
			return;
		}
		base.StartCoroutine(this.CR_OpenChest(this.list_Reward));
	}

	// Token: 0x0600047C RID: 1148 RVA: 0x000121CD File Offset: 0x000103CD
	private IEnumerator CR_OpenChest(List<DiscoverRewardData> list_Data)
	{
		this.isClickable = false;
		this.animator.SetTrigger("open");
		yield return new WaitForSeconds(0.5f);
		EventMgr.SendEvent(eGameEvents.RequestDiscoverReward);
		yield return new WaitForSeconds(2f);
		this.animator.SetBool("isOn", false);
		yield return new WaitForSeconds(0.5f);
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04000459 RID: 1113
	[SerializeField]
	private Animator animator;

	// Token: 0x0400045A RID: 1114
	private List<DiscoverRewardData> list_Reward;

	// Token: 0x0400045B RID: 1115
	private bool isClickable;
}
