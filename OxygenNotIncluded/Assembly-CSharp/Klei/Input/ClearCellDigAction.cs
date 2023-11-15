using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x02000E1E RID: 3614
	[Action("Clear Cell")]
	public class ClearCellDigAction : DigAction
	{
		// Token: 0x06006EB4 RID: 28340 RVA: 0x002B85FC File Offset: 0x002B67FC
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, true);
			}
		}

		// Token: 0x06006EB5 RID: 28341 RVA: 0x002B8620 File Offset: 0x002B6820
		protected override void Uproot(Uprootable uprootable)
		{
			if (uprootable == null)
			{
				return;
			}
			uprootable.AddTag(GameTags.Uprooted);
		}
	}
}
