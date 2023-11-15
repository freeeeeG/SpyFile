using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020005B9 RID: 1465
public class ArtifactAnalysisStationWorkable : Workable
{
	// Token: 0x060023D6 RID: 9174 RVA: 0x000C41B8 File Offset: 0x000C23B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanStudyArtifact.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingArtifact;
		this.attributeConverter = Db.Get().AttributeConverters.ArtSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_artifact_analysis_kanim")
		};
		base.SetWorkTime(150f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
		Components.ArtifactAnalysisStations.Add(this);
	}

	// Token: 0x060023D7 RID: 9175 RVA: 0x000C4281 File Offset: 0x000C2481
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.animController = base.GetComponent<KBatchedAnimController>();
		this.animController.SetSymbolVisiblity("snapTo_artifact", false);
	}

	// Token: 0x060023D8 RID: 9176 RVA: 0x000C42AB File Offset: 0x000C24AB
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.ArtifactAnalysisStations.Remove(this);
	}

	// Token: 0x060023D9 RID: 9177 RVA: 0x000C42BE File Offset: 0x000C24BE
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.InitialDisplayStoredArtifact();
	}

	// Token: 0x060023DA RID: 9178 RVA: 0x000C42CD File Offset: 0x000C24CD
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		this.PositionArtifact();
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x060023DB RID: 9179 RVA: 0x000C42E0 File Offset: 0x000C24E0
	private void InitialDisplayStoredArtifact()
	{
		GameObject gameObject = base.GetComponent<Storage>().GetItems()[0];
		KBatchedAnimController component = gameObject.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.GetBatchInstanceData().ClearOverrideTransformMatrix();
		}
		gameObject.transform.SetPosition(new Vector3(base.transform.position.x, base.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.BuildingBack)));
		gameObject.SetActive(true);
		component.enabled = false;
		component.enabled = true;
		this.PositionArtifact();
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, gameObject);
	}

	// Token: 0x060023DC RID: 9180 RVA: 0x000C438C File Offset: 0x000C258C
	private void ReleaseStoredArtifact()
	{
		Storage component = base.GetComponent<Storage>();
		GameObject gameObject = component.GetItems()[0];
		KBatchedAnimController component2 = gameObject.GetComponent<KBatchedAnimController>();
		gameObject.transform.SetPosition(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Grid.GetLayerZ(Grid.SceneLayer.Ore)));
		component2.enabled = false;
		component2.enabled = true;
		component.Drop(gameObject, true);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ArtifactAnalysisAnalyzing, gameObject);
	}

	// Token: 0x060023DD RID: 9181 RVA: 0x000C4420 File Offset: 0x000C2620
	private void PositionArtifact()
	{
		GameObject gameObject = base.GetComponent<Storage>().GetItems()[0];
		bool flag;
		Vector3 position = this.animController.GetSymbolTransform("snapTo_artifact", out flag).GetColumn(3);
		position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingBack);
		gameObject.transform.SetPosition(position);
	}

	// Token: 0x060023DE RID: 9182 RVA: 0x000C447E File Offset: 0x000C267E
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.ConsumeCharm();
		this.ReleaseStoredArtifact();
	}

	// Token: 0x060023DF RID: 9183 RVA: 0x000C4494 File Offset: 0x000C2694
	private void ConsumeCharm()
	{
		GameObject gameObject = this.storage.FindFirst(GameTags.CharmedArtifact);
		DebugUtil.DevAssertArgs(gameObject != null, new object[]
		{
			"ArtifactAnalysisStation finished studying a charmed artifact but there is not one in its storage"
		});
		if (gameObject != null)
		{
			this.YieldPayload(gameObject.GetComponent<SpaceArtifact>());
			gameObject.GetComponent<SpaceArtifact>().RemoveCharm();
		}
		if (ArtifactSelector.Instance.RecordArtifactAnalyzed(gameObject.GetComponent<KPrefabID>().PrefabID().ToString()))
		{
			if (gameObject.HasTag(GameTags.TerrestrialArtifact))
			{
				ArtifactSelector.Instance.IncrementAnalyzedTerrestrialArtifacts();
				return;
			}
			ArtifactSelector.Instance.IncrementAnalyzedSpaceArtifacts();
		}
	}

	// Token: 0x060023E0 RID: 9184 RVA: 0x000C4534 File Offset: 0x000C2734
	private void YieldPayload(SpaceArtifact artifact)
	{
		if (this.nextYeildRoll == -1f)
		{
			this.nextYeildRoll = UnityEngine.Random.Range(0f, 1f);
		}
		if (this.nextYeildRoll <= artifact.GetArtifactTier().payloadDropChance)
		{
			GameUtil.KInstantiate(Assets.GetPrefab("GeneShufflerRecharge"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore, null, 0).SetActive(true);
		}
		int num = Mathf.FloorToInt(artifact.GetArtifactTier().payloadDropChance * 20f);
		for (int i = 0; i < num; i++)
		{
			GameUtil.KInstantiate(Assets.GetPrefab("OrbitalResearchDatabank"), this.statesInstance.master.transform.position + this.finishedArtifactDropOffset, Grid.SceneLayer.Ore, null, 0).SetActive(true);
		}
		this.nextYeildRoll = UnityEngine.Random.Range(0f, 1f);
	}

	// Token: 0x04001485 RID: 5253
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001486 RID: 5254
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04001487 RID: 5255
	[SerializeField]
	public Vector3 finishedArtifactDropOffset;

	// Token: 0x04001488 RID: 5256
	private Notification notification;

	// Token: 0x04001489 RID: 5257
	public ArtifactAnalysisStation.StatesInstance statesInstance;

	// Token: 0x0400148A RID: 5258
	private KBatchedAnimController animController;

	// Token: 0x0400148B RID: 5259
	[Serialize]
	private float nextYeildRoll = -1f;
}
