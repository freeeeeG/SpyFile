using System;
using Klei.Input;

namespace Klei.Actions
{
	// Token: 0x02000E22 RID: 3618
	public class DigToolActionFactory : ActionFactory<DigToolActionFactory, DigAction, DigToolActionFactory.Actions>
	{
		// Token: 0x06006EC2 RID: 28354 RVA: 0x002B876F File Offset: 0x002B696F
		protected override DigAction CreateAction(DigToolActionFactory.Actions action)
		{
			if (action == DigToolActionFactory.Actions.Immediate)
			{
				return new ImmediateDigAction();
			}
			if (action == DigToolActionFactory.Actions.ClearCell)
			{
				return new ClearCellDigAction();
			}
			if (action == DigToolActionFactory.Actions.MarkCell)
			{
				return new MarkCellDigAction();
			}
			throw new InvalidOperationException("Can not create DigAction 'Count'. Please provide a valid action.");
		}

		// Token: 0x02001F9A RID: 8090
		public enum Actions
		{
			// Token: 0x04008EB5 RID: 36533
			MarkCell = 145163119,
			// Token: 0x04008EB6 RID: 36534
			Immediate = -1044758767,
			// Token: 0x04008EB7 RID: 36535
			ClearCell = -1011242513,
			// Token: 0x04008EB8 RID: 36536
			Count = -1427607121
		}
	}
}
