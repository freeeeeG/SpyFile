using System;
using UnityEngine;

// Token: 0x020000CC RID: 204
public class Obj_RangeIndicator : MonoBehaviour
{
	// Token: 0x060004A6 RID: 1190 RVA: 0x00012D7D File Offset: 0x00010F7D
	private void Start()
	{
		this.node_RangeRingScale.gameObject.SetActive(false);
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x00012D90 File Offset: 0x00010F90
	private void OnEnable()
	{
		EventMgr.Register<bool>(eGameEvents.ToggleRangeIndicator, new Action<bool>(this.OnToggleRangeIndicator));
		EventMgr.Register<ABaseTower, float>(eGameEvents.SetupRangeIndicator, new Action<ABaseTower, float>(this.OnSetupRangeIndicator));
		EventMgr.Register<bool>(eGameEvents.LockRangeIndicator, new Action<bool>(this.OnLockRangeIndicator));
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x00012DE8 File Offset: 0x00010FE8
	private void OnDisable()
	{
		EventMgr.Remove<bool>(eGameEvents.ToggleRangeIndicator, new Action<bool>(this.OnToggleRangeIndicator));
		EventMgr.Remove<ABaseTower, float>(eGameEvents.SetupRangeIndicator, new Action<ABaseTower, float>(this.OnSetupRangeIndicator));
		EventMgr.Remove<bool>(eGameEvents.LockRangeIndicator, new Action<bool>(this.OnLockRangeIndicator));
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x00012E3D File Offset: 0x0001103D
	private void Update()
	{
		if (this.isActivated)
		{
			base.transform.position = this.targetTower.transform.position.WithY(0.1f);
		}
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x00012E6C File Offset: 0x0001106C
	private void OnLockRangeIndicator(bool isLocked)
	{
		this.isLocked = isLocked;
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x00012E75 File Offset: 0x00011075
	private void OnToggleRangeIndicator(bool isOn)
	{
		if (this.isLocked)
		{
			return;
		}
		this.isActivated = isOn;
		this.node_RangeRingScale.gameObject.SetActive(isOn);
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x00012E98 File Offset: 0x00011098
	private void OnSetupRangeIndicator(ABaseTower tower, float range)
	{
		if (this.isLocked)
		{
			return;
		}
		this.targetTower = tower;
		base.transform.position = tower.transform.position.WithY(0.1f);
		this.SetRingRange(range);
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x00012ED1 File Offset: 0x000110D1
	public void SetRingRange(float range)
	{
		this.node_RangeRingScale.localScale = Vector3.one * range * 2f;
	}

	// Token: 0x04000482 RID: 1154
	[SerializeField]
	private Transform node_RangeRingScale;

	// Token: 0x04000483 RID: 1155
	[SerializeField]
	private Transform node_DottedRing_Blue;

	// Token: 0x04000484 RID: 1156
	[SerializeField]
	private Spin spin;

	// Token: 0x04000485 RID: 1157
	[SerializeField]
	private ABaseTower targetTower;

	// Token: 0x04000486 RID: 1158
	private bool isActivated;

	// Token: 0x04000487 RID: 1159
	private bool isLocked;
}
