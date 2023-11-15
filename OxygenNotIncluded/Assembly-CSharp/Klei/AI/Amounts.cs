using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DD0 RID: 3536
	public class Amounts : Modifications<Amount, AmountInstance>
	{
		// Token: 0x06006CE1 RID: 27873 RVA: 0x002AECD5 File Offset: 0x002ACED5
		public Amounts(GameObject go) : base(go, null)
		{
		}

		// Token: 0x06006CE2 RID: 27874 RVA: 0x002AECDF File Offset: 0x002ACEDF
		public float GetValue(string amount_id)
		{
			return base.Get(amount_id).value;
		}

		// Token: 0x06006CE3 RID: 27875 RVA: 0x002AECED File Offset: 0x002ACEED
		public void SetValue(string amount_id, float value)
		{
			base.Get(amount_id).value = value;
		}

		// Token: 0x06006CE4 RID: 27876 RVA: 0x002AECFC File Offset: 0x002ACEFC
		public override AmountInstance Add(AmountInstance instance)
		{
			instance.Activate();
			return base.Add(instance);
		}

		// Token: 0x06006CE5 RID: 27877 RVA: 0x002AED0B File Offset: 0x002ACF0B
		public override void Remove(AmountInstance instance)
		{
			instance.Deactivate();
			base.Remove(instance);
		}

		// Token: 0x06006CE6 RID: 27878 RVA: 0x002AED1C File Offset: 0x002ACF1C
		public void Cleanup()
		{
			for (int i = 0; i < base.Count; i++)
			{
				base[i].Deactivate();
			}
		}
	}
}
