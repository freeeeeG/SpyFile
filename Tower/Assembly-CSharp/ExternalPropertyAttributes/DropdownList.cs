using System;
using System.Collections;
using System.Collections.Generic;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000024 RID: 36
	public class DropdownList<T> : IDropdownList, IEnumerable<KeyValuePair<string, object>>, IEnumerable
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00003434 File Offset: 0x00001634
		public DropdownList()
		{
			this._values = new List<KeyValuePair<string, object>>();
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00003447 File Offset: 0x00001647
		public void Add(string displayName, T value)
		{
			this._values.Add(new KeyValuePair<string, object>(displayName, value));
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003460 File Offset: 0x00001660
		public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			return this._values.GetEnumerator();
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003472 File Offset: 0x00001672
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000347C File Offset: 0x0000167C
		public static explicit operator DropdownList<object>(DropdownList<T> target)
		{
			DropdownList<object> dropdownList = new DropdownList<object>();
			foreach (KeyValuePair<string, object> keyValuePair in target)
			{
				dropdownList.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return dropdownList;
		}

		// Token: 0x0400004C RID: 76
		private List<KeyValuePair<string, object>> _values;
	}
}
