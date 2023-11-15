using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000282 RID: 642
public class UIStandardMode : MonoBehaviour
{
	// Token: 0x1700053E RID: 1342
	// (get) Token: 0x06000FEC RID: 4076 RVA: 0x0002AA8F File Offset: 0x00028C8F
	// (set) Token: 0x06000FED RID: 4077 RVA: 0x0002AA97 File Offset: 0x00028C97
	private int SelectDifficulty
	{
		get
		{
			return this.selectDifficulty;
		}
		set
		{
			this.selectDifficulty = Mathf.Clamp(value, 0, Singleton<LevelManager>.Instance.PassDiifcutly);
		}
	}

	// Token: 0x06000FEE RID: 4078 RVA: 0x0002AAB0 File Offset: 0x00028CB0
	public void Initialize()
	{
		this.m_BattleRecipe.Initialize();
		this.selectDifficulty = Singleton<LevelManager>.Instance.PassDiifcutly;
		this.DifficultyBtnClick(0);
	}

	// Token: 0x06000FEF RID: 4079 RVA: 0x0002AAD4 File Offset: 0x00028CD4
	public void DifficultyBtnClick(int count)
	{
		this.SelectDifficulty += count;
		this.m_BattleRecipe.gameObject.SetActive(this.SelectDifficulty > 1);
		this.difficultyInfo_Txt.text = GameMultiLang.GetTraduction("DIFFICULTY" + this.SelectDifficulty.ToString());
		this.difficultyTxt.text = GameMultiLang.GetTraduction("DIFFICULTY") + " " + this.SelectDifficulty.ToString();
	}

	// Token: 0x06000FF0 RID: 4080 RVA: 0x0002AB5D File Offset: 0x00028D5D
	public void StandardModeStart()
	{
		this.m_BattleRecipe.UpdateRecipes();
		RuleFactory.Release();
		Singleton<LevelManager>.Instance.StartNewGame(this.SelectDifficulty);
	}

	// Token: 0x04000840 RID: 2112
	[SerializeField]
	private Text difficultyInfo_Txt;

	// Token: 0x04000841 RID: 2113
	[SerializeField]
	private Text difficultyTxt;

	// Token: 0x04000842 RID: 2114
	[SerializeField]
	private BattleRecipe m_BattleRecipe;

	// Token: 0x04000843 RID: 2115
	private int selectDifficulty;
}
