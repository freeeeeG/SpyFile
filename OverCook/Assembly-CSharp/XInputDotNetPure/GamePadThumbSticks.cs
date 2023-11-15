using System;
using UnityEngine;

namespace XInputDotNetPure
{
	// Token: 0x02000350 RID: 848
	public struct GamePadThumbSticks
	{
		// Token: 0x0600104D RID: 4173 RVA: 0x0005DDD9 File Offset: 0x0005C1D9
		internal GamePadThumbSticks(GamePadThumbSticks.StickValue left, GamePadThumbSticks.StickValue right)
		{
			this.left = left;
			this.right = right;
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600104E RID: 4174 RVA: 0x0005DDE9 File Offset: 0x0005C1E9
		public GamePadThumbSticks.StickValue Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x0005DDF1 File Offset: 0x0005C1F1
		public GamePadThumbSticks.StickValue Right
		{
			get
			{
				return this.right;
			}
		}

		// Token: 0x04000C3E RID: 3134
		private GamePadThumbSticks.StickValue left;

		// Token: 0x04000C3F RID: 3135
		private GamePadThumbSticks.StickValue right;

		// Token: 0x02000351 RID: 849
		public struct StickValue
		{
			// Token: 0x06001050 RID: 4176 RVA: 0x0005DDF9 File Offset: 0x0005C1F9
			internal StickValue(float x, float y)
			{
				this.vector = new Vector2(x, y);
			}

			// Token: 0x17000227 RID: 551
			// (get) Token: 0x06001051 RID: 4177 RVA: 0x0005DE08 File Offset: 0x0005C208
			public float X
			{
				get
				{
					return this.vector.x;
				}
			}

			// Token: 0x17000228 RID: 552
			// (get) Token: 0x06001052 RID: 4178 RVA: 0x0005DE15 File Offset: 0x0005C215
			public float Y
			{
				get
				{
					return this.vector.y;
				}
			}

			// Token: 0x17000229 RID: 553
			// (get) Token: 0x06001053 RID: 4179 RVA: 0x0005DE22 File Offset: 0x0005C222
			public Vector2 Vector
			{
				get
				{
					return this.vector;
				}
			}

			// Token: 0x04000C40 RID: 3136
			private Vector2 vector;
		}
	}
}
