using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200017D RID: 381
public class UI_Obj_TowerBuffIcon : MonoBehaviour
{
	// Token: 0x06000A0D RID: 2573 RVA: 0x000259C0 File Offset: 0x00023BC0
	public void Setup(ABaseTower tower, ABaseBuffSettingData buff)
	{
		this.tower = tower;
		this.curBuff = buff;
		this.image_Icon.sprite = buff.GetCardIcon();
		this.text_BuffTimeLeft.text = "";
		if (tower.SettingData.TowerSizeType == eTowerSizeType._1x1)
		{
			this.offset = Vector3.up * 2f;
		}
		else
		{
			this.offset = Vector3.up * 3f;
		}
		tower.OnTowerDespawn = (Action<ABaseTower>)Delegate.Combine(tower.OnTowerDespawn, new Action<ABaseTower>(this.OnTowerDespawn));
		buff.OnBuffRemove = (Action)Delegate.Combine(buff.OnBuffRemove, new Action(this.OnBuffRemove));
		buff.OnTimeUpdate = (Action<int>)Delegate.Combine(buff.OnTimeUpdate, new Action<int>(this.OnTimerUpdate));
		this.Toggle(true);
		Debug.Log(string.Format("啟動buff圖示: 目標: {0}", this.curBuff.Tower), this.curBuff.Tower);
		if (buff.DurationType == eBuffDurationType.TIME)
		{
			this.text_BuffTimeLeft.text = Mathf.CeilToInt(buff.Duration_Time).ToString();
			return;
		}
		this.text_BuffTimeLeft.text = buff.Duration_Round.ToString();
	}

	// Token: 0x06000A0E RID: 2574 RVA: 0x00025B08 File Offset: 0x00023D08
	private void Update()
	{
		if (this.curBuff != null)
		{
			this.tower2Dpos = Singleton<CameraManager>.Instance.Calculate2DPosFrom3DPos(this.curBuff.Tower.transform.position + this.offset);
			this.tower2Dpos.z = 0f;
			base.transform.position = this.tower2Dpos;
			this.tower2Dpos = base.transform.localPosition;
			this.tower2Dpos.z = 0f;
			base.transform.localPosition = this.tower2Dpos;
		}
	}

	// Token: 0x06000A0F RID: 2575 RVA: 0x00025BA9 File Offset: 0x00023DA9
	private void OnTowerDespawn(ABaseTower tower)
	{
		this.Remove();
	}

	// Token: 0x06000A10 RID: 2576 RVA: 0x00025BB1 File Offset: 0x00023DB1
	private void OnBuffRemove()
	{
		this.Remove();
	}

	// Token: 0x06000A11 RID: 2577 RVA: 0x00025BB9 File Offset: 0x00023DB9
	private void OnTimerUpdate(int time)
	{
		this.text_BuffTimeLeft.text = time.ToString();
	}

	// Token: 0x06000A12 RID: 2578 RVA: 0x00025BD0 File Offset: 0x00023DD0
	public void Remove()
	{
		if (this.tower != null)
		{
			ABaseTower abaseTower = this.tower;
			abaseTower.OnTowerDespawn = (Action<ABaseTower>)Delegate.Remove(abaseTower.OnTowerDespawn, new Action<ABaseTower>(this.OnTowerDespawn));
			this.tower = null;
		}
		if (this.curBuff != null)
		{
			ABaseBuffSettingData abaseBuffSettingData = this.curBuff;
			abaseBuffSettingData.OnBuffRemove = (Action)Delegate.Remove(abaseBuffSettingData.OnBuffRemove, new Action(this.OnBuffRemove));
			ABaseBuffSettingData abaseBuffSettingData2 = this.curBuff;
			abaseBuffSettingData2.OnTimeUpdate = (Action<int>)Delegate.Remove(abaseBuffSettingData2.OnTimeUpdate, new Action<int>(this.OnTimerUpdate));
			this.curBuff = null;
		}
		this.Toggle(false);
		Singleton<PrefabManager>.Instance.DespawnPrefab(base.gameObject, 0.5f);
	}

	// Token: 0x06000A13 RID: 2579 RVA: 0x00025C98 File Offset: 0x00023E98
	private void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x040007C8 RID: 1992
	[SerializeField]
	private Animator animator;

	// Token: 0x040007C9 RID: 1993
	[SerializeField]
	private Image image_Icon;

	// Token: 0x040007CA RID: 1994
	[SerializeField]
	private TMP_Text text_BuffTimeLeft;

	// Token: 0x040007CB RID: 1995
	[SerializeField]
	private Vector3 localOffset = new Vector3(0f, 25f, 0f);

	// Token: 0x040007CC RID: 1996
	private ABaseBuffSettingData curBuff;

	// Token: 0x040007CD RID: 1997
	private Vector3 tower2Dpos;

	// Token: 0x040007CE RID: 1998
	private ABaseTower tower;

	// Token: 0x040007CF RID: 1999
	private Vector3 offset = Vector3.zero;
}
