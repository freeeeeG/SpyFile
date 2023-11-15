using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x02000248 RID: 584
	public class JsonMockWrapper : IJsonWrapper, IList, IOrderedDictionary, IEnumerable, ICollection, IDictionary
	{
		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x00038C69 File Offset: 0x00037069
		public bool IsArray
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x00038C6C File Offset: 0x0003706C
		public bool IsBoolean
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x00038C6F File Offset: 0x0003706F
		public bool IsDouble
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x00038C72 File Offset: 0x00037072
		public bool IsInt
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x00038C75 File Offset: 0x00037075
		public bool IsLong
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00038C78 File Offset: 0x00037078
		public bool IsObject
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000A6A RID: 2666 RVA: 0x00038C7B File Offset: 0x0003707B
		public bool IsString
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00038C7E File Offset: 0x0003707E
		public bool GetBoolean()
		{
			return false;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00038C81 File Offset: 0x00037081
		public double GetDouble()
		{
			return 0.0;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00038C8C File Offset: 0x0003708C
		public int GetInt()
		{
			return 0;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00038C8F File Offset: 0x0003708F
		public JsonType GetJsonType()
		{
			return JsonType.None;
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00038C92 File Offset: 0x00037092
		public long GetLong()
		{
			return 0L;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00038C96 File Offset: 0x00037096
		public string GetString()
		{
			return string.Empty;
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00038C9D File Offset: 0x0003709D
		public void SetBoolean(bool val)
		{
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00038C9F File Offset: 0x0003709F
		public void SetDouble(double val)
		{
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00038CA1 File Offset: 0x000370A1
		public void SetInt(int val)
		{
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00038CA3 File Offset: 0x000370A3
		public void SetJsonType(JsonType type)
		{
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00038CA5 File Offset: 0x000370A5
		public void SetLong(long val)
		{
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00038CA7 File Offset: 0x000370A7
		public void SetString(string val)
		{
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00038CA9 File Offset: 0x000370A9
		public string ToJson()
		{
			return string.Empty;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00038CB0 File Offset: 0x000370B0
		public void ToJson(JsonWriter writer)
		{
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000A79 RID: 2681 RVA: 0x00038CB2 File Offset: 0x000370B2
		bool IList.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000A7A RID: 2682 RVA: 0x00038CB5 File Offset: 0x000370B5
		bool IList.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000F9 RID: 249
		object IList.this[int index]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00038CBD File Offset: 0x000370BD
		int IList.Add(object value)
		{
			return 0;
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00038CC0 File Offset: 0x000370C0
		void IList.Clear()
		{
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x00038CC2 File Offset: 0x000370C2
		bool IList.Contains(object value)
		{
			return false;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00038CC5 File Offset: 0x000370C5
		int IList.IndexOf(object value)
		{
			return -1;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x00038CC8 File Offset: 0x000370C8
		void IList.Insert(int i, object v)
		{
		}

		// Token: 0x06000A82 RID: 2690 RVA: 0x00038CCA File Offset: 0x000370CA
		void IList.Remove(object value)
		{
		}

		// Token: 0x06000A83 RID: 2691 RVA: 0x00038CCC File Offset: 0x000370CC
		void IList.RemoveAt(int index)
		{
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x00038CCE File Offset: 0x000370CE
		int ICollection.Count
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x00038CD1 File Offset: 0x000370D1
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x00038CD4 File Offset: 0x000370D4
		object ICollection.SyncRoot
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000A87 RID: 2695 RVA: 0x00038CD7 File Offset: 0x000370D7
		void ICollection.CopyTo(Array array, int index)
		{
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00038CD9 File Offset: 0x000370D9
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000A89 RID: 2697 RVA: 0x00038CDC File Offset: 0x000370DC
		bool IDictionary.IsFixedSize
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000A8A RID: 2698 RVA: 0x00038CDF File Offset: 0x000370DF
		bool IDictionary.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000A8B RID: 2699 RVA: 0x00038CE2 File Offset: 0x000370E2
		ICollection IDictionary.Keys
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000A8C RID: 2700 RVA: 0x00038CE5 File Offset: 0x000370E5
		ICollection IDictionary.Values
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000101 RID: 257
		object IDictionary.this[object key]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000A8F RID: 2703 RVA: 0x00038CED File Offset: 0x000370ED
		void IDictionary.Add(object k, object v)
		{
		}

		// Token: 0x06000A90 RID: 2704 RVA: 0x00038CEF File Offset: 0x000370EF
		void IDictionary.Clear()
		{
		}

		// Token: 0x06000A91 RID: 2705 RVA: 0x00038CF1 File Offset: 0x000370F1
		bool IDictionary.Contains(object key)
		{
			return false;
		}

		// Token: 0x06000A92 RID: 2706 RVA: 0x00038CF4 File Offset: 0x000370F4
		void IDictionary.Remove(object key)
		{
		}

		// Token: 0x06000A93 RID: 2707 RVA: 0x00038CF6 File Offset: 0x000370F6
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x17000102 RID: 258
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		// Token: 0x06000A96 RID: 2710 RVA: 0x00038CFE File Offset: 0x000370FE
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06000A97 RID: 2711 RVA: 0x00038D01 File Offset: 0x00037101
		void IOrderedDictionary.Insert(int i, object k, object v)
		{
		}

		// Token: 0x06000A98 RID: 2712 RVA: 0x00038D03 File Offset: 0x00037103
		void IOrderedDictionary.RemoveAt(int i)
		{
		}
	}
}
