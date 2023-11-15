using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000243 RID: 579
public class TurretItem : MonoBehaviour
{
	// Token: 0x06000EDD RID: 3805 RVA: 0x00027598 File Offset: 0x00025798
	public void SetItemData(TurretContent turret)
	{
		this.m_Turret = turret;
		StrategyType strategyType = this.m_Turret.Strategy.Attribute.StrategyType;
		if (strategyType != StrategyType.Element)
		{
			if (strategyType == StrategyType.Composite)
			{
				this.nameTxt.text = GameMultiLang.GetTraduction(this.m_Turret.Strategy.Attribute.Name);
			}
		}
		else
		{
			string str = StaticData.FormElementName(this.m_Turret.Strategy.Attribute.element, this.m_Turret.Strategy.Quality);
			this.nameTxt.text = str + " " + GameMultiLang.GetTraduction(this.m_Turret.Strategy.Attribute.Name);
		}
		this.icon.sprite = turret.Strategy.Attribute.TurretLevels[this.m_Turret.Strategy.Quality - 1].TurretIcon;
		this.damageValue.text = turret.Strategy.TotalDamage.ToString();
		this.TotalDamage = turret.Strategy.TotalDamage;
		if (Singleton<LevelManager>.Instance.LevelWin)
		{
			if ((float)this.TotalDamage / (float)GameRes.TotalDamage > 0.7f)
			{
				Singleton<LevelManager>.Instance.SetAchievement("ACH_SUPERCORE");
			}
			if (turret.Strategy.TotalElementCount >= 15)
			{
				Singleton<LevelManager>.Instance.SetAchievement("ACH_15ELEMENTS");
			}
		}
	}

	// Token: 0x06000EDE RID: 3806 RVA: 0x00027704 File Offset: 0x00025904
	public void SetRank(int rank)
	{
		if (rank <= 2)
		{
			this.medalImg.gameObject.SetActive(true);
			this.medalImg.sprite = this.medalSprites[rank];
			this.rankTxt.gameObject.SetActive(false);
			return;
		}
		this.rankTxt.text = (rank + 1).ToString();
		this.rankTxt.gameObject.SetActive(true);
		this.medalImg.gameObject.SetActive(false);
	}

	// Token: 0x06000EDF RID: 3807 RVA: 0x00027783 File Offset: 0x00025983
	public void LocateTurret()
	{
		BoardSystem.SelectingTile = this.m_Turret.m_GameTile;
		Singleton<GameManager>.Instance.LocateCamPos(this.m_Turret.transform.position);
	}

	// Token: 0x0400074F RID: 1871
	[SerializeField]
	private Image icon;

	// Token: 0x04000750 RID: 1872
	[SerializeField]
	private Text nameTxt;

	// Token: 0x04000751 RID: 1873
	[SerializeField]
	private Text damageValue;

	// Token: 0x04000752 RID: 1874
	public long TotalDamage;

	// Token: 0x04000753 RID: 1875
	public TurretContent m_Turret;

	// Token: 0x04000754 RID: 1876
	[SerializeField]
	private Sprite[] medalSprites;

	// Token: 0x04000755 RID: 1877
	[SerializeField]
	private Image medalImg;

	// Token: 0x04000756 RID: 1878
	[SerializeField]
	private Text rankTxt;
}
