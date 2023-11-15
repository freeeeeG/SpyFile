using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200085D RID: 2141
[AddComponentMenu("KMonoBehaviour/Workable/MedicinalPillWorkable")]
public class MedicinalPillWorkable : Workable, IConsumableUIItem
{
	// Token: 0x06003E97 RID: 16023 RVA: 0x0015B77C File Offset: 0x0015997C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(10f);
		this.showProgressBar = false;
		this.synchronizeAnims = false;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, Db.Get().BuildingStatusItems.Normal, null);
		this.CreateChore();
	}

	// Token: 0x06003E98 RID: 16024 RVA: 0x0015B7DC File Offset: 0x001599DC
	protected override void OnCompleteWork(Worker worker)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = worker.GetComponent<Effects>();
			EffectInstance effectInstance = component.Get(this.pill.info.effect);
			if (effectInstance != null)
			{
				effectInstance.timeRemaining = effectInstance.effect.duration;
			}
			else
			{
				component.Add(this.pill.info.effect, true);
			}
		}
		Sicknesses sicknesses = worker.GetSicknesses();
		foreach (string id in this.pill.info.curedSicknesses)
		{
			SicknessInstance sicknessInstance = sicknesses.Get(id);
			if (sicknessInstance != null)
			{
				Game.Instance.savedInfo.curedDisease = true;
				sicknessInstance.Cure();
			}
		}
		base.gameObject.DeleteObject();
	}

	// Token: 0x06003E99 RID: 16025 RVA: 0x0015B8D0 File Offset: 0x00159AD0
	private void CreateChore()
	{
		new TakeMedicineChore(this);
	}

	// Token: 0x06003E9A RID: 16026 RVA: 0x0015B8DC File Offset: 0x00159ADC
	public bool CanBeTakenBy(GameObject consumer)
	{
		if (!string.IsNullOrEmpty(this.pill.info.effect))
		{
			Effects component = consumer.GetComponent<Effects>();
			if (component == null || component.HasEffect(this.pill.info.effect))
			{
				return false;
			}
		}
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.Booster)
		{
			return true;
		}
		Sicknesses sicknesses = consumer.GetSicknesses();
		if (this.pill.info.medicineType == MedicineInfo.MedicineType.CureAny && sicknesses.Count > 0)
		{
			return true;
		}
		foreach (SicknessInstance sicknessInstance in sicknesses)
		{
			if (this.pill.info.curedSicknesses.Contains(sicknessInstance.modifier.Id))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x17000467 RID: 1127
	// (get) Token: 0x06003E9B RID: 16027 RVA: 0x0015B9C4 File Offset: 0x00159BC4
	public string ConsumableId
	{
		get
		{
			return this.PrefabID().Name;
		}
	}

	// Token: 0x17000468 RID: 1128
	// (get) Token: 0x06003E9C RID: 16028 RVA: 0x0015B9DF File Offset: 0x00159BDF
	public string ConsumableName
	{
		get
		{
			return this.GetProperName();
		}
	}

	// Token: 0x17000469 RID: 1129
	// (get) Token: 0x06003E9D RID: 16029 RVA: 0x0015B9E7 File Offset: 0x00159BE7
	public int MajorOrder
	{
		get
		{
			return (int)(this.pill.info.medicineType + 1000);
		}
	}

	// Token: 0x1700046A RID: 1130
	// (get) Token: 0x06003E9E RID: 16030 RVA: 0x0015B9FF File Offset: 0x00159BFF
	public int MinorOrder
	{
		get
		{
			return 0;
		}
	}

	// Token: 0x1700046B RID: 1131
	// (get) Token: 0x06003E9F RID: 16031 RVA: 0x0015BA02 File Offset: 0x00159C02
	public bool Display
	{
		get
		{
			return true;
		}
	}

	// Token: 0x04002893 RID: 10387
	public MedicinalPill pill;
}
