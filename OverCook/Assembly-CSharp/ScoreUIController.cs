using System;
using UnityEngine;

// Token: 0x02000B4B RID: 2891
public class ScoreUIController : DisplayIntUIController
{
	// Token: 0x06003AC3 RID: 15043 RVA: 0x00117E98 File Offset: 0x00116298
	protected override void Awake()
	{
		base.Awake();
		this.m_multiplierUIController.Value = 0;
		this.m_multiplierText.enabled = false;
		this.m_barImage.sprite = this.m_barSprites[0];
		this.m_dataStore = GameUtils.RequireManager<DataStore>();
		this.m_dataStore.Register(ScoreUIController.k_scoreTeamId, new DataStore.OnChangeNotification(this.OnTeamScoreNotification));
	}

	// Token: 0x06003AC4 RID: 15044 RVA: 0x00117EFD File Offset: 0x001162FD
	private void OnDestroy()
	{
		if (this.m_dataStore != null)
		{
			this.m_dataStore.Unregister(ScoreUIController.k_scoreTeamId, new DataStore.OnChangeNotification(this.OnTeamScoreNotification));
		}
	}

	// Token: 0x06003AC5 RID: 15045 RVA: 0x00117F2C File Offset: 0x0011632C
	private void OnTeamScoreNotification(DataStore.Id id, object data)
	{
		TeamScore teamScore = (TeamScore)data;
		this.ScoreUpdate(teamScore.m_team, teamScore.m_score);
	}

	// Token: 0x06003AC6 RID: 15046 RVA: 0x00117F54 File Offset: 0x00116354
	public void ScoreUpdate(TeamID team, TeamMonitor.TeamScoreStats score)
	{
		if (team == this.m_team)
		{
			int totalScore = score.GetTotalScore();
			base.Value = totalScore;
			this.m_multiplierText.enabled = (score.TotalMultiplier > 0);
			this.m_multiplierUIController.Value = score.TotalMultiplier;
			int num = Mathf.Max(score.TotalMultiplier, 1);
			if (this.m_barSprites[num - 1] != null)
			{
				this.m_barImage.sprite = this.m_barSprites[num - 1];
			}
			this.m_flameImage.gameObject.SetActive(num == 4);
			int num2 = totalScore - this.m_totalScore;
			if (num2 != 0 && base.isActiveAndEnabled)
			{
				GameObject obj = GameUtils.InstantiateUIControllerOnScalingHUDCanvas((num2 <= 0) ? this.m_removePointsFloatingTextPrefab : this.m_addPointsFloatingTextPrefab);
				RectTransform rectTransform = (RectTransform)base.transform;
				RectTransformExtension rectTransformExtension = obj.RequireComponent<RectTransformExtension>();
				Vector2 anchorOffset = this.m_floatingTextOffset.MultipliedBy(rectTransform.anchorMin + rectTransform.anchorMax);
				rectTransformExtension.AnchorOffset = anchorOffset;
				obj.RequireComponent<DisplayIntUIController>().Value = Mathf.Abs(num2);
				this.m_scoreAnimator.SetTrigger(ScoreUIController.m_scoreAnimatorHash);
			}
			this.m_totalScore = totalScore;
		}
	}

	// Token: 0x04002FA1 RID: 12193
	[SerializeField]
	private TeamID m_team;

	// Token: 0x04002FA2 RID: 12194
	[SerializeField]
	private Animator m_scoreAnimator;

	// Token: 0x04002FA3 RID: 12195
	private static readonly int m_scoreAnimatorHash = Animator.StringToHash("Score");

	// Token: 0x04002FA4 RID: 12196
	[Header("Floating Text")]
	[SerializeField]
	[AssignResource("AddPointsFloatingNumberUI", Editorbility.Editable)]
	private GameObject m_addPointsFloatingTextPrefab;

	// Token: 0x04002FA5 RID: 12197
	[SerializeField]
	[AssignResource("RemovePointsFloatingNumberUI", Editorbility.Editable)]
	private GameObject m_removePointsFloatingTextPrefab;

	// Token: 0x04002FA6 RID: 12198
	[SerializeField]
	private Vector2 m_floatingTextOffset = new Vector2(0.5f, 0.5f);

	// Token: 0x04002FA7 RID: 12199
	[Header("Multiplier")]
	[SerializeField]
	private DisplayIntUIController m_multiplierUIController;

	// Token: 0x04002FA8 RID: 12200
	[SerializeField]
	private T17Text m_multiplierText;

	// Token: 0x04002FA9 RID: 12201
	[SerializeField]
	private Sprite[] m_barSprites;

	// Token: 0x04002FAA RID: 12202
	[SerializeField]
	private T17Image m_barImage;

	// Token: 0x04002FAB RID: 12203
	[SerializeField]
	private T17Image m_flameImage;

	// Token: 0x04002FAC RID: 12204
	private int m_totalScore;

	// Token: 0x04002FAD RID: 12205
	private DataStore m_dataStore;

	// Token: 0x04002FAE RID: 12206
	private static readonly DataStore.Id k_scoreTeamId = new DataStore.Id("score.team");
}
