using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000490 RID: 1168
[AddComponentMenu("KMonoBehaviour/Workable/ComplexFabricatorWorkable")]
public class ComplexFabricatorWorkable : Workable
{
	// Token: 0x170000EA RID: 234
	// (get) Token: 0x06001A3C RID: 6716 RVA: 0x0008B53D File Offset: 0x0008973D
	// (set) Token: 0x06001A3D RID: 6717 RVA: 0x0008B545 File Offset: 0x00089745
	public StatusItem WorkerStatusItem
	{
		get
		{
			return this.workerStatusItem;
		}
		set
		{
			this.workerStatusItem = value;
		}
	}

	// Token: 0x170000EB RID: 235
	// (get) Token: 0x06001A3E RID: 6718 RVA: 0x0008B54E File Offset: 0x0008974E
	// (set) Token: 0x06001A3F RID: 6719 RVA: 0x0008B556 File Offset: 0x00089756
	public AttributeConverter AttributeConverter
	{
		get
		{
			return this.attributeConverter;
		}
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x170000EC RID: 236
	// (get) Token: 0x06001A40 RID: 6720 RVA: 0x0008B55F File Offset: 0x0008975F
	// (set) Token: 0x06001A41 RID: 6721 RVA: 0x0008B567 File Offset: 0x00089767
	public float AttributeExperienceMultiplier
	{
		get
		{
			return this.attributeExperienceMultiplier;
		}
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x170000ED RID: 237
	// (set) Token: 0x06001A42 RID: 6722 RVA: 0x0008B570 File Offset: 0x00089770
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x170000EE RID: 238
	// (set) Token: 0x06001A43 RID: 6723 RVA: 0x0008B579 File Offset: 0x00089779
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x170000EF RID: 239
	// (get) Token: 0x06001A44 RID: 6724 RVA: 0x0008B582 File Offset: 0x00089782
	public ComplexRecipe CurrentWorkingOrder
	{
		get
		{
			if (!(this.fabricator != null))
			{
				return null;
			}
			return this.fabricator.CurrentWorkingOrder;
		}
	}

	// Token: 0x06001A45 RID: 6725 RVA: 0x0008B5A0 File Offset: 0x000897A0
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Fabricating;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x06001A46 RID: 6726 RVA: 0x0008B610 File Offset: 0x00089810
	public override string GetConversationTopic()
	{
		string conversationTopic = this.fabricator.GetConversationTopic();
		if (conversationTopic == null)
		{
			return base.GetConversationTopic();
		}
		return conversationTopic;
	}

	// Token: 0x06001A47 RID: 6727 RVA: 0x0008B634 File Offset: 0x00089834
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		if (this.fabricator.CurrentWorkingOrder != null)
		{
			this.InstantiateVisualizer(this.fabricator.CurrentWorkingOrder);
			return;
		}
		DebugUtil.DevAssertArgs(false, new object[]
		{
			"ComplexFabricatorWorkable.OnStartWork called but CurrentMachineOrder is null",
			base.gameObject
		});
	}

	// Token: 0x06001A48 RID: 6728 RVA: 0x0008B692 File Offset: 0x00089892
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (this.OnWorkTickActions != null)
		{
			this.OnWorkTickActions(worker, dt);
		}
		this.UpdateOrderProgress(worker, dt);
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06001A49 RID: 6729 RVA: 0x0008B6BC File Offset: 0x000898BC
	public override float GetWorkTime()
	{
		ComplexRecipe currentWorkingOrder = this.fabricator.CurrentWorkingOrder;
		if (currentWorkingOrder != null)
		{
			this.workTime = currentWorkingOrder.time;
			return this.workTime;
		}
		return -1f;
	}

	// Token: 0x06001A4A RID: 6730 RVA: 0x0008B6F0 File Offset: 0x000898F0
	public Chore CreateWorkChore(ChoreType choreType, float order_progress)
	{
		Chore result = new WorkChore<ComplexFabricatorWorkable>(choreType, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		this.workTimeRemaining = this.GetWorkTime() * (1f - order_progress);
		return result;
	}

	// Token: 0x06001A4B RID: 6731 RVA: 0x0008B729 File Offset: 0x00089929
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.fabricator.CompleteWorkingOrder();
		this.DestroyVisualizer();
	}

	// Token: 0x06001A4C RID: 6732 RVA: 0x0008B744 File Offset: 0x00089944
	private void InstantiateVisualizer(ComplexRecipe recipe)
	{
		if (this.visualizer != null)
		{
			this.DestroyVisualizer();
		}
		if (this.visualizerLink != null)
		{
			this.visualizerLink.Unregister();
			this.visualizerLink = null;
		}
		if (recipe.FabricationVisualizer == null)
		{
			return;
		}
		this.visualizer = Util.KInstantiate(recipe.FabricationVisualizer, null, null);
		this.visualizer.transform.parent = this.meter.meterController.transform;
		this.visualizer.transform.SetLocalPosition(new Vector3(0f, 0f, 1f));
		this.visualizer.SetActive(true);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController component2 = this.visualizer.GetComponent<KBatchedAnimController>();
		this.visualizerLink = new KAnimLink(component, component2);
	}

	// Token: 0x06001A4D RID: 6733 RVA: 0x0008B814 File Offset: 0x00089A14
	private void UpdateOrderProgress(Worker worker, float dt)
	{
		float workTime = this.GetWorkTime();
		float num = Mathf.Clamp01((workTime - base.WorkTimeRemaining) / workTime);
		if (this.fabricator)
		{
			this.fabricator.OrderProgress = num;
		}
		if (this.meter != null)
		{
			this.meter.SetPositionPercent(num);
		}
	}

	// Token: 0x06001A4E RID: 6734 RVA: 0x0008B865 File Offset: 0x00089A65
	private void DestroyVisualizer()
	{
		if (this.visualizer != null)
		{
			if (this.visualizerLink != null)
			{
				this.visualizerLink.Unregister();
				this.visualizerLink = null;
			}
			Util.KDestroyGameObject(this.visualizer);
			this.visualizer = null;
		}
	}

	// Token: 0x04000E8B RID: 3723
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04000E8C RID: 3724
	[MyCmpReq]
	private ComplexFabricator fabricator;

	// Token: 0x04000E8D RID: 3725
	public Action<Worker, float> OnWorkTickActions;

	// Token: 0x04000E8E RID: 3726
	public MeterController meter;

	// Token: 0x04000E8F RID: 3727
	protected GameObject visualizer;

	// Token: 0x04000E90 RID: 3728
	protected KAnimLink visualizerLink;
}
