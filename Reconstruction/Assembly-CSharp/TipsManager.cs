using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x02000144 RID: 324
public class TipsManager : Singleton<TipsManager>
{
	// Token: 0x0600088D RID: 2189 RVA: 0x00017110 File Offset: 0x00015310
	private void Start()
	{
		this.m_TempTips.Initialize();
		this.m_TurretTips.Initialize();
		this.m_TrapTips.Initialize();
		this.m_EnemyTips.Initialize();
		this.m_BuyGroundTips.Initialize();
		this.m_BossTips.Initialize();
		this.m_TechInfoTips.Initialize();
		this.m_UnlockBonusTips.Initialize();
		this.m_MessageUI.Initialize();
		this.translators = base.GetComponentsInChildren<TextTranslator>().ToList<TextTranslator>();
	}

	// Token: 0x0600088E RID: 2190 RVA: 0x00017194 File Offset: 0x00015394
	public void UpdateTranslators()
	{
		foreach (TextTranslator textTranslator in this.translators)
		{
			textTranslator.UpdateTrans();
		}
	}

	// Token: 0x0600088F RID: 2191 RVA: 0x000171E4 File Offset: 0x000153E4
	public void SetCanvasCam()
	{
		this.m_Canvas.worldCamera = Camera.main;
	}

	// Token: 0x06000890 RID: 2192 RVA: 0x000171F8 File Offset: 0x000153F8
	private void SetCanvasPos(Transform tr, Vector2 pos)
	{
		Vector2 v;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(this.m_Canvas.transform as RectTransform, pos, this.m_Canvas.worldCamera, out v);
		tr.position = this.m_Canvas.transform.TransformPoint(v);
	}

	// Token: 0x06000891 RID: 2193 RVA: 0x00017245 File Offset: 0x00015445
	public void ShowBonusTips(GameLevelInfo info)
	{
		this.m_UnlockBonusTips.Show();
		this.m_UnlockBonusTips.SetBouns(info);
	}

	// Token: 0x06000892 RID: 2194 RVA: 0x0001725E File Offset: 0x0001545E
	public void ShowTurreTips(StrategyBase strategy, Vector2 pos, int showID)
	{
		this.HideTips();
		this.SetCanvasPos(this.m_TurretTips.transform, pos);
		this.m_TurretTips.ReadTurret(strategy, showID);
		this.m_TurretTips.Show();
	}

	// Token: 0x06000893 RID: 2195 RVA: 0x00017290 File Offset: 0x00015490
	public void ShowTrapTips(TrapAttribute att, Vector2 pos)
	{
		this.SetCanvasPos(this.m_TrapTips.transform, pos);
		this.m_TrapTips.ReadTrapAtt(att);
		this.m_TrapTips.Show();
	}

	// Token: 0x06000894 RID: 2196 RVA: 0x000172BB File Offset: 0x000154BB
	public void ShowTrapContentTips(TrapContent trap, Vector2 pos)
	{
		this.HideTips();
		this.SetCanvasPos(this.m_TrapTips.transform, pos);
		this.m_TrapTips.ReadTrap(trap, GameRes.SwitchTrapCost);
		this.m_TrapTips.Show();
	}

	// Token: 0x06000895 RID: 2197 RVA: 0x000172F1 File Offset: 0x000154F1
	public void ShowEnemyTips(List<EnemyAttribute> atts, Vector2 pos)
	{
		this.SetCanvasPos(this.m_EnemyTips.transform, pos);
		this.m_EnemyTips.ReadEnemyAtt(atts);
		this.m_EnemyTips.Show();
	}

	// Token: 0x06000896 RID: 2198 RVA: 0x0001731C File Offset: 0x0001551C
	public void ShowTechInfoTips(Technology tech, Vector2 pos, bool preview = false)
	{
		this.HideTips();
		this.SetCanvasPos(this.m_TechInfoTips.transform, pos);
		this.m_TechInfoTips.Show();
		this.m_TechInfoTips.SetInfo(tech, preview);
	}

	// Token: 0x06000897 RID: 2199 RVA: 0x0001734E File Offset: 0x0001554E
	public void ShowBuyGroundTips(Vector2 pos)
	{
		this.HideTips();
		this.SetCanvasPos(this.m_BuyGroundTips.transform, pos);
		this.m_BuyGroundTips.ReadInfo(GameRes.BuyGroundCost);
		this.m_BuyGroundTips.Show();
	}

	// Token: 0x06000898 RID: 2200 RVA: 0x00017383 File Offset: 0x00015583
	public void ShowTempTips(string text, Vector3 pos)
	{
		this.SetCanvasPos(this.m_TempTips.transform, pos);
		this.m_TempTips.Show();
		this.m_TempTips.SendText(text);
	}

	// Token: 0x06000899 RID: 2201 RVA: 0x000173B3 File Offset: 0x000155B3
	public void HideTempTips()
	{
		this.m_TempTips.Hide();
	}

	// Token: 0x0600089A RID: 2202 RVA: 0x000173C0 File Offset: 0x000155C0
	public void ShowBossTips(EnemyType bossType, int nextWave, Vector2 pos)
	{
		this.SetCanvasPos(this.m_BossTips.transform, pos);
		this.m_BossTips.Show();
		this.m_BossTips.ReadSequenceInfo(bossType, nextWave);
	}

	// Token: 0x0600089B RID: 2203 RVA: 0x000173EC File Offset: 0x000155EC
	public void HideBossTips()
	{
		this.m_BossTips.Hide();
	}

	// Token: 0x0600089C RID: 2204 RVA: 0x000173F9 File Offset: 0x000155F9
	public void HideTips()
	{
		this.m_TurretTips.CloseTips();
		this.m_TrapTips.CloseTips();
		this.m_EnemyTips.CloseTips();
		this.m_BuyGroundTips.CloseTips();
		this.m_TechInfoTips.CloseTips();
	}

	// Token: 0x0600089D RID: 2205 RVA: 0x00017432 File Offset: 0x00015632
	public void ShowMessage(string text)
	{
		this.m_MessageUI.SetText(text);
	}

	// Token: 0x04000476 RID: 1142
	[SerializeField]
	private TempTips m_TempTips;

	// Token: 0x04000477 RID: 1143
	[SerializeField]
	private TurretTips m_TurretTips;

	// Token: 0x04000478 RID: 1144
	[SerializeField]
	private TrapTips m_TrapTips;

	// Token: 0x04000479 RID: 1145
	[SerializeField]
	private EnemyInfoTips m_EnemyTips;

	// Token: 0x0400047A RID: 1146
	[SerializeField]
	private BuyGroundTips m_BuyGroundTips;

	// Token: 0x0400047B RID: 1147
	[SerializeField]
	private BossTips m_BossTips;

	// Token: 0x0400047C RID: 1148
	[SerializeField]
	private TechInfoTips m_TechInfoTips;

	// Token: 0x0400047D RID: 1149
	[SerializeField]
	private UnlockBonusTips m_UnlockBonusTips;

	// Token: 0x0400047E RID: 1150
	[SerializeField]
	private MessageUI m_MessageUI;

	// Token: 0x0400047F RID: 1151
	[SerializeField]
	private Canvas m_Canvas;

	// Token: 0x04000480 RID: 1152
	private List<TextTranslator> translators = new List<TextTranslator>();
}
