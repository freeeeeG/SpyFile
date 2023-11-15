using System;

namespace Characters
{
	// Token: 0x020006E4 RID: 1764
	public class TakeDamageEvent : PriorityList<TakeDamageDelegate>
	{
		// Token: 0x060023BD RID: 9149 RVA: 0x0006B144 File Offset: 0x00069344
		public bool Invoke(ref Damage damage)
		{
			for (int i = 0; i < this._items.Count; i++)
			{
				if (this._items[i].value(ref damage))
				{
					return true;
				}
			}
			return false;
		}
	}
}
