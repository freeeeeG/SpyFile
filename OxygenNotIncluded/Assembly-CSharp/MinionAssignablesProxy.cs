using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;
using UnityEngine;

// Token: 0x02000598 RID: 1432
[AddComponentMenu("KMonoBehaviour/scripts/MinionAssignablesProxy")]
public class MinionAssignablesProxy : KMonoBehaviour, IAssignableIdentity
{
	// Token: 0x17000198 RID: 408
	// (get) Token: 0x060022DD RID: 8925 RVA: 0x000BF2AB File Offset: 0x000BD4AB
	// (set) Token: 0x060022DE RID: 8926 RVA: 0x000BF2B3 File Offset: 0x000BD4B3
	public IAssignableIdentity target { get; private set; }

	// Token: 0x17000199 RID: 409
	// (get) Token: 0x060022DF RID: 8927 RVA: 0x000BF2BC File Offset: 0x000BD4BC
	public bool IsConfigured
	{
		get
		{
			return this.slotsConfigured;
		}
	}

	// Token: 0x1700019A RID: 410
	// (get) Token: 0x060022E0 RID: 8928 RVA: 0x000BF2C4 File Offset: 0x000BD4C4
	public int TargetInstanceID
	{
		get
		{
			return this.target_instance_id;
		}
	}

	// Token: 0x060022E1 RID: 8929 RVA: 0x000BF2CC File Offset: 0x000BD4CC
	public GameObject GetTargetGameObject()
	{
		if (this.target == null && this.target_instance_id != -1)
		{
			this.RestoreTargetFromInstanceID();
		}
		KMonoBehaviour kmonoBehaviour = (KMonoBehaviour)this.target;
		if (kmonoBehaviour != null)
		{
			return kmonoBehaviour.gameObject;
		}
		return null;
	}

	// Token: 0x060022E2 RID: 8930 RVA: 0x000BF310 File Offset: 0x000BD510
	public float GetArrivalTime()
	{
		if (this.GetTargetGameObject().GetComponent<MinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<MinionIdentity>().arrivalTime;
		}
		if (this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().arrivalTime;
		}
		global::Debug.LogError("Could not get minion arrival time");
		return -1f;
	}

	// Token: 0x060022E3 RID: 8931 RVA: 0x000BF374 File Offset: 0x000BD574
	public int GetTotalSkillpoints()
	{
		if (this.GetTargetGameObject().GetComponent<MinionIdentity>() != null)
		{
			return this.GetTargetGameObject().GetComponent<MinionResume>().TotalSkillPointsGained;
		}
		if (this.GetTargetGameObject().GetComponent<StoredMinionIdentity>() != null)
		{
			return MinionResume.CalculateTotalSkillPointsGained(this.GetTargetGameObject().GetComponent<StoredMinionIdentity>().TotalExperienceGained);
		}
		global::Debug.LogError("Could not get minion skill points time");
		return -1;
	}

	// Token: 0x060022E4 RID: 8932 RVA: 0x000BF3DC File Offset: 0x000BD5DC
	public void SetTarget(IAssignableIdentity target, GameObject targetGO)
	{
		global::Debug.Assert(target != null, "target was null");
		if (targetGO == null)
		{
			global::Debug.LogWarningFormat("{0} MinionAssignablesProxy.SetTarget {1}, {2}, {3}. DESTROYING", new object[]
			{
				base.GetInstanceID(),
				this.target_instance_id,
				target,
				targetGO
			});
			Util.KDestroyGameObject(base.gameObject);
		}
		this.target = target;
		this.target_instance_id = targetGO.GetComponent<KPrefabID>().InstanceID;
		base.gameObject.name = "Minion Assignables Proxy : " + targetGO.name;
	}

	// Token: 0x060022E5 RID: 8933 RVA: 0x000BF474 File Offset: 0x000BD674
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.ownables = new List<Ownables>
		{
			base.gameObject.AddOrGet<Ownables>()
		};
		Components.MinionAssignablesProxy.Add(this);
		base.Subscribe<MinionAssignablesProxy>(1502190696, MinionAssignablesProxy.OnQueueDestroyObjectDelegate);
		this.ConfigureAssignableSlots();
	}

	// Token: 0x060022E6 RID: 8934 RVA: 0x000BF4C5 File Offset: 0x000BD6C5
	[OnDeserialized]
	private void OnDeserialized()
	{
	}

	// Token: 0x060022E7 RID: 8935 RVA: 0x000BF4C8 File Offset: 0x000BD6C8
	public void ConfigureAssignableSlots()
	{
		if (this.slotsConfigured)
		{
			return;
		}
		Ownables component = base.GetComponent<Ownables>();
		Equipment component2 = base.GetComponent<Equipment>();
		if (component2 != null)
		{
			foreach (AssignableSlot assignableSlot in Db.Get().AssignableSlots.resources)
			{
				if (assignableSlot is OwnableSlot)
				{
					OwnableSlotInstance slot_instance = new OwnableSlotInstance(component, (OwnableSlot)assignableSlot);
					component.Add(slot_instance);
				}
				else if (assignableSlot is EquipmentSlot)
				{
					EquipmentSlotInstance slot_instance2 = new EquipmentSlotInstance(component2, (EquipmentSlot)assignableSlot);
					component2.Add(slot_instance2);
				}
			}
		}
		this.slotsConfigured = true;
	}

	// Token: 0x060022E8 RID: 8936 RVA: 0x000BF584 File Offset: 0x000BD784
	public void RestoreTargetFromInstanceID()
	{
		if (this.target_instance_id != -1 && this.target == null)
		{
			KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
			if (instance)
			{
				IAssignableIdentity component = instance.GetComponent<IAssignableIdentity>();
				if (component != null)
				{
					this.SetTarget(component, instance.gameObject);
					return;
				}
				global::Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was found but it wasn't an IAssignableIdentity, destroying proxy object.", new object[]
				{
					this.target_instance_id
				});
				Util.KDestroyGameObject(base.gameObject);
				return;
			}
			else
			{
				global::Debug.LogWarningFormat("RestoreTargetFromInstanceID target ID {0} was not found, destroying proxy object.", new object[]
				{
					this.target_instance_id
				});
				Util.KDestroyGameObject(base.gameObject);
			}
		}
	}

	// Token: 0x060022E9 RID: 8937 RVA: 0x000BF62C File Offset: 0x000BD82C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreTargetFromInstanceID();
		if (this.target != null)
		{
			base.Subscribe<MinionAssignablesProxy>(-1585839766, MinionAssignablesProxy.OnAssignablesChangedDelegate);
			Game.Instance.assignmentManager.AddToAssignmentGroup("public", this);
		}
	}

	// Token: 0x060022EA RID: 8938 RVA: 0x000BF668 File Offset: 0x000BD868
	private void OnQueueDestroyObject(object data)
	{
		Components.MinionAssignablesProxy.Remove(this);
	}

	// Token: 0x060022EB RID: 8939 RVA: 0x000BF675 File Offset: 0x000BD875
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Game.Instance.assignmentManager.RemoveFromAllGroups(this);
		base.GetComponent<Ownables>().UnassignAll();
		base.GetComponent<Equipment>().UnequipAll();
	}

	// Token: 0x060022EC RID: 8940 RVA: 0x000BF6A3 File Offset: 0x000BD8A3
	private void OnAssignablesChanged(object data)
	{
		if (!this.target.IsNull())
		{
			((KMonoBehaviour)this.target).Trigger(-1585839766, data);
		}
	}

	// Token: 0x060022ED RID: 8941 RVA: 0x000BF6C8 File Offset: 0x000BD8C8
	private void CheckTarget()
	{
		if (this.target == null)
		{
			KPrefabID instance = KPrefabIDTracker.Get().GetInstance(this.target_instance_id);
			if (instance != null)
			{
				this.target = instance.GetComponent<IAssignableIdentity>();
				if (this.target != null)
				{
					MinionIdentity minionIdentity = this.target as MinionIdentity;
					if (minionIdentity)
					{
						minionIdentity.ValidateProxy();
						return;
					}
					StoredMinionIdentity storedMinionIdentity = this.target as StoredMinionIdentity;
					if (storedMinionIdentity)
					{
						storedMinionIdentity.ValidateProxy();
					}
				}
			}
		}
	}

	// Token: 0x060022EE RID: 8942 RVA: 0x000BF740 File Offset: 0x000BD940
	public List<Ownables> GetOwners()
	{
		this.CheckTarget();
		return this.target.GetOwners();
	}

	// Token: 0x060022EF RID: 8943 RVA: 0x000BF753 File Offset: 0x000BD953
	public string GetProperName()
	{
		this.CheckTarget();
		return this.target.GetProperName();
	}

	// Token: 0x060022F0 RID: 8944 RVA: 0x000BF766 File Offset: 0x000BD966
	public Ownables GetSoleOwner()
	{
		this.CheckTarget();
		return this.target.GetSoleOwner();
	}

	// Token: 0x060022F1 RID: 8945 RVA: 0x000BF779 File Offset: 0x000BD979
	public bool HasOwner(Assignables owner)
	{
		this.CheckTarget();
		return this.target.HasOwner(owner);
	}

	// Token: 0x060022F2 RID: 8946 RVA: 0x000BF78D File Offset: 0x000BD98D
	public int NumOwners()
	{
		this.CheckTarget();
		return this.target.NumOwners();
	}

	// Token: 0x060022F3 RID: 8947 RVA: 0x000BF7A0 File Offset: 0x000BD9A0
	public bool IsNull()
	{
		this.CheckTarget();
		return this.target.IsNull();
	}

	// Token: 0x060022F4 RID: 8948 RVA: 0x000BF7B4 File Offset: 0x000BD9B4
	public static Ref<MinionAssignablesProxy> InitAssignableProxy(Ref<MinionAssignablesProxy> assignableProxyRef, IAssignableIdentity source)
	{
		if (assignableProxyRef == null)
		{
			assignableProxyRef = new Ref<MinionAssignablesProxy>();
		}
		GameObject gameObject = ((KMonoBehaviour)source).gameObject;
		MinionAssignablesProxy minionAssignablesProxy = assignableProxyRef.Get();
		if (minionAssignablesProxy == null)
		{
			GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(MinionAssignablesProxyConfig.ID), Grid.SceneLayer.NoLayer, null, 0);
			minionAssignablesProxy = gameObject2.GetComponent<MinionAssignablesProxy>();
			minionAssignablesProxy.SetTarget(source, gameObject);
			gameObject2.SetActive(true);
			assignableProxyRef.Set(minionAssignablesProxy);
		}
		else
		{
			minionAssignablesProxy.SetTarget(source, gameObject);
		}
		return assignableProxyRef;
	}

	// Token: 0x040013F6 RID: 5110
	public List<Ownables> ownables;

	// Token: 0x040013F8 RID: 5112
	[Serialize]
	private int target_instance_id = -1;

	// Token: 0x040013F9 RID: 5113
	private bool slotsConfigured;

	// Token: 0x040013FA RID: 5114
	private static readonly EventSystem.IntraObjectHandler<MinionAssignablesProxy> OnAssignablesChangedDelegate = new EventSystem.IntraObjectHandler<MinionAssignablesProxy>(delegate(MinionAssignablesProxy component, object data)
	{
		component.OnAssignablesChanged(data);
	});

	// Token: 0x040013FB RID: 5115
	private static readonly EventSystem.IntraObjectHandler<MinionAssignablesProxy> OnQueueDestroyObjectDelegate = new EventSystem.IntraObjectHandler<MinionAssignablesProxy>(delegate(MinionAssignablesProxy component, object data)
	{
		component.OnQueueDestroyObject(data);
	});
}
