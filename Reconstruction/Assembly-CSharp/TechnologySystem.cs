using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001C2 RID: 450
public class TechnologySystem : IGameSystem
{
	// Token: 0x06000B7A RID: 2938 RVA: 0x0001D777 File Offset: 0x0001B977
	public override void Initialize()
	{
		base.Initialize();
		TechnologySystem.GetTechnologies = new List<Technology>();
		TechnologySystem.GetGlobalSkillTechs = new List<Technology>();
		TechnologySystem.PickingTechs = new List<Technology>();
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x0001D79D File Offset: 0x0001B99D
	public void AddTech(Technology tech)
	{
		TechnologySystem.GetTechnologies.Add(tech);
		this.m_TechPanel.AddTech(tech);
		tech.OnGet();
		TechnologyFactory.BattleTechs.Remove(tech);
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x0001D7C8 File Offset: 0x0001B9C8
	public void TechBtnClick()
	{
		if (TechnologySystem.GetTechnologies.Count <= 0)
		{
			Singleton<TipsManager>.Instance.ShowMessage(GameMultiLang.GetTraduction("NOTECH"));
			return;
		}
		if (!this.m_TechPanel.IsVisible())
		{
			this.m_TechPanel.Show();
			return;
		}
		this.m_TechPanel.Hide();
	}

	// Token: 0x06000B7D RID: 2941 RVA: 0x0001D81C File Offset: 0x0001BA1C
	public void OnTurnEnd()
	{
		foreach (Technology technology in TechnologySystem.GetTechnologies)
		{
			technology.OnTurnEnd();
		}
	}

	// Token: 0x06000B7E RID: 2942 RVA: 0x0001D86C File Offset: 0x0001BA6C
	public void OnTurnStart()
	{
		foreach (Technology technology in TechnologySystem.GetTechnologies)
		{
			technology.OnTurnStart();
		}
	}

	// Token: 0x06000B7F RID: 2943 RVA: 0x0001D8BC File Offset: 0x0001BABC
	public static void OnRefactor(StrategyBase strategy)
	{
		foreach (Technology technology in TechnologySystem.GetTechnologies)
		{
			technology.OnRefactor(strategy);
		}
	}

	// Token: 0x06000B80 RID: 2944 RVA: 0x0001D90C File Offset: 0x0001BB0C
	public static void OnEquip(StrategyBase strategy)
	{
		foreach (Technology technology in TechnologySystem.GetTechnologies)
		{
			technology.OnEquip(strategy);
		}
	}

	// Token: 0x06000B81 RID: 2945 RVA: 0x0001D95C File Offset: 0x0001BB5C
	public void ConfirmTechSelect()
	{
		TechnologySystem.PickingTechs.Clear();
		TechSelectPanel.SelectingTech = null;
		GameRes.RefreashTechCost = 100;
		GameRes.FreeRefreshTech = 1;
	}

	// Token: 0x06000B82 RID: 2946 RVA: 0x0001D97C File Offset: 0x0001BB7C
	public void LoadSaveGame()
	{
		foreach (TechnologyStruct technologyStruct in Singleton<LevelManager>.Instance.LastGameSave.SaveTechnologies)
		{
			Technology tech = TechnologyFactory.GetTech(technologyStruct.TechName);
			tech.IsAbnormal = technologyStruct.IsAbnormal;
			tech.CanAbnormal = technologyStruct.CanAbnormal;
			tech.SaveValue = technologyStruct.TechSaveValue;
			this.AddTech(tech);
		}
		if (Singleton<LevelManager>.Instance.LastGameSave.SavePickingTechs != null)
		{
			foreach (TechnologyStruct technologyStruct2 in Singleton<LevelManager>.Instance.LastGameSave.SavePickingTechs)
			{
				Technology tech2 = TechnologyFactory.GetTech(technologyStruct2.TechName);
				tech2.IsAbnormal = technologyStruct2.IsAbnormal;
				tech2.SaveValue = technologyStruct2.TechSaveValue;
				tech2.CanAbnormal = technologyStruct2.CanAbnormal;
				TechnologySystem.PickingTechs.Add(tech2);
			}
		}
	}

	// Token: 0x06000B83 RID: 2947 RVA: 0x0001DAA0 File Offset: 0x0001BCA0
	public void RemoveTech(Technology tech)
	{
		TechnologySystem.GetTechnologies.Remove(tech);
		this.m_TechPanel.RemoveTech(tech);
		TechnologyFactory.BattleTechs.Add(tech);
	}

	// Token: 0x040005AB RID: 1451
	public static List<Technology> GetTechnologies;

	// Token: 0x040005AC RID: 1452
	public static List<Technology> GetGlobalSkillTechs;

	// Token: 0x040005AD RID: 1453
	public static List<Technology> PickingTechs;

	// Token: 0x040005AE RID: 1454
	[SerializeField]
	private TechListPanel m_TechPanel;
}
