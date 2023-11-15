using System;
using System.Collections.Generic;
using System.Diagnostics;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DE3 RID: 3555
	[DebuggerDisplay("{base.Id}")]
	public abstract class Sickness : Resource
	{
		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06006D5F RID: 27999 RVA: 0x002B1DF9 File Offset: 0x002AFFF9
		public new string Name
		{
			get
			{
				return Strings.Get(this.name);
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06006D60 RID: 28000 RVA: 0x002B1E0B File Offset: 0x002B000B
		public float SicknessDuration
		{
			get
			{
				return this.sicknessDuration;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06006D61 RID: 28001 RVA: 0x002B1E13 File Offset: 0x002B0013
		public StringKey DescriptiveSymptoms
		{
			get
			{
				return this.descriptiveSymptoms;
			}
		}

		// Token: 0x06006D62 RID: 28002 RVA: 0x002B1E1C File Offset: 0x002B001C
		public Sickness(string id, Sickness.SicknessType type, Sickness.Severity severity, float immune_attack_strength, List<Sickness.InfectionVector> infection_vectors, float sickness_duration, string recovery_effect = null) : base(id, null, null)
		{
			this.name = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".NAME");
			this.id = id;
			this.sicknessType = type;
			this.severity = severity;
			this.infectionVectors = infection_vectors;
			this.sicknessDuration = sickness_duration;
			this.recoveryEffect = recovery_effect;
			this.descriptiveSymptoms = new StringKey("STRINGS.DUPLICANTS.DISEASES." + id.ToUpper() + ".DESCRIPTIVE_SYMPTOMS");
			this.cureSpeedBase = new Attribute(id + "CureSpeed", false, Attribute.Display.Normal, false, 0f, null, null, null, null);
			this.cureSpeedBase.BaseValue = 1f;
			this.cureSpeedBase.SetFormatter(new ToPercentAttributeFormatter(1f, GameUtil.TimeSlice.None));
			Db.Get().Attributes.Add(this.cureSpeedBase);
		}

		// Token: 0x06006D63 RID: 28003 RVA: 0x002B1F18 File Offset: 0x002B0118
		public object[] Infect(GameObject go, SicknessInstance diseaseInstance, SicknessExposureInfo exposure_info)
		{
			object[] array = new object[this.components.Count];
			for (int i = 0; i < this.components.Count; i++)
			{
				array[i] = this.components[i].OnInfect(go, diseaseInstance);
			}
			return array;
		}

		// Token: 0x06006D64 RID: 28004 RVA: 0x002B1F64 File Offset: 0x002B0164
		public void Cure(GameObject go, object[] componentData)
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				this.components[i].OnCure(go, componentData[i]);
			}
		}

		// Token: 0x06006D65 RID: 28005 RVA: 0x002B1F9C File Offset: 0x002B019C
		public List<Descriptor> GetSymptoms()
		{
			List<Descriptor> list = new List<Descriptor>();
			for (int i = 0; i < this.components.Count; i++)
			{
				List<Descriptor> symptoms = this.components[i].GetSymptoms();
				if (symptoms != null)
				{
					list.AddRange(symptoms);
				}
			}
			if (this.fatalityDuration > 0f)
			{
				list.Add(new Descriptor(string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), string.Format(DUPLICANTS.DISEASES.DEATH_SYMPTOM_TOOLTIP, GameUtil.GetFormattedCycles(this.fatalityDuration, "F1", false)), Descriptor.DescriptorType.SymptomAidable, false));
			}
			return list;
		}

		// Token: 0x06006D66 RID: 28006 RVA: 0x002B203C File Offset: 0x002B023C
		protected void AddSicknessComponent(Sickness.SicknessComponent cmp)
		{
			this.components.Add(cmp);
		}

		// Token: 0x06006D67 RID: 28007 RVA: 0x002B204C File Offset: 0x002B024C
		public T GetSicknessComponent<T>() where T : Sickness.SicknessComponent
		{
			for (int i = 0; i < this.components.Count; i++)
			{
				if (this.components[i] is T)
				{
					return this.components[i] as T;
				}
			}
			return default(T);
		}

		// Token: 0x06006D68 RID: 28008 RVA: 0x002B20A2 File Offset: 0x002B02A2
		public virtual List<Descriptor> GetSicknessSourceDescriptors()
		{
			return new List<Descriptor>();
		}

		// Token: 0x06006D69 RID: 28009 RVA: 0x002B20AC File Offset: 0x002B02AC
		public List<Descriptor> GetQualitativeDescriptors()
		{
			List<Descriptor> list = new List<Descriptor>();
			using (List<Sickness.InfectionVector>.Enumerator enumerator = this.infectionVectors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					switch (enumerator.Current)
					{
					case Sickness.InfectionVector.Contact:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SKINBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Digestion:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.FOODBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Inhalation:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.AIRBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					case Sickness.InfectionVector.Exposure:
						list.Add(new Descriptor(DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE, DUPLICANTS.DISEASES.DESCRIPTORS.INFO.SUNBORNE_TOOLTIP, Descriptor.DescriptorType.Information, false));
						break;
					}
				}
			}
			list.Add(new Descriptor(Strings.Get(this.descriptiveSymptoms), "", Descriptor.DescriptorType.Information, false));
			return list;
		}

		// Token: 0x04005205 RID: 20997
		private StringKey name;

		// Token: 0x04005206 RID: 20998
		private StringKey descriptiveSymptoms;

		// Token: 0x04005207 RID: 20999
		private float sicknessDuration = 600f;

		// Token: 0x04005208 RID: 21000
		public float fatalityDuration;

		// Token: 0x04005209 RID: 21001
		public HashedString id;

		// Token: 0x0400520A RID: 21002
		public Sickness.SicknessType sicknessType;

		// Token: 0x0400520B RID: 21003
		public Sickness.Severity severity;

		// Token: 0x0400520C RID: 21004
		public string recoveryEffect;

		// Token: 0x0400520D RID: 21005
		public List<Sickness.InfectionVector> infectionVectors;

		// Token: 0x0400520E RID: 21006
		private List<Sickness.SicknessComponent> components = new List<Sickness.SicknessComponent>();

		// Token: 0x0400520F RID: 21007
		public Amount amount;

		// Token: 0x04005210 RID: 21008
		public Attribute amountDeltaAttribute;

		// Token: 0x04005211 RID: 21009
		public Attribute cureSpeedBase;

		// Token: 0x02001F61 RID: 8033
		public abstract class SicknessComponent
		{
			// Token: 0x0600A24B RID: 41547
			public abstract object OnInfect(GameObject go, SicknessInstance diseaseInstance);

			// Token: 0x0600A24C RID: 41548
			public abstract void OnCure(GameObject go, object instance_data);

			// Token: 0x0600A24D RID: 41549 RVA: 0x00363C6B File Offset: 0x00361E6B
			public virtual List<Descriptor> GetSymptoms()
			{
				return null;
			}
		}

		// Token: 0x02001F62 RID: 8034
		public enum InfectionVector
		{
			// Token: 0x04008DF6 RID: 36342
			Contact,
			// Token: 0x04008DF7 RID: 36343
			Digestion,
			// Token: 0x04008DF8 RID: 36344
			Inhalation,
			// Token: 0x04008DF9 RID: 36345
			Exposure
		}

		// Token: 0x02001F63 RID: 8035
		public enum SicknessType
		{
			// Token: 0x04008DFB RID: 36347
			Pathogen,
			// Token: 0x04008DFC RID: 36348
			Ailment,
			// Token: 0x04008DFD RID: 36349
			Injury
		}

		// Token: 0x02001F64 RID: 8036
		public enum Severity
		{
			// Token: 0x04008DFF RID: 36351
			Benign,
			// Token: 0x04008E00 RID: 36352
			Minor,
			// Token: 0x04008E01 RID: 36353
			Major,
			// Token: 0x04008E02 RID: 36354
			Critical
		}
	}
}
