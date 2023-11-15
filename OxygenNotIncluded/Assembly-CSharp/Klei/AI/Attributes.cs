using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DD9 RID: 3545
	public class Attributes
	{
		// Token: 0x06006D34 RID: 27956 RVA: 0x002AFDF9 File Offset: 0x002ADFF9
		public IEnumerator<AttributeInstance> GetEnumerator()
		{
			return this.AttributeTable.GetEnumerator();
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x06006D35 RID: 27957 RVA: 0x002AFE0B File Offset: 0x002AE00B
		public int Count
		{
			get
			{
				return this.AttributeTable.Count;
			}
		}

		// Token: 0x06006D36 RID: 27958 RVA: 0x002AFE18 File Offset: 0x002AE018
		public Attributes(GameObject game_object)
		{
			this.gameObject = game_object;
		}

		// Token: 0x06006D37 RID: 27959 RVA: 0x002AFE34 File Offset: 0x002AE034
		public AttributeInstance Add(Attribute attribute)
		{
			AttributeInstance attributeInstance = this.Get(attribute.Id);
			if (attributeInstance == null)
			{
				attributeInstance = new AttributeInstance(this.gameObject, attribute);
				this.AttributeTable.Add(attributeInstance);
			}
			return attributeInstance;
		}

		// Token: 0x06006D38 RID: 27960 RVA: 0x002AFE6C File Offset: 0x002AE06C
		public void Add(AttributeModifier modifier)
		{
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Add(modifier);
			}
		}

		// Token: 0x06006D39 RID: 27961 RVA: 0x002AFE90 File Offset: 0x002AE090
		public void Remove(AttributeModifier modifier)
		{
			if (modifier == null)
			{
				return;
			}
			AttributeInstance attributeInstance = this.Get(modifier.AttributeId);
			if (attributeInstance != null)
			{
				attributeInstance.Remove(modifier);
			}
		}

		// Token: 0x06006D3A RID: 27962 RVA: 0x002AFEB8 File Offset: 0x002AE0B8
		public float GetValuePercent(string attribute_id)
		{
			float result = 1f;
			AttributeInstance attributeInstance = this.Get(attribute_id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue() / attributeInstance.GetBaseValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + attribute_id);
			}
			return result;
		}

		// Token: 0x06006D3B RID: 27963 RVA: 0x002AFEF8 File Offset: 0x002AE0F8
		public AttributeInstance Get(string attribute_id)
		{
			for (int i = 0; i < this.AttributeTable.Count; i++)
			{
				if (this.AttributeTable[i].Id == attribute_id)
				{
					return this.AttributeTable[i];
				}
			}
			return null;
		}

		// Token: 0x06006D3C RID: 27964 RVA: 0x002AFF42 File Offset: 0x002AE142
		public AttributeInstance Get(Attribute attribute)
		{
			return this.Get(attribute.Id);
		}

		// Token: 0x06006D3D RID: 27965 RVA: 0x002AFF50 File Offset: 0x002AE150
		public float GetValue(string id)
		{
			float result = 0f;
			AttributeInstance attributeInstance = this.Get(id);
			if (attributeInstance != null)
			{
				result = attributeInstance.GetTotalValue();
			}
			else
			{
				global::Debug.LogError("Could not find attribute " + id);
			}
			return result;
		}

		// Token: 0x06006D3E RID: 27966 RVA: 0x002AFF88 File Offset: 0x002AE188
		public AttributeInstance GetProfession()
		{
			AttributeInstance attributeInstance = null;
			foreach (AttributeInstance attributeInstance2 in this)
			{
				if (attributeInstance2.modifier.IsProfession)
				{
					if (attributeInstance == null)
					{
						attributeInstance = attributeInstance2;
					}
					else if (attributeInstance.GetTotalValue() < attributeInstance2.GetTotalValue())
					{
						attributeInstance = attributeInstance2;
					}
				}
			}
			return attributeInstance;
		}

		// Token: 0x06006D3F RID: 27967 RVA: 0x002AFFF0 File Offset: 0x002AE1F0
		public string GetProfessionString(bool longform = true)
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, 0, DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_NAME);
			}
			return string.Format(longform ? UI.ATTRIBUTELEVEL : UI.ATTRIBUTELEVEL_SHORT, (int)profession.GetTotalValue(), profession.modifier.ProfessionName);
		}

		// Token: 0x06006D40 RID: 27968 RVA: 0x002B0064 File Offset: 0x002AE264
		public string GetProfessionDescriptionString()
		{
			AttributeInstance profession = this.GetProfession();
			if ((int)profession.GetTotalValue() == 0)
			{
				return DUPLICANTS.ATTRIBUTES.UNPROFESSIONAL_DESC;
			}
			return string.Format(DUPLICANTS.ATTRIBUTES.PROFESSION_DESC, profession.modifier.Name);
		}

		// Token: 0x040051E2 RID: 20962
		public List<AttributeInstance> AttributeTable = new List<AttributeInstance>();

		// Token: 0x040051E3 RID: 20963
		public GameObject gameObject;
	}
}
