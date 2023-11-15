using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x0200068A RID: 1674
[AddComponentMenu("KMonoBehaviour/Workable/SingleEntityReceptacle")]
public class SingleEntityReceptacle : Workable, IRender1000ms
{
	// Token: 0x170002F3 RID: 755
	// (get) Token: 0x06002CB4 RID: 11444 RVA: 0x000EDAB3 File Offset: 0x000EBCB3
	public FetchChore GetActiveRequest
	{
		get
		{
			return this.fetchChore;
		}
	}

	// Token: 0x170002F4 RID: 756
	// (get) Token: 0x06002CB5 RID: 11445 RVA: 0x000EDABB File Offset: 0x000EBCBB
	// (set) Token: 0x06002CB6 RID: 11446 RVA: 0x000EDAE2 File Offset: 0x000EBCE2
	protected GameObject occupyingObject
	{
		get
		{
			if (this.occupyObjectRef.Get() != null)
			{
				return this.occupyObjectRef.Get().gameObject;
			}
			return null;
		}
		set
		{
			if (value == null)
			{
				this.occupyObjectRef.Set(null);
				return;
			}
			this.occupyObjectRef.Set(value.GetComponent<KSelectable>());
		}
	}

	// Token: 0x170002F5 RID: 757
	// (get) Token: 0x06002CB7 RID: 11447 RVA: 0x000EDB0B File Offset: 0x000EBD0B
	public GameObject Occupant
	{
		get
		{
			return this.occupyingObject;
		}
	}

	// Token: 0x170002F6 RID: 758
	// (get) Token: 0x06002CB8 RID: 11448 RVA: 0x000EDB13 File Offset: 0x000EBD13
	public IReadOnlyList<Tag> possibleDepositObjectTags
	{
		get
		{
			return this.possibleDepositTagsList;
		}
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x000EDB1B File Offset: 0x000EBD1B
	public bool HasDepositTag(Tag tag)
	{
		return this.possibleDepositTagsList.Contains(tag);
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x000EDB2C File Offset: 0x000EBD2C
	public bool IsValidEntity(GameObject candidate)
	{
		IReceptacleDirection component = candidate.GetComponent<IReceptacleDirection>();
		bool flag = this.rotatable != null || component == null || component.Direction == this.Direction;
		int num = 0;
		while (flag && num < this.additionalCriteria.Count)
		{
			flag = this.additionalCriteria[num](candidate);
			num++;
		}
		return flag;
	}

	// Token: 0x170002F7 RID: 759
	// (get) Token: 0x06002CBB RID: 11451 RVA: 0x000EDB90 File Offset: 0x000EBD90
	public SingleEntityReceptacle.ReceptacleDirection Direction
	{
		get
		{
			return this.direction;
		}
	}

	// Token: 0x06002CBC RID: 11452 RVA: 0x000EDB98 File Offset: 0x000EBD98
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002CBD RID: 11453 RVA: 0x000EDBA0 File Offset: 0x000EBDA0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.occupyingObject != null)
		{
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		this.UpdateStatusItem();
		if (this.occupyingObject == null && !this.requestedEntityTag.IsValid)
		{
			this.requestedEntityAdditionalFilterTag = null;
		}
		if (this.occupyingObject == null && this.requestedEntityTag.IsValid)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
		base.Subscribe<SingleEntityReceptacle>(-592767678, SingleEntityReceptacle.OnOperationalChangedDelegate);
	}

	// Token: 0x06002CBE RID: 11454 RVA: 0x000EDC38 File Offset: 0x000EBE38
	public void AddDepositTag(Tag t)
	{
		this.possibleDepositTagsList.Add(t);
	}

	// Token: 0x06002CBF RID: 11455 RVA: 0x000EDC46 File Offset: 0x000EBE46
	public void AddAdditionalCriteria(Func<GameObject, bool> criteria)
	{
		this.additionalCriteria.Add(criteria);
	}

	// Token: 0x06002CC0 RID: 11456 RVA: 0x000EDC54 File Offset: 0x000EBE54
	public void SetReceptacleDirection(SingleEntityReceptacle.ReceptacleDirection d)
	{
		this.direction = d;
	}

	// Token: 0x06002CC1 RID: 11457 RVA: 0x000EDC5D File Offset: 0x000EBE5D
	public virtual void SetPreview(Tag entityTag, bool solid = false)
	{
	}

	// Token: 0x06002CC2 RID: 11458 RVA: 0x000EDC5F File Offset: 0x000EBE5F
	public virtual void CreateOrder(Tag entityTag, Tag additionalFilterTag)
	{
		this.requestedEntityTag = entityTag;
		this.requestedEntityAdditionalFilterTag = additionalFilterTag;
		this.CreateFetchChore(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		this.SetPreview(entityTag, true);
		this.UpdateStatusItem();
	}

	// Token: 0x06002CC3 RID: 11459 RVA: 0x000EDC8F File Offset: 0x000EBE8F
	public void Render1000ms(float dt)
	{
		this.UpdateStatusItem();
	}

	// Token: 0x06002CC4 RID: 11460 RVA: 0x000EDC98 File Offset: 0x000EBE98
	protected virtual void UpdateStatusItem()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.Occupant != null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, null, null);
			return;
		}
		if (this.fetchChore == null)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNeed, null);
			return;
		}
		bool flag = this.fetchChore.fetcher != null;
		WorldContainer myWorld = this.GetMyWorld();
		if (!flag && myWorld != null)
		{
			foreach (Tag tag in this.fetchChore.tags)
			{
				if (myWorld.worldInventory.GetTotalAmount(tag, true) > 0f)
				{
					if (myWorld.worldInventory.GetTotalAmount(this.requestedEntityAdditionalFilterTag, true) > 0f || this.requestedEntityAdditionalFilterTag == Tag.Invalid)
					{
						flag = true;
						break;
					}
					break;
				}
			}
		}
		if (flag)
		{
			component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemAwaitingDelivery, null);
			return;
		}
		component.SetStatusItem(Db.Get().StatusItemCategories.EntityReceptacle, this.statusItemNoneAvailable, null);
	}

	// Token: 0x06002CC5 RID: 11461 RVA: 0x000EDDEC File Offset: 0x000EBFEC
	protected void CreateFetchChore(Tag entityTag, Tag additionalRequiredTag)
	{
		if (this.fetchChore == null && entityTag.IsValid && entityTag != GameTags.Empty)
		{
			this.fetchChore = new FetchChore(this.choreType, this.storage, 1f, new HashSet<Tag>
			{
				entityTag
			}, FetchChore.MatchCriteria.MatchID, (additionalRequiredTag.IsValid && additionalRequiredTag != GameTags.Empty) ? additionalRequiredTag : Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, delegate(Chore chore)
			{
				this.UpdateStatusItem();
			}, Operational.State.Functional, 0);
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, 1f, base.gameObject.GetMyWorldId());
			this.UpdateStatusItem();
		}
	}

	// Token: 0x06002CC6 RID: 11462 RVA: 0x000EDEB2 File Offset: 0x000EC0B2
	public virtual void OrderRemoveOccupant()
	{
		this.ClearOccupant();
	}

	// Token: 0x06002CC7 RID: 11463 RVA: 0x000EDEBC File Offset: 0x000EC0BC
	protected virtual void ClearOccupant()
	{
		if (this.occupyingObject)
		{
			this.UnsubscribeFromOccupant();
			this.storage.DropAll(false, false, default(Vector3), true, null);
		}
		this.occupyingObject = null;
		this.UpdateActive();
		this.UpdateStatusItem();
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x06002CC8 RID: 11464 RVA: 0x000EDF18 File Offset: 0x000EC118
	public void CancelActiveRequest()
	{
		if (this.fetchChore != null)
		{
			MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
			this.fetchChore.Cancel("User canceled");
			this.fetchChore = null;
		}
		this.requestedEntityTag = Tag.Invalid;
		this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		this.UpdateStatusItem();
		this.SetPreview(Tag.Invalid, false);
	}

	// Token: 0x06002CC9 RID: 11465 RVA: 0x000EDF88 File Offset: 0x000EC188
	private void OnOccupantDestroyed(object data)
	{
		this.occupyingObject = null;
		this.ClearOccupant();
		if (this.autoReplaceEntity && this.requestedEntityTag.IsValid && this.requestedEntityTag != GameTags.Empty)
		{
			this.CreateOrder(this.requestedEntityTag, this.requestedEntityAdditionalFilterTag);
		}
	}

	// Token: 0x06002CCA RID: 11466 RVA: 0x000EDFDB File Offset: 0x000EC1DB
	protected virtual void SubscribeToOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Subscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x06002CCB RID: 11467 RVA: 0x000EE009 File Offset: 0x000EC209
	protected virtual void UnsubscribeFromOccupant()
	{
		if (this.occupyingObject != null)
		{
			base.Unsubscribe(this.occupyingObject, 1969584890, new Action<object>(this.OnOccupantDestroyed));
		}
	}

	// Token: 0x06002CCC RID: 11468 RVA: 0x000EE038 File Offset: 0x000EC238
	private void OnFetchComplete(Chore chore)
	{
		if (this.fetchChore == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore null", new object[]
			{
				base.gameObject
			});
			return;
		}
		if (this.fetchChore.fetchTarget == null)
		{
			global::Debug.LogWarningFormat(base.gameObject, "{0} OnFetchComplete fetchChore.fetchTarget null", new object[]
			{
				base.gameObject
			});
			return;
		}
		this.OnDepositObject(this.fetchChore.fetchTarget.gameObject);
	}

	// Token: 0x06002CCD RID: 11469 RVA: 0x000EE0B6 File Offset: 0x000EC2B6
	public void ForceDeposit(GameObject depositedObject)
	{
		if (this.occupyingObject != null)
		{
			this.ClearOccupant();
		}
		this.OnDepositObject(depositedObject);
	}

	// Token: 0x06002CCE RID: 11470 RVA: 0x000EE0D4 File Offset: 0x000EC2D4
	private void OnDepositObject(GameObject depositedObject)
	{
		this.SetPreview(Tag.Invalid, false);
		MaterialNeeds.UpdateNeed(this.requestedEntityTag, -1f, base.gameObject.GetMyWorldId());
		KBatchedAnimController component = depositedObject.GetComponent<KBatchedAnimController>();
		if (component != null)
		{
			component.GetBatchInstanceData().ClearOverrideTransformMatrix();
		}
		this.occupyingObject = this.SpawnOccupyingObject(depositedObject);
		if (this.occupyingObject != null)
		{
			this.ConfigureOccupyingObject(this.occupyingObject);
			this.occupyingObject.SetActive(true);
			this.PositionOccupyingObject();
			this.SubscribeToOccupant();
		}
		else
		{
			global::Debug.LogWarning(base.gameObject.name + " EntityReceptacle did not spawn occupying entity.");
		}
		if (this.fetchChore != null)
		{
			this.fetchChore.Cancel("receptacle filled");
			this.fetchChore = null;
		}
		if (!this.autoReplaceEntity)
		{
			this.requestedEntityTag = Tag.Invalid;
			this.requestedEntityAdditionalFilterTag = Tag.Invalid;
		}
		this.UpdateActive();
		this.UpdateStatusItem();
		if (this.destroyEntityOnDeposit)
		{
			Util.KDestroyGameObject(depositedObject);
		}
		base.Trigger(-731304873, this.occupyingObject);
	}

	// Token: 0x06002CCF RID: 11471 RVA: 0x000EE1E6 File Offset: 0x000EC3E6
	protected virtual GameObject SpawnOccupyingObject(GameObject depositedEntity)
	{
		return depositedEntity;
	}

	// Token: 0x06002CD0 RID: 11472 RVA: 0x000EE1E9 File Offset: 0x000EC3E9
	protected virtual void ConfigureOccupyingObject(GameObject source)
	{
	}

	// Token: 0x06002CD1 RID: 11473 RVA: 0x000EE1EC File Offset: 0x000EC3EC
	protected virtual void PositionOccupyingObject()
	{
		if (this.rotatable != null)
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + this.rotatable.GetRotatedOffset(this.occupyingObjectRelativePosition));
		}
		else
		{
			this.occupyingObject.transform.SetPosition(base.gameObject.transform.GetPosition() + this.occupyingObjectRelativePosition);
		}
		KBatchedAnimController component = this.occupyingObject.GetComponent<KBatchedAnimController>();
		component.enabled = false;
		component.enabled = true;
	}

	// Token: 0x06002CD2 RID: 11474 RVA: 0x000EE284 File Offset: 0x000EC484
	protected void UpdateActive()
	{
		if (this.Equals(null) || this == null || base.gameObject.Equals(null) || base.gameObject == null)
		{
			return;
		}
		if (this.operational != null)
		{
			this.operational.SetActive(this.operational.IsOperational && this.occupyingObject != null, false);
		}
	}

	// Token: 0x06002CD3 RID: 11475 RVA: 0x000EE2F6 File Offset: 0x000EC4F6
	protected override void OnCleanUp()
	{
		this.CancelActiveRequest();
		this.UnsubscribeFromOccupant();
		base.OnCleanUp();
	}

	// Token: 0x06002CD4 RID: 11476 RVA: 0x000EE30A File Offset: 0x000EC50A
	private void OnOperationalChanged(object data)
	{
		this.UpdateActive();
		if (this.occupyingObject)
		{
			this.occupyingObject.Trigger(this.operational.IsOperational ? 1628751838 : 960378201, null);
		}
	}

	// Token: 0x04001A58 RID: 6744
	[MyCmpGet]
	protected Operational operational;

	// Token: 0x04001A59 RID: 6745
	[MyCmpReq]
	protected Storage storage;

	// Token: 0x04001A5A RID: 6746
	[MyCmpGet]
	public Rotatable rotatable;

	// Token: 0x04001A5B RID: 6747
	protected FetchChore fetchChore;

	// Token: 0x04001A5C RID: 6748
	public ChoreType choreType = Db.Get().ChoreTypes.Fetch;

	// Token: 0x04001A5D RID: 6749
	[Serialize]
	public bool autoReplaceEntity;

	// Token: 0x04001A5E RID: 6750
	[Serialize]
	public Tag requestedEntityTag;

	// Token: 0x04001A5F RID: 6751
	[Serialize]
	public Tag requestedEntityAdditionalFilterTag;

	// Token: 0x04001A60 RID: 6752
	[Serialize]
	protected Ref<KSelectable> occupyObjectRef = new Ref<KSelectable>();

	// Token: 0x04001A61 RID: 6753
	[SerializeField]
	private List<Tag> possibleDepositTagsList = new List<Tag>();

	// Token: 0x04001A62 RID: 6754
	[SerializeField]
	private List<Func<GameObject, bool>> additionalCriteria = new List<Func<GameObject, bool>>();

	// Token: 0x04001A63 RID: 6755
	[SerializeField]
	protected bool destroyEntityOnDeposit;

	// Token: 0x04001A64 RID: 6756
	[SerializeField]
	protected SingleEntityReceptacle.ReceptacleDirection direction;

	// Token: 0x04001A65 RID: 6757
	public Vector3 occupyingObjectRelativePosition = new Vector3(0f, 1f, 3f);

	// Token: 0x04001A66 RID: 6758
	protected StatusItem statusItemAwaitingDelivery;

	// Token: 0x04001A67 RID: 6759
	protected StatusItem statusItemNeed;

	// Token: 0x04001A68 RID: 6760
	protected StatusItem statusItemNoneAvailable;

	// Token: 0x04001A69 RID: 6761
	private static readonly EventSystem.IntraObjectHandler<SingleEntityReceptacle> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<SingleEntityReceptacle>(delegate(SingleEntityReceptacle component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x0200139D RID: 5021
	public enum ReceptacleDirection
	{
		// Token: 0x040062F9 RID: 25337
		Top,
		// Token: 0x040062FA RID: 25338
		Side,
		// Token: 0x040062FB RID: 25339
		Bottom
	}
}
