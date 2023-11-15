using System;
using System.Collections.Generic;

// Token: 0x02000497 RID: 1175
public class Diet
{
	// Token: 0x170000F2 RID: 242
	// (get) Token: 0x06001A81 RID: 6785 RVA: 0x0008D58C File Offset: 0x0008B78C
	// (set) Token: 0x06001A82 RID: 6786 RVA: 0x0008D594 File Offset: 0x0008B794
	public Diet.Info[] infos { get; private set; }

	// Token: 0x06001A83 RID: 6787 RVA: 0x0008D5A0 File Offset: 0x0008B7A0
	public Diet(params Diet.Info[] infos)
	{
		this.infos = infos;
		this.consumedTags = new List<KeyValuePair<Tag, float>>();
		this.producedTags = new List<KeyValuePair<Tag, float>>();
		for (int i = 0; i < infos.Length; i++)
		{
			Diet.Info info = infos[i];
			if (info.eatsPlantsDirectly)
			{
				this.eatsPlantsDirectly = true;
			}
			using (HashSet<Tag>.Enumerator enumerator = info.consumedTags.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Tag tag = enumerator.Current;
					if (-1 == this.consumedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == tag))
					{
						this.consumedTags.Add(new KeyValuePair<Tag, float>(tag, info.caloriesPerKg));
					}
					if (this.consumedTagToInfo.ContainsKey(tag))
					{
						string str = "Duplicate diet entry: ";
						Tag tag2 = tag;
						Debug.LogError(str + tag2.ToString());
					}
					this.consumedTagToInfo[tag] = info;
				}
			}
			if (info.producedElement != Tag.Invalid && -1 == this.producedTags.FindIndex((KeyValuePair<Tag, float> e) => e.Key == info.producedElement))
			{
				this.producedTags.Add(new KeyValuePair<Tag, float>(info.producedElement, info.producedConversionRate));
			}
		}
	}

	// Token: 0x06001A84 RID: 6788 RVA: 0x0008D750 File Offset: 0x0008B950
	public Diet.Info GetDietInfo(Tag tag)
	{
		Diet.Info result = null;
		this.consumedTagToInfo.TryGetValue(tag, out result);
		return result;
	}

	// Token: 0x04000EBB RID: 3771
	public List<KeyValuePair<Tag, float>> consumedTags;

	// Token: 0x04000EBC RID: 3772
	public List<KeyValuePair<Tag, float>> producedTags;

	// Token: 0x04000EBD RID: 3773
	public bool eatsPlantsDirectly;

	// Token: 0x04000EBE RID: 3774
	private Dictionary<Tag, Diet.Info> consumedTagToInfo = new Dictionary<Tag, Diet.Info>();

	// Token: 0x02001124 RID: 4388
	public class Info
	{
		// Token: 0x1700081C RID: 2076
		// (get) Token: 0x0600786F RID: 30831 RVA: 0x002D6679 File Offset: 0x002D4879
		// (set) Token: 0x06007870 RID: 30832 RVA: 0x002D6681 File Offset: 0x002D4881
		public HashSet<Tag> consumedTags { get; private set; }

		// Token: 0x1700081D RID: 2077
		// (get) Token: 0x06007871 RID: 30833 RVA: 0x002D668A File Offset: 0x002D488A
		// (set) Token: 0x06007872 RID: 30834 RVA: 0x002D6692 File Offset: 0x002D4892
		public Tag producedElement { get; private set; }

		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x06007873 RID: 30835 RVA: 0x002D669B File Offset: 0x002D489B
		// (set) Token: 0x06007874 RID: 30836 RVA: 0x002D66A3 File Offset: 0x002D48A3
		public float caloriesPerKg { get; private set; }

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x06007875 RID: 30837 RVA: 0x002D66AC File Offset: 0x002D48AC
		// (set) Token: 0x06007876 RID: 30838 RVA: 0x002D66B4 File Offset: 0x002D48B4
		public float producedConversionRate { get; private set; }

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x06007877 RID: 30839 RVA: 0x002D66BD File Offset: 0x002D48BD
		// (set) Token: 0x06007878 RID: 30840 RVA: 0x002D66C5 File Offset: 0x002D48C5
		public byte diseaseIdx { get; private set; }

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x06007879 RID: 30841 RVA: 0x002D66CE File Offset: 0x002D48CE
		// (set) Token: 0x0600787A RID: 30842 RVA: 0x002D66D6 File Offset: 0x002D48D6
		public float diseasePerKgProduced { get; private set; }

		// Token: 0x17000822 RID: 2082
		// (get) Token: 0x0600787B RID: 30843 RVA: 0x002D66DF File Offset: 0x002D48DF
		// (set) Token: 0x0600787C RID: 30844 RVA: 0x002D66E7 File Offset: 0x002D48E7
		public bool produceSolidTile { get; private set; }

		// Token: 0x17000823 RID: 2083
		// (get) Token: 0x0600787D RID: 30845 RVA: 0x002D66F0 File Offset: 0x002D48F0
		// (set) Token: 0x0600787E RID: 30846 RVA: 0x002D66F8 File Offset: 0x002D48F8
		public bool eatsPlantsDirectly { get; private set; }

		// Token: 0x0600787F RID: 30847 RVA: 0x002D6704 File Offset: 0x002D4904
		public Info(HashSet<Tag> consumed_tags, Tag produced_element, float calories_per_kg, float produced_conversion_rate = 1f, string disease_id = null, float disease_per_kg_produced = 0f, bool produce_solid_tile = false, bool eats_plants_directly = false)
		{
			this.consumedTags = consumed_tags;
			this.producedElement = produced_element;
			this.caloriesPerKg = calories_per_kg;
			this.producedConversionRate = produced_conversion_rate;
			if (!string.IsNullOrEmpty(disease_id))
			{
				this.diseaseIdx = Db.Get().Diseases.GetIndex(disease_id);
			}
			else
			{
				this.diseaseIdx = byte.MaxValue;
			}
			this.produceSolidTile = produce_solid_tile;
			this.eatsPlantsDirectly = eats_plants_directly;
		}

		// Token: 0x06007880 RID: 30848 RVA: 0x002D6776 File Offset: 0x002D4976
		public bool IsMatch(Tag tag)
		{
			return this.consumedTags.Contains(tag);
		}

		// Token: 0x06007881 RID: 30849 RVA: 0x002D6784 File Offset: 0x002D4984
		public bool IsMatch(HashSet<Tag> tags)
		{
			if (tags.Count < this.consumedTags.Count)
			{
				foreach (Tag item in tags)
				{
					if (this.consumedTags.Contains(item))
					{
						return true;
					}
				}
				return false;
			}
			foreach (Tag item2 in this.consumedTags)
			{
				if (tags.Contains(item2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007882 RID: 30850 RVA: 0x002D6840 File Offset: 0x002D4A40
		public float ConvertCaloriesToConsumptionMass(float calories)
		{
			return calories / this.caloriesPerKg;
		}

		// Token: 0x06007883 RID: 30851 RVA: 0x002D684A File Offset: 0x002D4A4A
		public float ConvertConsumptionMassToCalories(float mass)
		{
			return this.caloriesPerKg * mass;
		}

		// Token: 0x06007884 RID: 30852 RVA: 0x002D6854 File Offset: 0x002D4A54
		public float ConvertConsumptionMassToProducedMass(float consumed_mass)
		{
			return consumed_mass * this.producedConversionRate;
		}
	}
}
