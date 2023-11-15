using System;

namespace XInputDotNetPure
{
	// Token: 0x02000352 RID: 850
	public struct GamePadTriggers
	{
		// Token: 0x06001054 RID: 4180 RVA: 0x0005DE2A File Offset: 0x0005C22A
		internal GamePadTriggers(float left, float right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x0005DE3A File Offset: 0x0005C23A
		public float Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x0005DE42 File Offset: 0x0005C242
		public float Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x04000C41 RID: 3137
		private float left;

		// Token: 0x04000C42 RID: 3138
		private float right;
	}
}
