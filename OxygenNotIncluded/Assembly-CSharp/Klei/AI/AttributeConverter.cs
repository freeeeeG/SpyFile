using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DD2 RID: 3538
	public class AttributeConverter : Resource
	{
		// Token: 0x06006CEF RID: 27887 RVA: 0x002AEF15 File Offset: 0x002AD115
		public AttributeConverter(string id, string name, string description, float multiplier, float base_value, Attribute attribute, IAttributeFormatter formatter = null) : base(id, name)
		{
			this.description = description;
			this.multiplier = multiplier;
			this.baseValue = base_value;
			this.attribute = attribute;
			this.formatter = formatter;
		}

		// Token: 0x06006CF0 RID: 27888 RVA: 0x002AEF46 File Offset: 0x002AD146
		public AttributeConverterInstance Lookup(Component cmp)
		{
			return this.Lookup(cmp.gameObject);
		}

		// Token: 0x06006CF1 RID: 27889 RVA: 0x002AEF54 File Offset: 0x002AD154
		public AttributeConverterInstance Lookup(GameObject go)
		{
			AttributeConverters component = go.GetComponent<AttributeConverters>();
			if (component != null)
			{
				return component.Get(this);
			}
			return null;
		}

		// Token: 0x06006CF2 RID: 27890 RVA: 0x002AEF7C File Offset: 0x002AD17C
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			string text;
			if (this.formatter != null)
			{
				text = this.formatter.GetFormattedValue(value, this.formatter.DeltaTimeSlice);
			}
			else if (this.attribute.formatter != null)
			{
				text = this.attribute.formatter.GetFormattedValue(value, this.attribute.formatter.DeltaTimeSlice);
			}
			else
			{
				text = GameUtil.GetFormattedSimple(value, GameUtil.TimeSlice.None, null);
			}
			if (text != null)
			{
				text = GameUtil.AddPositiveSign(text, value > 0f);
				return string.Format(this.description, text);
			}
			return null;
		}

		// Token: 0x040051C8 RID: 20936
		public string description;

		// Token: 0x040051C9 RID: 20937
		public float multiplier;

		// Token: 0x040051CA RID: 20938
		public float baseValue;

		// Token: 0x040051CB RID: 20939
		public Attribute attribute;

		// Token: 0x040051CC RID: 20940
		public IAttributeFormatter formatter;
	}
}
