using System;

namespace InControl
{
	// Token: 0x020002A5 RID: 677
	public class PlayerOneAxisAction : OneAxisInputControl
	{
		// Token: 0x06000CC7 RID: 3271 RVA: 0x00041ECD File Offset: 0x000402CD
		internal PlayerOneAxisAction(PlayerAction negativeAction, PlayerAction positiveAction)
		{
			this.negativeAction = negativeAction;
			this.positiveAction = positiveAction;
			this.Raw = true;
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00041EEC File Offset: 0x000402EC
		internal void Update(ulong updateTick, float deltaTime)
		{
			float value = Utility.ValueFromSides(this.negativeAction, this.positiveAction);
			base.CommitWithValue(value, updateTick, deltaTime);
		}

		// Token: 0x040009F7 RID: 2551
		private PlayerAction negativeAction;

		// Token: 0x040009F8 RID: 2552
		private PlayerAction positiveAction;
	}
}
