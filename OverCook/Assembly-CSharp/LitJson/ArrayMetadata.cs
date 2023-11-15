using System;

namespace LitJson
{
	// Token: 0x02000240 RID: 576
	internal struct ArrayMetadata
	{
		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x00037415 File Offset: 0x00035815
		// (set) Token: 0x06000A10 RID: 2576 RVA: 0x00037433 File Offset: 0x00035833
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

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x0003743C File Offset: 0x0003583C
		// (set) Token: 0x06000A12 RID: 2578 RVA: 0x00037444 File Offset: 0x00035844
		public bool IsArray
		{
			get
			{
				return this.is_array;
			}
			set
			{
				this.is_array = value;
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06000A13 RID: 2579 RVA: 0x0003744D File Offset: 0x0003584D
		// (set) Token: 0x06000A14 RID: 2580 RVA: 0x00037455 File Offset: 0x00035855
		public bool IsList
		{
			get
			{
				return this.is_list;
			}
			set
			{
				this.is_list = value;
			}
		}

		// Token: 0x04000804 RID: 2052
		private Type element_type;

		// Token: 0x04000805 RID: 2053
		private bool is_array;

		// Token: 0x04000806 RID: 2054
		private bool is_list;
	}
}
