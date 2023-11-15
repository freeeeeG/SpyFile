using System;
using Klei.Actions;
using UnityEngine;

namespace Klei.Input
{
	// Token: 0x02000E1C RID: 3612
	[Action("Mark Cell")]
	public class MarkCellDigAction : DigAction
	{
		// Token: 0x06006EAE RID: 28334 RVA: 0x002B8560 File Offset: 0x002B6760
		public override void Dig(int cell, int distFromOrigin)
		{
			GameObject gameObject = DigTool.PlaceDig(cell, distFromOrigin);
			if (gameObject != null)
			{
				Prioritizable component = gameObject.GetComponent<Prioritizable>();
				if (component != null)
				{
					component.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
				}
			}
		}

		// Token: 0x06006EAF RID: 28335 RVA: 0x002B85A3 File Offset: 0x002B67A3
		protected override void Uproot(Uprootable uprootable)
		{
			if (uprootable == null)
			{
				return;
			}
			uprootable.MarkForUproot(true);
		}
	}
}
