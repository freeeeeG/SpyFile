using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000041 RID: 65
public class BuffEffectHandler : MonoBehaviour
{
	// Token: 0x06000137 RID: 311 RVA: 0x000058B6 File Offset: 0x00003AB6
	private void Awake()
	{
		this.dic_BuffToScript = new Dictionary<eItemType, Type>
		{
			{
				eItemType._3001_DAMAGE_UP,
				typeof(PercentageDamageBuff)
			},
			{
				eItemType._3003_SHOOT_SPEED_UP,
				typeof(PercentageShootSpeedBuff)
			}
		};
	}

	// Token: 0x06000138 RID: 312 RVA: 0x000058F0 File Offset: 0x00003AF0
	private void OnEnable()
	{
		EventMgr.Register<ABaseTower, eItemType>(eGameEvents.RequestGiveBuffToTower, new Action<ABaseTower, eItemType>(this.OnRequestGiveBuff));
		EventMgr.Register<ABaseBuffSettingData, Action>(eGameEvents.RequestStartBuffSelection, new Action<ABaseBuffSettingData, Action>(this.OnRequestStartBuffSelection));
		EventMgr.Register(eGameEvents.ConfirmBuffSelection, new Action(this.OnConfirmBuffSelection));
		EventMgr.Register(eGameEvents.CancelBuffSelection, new Action(this.OnCancelBuffSelection));
	}

	// Token: 0x06000139 RID: 313 RVA: 0x00005960 File Offset: 0x00003B60
	private void OnDisable()
	{
		EventMgr.Remove<ABaseTower, eItemType>(eGameEvents.RequestGiveBuffToTower, new Action<ABaseTower, eItemType>(this.OnRequestGiveBuff));
		EventMgr.Remove<ABaseBuffSettingData, Action>(eGameEvents.RequestStartBuffSelection, new Action<ABaseBuffSettingData, Action>(this.OnRequestStartBuffSelection));
		EventMgr.Remove(eGameEvents.ConfirmBuffSelection, new Action(this.OnConfirmBuffSelection));
		EventMgr.Remove(eGameEvents.CancelBuffSelection, new Action(this.OnCancelBuffSelection));
	}

	// Token: 0x0600013A RID: 314 RVA: 0x000059D0 File Offset: 0x00003BD0
	private void OnRequestGiveBuff(ABaseTower tower, eItemType type)
	{
		ABaseBuffSettingData buffData = Singleton<ResourceManager>.Instance.GetItemDataByType(type) as ABaseBuffSettingData;
		this.AddBuffToTower(buffData, tower);
	}

	// Token: 0x0600013B RID: 315 RVA: 0x000059F6 File Offset: 0x00003BF6
	private void OnRequestStartBuffSelection(ABaseBuffSettingData data, Action callback)
	{
		this.curData = data;
		this.placementSuccessCallback = callback;
		this.isEffectOn = true;
	}

	// Token: 0x0600013C RID: 316 RVA: 0x00005A10 File Offset: 0x00003C10
	private void OnConfirmBuffSelection()
	{
		if (this.currentPointingTower == null)
		{
			this.OnCancelBuffSelection();
			return;
		}
		if (this.currentPointingTower != null)
		{
			this.AddBuffToTower(this.curData, this.currentPointingTower);
			this.placementSuccessCallback();
		}
		Vector3 zero = Vector3.zero;
		if (this.currentPointingTower.SettingData.TowerSizeType == eTowerSizeType._1x1)
		{
			Vector3.up * 2f;
		}
		else
		{
			Vector3.up * 3f;
		}
		EventMgr.SendEvent<Vector3, string>(eGameEvents.UI_ShowBuffApplyText, this.currentPointingTower.transform.position + Vector3.up * 3f, this.curData.GetLocNameString(true));
		EventMgr.SendEvent<bool>(eGameEvents.UI_TogglePlacementPointerArrow, false);
		this.isEffectOn = false;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x00005AED File Offset: 0x00003CED
	private void OnCancelBuffSelection()
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_TogglePlacementPointerArrow, false);
		this.isEffectOn = false;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x00005B08 File Offset: 0x00003D08
	private void AddBuffToTower(ABaseBuffSettingData buffData, ABaseTower tower)
	{
		ABaseBuffSettingData buff = Object.Instantiate<ABaseBuffSettingData>(buffData);
		tower.GetComponent<TowerBuffModule>().ApplyBuff(buff);
		Object.Instantiate<GameObject>(Resources.Load<GameObject>("VFX/VFX_ApplyBuff")).transform.position = tower.transform.position;
		SoundManager.PlaySound("UI", "ApplyBuffCard", -1f, -1f, -1f);
	}

	// Token: 0x0600013F RID: 319 RVA: 0x00005B6B File Offset: 0x00003D6B
	private void Update()
	{
		if (!this.isEffectOn)
		{
			return;
		}
		this.IsMouseOnTower();
	}

	// Token: 0x06000140 RID: 320 RVA: 0x00005B80 File Offset: 0x00003D80
	private bool IsMouseOnTower()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(Singleton<CameraManager>.Instance.MainCamera.ScreenPointToRay(Input.mousePosition), out raycastHit))
		{
			ABaseTower component = raycastHit.collider.gameObject.GetComponent<ABaseTower>();
			if (component != null)
			{
				this.currentPointingTower = component;
				return true;
			}
		}
		this.currentPointingTower = null;
		return false;
	}

	// Token: 0x040000D7 RID: 215
	[SerializeField]
	private LineRenderer lineRenderer_BuffEffect;

	// Token: 0x040000D8 RID: 216
	protected Dictionary<eItemType, Type> dic_BuffToScript;

	// Token: 0x040000D9 RID: 217
	private ABaseBuffSettingData curData;

	// Token: 0x040000DA RID: 218
	private Action placementSuccessCallback;

	// Token: 0x040000DB RID: 219
	private bool isEffectOn;

	// Token: 0x040000DC RID: 220
	private ABaseTower currentPointingTower;
}
