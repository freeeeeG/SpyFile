using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020007F9 RID: 2041
public class HighEnergyParticleStorage : KMonoBehaviour, IStorage
{
	// Token: 0x17000438 RID: 1080
	// (get) Token: 0x06003A35 RID: 14901 RVA: 0x00144341 File Offset: 0x00142541
	public float Particles
	{
		get
		{
			return this.particles;
		}
	}

	// Token: 0x17000439 RID: 1081
	// (get) Token: 0x06003A36 RID: 14902 RVA: 0x00144349 File Offset: 0x00142549
	// (set) Token: 0x06003A37 RID: 14903 RVA: 0x00144351 File Offset: 0x00142551
	public bool allowUIItemRemoval { get; set; }

	// Token: 0x06003A38 RID: 14904 RVA: 0x0014435C File Offset: 0x0014255C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Combine(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
			component.onParticleCaptureAllowed = (HighEnergyParticlePort.OnParticleCaptureAllowed)Delegate.Combine(component.onParticleCaptureAllowed, new HighEnergyParticlePort.OnParticleCaptureAllowed(this.OnParticleCaptureAllowed));
		}
		this.SetupStorageStatusItems();
	}

	// Token: 0x06003A39 RID: 14905 RVA: 0x001443CB File Offset: 0x001425CB
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.UpdateLogicPorts();
	}

	// Token: 0x06003A3A RID: 14906 RVA: 0x001443DC File Offset: 0x001425DC
	private void UpdateLogicPorts()
	{
		if (this._logicPorts != null)
		{
			bool value = this.IsFull();
			this._logicPorts.SendSignal(this.PORT_ID, Convert.ToInt32(value));
		}
	}

	// Token: 0x06003A3B RID: 14907 RVA: 0x0014441A File Offset: 0x0014261A
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		if (this.autoStore)
		{
			HighEnergyParticlePort component = base.gameObject.GetComponent<HighEnergyParticlePort>();
			component.onParticleCapture = (HighEnergyParticlePort.OnParticleCapture)Delegate.Remove(component.onParticleCapture, new HighEnergyParticlePort.OnParticleCapture(this.OnParticleCapture));
		}
	}

	// Token: 0x06003A3C RID: 14908 RVA: 0x00144458 File Offset: 0x00142658
	private void OnParticleCapture(HighEnergyParticle particle)
	{
		float num = Mathf.Min(particle.payload, this.capacity - this.particles);
		this.Store(num);
		particle.payload -= num;
		if (particle.payload > 0f)
		{
			base.gameObject.GetComponent<HighEnergyParticlePort>().Uncapture(particle);
		}
	}

	// Token: 0x06003A3D RID: 14909 RVA: 0x001444B2 File Offset: 0x001426B2
	private bool OnParticleCaptureAllowed(HighEnergyParticle particle)
	{
		return this.particles < this.capacity && this.receiverOpen;
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x001444CC File Offset: 0x001426CC
	private void DeltaParticles(float delta)
	{
		this.particles += delta;
		if (this.particles <= 0f)
		{
			base.Trigger(155636535, base.transform.gameObject);
		}
		base.Trigger(-1837862626, base.transform.gameObject);
		this.UpdateLogicPorts();
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x00144528 File Offset: 0x00142728
	public float Store(float amount)
	{
		float num = Mathf.Min(amount, this.RemainingCapacity());
		this.DeltaParticles(num);
		return num;
	}

	// Token: 0x06003A40 RID: 14912 RVA: 0x0014454A File Offset: 0x0014274A
	public float ConsumeAndGet(float amount)
	{
		amount = Mathf.Min(this.Particles, amount);
		this.DeltaParticles(-amount);
		return amount;
	}

	// Token: 0x06003A41 RID: 14913 RVA: 0x00144563 File Offset: 0x00142763
	[ContextMenu("Trigger Stored Event")]
	public void DEBUG_TriggerStorageEvent()
	{
		base.Trigger(-1837862626, base.transform.gameObject);
	}

	// Token: 0x06003A42 RID: 14914 RVA: 0x0014457B File Offset: 0x0014277B
	[ContextMenu("Trigger Zero Event")]
	public void DEBUG_TriggerZeroEvent()
	{
		this.ConsumeAndGet(this.particles + 1f);
	}

	// Token: 0x06003A43 RID: 14915 RVA: 0x00144590 File Offset: 0x00142790
	public float ConsumeAll()
	{
		return this.ConsumeAndGet(this.particles);
	}

	// Token: 0x06003A44 RID: 14916 RVA: 0x0014459E File Offset: 0x0014279E
	public bool HasRadiation()
	{
		return this.Particles > 0f;
	}

	// Token: 0x06003A45 RID: 14917 RVA: 0x001445AD File Offset: 0x001427AD
	public GameObject Drop(GameObject go, bool do_disease_transfer = true)
	{
		return null;
	}

	// Token: 0x06003A46 RID: 14918 RVA: 0x001445B0 File Offset: 0x001427B0
	public List<GameObject> GetItems()
	{
		return new List<GameObject>
		{
			base.gameObject
		};
	}

	// Token: 0x06003A47 RID: 14919 RVA: 0x001445C3 File Offset: 0x001427C3
	public bool IsFull()
	{
		return this.RemainingCapacity() <= 0f;
	}

	// Token: 0x06003A48 RID: 14920 RVA: 0x001445D5 File Offset: 0x001427D5
	public bool IsEmpty()
	{
		return this.Particles == 0f;
	}

	// Token: 0x06003A49 RID: 14921 RVA: 0x001445E4 File Offset: 0x001427E4
	public float Capacity()
	{
		return this.capacity;
	}

	// Token: 0x06003A4A RID: 14922 RVA: 0x001445EC File Offset: 0x001427EC
	public float RemainingCapacity()
	{
		return Mathf.Max(this.capacity - this.Particles, 0f);
	}

	// Token: 0x06003A4B RID: 14923 RVA: 0x00144605 File Offset: 0x00142805
	public bool ShouldShowInUI()
	{
		return this.showInUI;
	}

	// Token: 0x06003A4C RID: 14924 RVA: 0x0014460D File Offset: 0x0014280D
	public float GetAmountAvailable(Tag tag)
	{
		if (tag != GameTags.HighEnergyParticle)
		{
			return 0f;
		}
		return this.Particles;
	}

	// Token: 0x06003A4D RID: 14925 RVA: 0x00144628 File Offset: 0x00142828
	public void ConsumeIgnoringDisease(Tag tag, float amount)
	{
		DebugUtil.DevAssert(tag == GameTags.HighEnergyParticle, "Consuming non-particle tag as amount", null);
		this.ConsumeAndGet(amount);
	}

	// Token: 0x06003A4E RID: 14926 RVA: 0x00144648 File Offset: 0x00142848
	private void SetupStorageStatusItems()
	{
		if (HighEnergyParticleStorage.capacityStatusItem == null)
		{
			HighEnergyParticleStorage.capacityStatusItem = new StatusItem("StorageLocker", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.None.ID, true, 129022, null);
			HighEnergyParticleStorage.capacityStatusItem.resolveStringCallback = delegate(string str, object data)
			{
				HighEnergyParticleStorage highEnergyParticleStorage = (HighEnergyParticleStorage)data;
				string newValue = Util.FormatWholeNumber(highEnergyParticleStorage.particles);
				string newValue2 = Util.FormatWholeNumber(highEnergyParticleStorage.capacity);
				str = str.Replace("{Stored}", newValue);
				str = str.Replace("{Capacity}", newValue2);
				str = str.Replace("{Units}", UI.UNITSUFFIXES.HIGHENERGYPARTICLES.PARTRICLES);
				return str;
			};
		}
		if (this.showCapacityStatusItem)
		{
			if (this.showCapacityAsMainStatus)
			{
				base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, HighEnergyParticleStorage.capacityStatusItem, this);
				return;
			}
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Stored, HighEnergyParticleStorage.capacityStatusItem, this);
		}
	}

	// Token: 0x040026C1 RID: 9921
	[Serialize]
	[SerializeField]
	private float particles;

	// Token: 0x040026C2 RID: 9922
	public float capacity = float.MaxValue;

	// Token: 0x040026C3 RID: 9923
	public bool showInUI = true;

	// Token: 0x040026C4 RID: 9924
	public bool showCapacityStatusItem;

	// Token: 0x040026C5 RID: 9925
	public bool showCapacityAsMainStatus;

	// Token: 0x040026C7 RID: 9927
	public bool autoStore;

	// Token: 0x040026C8 RID: 9928
	[Serialize]
	public bool receiverOpen = true;

	// Token: 0x040026C9 RID: 9929
	[MyCmpGet]
	private LogicPorts _logicPorts;

	// Token: 0x040026CA RID: 9930
	public string PORT_ID = "";

	// Token: 0x040026CB RID: 9931
	private static StatusItem capacityStatusItem;
}
