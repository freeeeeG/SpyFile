using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000158 RID: 344
public class UI_ControlTip : AUISituational
{
	// Token: 0x060008FF RID: 2303 RVA: 0x000220E9 File Offset: 0x000202E9
	private void OnEnable()
	{
		EventMgr.Register<bool, UI_ControlTip.eControlTipType>(eGameEvents.UI_ToggleControlTip, new Action<bool, UI_ControlTip.eControlTipType>(this.OnToggleControlTip));
		EventMgr.Register<bool>(eGameEvents.UI_ToggleControlTipError_PathBlocked, new Action<bool>(this.OnToggleControlTipError_PathBlocked));
	}

	// Token: 0x06000900 RID: 2304 RVA: 0x00022121 File Offset: 0x00020321
	private void OnDisable()
	{
		EventMgr.Remove<bool, UI_ControlTip.eControlTipType>(eGameEvents.UI_ToggleControlTip, new Action<bool, UI_ControlTip.eControlTipType>(this.OnToggleControlTip));
		EventMgr.Remove<bool>(eGameEvents.UI_ToggleControlTipError_PathBlocked, new Action<bool>(this.OnToggleControlTipError_PathBlocked));
	}

	// Token: 0x06000901 RID: 2305 RVA: 0x00022159 File Offset: 0x00020359
	private void OnToggleControlTipError_PathBlocked(bool isOn)
	{
		this.node_Error_PathBlocked.gameObject.SetActive(isOn);
	}

	// Token: 0x06000902 RID: 2306 RVA: 0x0002216C File Offset: 0x0002036C
	private void OnToggleControlTip(bool isOn, UI_ControlTip.eControlTipType type)
	{
		if (isOn)
		{
			foreach (UI_ControlTip.TipTypeToNodePair tipTypeToNodePair in this.list_TipTypeToNodePair)
			{
				tipTypeToNodePair.node.SetActive(type == tipTypeToNodePair.type);
			}
			this.OnToggleControlTipError_PathBlocked(false);
		}
		base.Toggle(isOn);
	}

	// Token: 0x06000903 RID: 2307 RVA: 0x000221E0 File Offset: 0x000203E0
	private void Update()
	{
		if (base.IsUIActivated)
		{
			this.UpdatePosition();
		}
	}

	// Token: 0x06000904 RID: 2308 RVA: 0x000221F0 File Offset: 0x000203F0
	private void UpdatePosition()
	{
		base.transform.position = Singleton<CameraManager>.Instance.GetMouseWorldPos().WithZ(0f) + this.cursorOffset;
	}

	// Token: 0x0400071E RID: 1822
	[SerializeField]
	private Vector3 cursorOffset;

	// Token: 0x0400071F RID: 1823
	[SerializeField]
	private List<UI_ControlTip.TipTypeToNodePair> list_TipTypeToNodePair;

	// Token: 0x04000720 RID: 1824
	[SerializeField]
	private Transform node_Error_PathBlocked;

	// Token: 0x02000288 RID: 648
	public enum eControlTipType
	{
		// Token: 0x04000C07 RID: 3079
		NONE,
		// Token: 0x04000C08 RID: 3080
		PLACE_TOWER,
		// Token: 0x04000C09 RID: 3081
		PLACE_BLOCK
	}

	// Token: 0x02000289 RID: 649
	[Serializable]
	public class TipTypeToNodePair
	{
		// Token: 0x04000C0A RID: 3082
		public UI_ControlTip.eControlTipType type;

		// Token: 0x04000C0B RID: 3083
		public GameObject node;
	}
}
