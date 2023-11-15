using System;
using System.Collections;
using System.Collections.Generic;

namespace LitJson
{
	// Token: 0x0200023D RID: 573
	internal class OrderedDictionaryEnumerator : IDictionaryEnumerator, IEnumerator
	{
		// Token: 0x06000A01 RID: 2561 RVA: 0x000372F1 File Offset: 0x000356F1
		public OrderedDictionaryEnumerator(IEnumerator<KeyValuePair<string, JsonData>> enumerator)
		{
			this.list_enumerator = enumerator;
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00037300 File Offset: 0x00035700
		public object Current
		{
			get
			{
				return this.Entry;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x06000A03 RID: 2563 RVA: 0x00037310 File Offset: 0x00035710
		public DictionaryEntry Entry
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return new DictionaryEntry(keyValuePair.Key, keyValuePair.Value);
			}
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x06000A04 RID: 2564 RVA: 0x0003733C File Offset: 0x0003573C
		public object Key
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Key;
			}
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x0003735C File Offset: 0x0003575C
		public object Value
		{
			get
			{
				KeyValuePair<string, JsonData> keyValuePair = this.list_enumerator.Current;
				return keyValuePair.Value;
			}
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0003737C File Offset: 0x0003577C
		public bool MoveNext()
		{
			return this.list_enumerator.MoveNext();
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00037389 File Offset: 0x00035789
		public void Reset()
		{
			this.list_enumerator.Reset();
		}

		// Token: 0x04000800 RID: 2048
		private IEnumerator<KeyValuePair<string, JsonData>> list_enumerator;
	}
}
