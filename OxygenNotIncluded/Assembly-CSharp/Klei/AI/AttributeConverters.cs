using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DD4 RID: 3540
	[AddComponentMenu("KMonoBehaviour/scripts/AttributeConverters")]
	public class AttributeConverters : KMonoBehaviour
	{
		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06006CF6 RID: 27894 RVA: 0x002AF056 File Offset: 0x002AD256
		public int Count
		{
			get
			{
				return this.converters.Count;
			}
		}

		// Token: 0x06006CF7 RID: 27895 RVA: 0x002AF064 File Offset: 0x002AD264
		protected override void OnPrefabInit()
		{
			foreach (AttributeInstance attributeInstance in this.GetAttributes())
			{
				foreach (AttributeConverter converter in attributeInstance.Attribute.converters)
				{
					AttributeConverterInstance item = new AttributeConverterInstance(base.gameObject, converter, attributeInstance);
					this.converters.Add(item);
				}
			}
		}

		// Token: 0x06006CF8 RID: 27896 RVA: 0x002AF108 File Offset: 0x002AD308
		public AttributeConverterInstance Get(AttributeConverter converter)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter == converter)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x06006CF9 RID: 27897 RVA: 0x002AF164 File Offset: 0x002AD364
		public AttributeConverterInstance GetConverter(string id)
		{
			foreach (AttributeConverterInstance attributeConverterInstance in this.converters)
			{
				if (attributeConverterInstance.converter.Id == id)
				{
					return attributeConverterInstance;
				}
			}
			return null;
		}

		// Token: 0x040051CF RID: 20943
		public List<AttributeConverterInstance> converters = new List<AttributeConverterInstance>();
	}
}
