using System;

namespace InControl
{
	// Token: 0x020002A6 RID: 678
	public class PlayerTwoAxisAction : TwoAxisInputControl
	{
		// Token: 0x06000CC9 RID: 3273 RVA: 0x00042312 File Offset: 0x00040712
		internal PlayerTwoAxisAction(PlayerAction negativeXAction, PlayerAction positiveXAction, PlayerAction negativeYAction, PlayerAction positiveYAction)
		{
			this.negativeXAction = negativeXAction;
			this.positiveXAction = positiveXAction;
			this.negativeYAction = negativeYAction;
			this.positiveYAction = positiveYAction;
			this.InvertYAxis = false;
			this.Raw = true;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000CCA RID: 3274 RVA: 0x00042345 File Offset: 0x00040745
		// (set) Token: 0x06000CCB RID: 3275 RVA: 0x0004234D File Offset: 0x0004074D
		public bool InvertYAxis { get; set; }

		// Token: 0x06000CCC RID: 3276 RVA: 0x00042358 File Offset: 0x00040758
		internal void Update(ulong updateTick, float deltaTime)
		{
			float x = Utility.ValueFromSides(this.negativeXAction, this.positiveXAction, false);
			float y = Utility.ValueFromSides(this.negativeYAction, this.positiveYAction, InputManager.InvertYAxis || this.InvertYAxis);
			base.UpdateWithAxes(x, y, updateTick, deltaTime);
		}

		// Token: 0x040009F9 RID: 2553
		private PlayerAction negativeXAction;

		// Token: 0x040009FA RID: 2554
		private PlayerAction positiveXAction;

		// Token: 0x040009FB RID: 2555
		private PlayerAction negativeYAction;

		// Token: 0x040009FC RID: 2556
		private PlayerAction positiveYAction;
	}
}
