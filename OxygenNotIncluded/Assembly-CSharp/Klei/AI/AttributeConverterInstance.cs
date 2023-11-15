using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DD3 RID: 3539
	public class AttributeConverterInstance : ModifierInstance<AttributeConverter>
	{
		// Token: 0x06006CF3 RID: 27891 RVA: 0x002AF005 File Offset: 0x002AD205
		public AttributeConverterInstance(GameObject game_object, AttributeConverter converter, AttributeInstance attribute_instance) : base(game_object, converter)
		{
			this.converter = converter;
			this.attributeInstance = attribute_instance;
		}

		// Token: 0x06006CF4 RID: 27892 RVA: 0x002AF01D File Offset: 0x002AD21D
		public float Evaluate()
		{
			return this.converter.multiplier * this.attributeInstance.GetTotalValue() + this.converter.baseValue;
		}

		// Token: 0x06006CF5 RID: 27893 RVA: 0x002AF042 File Offset: 0x002AD242
		public string DescriptionFromAttribute(float value, GameObject go)
		{
			return this.converter.DescriptionFromAttribute(this.Evaluate(), go);
		}

		// Token: 0x040051CD RID: 20941
		public AttributeConverter converter;

		// Token: 0x040051CE RID: 20942
		public AttributeInstance attributeInstance;
	}
}
