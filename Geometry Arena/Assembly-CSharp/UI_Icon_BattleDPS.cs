using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000085 RID: 133
public class UI_Icon_BattleDPS : UI_Text_SimpleTooltip
{
	// Token: 0x060004B8 RID: 1208 RVA: 0x0001CBCC File Offset: 0x0001ADCC
	private void Awake()
	{
		UI_Icon_BattleDPS.inst = this;
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x000040FB File Offset: 0x000022FB
	protected override bool ifUnlocked()
	{
		return true;
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x0001CBD4 File Offset: 0x0001ADD4
	private void Update()
	{
		this.ApplySetting();
		this.time += (double)Time.deltaTime;
		this.textName.text = LanguageText.Inst.toolTip_TipStringsHead[22];
		this.updateTimeLeft -= Time.deltaTime;
		if (this.updateTimeLeft <= 0f)
		{
			this.updateTimeLeft = this.updateDelta;
			this.textNumber.text = this.GetDouble_DPS().ToSmartString();
		}
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0001CC53 File Offset: 0x0001AE53
	public void ClearData()
	{
		this.time = 0.0;
		this.damage = 0.0;
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x0001CC73 File Offset: 0x0001AE73
	public void Damage_Add(double num)
	{
		this.damage += num;
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x0001CC84 File Offset: 0x0001AE84
	public double GetDouble_DPS()
	{
		if (this.time == 0.0)
		{
			return 0.0;
		}
		if (this.time < 0.0)
		{
			Debug.LogError("Error_DPS_Time<0!");
			return 0.0;
		}
		return this.damage / this.time;
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0001CCDE File Offset: 0x0001AEDE
	private void ApplySetting()
	{
		if (Setting.Inst != null)
		{
			this.textName.gameObject.SetActive(Setting.Inst.Option_DPS);
			this.textNumber.gameObject.SetActive(Setting.Inst.Option_DPS);
		}
	}

	// Token: 0x040003FF RID: 1023
	public static UI_Icon_BattleDPS inst;

	// Token: 0x04000400 RID: 1024
	[SerializeField]
	private double damage;

	// Token: 0x04000401 RID: 1025
	[SerializeField]
	private double time;

	// Token: 0x04000402 RID: 1026
	[SerializeField]
	private Text textName;

	// Token: 0x04000403 RID: 1027
	[SerializeField]
	private Text textNumber;

	// Token: 0x04000404 RID: 1028
	[SerializeField]
	private float updateDelta = 0.5f;

	// Token: 0x04000405 RID: 1029
	[SerializeField]
	private float updateTimeLeft = 0.5f;
}
