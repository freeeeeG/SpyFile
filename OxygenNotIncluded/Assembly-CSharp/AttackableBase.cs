using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200047F RID: 1151
[AddComponentMenu("KMonoBehaviour/Workable/AttackableBase")]
public class AttackableBase : Workable, IApproachable
{
	// Token: 0x06001946 RID: 6470 RVA: 0x000846D0 File Offset: 0x000828D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.attributeConverter = Db.Get().AttributeConverters.AttackDamage;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Mining.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
		this.SetupScenePartitioner(null);
		base.Subscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate);
		GameUtil.SubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate, true);
		base.Subscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate);
		base.Subscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate);
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x00084770 File Offset: 0x00082970
	public float GetDamageMultiplier()
	{
		if (this.attributeConverter != null && base.worker != null)
		{
			AttributeConverterInstance converter = base.worker.GetComponent<AttributeConverters>().GetConverter(this.attributeConverter.Id);
			return Mathf.Max(1f + converter.Evaluate(), 0.1f);
		}
		return 1f;
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x000847CC File Offset: 0x000829CC
	private void SetupScenePartitioner(object data = null)
	{
		Extents extents = new Extents(Grid.PosToXY(base.transform.GetPosition()).x, Grid.PosToXY(base.transform.GetPosition()).y, 1, 1);
		this.scenePartitionerEntry = GameScenePartitioner.Instance.Add(base.gameObject.name, base.GetComponent<FactionAlignment>(), extents, GameScenePartitioner.Instance.attackableEntitiesLayer, null);
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x00084839 File Offset: 0x00082A39
	private void OnDefeated(object data = null)
	{
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x0008484B File Offset: 0x00082A4B
	public override float GetEfficiencyMultiplier(Worker worker)
	{
		return 1f;
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x00084854 File Offset: 0x00082A54
	protected override void OnCleanUp()
	{
		base.Unsubscribe<AttackableBase>(1088554450, AttackableBase.OnCellChangedDelegate, false);
		GameUtil.UnsubscribeToTags<AttackableBase>(this, AttackableBase.OnDeadTagAddedDelegate);
		base.Unsubscribe<AttackableBase>(-1506500077, AttackableBase.OnDefeatedDelegate, false);
		base.Unsubscribe<AttackableBase>(-1256572400, AttackableBase.SetupScenePartitionerDelegate, false);
		GameScenePartitioner.Instance.Free(ref this.scenePartitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x04000DF1 RID: 3569
	private HandleVector<int>.Handle scenePartitionerEntry;

	// Token: 0x04000DF2 RID: 3570
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDeadTagAddedDelegate = GameUtil.CreateHasTagHandler<AttackableBase>(GameTags.Dead, delegate(AttackableBase component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x04000DF3 RID: 3571
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnDefeatedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		component.OnDefeated(data);
	});

	// Token: 0x04000DF4 RID: 3572
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> SetupScenePartitionerDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		component.SetupScenePartitioner(data);
	});

	// Token: 0x04000DF5 RID: 3573
	private static readonly EventSystem.IntraObjectHandler<AttackableBase> OnCellChangedDelegate = new EventSystem.IntraObjectHandler<AttackableBase>(delegate(AttackableBase component, object data)
	{
		GameScenePartitioner.Instance.UpdatePosition(component.scenePartitionerEntry, Grid.PosToCell(component.gameObject));
	});
}
