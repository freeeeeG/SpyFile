using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000B52 RID: 2898
public class SurvivalModeLevelTimerUIController : UIControllerBase
{
	// Token: 0x06003AE2 RID: 15074 RVA: 0x00118458 File Offset: 0x00116858
	protected void Awake()
	{
		this.m_dataStore = GameUtils.RequireManager<DataStore>();
		this.m_dataStore.Register(SurvivalModeLevelTimerUIController.k_timeAddedId, new DataStore.OnChangeNotification(this.OnTimeAddedNotification));
		this.m_dataStore.Register(SurvivalModeLevelTimerUIController.k_timeUpdatedId, new DataStore.OnChangeNotification(this.OnTimeUpdatedNotification));
		this.m_dataStore.Register(SurvivalModeLevelTimerUIController.k_timeSurvivedId, new DataStore.OnChangeNotification(this.OnTimeSurvivedNotification));
		GameSession gameSession = GameUtils.GetGameSession();
		HighScoreRepository highScoreRepository = gameSession.HighScoreRepository;
		GameProgress.HighScores.Score score = new GameProgress.HighScores.Score();
		if (highScoreRepository != null && highScoreRepository.GetScore(ClientUserSystem.s_LocalMachineId, GameUtils.GetLevelID(), ref score))
		{
			this.m_survivalTimeRecord = score.iSurvivalModeTime;
		}
	}

	// Token: 0x06003AE3 RID: 15075 RVA: 0x00118500 File Offset: 0x00116900
	protected void OnDestroy()
	{
		if (this.m_dataStore != null)
		{
			this.m_dataStore.Unregister(SurvivalModeLevelTimerUIController.k_timeAddedId, new DataStore.OnChangeNotification(this.OnTimeAddedNotification));
			this.m_dataStore.Unregister(SurvivalModeLevelTimerUIController.k_timeUpdatedId, new DataStore.OnChangeNotification(this.OnTimeUpdatedNotification));
			this.m_dataStore.Unregister(SurvivalModeLevelTimerUIController.k_timeSurvivedId, new DataStore.OnChangeNotification(this.OnTimeSurvivedNotification));
		}
	}

	// Token: 0x06003AE4 RID: 15076 RVA: 0x00118574 File Offset: 0x00116974
	private void OnTimeAddedNotification(DataStore.Id id, object data)
	{
		int num = Convert.ToInt32(data);
		GameObject obj = GameUtils.InstantiateUIControllerOnScalingHUDCanvas((num <= 0) ? this.m_removeTimeFloatingTextPrefab : this.m_addTimeFloatingTextPrefab);
		RectTransform rectTransform = (RectTransform)base.transform;
		RectTransformExtension rectTransformExtension = obj.RequireComponent<RectTransformExtension>();
		Vector2 anchorOffset = this.m_floatingTextOffset.MultipliedBy(rectTransform.anchorMin + rectTransform.anchorMax);
		rectTransformExtension.AnchorOffset = anchorOffset;
		obj.RequireComponent<DisplayIntUIController>().Value = Math.Abs(num);
	}

	// Token: 0x06003AE5 RID: 15077 RVA: 0x001185F0 File Offset: 0x001169F0
	private void OnTimeUpdatedNotification(DataStore.Id id, object data)
	{
		this.m_roundTimerController.Value = Convert.ToInt32(data);
	}

	// Token: 0x06003AE6 RID: 15078 RVA: 0x00118604 File Offset: 0x00116A04
	private void OnTimeSurvivedNotification(DataStore.Id id, object data)
	{
		int num = Convert.ToInt32(data);
		this.m_survivalTimerController.Value = num;
		if (!this.m_newSurvivalTimeRecord && this.m_survivalTimeRecord != 0 && num > this.m_survivalTimeRecord)
		{
			this.m_newSurvivalTimeRecord = true;
			this.m_survivalTimeRecord = num;
			this.m_survivalTimerText.color = this.m_newRecordTextColour;
			GameObject obj = GameUtils.InstantiateUIControllerOnScalingHUDCanvas(this.m_newRecordFloatingTextPrefab);
			RectTransform rectTransform = (RectTransform)base.transform;
			RectTransformExtension rectTransformExtension = obj.RequireComponent<RectTransformExtension>();
			Vector2 anchorOffset = this.m_floatingTextOffset.MultipliedBy(rectTransform.anchorMin + rectTransform.anchorMax);
			rectTransformExtension.AnchorOffset = anchorOffset;
		}
	}

	// Token: 0x04002FC5 RID: 12229
	[Header("Timers")]
	[SerializeField]
	private DisplayTimeUIController m_roundTimerController;

	// Token: 0x04002FC6 RID: 12230
	[SerializeField]
	private DisplayTimeUIController m_survivalTimerController;

	// Token: 0x04002FC7 RID: 12231
	[SerializeField]
	private T17Text m_survivalTimerText;

	// Token: 0x04002FC8 RID: 12232
	[Header("Floating Text")]
	[SerializeField]
	[AssignResource("NewRecordFloatingText", Editorbility.Editable)]
	private GameObject m_newRecordFloatingTextPrefab;

	// Token: 0x04002FC9 RID: 12233
	[SerializeField]
	private Color m_newRecordTextColour = new Color(1f, 0.682f, 0f, 1f);

	// Token: 0x04002FCA RID: 12234
	[SerializeField]
	[AssignResource("AddPointsFloatingNumberUI", Editorbility.Editable)]
	private GameObject m_addTimeFloatingTextPrefab;

	// Token: 0x04002FCB RID: 12235
	[SerializeField]
	[AssignResource("RemovePointsFloatingNumberUI", Editorbility.Editable)]
	private GameObject m_removeTimeFloatingTextPrefab;

	// Token: 0x04002FCC RID: 12236
	[SerializeField]
	private Vector2 m_floatingTextOffset = new Vector2(0f, 0f);

	// Token: 0x04002FCD RID: 12237
	private DataStore m_dataStore;

	// Token: 0x04002FCE RID: 12238
	private static readonly DataStore.Id k_timeAddedId = new DataStore.Id("time.added");

	// Token: 0x04002FCF RID: 12239
	private static readonly DataStore.Id k_timeUpdatedId = new DataStore.Id("time.updated");

	// Token: 0x04002FD0 RID: 12240
	private static readonly DataStore.Id k_timeSurvivedId = new DataStore.Id("time.survived");

	// Token: 0x04002FD1 RID: 12241
	private bool m_newSurvivalTimeRecord;

	// Token: 0x04002FD2 RID: 12242
	private int m_survivalTimeRecord;
}
