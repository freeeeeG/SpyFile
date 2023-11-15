using System;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x02000241 RID: 577
	internal struct ObjectMetadata
	{
		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000A15 RID: 2581 RVA: 0x0003745E File Offset: 0x0003585E
		// (set) Token: 0x06000A16 RID: 2582 RVA: 0x0003747C File Offset: 0x0003587C
		public Type ElementType
		{
			get
			{
				if (this.element_type == null)
				{
					return typeof(JsonData);
				}
				return this.element_type;
			}
			set
			{
				this.element_type = value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000A17 RID: 2583 RVA: 0x00037485 File Offset: 0x00035885
		// (set) Token: 0x06000A18 RID: 2584 RVA: 0x0003748D File Offset: 0x0003588D
		public bool IsDictionary
		{
			get
			{
				return this.is_dictionary;
			}
			set
			{
				this.is_dictionary = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x00037496 File Offset: 0x00035896
		// (set) Token: 0x06000A1A RID: 2586 RVA: 0x0003749E File Offset: 0x0003589E
		public IDictionary<string, PropertyMetadata> Properties
		{
			get
			{
				return this.properties;
			}
			set
			{
				this.properties = value;
			}
		}

		// Token: 0x04000807 RID: 2055
		private Type element_type;

		// Token: 0x04000808 RID: 2056
		private bool is_dictionary;

		// Token: 0x04000809 RID: 2057
		private IDictionary<string, PropertyMetadata> properties;
	}
}
