using System;
using Klei.Actions;

namespace Klei.Input
{
	// Token: 0x02000E1D RID: 3613
	[Action("Immediate")]
	public class ImmediateDigAction : DigAction
	{
		// Token: 0x06006EB1 RID: 28337 RVA: 0x002B85BE File Offset: 0x002B67BE
		public override void Dig(int cell, int distFromOrigin)
		{
			if (Grid.Solid[cell] && !Grid.Foundation[cell])
			{
				SimMessages.Dig(cell, -1, false);
			}
		}

		// Token: 0x06006EB2 RID: 28338 RVA: 0x002B85E2 File Offset: 0x002B67E2
		protected override void Uproot(Uprootable uprootable)
		{
			if (uprootable == null)
			{
				return;
			}
			uprootable.Uproot();
		}
	}
}
