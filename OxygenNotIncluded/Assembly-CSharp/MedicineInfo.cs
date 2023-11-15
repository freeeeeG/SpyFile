using System;
using System.Collections.Generic;

// Token: 0x0200085B RID: 2139
[Serializable]
public class MedicineInfo
{
	// Token: 0x06003E90 RID: 16016 RVA: 0x0015B3E0 File Offset: 0x001595E0
	public MedicineInfo(string id, string effect, MedicineInfo.MedicineType medicineType, string doctorStationId, string[] curedDiseases = null)
	{
		Debug.Assert(!string.IsNullOrEmpty(effect) || (curedDiseases != null && curedDiseases.Length != 0), "Medicine should have an effect or cure diseases");
		this.id = id;
		this.effect = effect;
		this.medicineType = medicineType;
		this.doctorStationId = doctorStationId;
		if (curedDiseases != null)
		{
			this.curedSicknesses = new List<string>(curedDiseases);
			return;
		}
		this.curedSicknesses = new List<string>();
	}

	// Token: 0x06003E91 RID: 16017 RVA: 0x0015B44F File Offset: 0x0015964F
	public Tag GetSupplyTag()
	{
		return MedicineInfo.GetSupplyTagForStation(this.doctorStationId);
	}

	// Token: 0x06003E92 RID: 16018 RVA: 0x0015B45C File Offset: 0x0015965C
	public static Tag GetSupplyTagForStation(string stationID)
	{
		Tag tag = TagManager.Create(stationID + GameTags.MedicalSupplies.Name);
		Assets.AddCountableTag(tag);
		return tag;
	}

	// Token: 0x0400288D RID: 10381
	public string id;

	// Token: 0x0400288E RID: 10382
	public string effect;

	// Token: 0x0400288F RID: 10383
	public MedicineInfo.MedicineType medicineType;

	// Token: 0x04002890 RID: 10384
	public List<string> curedSicknesses;

	// Token: 0x04002891 RID: 10385
	public string doctorStationId;

	// Token: 0x0200162E RID: 5678
	public enum MedicineType
	{
		// Token: 0x04006ADC RID: 27356
		Booster,
		// Token: 0x04006ADD RID: 27357
		CureAny,
		// Token: 0x04006ADE RID: 27358
		CureSpecific
	}
}
