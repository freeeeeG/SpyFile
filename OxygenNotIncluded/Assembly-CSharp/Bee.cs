using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000484 RID: 1156
public class Bee : KMonoBehaviour
{
	// Token: 0x06001969 RID: 6505 RVA: 0x00084F20 File Offset: 0x00083120
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<Bee>(-739654666, Bee.OnAttackDelegate);
		base.Subscribe<Bee>(-1283701846, Bee.OnSleepDelegate);
		base.Subscribe<Bee>(-2090444759, Bee.OnWakeUpDelegate);
		base.Subscribe<Bee>(1623392196, Bee.OnDeathDelegate);
		base.Subscribe<Bee>(1890751808, Bee.OnHappyDelegate);
		base.Subscribe<Bee>(-647798969, Bee.OnUnhappyDelegate);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("tag", false);
		base.GetComponent<KBatchedAnimController>().SetSymbolVisiblity("snapto_tag", false);
		this.StopSleep();
	}

	// Token: 0x0600196A RID: 6506 RVA: 0x00084FCC File Offset: 0x000831CC
	private void OnDeath(object data)
	{
		PrimaryElement component = base.GetComponent<PrimaryElement>();
		Storage component2 = base.GetComponent<Storage>();
		byte index = Db.Get().Diseases.GetIndex(Db.Get().Diseases.RadiationPoisoning.id);
		component2.AddOre(SimHashes.NuclearWaste, BeeTuning.WASTE_DROPPED_ON_DEATH, component.Temperature, index, BeeTuning.GERMS_DROPPED_ON_DEATH, false, true);
		component2.DropAll(base.transform.position, true, true, default(Vector3), true, null);
	}

	// Token: 0x0600196B RID: 6507 RVA: 0x00085046 File Offset: 0x00083246
	private void StartSleep()
	{
		this.RemoveRadiationMod(this.awakeRadiationModKey);
		base.GetComponent<ElementConsumer>().EnableConsumption(true);
	}

	// Token: 0x0600196C RID: 6508 RVA: 0x00085060 File Offset: 0x00083260
	private void StopSleep()
	{
		this.AddRadiationModifier(this.awakeRadiationModKey, this.awakeRadiationMod);
		base.GetComponent<ElementConsumer>().EnableConsumption(false);
	}

	// Token: 0x0600196D RID: 6509 RVA: 0x00085080 File Offset: 0x00083280
	private void AddRadiationModifier(HashedString name, float mod)
	{
		this.radiationModifiers.Add(name, mod);
		this.RefreshRadiationOutput();
	}

	// Token: 0x0600196E RID: 6510 RVA: 0x00085095 File Offset: 0x00083295
	private void RemoveRadiationMod(HashedString name)
	{
		this.radiationModifiers.Remove(name);
		this.RefreshRadiationOutput();
	}

	// Token: 0x0600196F RID: 6511 RVA: 0x000850AC File Offset: 0x000832AC
	public void RefreshRadiationOutput()
	{
		float num = this.radiationOutputAmount;
		foreach (KeyValuePair<HashedString, float> keyValuePair in this.radiationModifiers)
		{
			num *= keyValuePair.Value;
		}
		RadiationEmitter component = base.GetComponent<RadiationEmitter>();
		component.SetEmitting(true);
		component.emitRads = num;
		component.Refresh();
	}

	// Token: 0x06001970 RID: 6512 RVA: 0x00085124 File Offset: 0x00083324
	private void OnAttack(object data)
	{
		if ((Tag)data == GameTags.Creatures.Attack)
		{
			base.GetComponent<Health>().Damage(base.GetComponent<Health>().hitPoints);
		}
	}

	// Token: 0x06001971 RID: 6513 RVA: 0x00085150 File Offset: 0x00083350
	public KPrefabID FindHiveInRoom()
	{
		List<BeeHive.StatesInstance> list = new List<BeeHive.StatesInstance>();
		Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(base.gameObject);
		foreach (BeeHive.StatesInstance statesInstance in Components.BeeHives.Items)
		{
			if (Game.Instance.roomProber.GetRoomOfGameObject(statesInstance.gameObject) == roomOfGameObject)
			{
				list.Add(statesInstance);
			}
		}
		int num = int.MaxValue;
		KPrefabID result = null;
		foreach (BeeHive.StatesInstance statesInstance2 in list)
		{
			int navigationCost = base.gameObject.GetComponent<Navigator>().GetNavigationCost(Grid.PosToCell(statesInstance2.transform.GetLocalPosition()));
			if (navigationCost < num)
			{
				num = navigationCost;
				result = statesInstance2.GetComponent<KPrefabID>();
			}
		}
		return result;
	}

	// Token: 0x04000E07 RID: 3591
	public float radiationOutputAmount;

	// Token: 0x04000E08 RID: 3592
	private Dictionary<HashedString, float> radiationModifiers = new Dictionary<HashedString, float>();

	// Token: 0x04000E09 RID: 3593
	private float unhappyRadiationMod = 0.1f;

	// Token: 0x04000E0A RID: 3594
	private float awakeRadiationMod;

	// Token: 0x04000E0B RID: 3595
	private HashedString unhappyRadiationModKey = "UNHAPPY";

	// Token: 0x04000E0C RID: 3596
	private HashedString awakeRadiationModKey = "AWAKE";

	// Token: 0x04000E0D RID: 3597
	private static readonly EventSystem.IntraObjectHandler<Bee> OnAttackDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.OnAttack(data);
	});

	// Token: 0x04000E0E RID: 3598
	private static readonly EventSystem.IntraObjectHandler<Bee> OnSleepDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.StartSleep();
	});

	// Token: 0x04000E0F RID: 3599
	private static readonly EventSystem.IntraObjectHandler<Bee> OnWakeUpDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.StopSleep();
	});

	// Token: 0x04000E10 RID: 3600
	private static readonly EventSystem.IntraObjectHandler<Bee> OnDeathDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.OnDeath(data);
	});

	// Token: 0x04000E11 RID: 3601
	private static readonly EventSystem.IntraObjectHandler<Bee> OnHappyDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.RemoveRadiationMod(component.unhappyRadiationModKey);
	});

	// Token: 0x04000E12 RID: 3602
	private static readonly EventSystem.IntraObjectHandler<Bee> OnUnhappyDelegate = new EventSystem.IntraObjectHandler<Bee>(delegate(Bee component, object data)
	{
		component.AddRadiationModifier(component.unhappyRadiationModKey, component.unhappyRadiationMod);
	});
}
