using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DE5 RID: 3557
	public class Sicknesses : Modifications<Sickness, SicknessInstance>
	{
		// Token: 0x06006D7B RID: 28027 RVA: 0x002B278F File Offset: 0x002B098F
		public Sicknesses(GameObject go) : base(go, Db.Get().Sicknesses)
		{
		}

		// Token: 0x06006D7C RID: 28028 RVA: 0x002B27A4 File Offset: 0x002B09A4
		public void Infect(SicknessExposureInfo exposure_info)
		{
			Sickness modifier = Db.Get().Sicknesses.Get(exposure_info.sicknessID);
			if (!base.Has(modifier))
			{
				this.CreateInstance(modifier).ExposureInfo = exposure_info;
			}
		}

		// Token: 0x06006D7D RID: 28029 RVA: 0x002B27E0 File Offset: 0x002B09E0
		public override SicknessInstance CreateInstance(Sickness sickness)
		{
			SicknessInstance sicknessInstance = new SicknessInstance(base.gameObject, sickness);
			this.Add(sicknessInstance);
			base.Trigger(GameHashes.SicknessAdded, sicknessInstance);
			ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, 1f, base.gameObject.GetProperName(), null);
			return sicknessInstance;
		}

		// Token: 0x06006D7E RID: 28030 RVA: 0x002B282B File Offset: 0x002B0A2B
		public bool IsInfected()
		{
			return base.Count > 0;
		}

		// Token: 0x06006D7F RID: 28031 RVA: 0x002B2836 File Offset: 0x002B0A36
		public bool Cure(Sickness sickness)
		{
			return this.Cure(sickness.Id);
		}

		// Token: 0x06006D80 RID: 28032 RVA: 0x002B2844 File Offset: 0x002B0A44
		public bool Cure(string sickness_id)
		{
			SicknessInstance sicknessInstance = null;
			foreach (SicknessInstance sicknessInstance2 in this)
			{
				if (sicknessInstance2.modifier.Id == sickness_id)
				{
					sicknessInstance = sicknessInstance2;
					break;
				}
			}
			if (sicknessInstance != null)
			{
				this.Remove(sicknessInstance);
				base.Trigger(GameHashes.SicknessCured, sicknessInstance);
				ReportManager.Instance.ReportValue(ReportManager.ReportType.DiseaseAdded, -1f, base.gameObject.GetProperName(), null);
				return true;
			}
			return false;
		}
	}
}
