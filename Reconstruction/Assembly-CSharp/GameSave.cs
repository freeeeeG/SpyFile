using System;
using System.Collections.Generic;

// Token: 0x02000134 RID: 308
[Serializable]
public class GameSave
{
	// Token: 0x0600080F RID: 2063 RVA: 0x00015168 File Offset: 0x00013368
	public void ClearGame()
	{
		this.HasLastGame = false;
		this.SaveBattleRecipes.Clear();
		this.SaveBluePrints.Clear();
		this.SaveTechnologies.Clear();
		this.SavePickingTechs.Clear();
		this.ChallengeChoicePicked = false;
		this.SaveRules.Clear();
		this.SaveRes = null;
		this.SaveContents.Clear();
		this.SaveSequences.Clear();
	}

	// Token: 0x06000810 RID: 2064 RVA: 0x000151D8 File Offset: 0x000133D8
	public void SaveGame(List<TechnologyStruct> saveTechs, List<TechnologyStruct> savePickingTechs, List<BlueprintStruct> saveBlueprints, GameResStruct saveRes, List<ContentStruct> saveContents, List<EnemySequenceStruct> saveSequences, List<ShapeInfo> currentShapes, List<int> saveRules, List<string> saveRecipes)
	{
		this.ChallengeChoicePicked = GameRes.ChallengeChoicePicked;
		this.SavePickingTechs = savePickingTechs;
		this.HasLastGame = true;
		this.SaveTechnologies = saveTechs;
		this.SaveBluePrints = saveBlueprints;
		this.SaveRes = saveRes;
		this.SaveContents = saveContents;
		this.SaveSequences = saveSequences;
		this.SaveShapes = currentShapes;
		this.SaveRules = saveRules;
		this.SaveBattleRecipes = saveRecipes;
	}

	// Token: 0x040003D5 RID: 981
	public bool HasLastGame;

	// Token: 0x040003D6 RID: 982
	public List<BlueprintStruct> SaveBluePrints = new List<BlueprintStruct>();

	// Token: 0x040003D7 RID: 983
	public List<TechnologyStruct> SaveTechnologies = new List<TechnologyStruct>();

	// Token: 0x040003D8 RID: 984
	public List<TechnologyStruct> SavePickingTechs = new List<TechnologyStruct>();

	// Token: 0x040003D9 RID: 985
	public bool ChallengeChoicePicked;

	// Token: 0x040003DA RID: 986
	public List<int> SaveRules = new List<int>();

	// Token: 0x040003DB RID: 987
	public GameResStruct SaveRes;

	// Token: 0x040003DC RID: 988
	public List<ContentStruct> SaveContents = new List<ContentStruct>();

	// Token: 0x040003DD RID: 989
	public List<string> SaveBattleRecipes = new List<string>();

	// Token: 0x040003DE RID: 990
	public List<EnemySequenceStruct> SaveSequences = new List<EnemySequenceStruct>();

	// Token: 0x040003DF RID: 991
	public List<ShapeInfo> SaveShapes = new List<ShapeInfo>();
}
