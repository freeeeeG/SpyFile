using System;
using System.Text;
using UnityEngine;

// Token: 0x020000B6 RID: 182
public class UI_Panel_EnemyLibrary_EnemyIcon : MonoBehaviour
{
	// Token: 0x06000660 RID: 1632 RVA: 0x000248FE File Offset: 0x00022AFE
	private void OnMouseEnter()
	{
		UI_ToolTip.inst.ShowWithString(this.GetString_EnemyInfo());
	}

	// Token: 0x06000661 RID: 1633 RVA: 0x0001E3C5 File Offset: 0x0001C5C5
	private void OnMouseExit()
	{
		UI_ToolTip.inst.Close();
	}

	// Token: 0x06000662 RID: 1634 RVA: 0x00024910 File Offset: 0x00022B10
	private string GetString_EnemyInfo()
	{
		EnemyModel enemyModel = DataBase.Inst.Data_EnemyModels[this.enemyID];
		GameObject gameObject = ResourceLibrary.Inst.Prefab_Enemys[this.enemyID];
		Enemy component = gameObject.GetComponent<Enemy>();
		BasicUnit component2 = gameObject.GetComponent<BasicUnit>();
		StringBuilder stringBuilder = new StringBuilder();
		UI_Setting inst = UI_Setting.Inst;
		LanguageText inst2 = LanguageText.Inst;
		LanguageText.EnemyLibrary enemyLibrary = inst2.enemyLibrary;
		stringBuilder.Append(enemyModel.Language_Name.TextSet(inst.commonSets.blueSmallTile)).AppendLine();
		stringBuilder.Append(enemyLibrary.enemyInfo_Shape).Append(inst2.shapes[(int)component2.shapeType].shapeName).AppendLine();
		int num = (int)enemyModel.rank;
		if ((this.enemyID + 1) % 12 == 0)
		{
			num = 3;
		}
		stringBuilder.Append(enemyLibrary.enemyInfo_Rank).Append(enemyLibrary.enemyRanks[num].Colored(UI_Setting.Inst.rankColors[num])).AppendLine();
		stringBuilder.Append(enemyLibrary.enemyInfo_Life).Append(GameParameters.Inst.DefaultFactorEnemy.lifeMaxEnemy * (double)enemyModel.factorMultis.factorMultis[0]).AppendLine();
		stringBuilder.Append(enemyLibrary.enemyInfo_Size).Append(enemyModel.factorMultis.mod_BodySize).AppendLine();
		stringBuilder.Append(enemyLibrary.enemyInfo_Speed).Append(GameParameters.Inst.DefaultFactorEnemy.moveSpd * enemyModel.factorMultis.mod_MoveSpd).AppendLine();
		stringBuilder.Append(enemyLibrary.enemyInfo_MoveType).Append(enemyLibrary.enemyMoveType[component.move_Type - EnumMoveType.TRACKING]);
		int summonType = enemyModel.summonType;
		if (summonType != -1)
		{
			stringBuilder.AppendLine();
			int num2 = -1;
			for (int i = 0; i < ResourceLibrary.Inst.Prefab_Enemys.Length; i++)
			{
				if (component.shoot_PrefabShootObject == ResourceLibrary.Inst.Prefab_Enemys[i])
				{
					num2 = i;
					break;
				}
			}
			if (num2 == -1)
			{
				stringBuilder.Append("Error");
			}
			else
			{
				EnemyModel enemyModel2 = DataBase.Inst.Data_EnemyModels[num2];
				int num3 = (int)enemyModel2.rank;
				if ((num2 + 1) % 12 == 0)
				{
					num3 = 3;
				}
				stringBuilder.Append(enemyLibrary.enemySummonType[summonType].Replace("sSummon", enemyModel2.Language_Name.Colored(UI_Setting.Inst.rankColors[num3])));
			}
		}
		int splitType = enemyModel.splitType;
		if (splitType != -1)
		{
			stringBuilder.AppendLine();
			stringBuilder.Append(enemyLibrary.enemySplitType[splitType]);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x04000550 RID: 1360
	public int enemyID;
}
