using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using KSerialization;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x0200053C RID: 1340
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Workable")]
public class Workable : KMonoBehaviour, ISaveLoadable, IApproachable
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06002028 RID: 8232 RVA: 0x000ACCE4 File Offset: 0x000AAEE4
	// (set) Token: 0x06002029 RID: 8233 RVA: 0x000ACCEC File Offset: 0x000AAEEC
	public Worker worker { get; protected set; }

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x0600202A RID: 8234 RVA: 0x000ACCF5 File Offset: 0x000AAEF5
	// (set) Token: 0x0600202B RID: 8235 RVA: 0x000ACCFD File Offset: 0x000AAEFD
	public float WorkTimeRemaining
	{
		get
		{
			return this.workTimeRemaining;
		}
		set
		{
			this.workTimeRemaining = value;
		}
	}

	// Token: 0x17000160 RID: 352
	// (get) Token: 0x0600202C RID: 8236 RVA: 0x000ACD06 File Offset: 0x000AAF06
	// (set) Token: 0x0600202D RID: 8237 RVA: 0x000ACD0E File Offset: 0x000AAF0E
	public bool preferUnreservedCell { get; set; }

	// Token: 0x0600202E RID: 8238 RVA: 0x000ACD17 File Offset: 0x000AAF17
	public virtual float GetWorkTime()
	{
		return this.workTime;
	}

	// Token: 0x0600202F RID: 8239 RVA: 0x000ACD1F File Offset: 0x000AAF1F
	public Worker GetWorker()
	{
		return this.worker;
	}

	// Token: 0x06002030 RID: 8240 RVA: 0x000ACD27 File Offset: 0x000AAF27
	public virtual float GetPercentComplete()
	{
		if (this.workTimeRemaining > this.workTime)
		{
			return -1f;
		}
		return 1f - this.workTimeRemaining / this.workTime;
	}

	// Token: 0x06002031 RID: 8241 RVA: 0x000ACD50 File Offset: 0x000AAF50
	public void ConfigureMultitoolContext(HashedString context, Tag hitEffectTag)
	{
		this.multitoolContext = context;
		this.multitoolHitEffectTag = hitEffectTag;
	}

	// Token: 0x06002032 RID: 8242 RVA: 0x000ACD60 File Offset: 0x000AAF60
	public virtual Workable.AnimInfo GetAnim(Worker worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		if (this.overrideAnims != null && this.overrideAnims.Length != 0)
		{
			BuildingFacade buildingFacade = this.GetBuildingFacade();
			bool flag = false;
			if (buildingFacade != null && !buildingFacade.IsOriginal)
			{
				flag = buildingFacade.interactAnims.TryGetValue(base.name, out result.overrideAnims);
			}
			if (!flag)
			{
				result.overrideAnims = this.overrideAnims;
			}
		}
		if (this.multitoolContext.IsValid && this.multitoolHitEffectTag.IsValid)
		{
			result.smi = new MultitoolController.Instance(this, worker, this.multitoolContext, Assets.GetPrefab(this.multitoolHitEffectTag));
		}
		return result;
	}

	// Token: 0x06002033 RID: 8243 RVA: 0x000ACE03 File Offset: 0x000AB003
	public virtual HashedString[] GetWorkAnims(Worker worker)
	{
		return this.workAnims;
	}

	// Token: 0x06002034 RID: 8244 RVA: 0x000ACE0B File Offset: 0x000AB00B
	public virtual KAnim.PlayMode GetWorkAnimPlayMode()
	{
		return this.workAnimPlayMode;
	}

	// Token: 0x06002035 RID: 8245 RVA: 0x000ACE13 File Offset: 0x000AB013
	public virtual HashedString[] GetWorkPstAnims(Worker worker, bool successfully_completed)
	{
		if (successfully_completed)
		{
			return this.workingPstComplete;
		}
		return this.workingPstFailed;
	}

	// Token: 0x06002036 RID: 8246 RVA: 0x000ACE25 File Offset: 0x000AB025
	public virtual Vector3 GetWorkOffset()
	{
		return Vector3.zero;
	}

	// Token: 0x06002037 RID: 8247 RVA: 0x000ACE2C File Offset: 0x000AB02C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().MiscStatusItems.Using;
		this.workingStatusItem = Db.Get().MiscStatusItems.Operating;
		this.readyForSkillWorkStatusItem = Db.Get().BuildingStatusItems.RequiresSkillPerk;
		this.workTime = this.GetWorkTime();
		this.workTimeRemaining = Mathf.Min(this.workTimeRemaining, this.workTime);
	}

	// Token: 0x06002038 RID: 8248 RVA: 0x000ACEA4 File Offset: 0x000AB0A4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
		{
			if (this.skillsUpdateHandle != -1)
			{
				Game.Instance.Unsubscribe(this.skillsUpdateHandle);
			}
			this.skillsUpdateHandle = Game.Instance.Subscribe(-1523247426, new Action<object>(this.UpdateStatusItem));
		}
		if (this.requireMinionToWork && this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		this.minionUpdateHandle = Game.Instance.Subscribe(586301400, new Action<object>(this.UpdateStatusItem));
		base.GetComponent<KPrefabID>().AddTag(GameTags.HasChores, false);
		if (base.gameObject.HasTag(this.laboratoryEfficiencyBonusTagRequired))
		{
			this.useLaboratoryEfficiencyBonus = true;
			base.Subscribe<Workable>(144050788, Workable.OnUpdateRoomDelegate);
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		this.UpdateStatusItem(null);
	}

	// Token: 0x06002039 RID: 8249 RVA: 0x000ACFAC File Offset: 0x000AB1AC
	private void RefreshRoom()
	{
		CavityInfo cavityForCell = Game.Instance.roomProber.GetCavityForCell(Grid.PosToCell(base.gameObject));
		if (cavityForCell != null && cavityForCell.room != null)
		{
			this.OnUpdateRoom(cavityForCell.room);
			return;
		}
		this.OnUpdateRoom(null);
	}

	// Token: 0x0600203A RID: 8250 RVA: 0x000ACFF4 File Offset: 0x000AB1F4
	private void OnUpdateRoom(object data)
	{
		if (this.worker == null)
		{
			return;
		}
		Room room = (Room)data;
		if (room != null && room.roomType == Db.Get().RoomTypes.Laboratory)
		{
			this.currentlyInLaboratory = true;
			if (this.laboratoryEfficiencyBonusStatusItemHandle == Guid.Empty)
			{
				this.laboratoryEfficiencyBonusStatusItemHandle = this.worker.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.LaboratoryWorkEfficiencyBonus, this);
				return;
			}
		}
		else
		{
			this.currentlyInLaboratory = false;
			if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
			{
				this.laboratoryEfficiencyBonusStatusItemHandle = this.worker.GetComponent<KSelectable>().RemoveStatusItem(this.laboratoryEfficiencyBonusStatusItemHandle, false);
			}
		}
	}

	// Token: 0x0600203B RID: 8251 RVA: 0x000AD0A8 File Offset: 0x000AB2A8
	protected virtual void UpdateStatusItem(object data = null)
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (component == null)
		{
			return;
		}
		component.RemoveStatusItem(this.workStatusItemHandle, false);
		if (this.worker == null)
		{
			if (this.requireMinionToWork && Components.LiveMinionIdentities.GetWorldItems(this.GetMyWorldId(), false).Count == 0)
			{
				this.workStatusItemHandle = component.AddStatusItem(Db.Get().BuildingStatusItems.WorkRequiresMinion, null);
				return;
			}
			if (this.shouldShowSkillPerkStatusItem && !string.IsNullOrEmpty(this.requiredSkillPerk))
			{
				if (!MinionResume.AnyMinionHasPerk(this.requiredSkillPerk, this.GetMyWorldId()))
				{
					StatusItem status_item = DlcManager.FeatureClusterSpaceEnabled() ? Db.Get().BuildingStatusItems.ClusterColonyLacksRequiredSkillPerk : Db.Get().BuildingStatusItems.ColonyLacksRequiredSkillPerk;
					this.workStatusItemHandle = component.AddStatusItem(status_item, this.requiredSkillPerk);
					return;
				}
				this.workStatusItemHandle = component.AddStatusItem(this.readyForSkillWorkStatusItem, this.requiredSkillPerk);
				return;
			}
		}
		else if (this.workingStatusItem != null)
		{
			this.workStatusItemHandle = component.AddStatusItem(this.workingStatusItem, this);
		}
	}

	// Token: 0x0600203C RID: 8252 RVA: 0x000AD1C0 File Offset: 0x000AB3C0
	protected override void OnLoadLevel()
	{
		this.overrideAnims = null;
		base.OnLoadLevel();
	}

	// Token: 0x0600203D RID: 8253 RVA: 0x000AD1CF File Offset: 0x000AB3CF
	public int GetCell()
	{
		return Grid.PosToCell(this);
	}

	// Token: 0x0600203E RID: 8254 RVA: 0x000AD1D8 File Offset: 0x000AB3D8
	public void StartWork(Worker worker_to_start)
	{
		global::Debug.Assert(worker_to_start != null, "How did we get a null worker?");
		this.worker = worker_to_start;
		this.UpdateStatusItem(null);
		if (this.showProgressBar)
		{
			this.ShowProgressBar(true);
		}
		if (this.useLaboratoryEfficiencyBonus)
		{
			this.RefreshRoom();
		}
		this.OnStartWork(this.worker);
		if (this.worker != null)
		{
			string conversationTopic = this.GetConversationTopic();
			if (conversationTopic != null)
			{
				this.worker.Trigger(937885943, conversationTopic);
			}
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStarted);
		}
		this.numberOfUses++;
		if (this.worker != null)
		{
			if (base.gameObject.GetComponent<KSelectable>() != null && base.gameObject.GetComponent<KSelectable>().IsSelected && this.worker.gameObject.GetComponent<LoopingSounds>() != null)
			{
				this.worker.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
			else if (this.worker.gameObject.GetComponent<KSelectable>() != null && this.worker.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
			{
				base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(true);
			}
		}
		base.gameObject.Trigger(853695848, this);
	}

	// Token: 0x0600203F RID: 8255 RVA: 0x000AD344 File Offset: 0x000AB544
	public bool WorkTick(Worker worker, float dt)
	{
		bool flag = false;
		if (dt > 0f)
		{
			this.workTimeRemaining -= dt;
			flag = this.OnWorkTick(worker, dt);
		}
		return flag || this.workTimeRemaining < 0f;
	}

	// Token: 0x06002040 RID: 8256 RVA: 0x000AD384 File Offset: 0x000AB584
	public virtual float GetEfficiencyMultiplier(Worker worker)
	{
		float num = 1f;
		if (this.attributeConverter != null)
		{
			AttributeConverterInstance converter = worker.GetComponent<AttributeConverters>().GetConverter(this.attributeConverter.Id);
			num += converter.Evaluate();
		}
		if (this.lightEfficiencyBonus)
		{
			int num2 = Grid.PosToCell(worker.gameObject);
			if (Grid.IsValidCell(num2))
			{
				if (Grid.LightIntensity[num2] > 0)
				{
					this.currentlyLit = true;
					num += 0.15f;
					if (this.lightEfficiencyBonusStatusItemHandle == Guid.Empty)
					{
						this.lightEfficiencyBonusStatusItemHandle = worker.GetComponent<KSelectable>().AddStatusItem(Db.Get().DuplicantStatusItems.LightWorkEfficiencyBonus, this);
					}
				}
				else
				{
					this.currentlyLit = false;
					if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
					{
						worker.GetComponent<KSelectable>().RemoveStatusItem(this.lightEfficiencyBonusStatusItemHandle, false);
					}
				}
			}
		}
		if (this.useLaboratoryEfficiencyBonus && this.currentlyInLaboratory)
		{
			num += 0.1f;
		}
		return Mathf.Max(num, this.minimumAttributeMultiplier);
	}

	// Token: 0x06002041 RID: 8257 RVA: 0x000AD480 File Offset: 0x000AB680
	public virtual Klei.AI.Attribute GetWorkAttribute()
	{
		if (this.attributeConverter != null)
		{
			return this.attributeConverter.attribute;
		}
		return null;
	}

	// Token: 0x06002042 RID: 8258 RVA: 0x000AD498 File Offset: 0x000AB698
	public virtual string GetConversationTopic()
	{
		KPrefabID component = base.GetComponent<KPrefabID>();
		if (!component.HasTag(GameTags.NotConversationTopic))
		{
			return component.PrefabTag.Name;
		}
		return null;
	}

	// Token: 0x06002043 RID: 8259 RVA: 0x000AD4C6 File Offset: 0x000AB6C6
	public float GetAttributeExperienceMultiplier()
	{
		return this.attributeExperienceMultiplier;
	}

	// Token: 0x06002044 RID: 8260 RVA: 0x000AD4CE File Offset: 0x000AB6CE
	public string GetSkillExperienceSkillGroup()
	{
		return this.skillExperienceSkillGroup;
	}

	// Token: 0x06002045 RID: 8261 RVA: 0x000AD4D6 File Offset: 0x000AB6D6
	public float GetSkillExperienceMultiplier()
	{
		return this.skillExperienceMultiplier;
	}

	// Token: 0x06002046 RID: 8262 RVA: 0x000AD4DE File Offset: 0x000AB6DE
	protected virtual bool OnWorkTick(Worker worker, float dt)
	{
		return false;
	}

	// Token: 0x06002047 RID: 8263 RVA: 0x000AD4E4 File Offset: 0x000AB6E4
	public void StopWork(Worker workerToStop, bool aborted)
	{
		if (this.worker == workerToStop && aborted)
		{
			this.OnAbortWork(workerToStop);
		}
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(workerToStop);
		}
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkStopped);
		}
		this.OnStopWork(workerToStop);
		if (this.resetProgressOnStop)
		{
			this.workTimeRemaining = this.GetWorkTime();
		}
		this.ShowProgressBar(this.alwaysShowProgressBar && this.workTimeRemaining < this.GetWorkTime());
		if (this.lightEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			this.lightEfficiencyBonusStatusItemHandle = workerToStop.GetComponent<KSelectable>().RemoveStatusItem(this.lightEfficiencyBonusStatusItemHandle, false);
		}
		if (this.laboratoryEfficiencyBonusStatusItemHandle != Guid.Empty)
		{
			this.laboratoryEfficiencyBonusStatusItemHandle = this.worker.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().DuplicantStatusItems.LaboratoryWorkEfficiencyBonus, false);
		}
		if (base.gameObject.GetComponent<KSelectable>() != null && !base.gameObject.GetComponent<KSelectable>().IsSelected && base.gameObject.GetComponent<LoopingSounds>() != null)
		{
			base.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		else if (workerToStop.gameObject.GetComponent<KSelectable>() != null && !workerToStop.gameObject.GetComponent<KSelectable>().IsSelected && workerToStop.gameObject.GetComponent<LoopingSounds>() != null)
		{
			workerToStop.gameObject.GetComponent<LoopingSounds>().UpdateObjectSelection(false);
		}
		this.worker = null;
		base.gameObject.Trigger(679550494, this);
		this.UpdateStatusItem(null);
	}

	// Token: 0x06002048 RID: 8264 RVA: 0x000AD67B File Offset: 0x000AB87B
	public virtual StatusItem GetWorkerStatusItem()
	{
		return this.workerStatusItem;
	}

	// Token: 0x06002049 RID: 8265 RVA: 0x000AD683 File Offset: 0x000AB883
	public void SetWorkerStatusItem(StatusItem item)
	{
		this.workerStatusItem = item;
	}

	// Token: 0x0600204A RID: 8266 RVA: 0x000AD68C File Offset: 0x000AB88C
	public void CompleteWork(Worker worker)
	{
		if (this.shouldTransferDiseaseWithWorker)
		{
			this.TransferDiseaseWithWorker(worker);
		}
		this.OnCompleteWork(worker);
		if (this.OnWorkableEventCB != null)
		{
			this.OnWorkableEventCB(this, Workable.WorkableEvent.WorkCompleted);
		}
		this.workTimeRemaining = this.GetWorkTime();
		this.ShowProgressBar(false);
		base.gameObject.Trigger(-2011693419, this);
	}

	// Token: 0x0600204B RID: 8267 RVA: 0x000AD6E8 File Offset: 0x000AB8E8
	public void SetReportType(ReportManager.ReportType report_type)
	{
		this.reportType = report_type;
	}

	// Token: 0x0600204C RID: 8268 RVA: 0x000AD6F1 File Offset: 0x000AB8F1
	public ReportManager.ReportType GetReportType()
	{
		return this.reportType;
	}

	// Token: 0x0600204D RID: 8269 RVA: 0x000AD6F9 File Offset: 0x000AB8F9
	protected virtual void OnStartWork(Worker worker)
	{
	}

	// Token: 0x0600204E RID: 8270 RVA: 0x000AD6FB File Offset: 0x000AB8FB
	protected virtual void OnStopWork(Worker worker)
	{
	}

	// Token: 0x0600204F RID: 8271 RVA: 0x000AD6FD File Offset: 0x000AB8FD
	protected virtual void OnCompleteWork(Worker worker)
	{
	}

	// Token: 0x06002050 RID: 8272 RVA: 0x000AD6FF File Offset: 0x000AB8FF
	protected virtual void OnAbortWork(Worker worker)
	{
	}

	// Token: 0x06002051 RID: 8273 RVA: 0x000AD701 File Offset: 0x000AB901
	public virtual void OnPendingCompleteWork(Worker worker)
	{
	}

	// Token: 0x06002052 RID: 8274 RVA: 0x000AD703 File Offset: 0x000AB903
	public void SetOffsets(CellOffset[] offsets)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new StandardOffsetTracker(offsets);
	}

	// Token: 0x06002053 RID: 8275 RVA: 0x000AD724 File Offset: 0x000AB924
	public void SetOffsetTable(CellOffset[][] offset_table)
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		this.offsetTracker = new OffsetTableTracker(offset_table, this);
	}

	// Token: 0x06002054 RID: 8276 RVA: 0x000AD746 File Offset: 0x000AB946
	public virtual CellOffset[] GetOffsets(int cell)
	{
		if (this.offsetTracker == null)
		{
			this.offsetTracker = new StandardOffsetTracker(new CellOffset[1]);
		}
		return this.offsetTracker.GetOffsets(cell);
	}

	// Token: 0x06002055 RID: 8277 RVA: 0x000AD76D File Offset: 0x000AB96D
	public CellOffset[] GetOffsets()
	{
		return this.GetOffsets(Grid.PosToCell(this));
	}

	// Token: 0x06002056 RID: 8278 RVA: 0x000AD77B File Offset: 0x000AB97B
	public void SetWorkTime(float work_time)
	{
		this.workTime = work_time;
		this.workTimeRemaining = work_time;
	}

	// Token: 0x06002057 RID: 8279 RVA: 0x000AD78B File Offset: 0x000AB98B
	public bool ShouldFaceTargetWhenWorking()
	{
		return this.faceTargetWhenWorking;
	}

	// Token: 0x06002058 RID: 8280 RVA: 0x000AD793 File Offset: 0x000AB993
	public virtual Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition();
	}

	// Token: 0x06002059 RID: 8281 RVA: 0x000AD7A0 File Offset: 0x000AB9A0
	public void ShowProgressBar(bool show)
	{
		if (show)
		{
			if (this.progressBar == null)
			{
				this.progressBar = ProgressBar.CreateProgressBar(base.gameObject, new Func<float>(this.GetPercentComplete));
			}
			this.progressBar.SetVisibility(true);
			return;
		}
		if (this.progressBar != null)
		{
			this.progressBar.gameObject.DeleteObject();
			this.progressBar = null;
		}
	}

	// Token: 0x0600205A RID: 8282 RVA: 0x000AD810 File Offset: 0x000ABA10
	protected override void OnCleanUp()
	{
		this.ShowProgressBar(false);
		if (this.offsetTracker != null)
		{
			this.offsetTracker.Clear();
		}
		if (this.skillsUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.skillsUpdateHandle);
		}
		if (this.minionUpdateHandle != -1)
		{
			Game.Instance.Unsubscribe(this.minionUpdateHandle);
		}
		base.OnCleanUp();
		this.OnWorkableEventCB = null;
	}

	// Token: 0x0600205B RID: 8283 RVA: 0x000AD878 File Offset: 0x000ABA78
	public virtual Vector3 GetTargetPoint()
	{
		Vector3 vector = base.transform.GetPosition();
		float y = vector.y + 0.65f;
		KBoxCollider2D component = base.GetComponent<KBoxCollider2D>();
		if (component != null)
		{
			vector = component.bounds.center;
		}
		vector.y = y;
		vector.z = 0f;
		return vector;
	}

	// Token: 0x0600205C RID: 8284 RVA: 0x000AD8D2 File Offset: 0x000ABAD2
	public int GetNavigationCost(Navigator navigator, int cell)
	{
		return navigator.GetNavigationCost(cell, this.GetOffsets(cell));
	}

	// Token: 0x0600205D RID: 8285 RVA: 0x000AD8E2 File Offset: 0x000ABAE2
	public int GetNavigationCost(Navigator navigator)
	{
		return this.GetNavigationCost(navigator, Grid.PosToCell(this));
	}

	// Token: 0x0600205E RID: 8286 RVA: 0x000AD8F1 File Offset: 0x000ABAF1
	private void TransferDiseaseWithWorker(Worker worker)
	{
		if (this == null || worker == null)
		{
			return;
		}
		Workable.TransferDiseaseWithWorker(base.gameObject, worker.gameObject);
	}

	// Token: 0x0600205F RID: 8287 RVA: 0x000AD918 File Offset: 0x000ABB18
	public static void TransferDiseaseWithWorker(GameObject workable, GameObject worker)
	{
		if (workable == null || worker == null)
		{
			return;
		}
		PrimaryElement component = workable.GetComponent<PrimaryElement>();
		if (component == null)
		{
			return;
		}
		PrimaryElement component2 = worker.GetComponent<PrimaryElement>();
		if (component2 == null)
		{
			return;
		}
		SimUtil.DiseaseInfo invalid = SimUtil.DiseaseInfo.Invalid;
		invalid.idx = component2.DiseaseIdx;
		invalid.count = (int)((float)component2.DiseaseCount * 0.33f);
		SimUtil.DiseaseInfo invalid2 = SimUtil.DiseaseInfo.Invalid;
		invalid2.idx = component.DiseaseIdx;
		invalid2.count = (int)((float)component.DiseaseCount * 0.33f);
		component2.ModifyDiseaseCount(-invalid.count, "Workable.TransferDiseaseWithWorker");
		component.ModifyDiseaseCount(-invalid2.count, "Workable.TransferDiseaseWithWorker");
		if (invalid.count > 0)
		{
			component.AddDisease(invalid.idx, invalid.count, "Workable.TransferDiseaseWithWorker");
		}
		if (invalid2.count > 0)
		{
			component2.AddDisease(invalid2.idx, invalid2.count, "Workable.TransferDiseaseWithWorker");
		}
	}

	// Token: 0x06002060 RID: 8288 RVA: 0x000ADA10 File Offset: 0x000ABC10
	public virtual bool InstantlyFinish(Worker worker)
	{
		float num = worker.workable.WorkTimeRemaining;
		if (!float.IsInfinity(num))
		{
			worker.Work(num);
			return true;
		}
		DebugUtil.DevAssert(false, this.ToString() + " was asked to instantly finish but it has infinite work time! Override InstantlyFinish in your workable!", null);
		return false;
	}

	// Token: 0x06002061 RID: 8289 RVA: 0x000ADA54 File Offset: 0x000ABC54
	public virtual List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> list = new List<Descriptor>();
		if (this.trackUses)
		{
			Descriptor item = new Descriptor(string.Format(BUILDING.DETAILS.USE_COUNT, this.numberOfUses), string.Format(BUILDING.DETAILS.USE_COUNT_TOOLTIP, this.numberOfUses), Descriptor.DescriptorType.Detail, false);
			list.Add(item);
		}
		return list;
	}

	// Token: 0x06002062 RID: 8290 RVA: 0x000ADAB4 File Offset: 0x000ABCB4
	public virtual BuildingFacade GetBuildingFacade()
	{
		return base.GetComponent<BuildingFacade>();
	}

	// Token: 0x06002063 RID: 8291 RVA: 0x000ADABC File Offset: 0x000ABCBC
	[ContextMenu("Refresh Reachability")]
	public void RefreshReachability()
	{
		if (this.offsetTracker != null)
		{
			this.offsetTracker.ForceRefresh();
		}
	}

	// Token: 0x040011F0 RID: 4592
	public float workTime;

	// Token: 0x040011F1 RID: 4593
	protected bool showProgressBar = true;

	// Token: 0x040011F2 RID: 4594
	public bool alwaysShowProgressBar;

	// Token: 0x040011F3 RID: 4595
	protected bool lightEfficiencyBonus = true;

	// Token: 0x040011F4 RID: 4596
	protected Guid lightEfficiencyBonusStatusItemHandle;

	// Token: 0x040011F5 RID: 4597
	public bool currentlyLit;

	// Token: 0x040011F6 RID: 4598
	public Tag laboratoryEfficiencyBonusTagRequired = RoomConstraints.ConstraintTags.ScienceBuilding;

	// Token: 0x040011F7 RID: 4599
	private bool useLaboratoryEfficiencyBonus;

	// Token: 0x040011F8 RID: 4600
	protected Guid laboratoryEfficiencyBonusStatusItemHandle;

	// Token: 0x040011F9 RID: 4601
	private bool currentlyInLaboratory;

	// Token: 0x040011FA RID: 4602
	protected StatusItem workerStatusItem;

	// Token: 0x040011FB RID: 4603
	protected StatusItem workingStatusItem;

	// Token: 0x040011FC RID: 4604
	protected Guid workStatusItemHandle;

	// Token: 0x040011FD RID: 4605
	protected OffsetTracker offsetTracker;

	// Token: 0x040011FE RID: 4606
	[SerializeField]
	protected string attributeConverterId;

	// Token: 0x040011FF RID: 4607
	protected AttributeConverter attributeConverter;

	// Token: 0x04001200 RID: 4608
	protected float minimumAttributeMultiplier = 0.5f;

	// Token: 0x04001201 RID: 4609
	public bool resetProgressOnStop;

	// Token: 0x04001202 RID: 4610
	protected bool shouldTransferDiseaseWithWorker = true;

	// Token: 0x04001203 RID: 4611
	[SerializeField]
	protected float attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;

	// Token: 0x04001204 RID: 4612
	[SerializeField]
	protected string skillExperienceSkillGroup;

	// Token: 0x04001205 RID: 4613
	[SerializeField]
	protected float skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;

	// Token: 0x04001206 RID: 4614
	public bool triggerWorkReactions = true;

	// Token: 0x04001207 RID: 4615
	public ReportManager.ReportType reportType = ReportManager.ReportType.WorkTime;

	// Token: 0x04001208 RID: 4616
	[SerializeField]
	[Tooltip("What layer does the dupe switch to when interacting with the building")]
	public Grid.SceneLayer workLayer = Grid.SceneLayer.Move;

	// Token: 0x04001209 RID: 4617
	[SerializeField]
	[Serialize]
	protected float workTimeRemaining = float.PositiveInfinity;

	// Token: 0x0400120A RID: 4618
	[SerializeField]
	public KAnimFile[] overrideAnims;

	// Token: 0x0400120B RID: 4619
	[SerializeField]
	protected HashedString multitoolContext;

	// Token: 0x0400120C RID: 4620
	[SerializeField]
	protected Tag multitoolHitEffectTag;

	// Token: 0x0400120D RID: 4621
	[SerializeField]
	[Tooltip("Whether to user the KAnimSynchronizer or not")]
	public bool synchronizeAnims = true;

	// Token: 0x0400120E RID: 4622
	[SerializeField]
	[Tooltip("Whether to display number of uses in the details panel")]
	public bool trackUses;

	// Token: 0x0400120F RID: 4623
	[Serialize]
	protected int numberOfUses;

	// Token: 0x04001210 RID: 4624
	public Action<Workable, Workable.WorkableEvent> OnWorkableEventCB;

	// Token: 0x04001211 RID: 4625
	protected int skillsUpdateHandle = -1;

	// Token: 0x04001212 RID: 4626
	private int minionUpdateHandle = -1;

	// Token: 0x04001213 RID: 4627
	public string requiredSkillPerk;

	// Token: 0x04001214 RID: 4628
	[SerializeField]
	protected bool shouldShowSkillPerkStatusItem = true;

	// Token: 0x04001215 RID: 4629
	[SerializeField]
	public bool requireMinionToWork;

	// Token: 0x04001216 RID: 4630
	protected StatusItem readyForSkillWorkStatusItem;

	// Token: 0x04001217 RID: 4631
	public HashedString[] workAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04001218 RID: 4632
	public HashedString[] workingPstComplete = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04001219 RID: 4633
	public HashedString[] workingPstFailed = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x0400121A RID: 4634
	public KAnim.PlayMode workAnimPlayMode;

	// Token: 0x0400121B RID: 4635
	public bool faceTargetWhenWorking;

	// Token: 0x0400121C RID: 4636
	private static readonly EventSystem.IntraObjectHandler<Workable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Workable>(delegate(Workable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x0400121D RID: 4637
	protected ProgressBar progressBar;

	// Token: 0x020011DC RID: 4572
	public enum WorkableEvent
	{
		// Token: 0x04005DDA RID: 24026
		WorkStarted,
		// Token: 0x04005DDB RID: 24027
		WorkCompleted,
		// Token: 0x04005DDC RID: 24028
		WorkStopped
	}

	// Token: 0x020011DD RID: 4573
	public struct AnimInfo
	{
		// Token: 0x04005DDD RID: 24029
		public KAnimFile[] overrideAnims;

		// Token: 0x04005DDE RID: 24030
		public StateMachine.Instance smi;
	}
}
