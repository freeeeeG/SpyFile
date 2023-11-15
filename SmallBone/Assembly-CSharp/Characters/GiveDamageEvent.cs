using System;

namespace Characters
{
	// Token: 0x020006E3 RID: 1763
	public class GiveDamageEvent : PriorityList<GiveDamageDelegate>
	{
		// Token: 0x060023BB RID: 9147 RVA: 0x0006B0FC File Offset: 0x000692FC
		public bool Invoke(ITarget target, ref Damage damage)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				if (this._items[i].value(target, ref damage))
				{
					return true;
				}
			}
			return false;
		}
	}
}
