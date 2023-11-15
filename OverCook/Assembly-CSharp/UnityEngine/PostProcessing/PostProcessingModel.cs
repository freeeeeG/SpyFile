using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E7 RID: 231
	[Serializable]
	public abstract class PostProcessingModel
	{
		// Token: 0x17000094 RID: 148
		// (get) Token: 0x06000454 RID: 1108 RVA: 0x00024940 File Offset: 0x00022D40
		// (set) Token: 0x06000455 RID: 1109 RVA: 0x00024948 File Offset: 0x00022D48
		public bool enabled
		{
			get
			{
				return this.m_Enabled;
			}
			set
			{
				this.m_Enabled = value;
				if (value)
				{
					this.OnValidate();
				}
			}
		}

		// Token: 0x06000456 RID: 1110
		public abstract void Reset();

		// Token: 0x06000457 RID: 1111 RVA: 0x0002495D File Offset: 0x00022D5D
		public virtual void OnValidate()
		{
		}

		// Token: 0x040003DF RID: 991
		[SerializeField]
		[GetSet("enabled")]
		private bool m_Enabled;
	}
}
