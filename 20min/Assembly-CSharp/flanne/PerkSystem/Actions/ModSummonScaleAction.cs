using System;
using UnityEngine;

namespace flanne.PerkSystem.Actions
{
	// Token: 0x020001C5 RID: 453
	public class ModSummonScaleAction : Action
	{
		// Token: 0x06000A34 RID: 2612 RVA: 0x00027E70 File Offset: 0x00026070
		public override void Activate(GameObject target)
		{
			foreach (Summon summon in PlayerController.Instance.GetComponentsInChildren<Summon>(true))
			{
				if (summon.SummonTypeID == this.SummonTypeID)
				{
					summon.transform.localScale *= this.scale;
				}
			}
		}

		// Token: 0x0400072E RID: 1838
		[SerializeField]
		private string SummonTypeID;

		// Token: 0x0400072F RID: 1839
		[SerializeField]
		private float scale = 1f;
	}
}
