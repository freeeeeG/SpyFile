using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;

namespace LitJson
{
	// Token: 0x0200023C RID: 572
	public class JsonData : IJsonWrapper, IEquatable<JsonData>, IList, IOrderedDictionary, IEnumerable, ICollection, IDictionary
	{
		// Token: 0x060009A3 RID: 2467 RVA: 0x000363CA File Offset: 0x000347CA
		public JsonData()
		{
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000363D2 File Offset: 0x000347D2
		public JsonData(bool boolean)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = boolean;
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x000363E8 File Offset: 0x000347E8
		public JsonData(double number)
		{
			this.type = JsonType.Double;
			this.inst_double = number;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x000363FE File Offset: 0x000347FE
		public JsonData(int number)
		{
			this.type = JsonType.Int;
			this.inst_int = number;
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00036414 File Offset: 0x00034814
		public JsonData(long number)
		{
			this.type = JsonType.Long;
			this.inst_long = number;
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x0003642C File Offset: 0x0003482C
		public JsonData(object obj)
		{
			if (obj is bool)
			{
				this.type = JsonType.Boolean;
				this.inst_boolean = (bool)obj;
				return;
			}
			if (obj is double)
			{
				this.type = JsonType.Double;
				this.inst_double = (double)obj;
				return;
			}
			if (obj is int)
			{
				this.type = JsonType.Int;
				this.inst_int = (int)obj;
				return;
			}
			if (obj is long)
			{
				this.type = JsonType.Long;
				this.inst_long = (long)obj;
				return;
			}
			if (obj is string)
			{
				this.type = JsonType.String;
				this.inst_string = (string)obj;
				return;
			}
			throw new ArgumentException("Unable to wrap the given object with JsonData");
		}

		// Token: 0x060009A9 RID: 2473 RVA: 0x000364E4 File Offset: 0x000348E4
		public JsonData(string str)
		{
			this.type = JsonType.String;
			this.inst_string = str;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060009AA RID: 2474 RVA: 0x000364FA File Offset: 0x000348FA
		public int Count
		{
			get
			{
				return this.EnsureCollection().Count;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x00036507 File Offset: 0x00034907
		public bool IsArray
		{
			get
			{
				return this.type == JsonType.Array;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x00036512 File Offset: 0x00034912
		public bool IsBoolean
		{
			get
			{
				return this.type == JsonType.Boolean;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x060009AD RID: 2477 RVA: 0x0003651D File Offset: 0x0003491D
		public bool IsDouble
		{
			get
			{
				return this.type == JsonType.Double;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x00036528 File Offset: 0x00034928
		public bool IsInt
		{
			get
			{
				return this.type == JsonType.Int;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060009AF RID: 2479 RVA: 0x00036533 File Offset: 0x00034933
		public bool IsLong
		{
			get
			{
				return this.type == JsonType.Long;
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x0003653E File Offset: 0x0003493E
		public bool IsObject
		{
			get
			{
				return this.type == JsonType.Object;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060009B1 RID: 2481 RVA: 0x00036549 File Offset: 0x00034949
		public bool IsString
		{
			get
			{
				return this.type == JsonType.String;
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x060009B2 RID: 2482 RVA: 0x00036554 File Offset: 0x00034954
		public ICollection<string> Keys
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object.Keys;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060009B3 RID: 2483 RVA: 0x00036568 File Offset: 0x00034968
		int ICollection.Count
		{
			get
			{
				return this.Count;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x060009B4 RID: 2484 RVA: 0x00036570 File Offset: 0x00034970
		bool ICollection.IsSynchronized
		{
			get
			{
				return this.EnsureCollection().IsSynchronized;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060009B5 RID: 2485 RVA: 0x0003657D File Offset: 0x0003497D
		object ICollection.SyncRoot
		{
			get
			{
				return this.EnsureCollection().SyncRoot;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060009B6 RID: 2486 RVA: 0x0003658A File Offset: 0x0003498A
		bool IDictionary.IsFixedSize
		{
			get
			{
				return this.EnsureDictionary().IsFixedSize;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060009B7 RID: 2487 RVA: 0x00036597 File Offset: 0x00034997
		bool IDictionary.IsReadOnly
		{
			get
			{
				return this.EnsureDictionary().IsReadOnly;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060009B8 RID: 2488 RVA: 0x000365A4 File Offset: 0x000349A4
		ICollection IDictionary.Keys
		{
			get
			{
				this.EnsureDictionary();
				IList<string> list = new List<string>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Key);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060009B9 RID: 2489 RVA: 0x00036618 File Offset: 0x00034A18
		ICollection IDictionary.Values
		{
			get
			{
				this.EnsureDictionary();
				IList<JsonData> list = new List<JsonData>();
				foreach (KeyValuePair<string, JsonData> keyValuePair in this.object_list)
				{
					list.Add(keyValuePair.Value);
				}
				return (ICollection)list;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x060009BA RID: 2490 RVA: 0x0003668C File Offset: 0x00034A8C
		bool IJsonWrapper.IsArray
		{
			get
			{
				return this.IsArray;
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060009BB RID: 2491 RVA: 0x00036694 File Offset: 0x00034A94
		bool IJsonWrapper.IsBoolean
		{
			get
			{
				return this.IsBoolean;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060009BC RID: 2492 RVA: 0x0003669C File Offset: 0x00034A9C
		bool IJsonWrapper.IsDouble
		{
			get
			{
				return this.IsDouble;
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060009BD RID: 2493 RVA: 0x000366A4 File Offset: 0x00034AA4
		bool IJsonWrapper.IsInt
		{
			get
			{
				return this.IsInt;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x000366AC File Offset: 0x00034AAC
		bool IJsonWrapper.IsLong
		{
			get
			{
				return this.IsLong;
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060009BF RID: 2495 RVA: 0x000366B4 File Offset: 0x00034AB4
		bool IJsonWrapper.IsObject
		{
			get
			{
				return this.IsObject;
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x000366BC File Offset: 0x00034ABC
		bool IJsonWrapper.IsString
		{
			get
			{
				return this.IsString;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x000366C4 File Offset: 0x00034AC4
		bool IList.IsFixedSize
		{
			get
			{
				return this.EnsureList().IsFixedSize;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060009C2 RID: 2498 RVA: 0x000366D1 File Offset: 0x00034AD1
		bool IList.IsReadOnly
		{
			get
			{
				return this.EnsureList().IsReadOnly;
			}
		}

		// Token: 0x170000DF RID: 223
		object IDictionary.this[object key]
		{
			get
			{
				return this.EnsureDictionary()[key];
			}
			set
			{
				if (!(key is string))
				{
					throw new ArgumentException("The key has to be a string");
				}
				JsonData value2 = this.ToJsonData(value);
				this[(string)key] = value2;
			}
		}

		// Token: 0x170000E0 RID: 224
		object IOrderedDictionary.this[int idx]
		{
			get
			{
				this.EnsureDictionary();
				return this.object_list[idx].Value;
			}
			set
			{
				this.EnsureDictionary();
				JsonData value2 = this.ToJsonData(value);
				KeyValuePair<string, JsonData> keyValuePair = this.object_list[idx];
				this.inst_object[keyValuePair.Key] = value2;
				KeyValuePair<string, JsonData> value3 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value2);
				this.object_list[idx] = value3;
			}
		}

		// Token: 0x170000E1 RID: 225
		object IList.this[int index]
		{
			get
			{
				return this.EnsureList()[index];
			}
			set
			{
				this.EnsureList();
				JsonData value2 = this.ToJsonData(value);
				this[index] = value2;
			}
		}

		// Token: 0x170000EB RID: 235
		public JsonData this[string prop_name]
		{
			get
			{
				this.EnsureDictionary();
				return this.inst_object[prop_name];
			}
			set
			{
				this.EnsureDictionary();
				KeyValuePair<string, JsonData> keyValuePair = new KeyValuePair<string, JsonData>(prop_name, value);
				if (this.inst_object.ContainsKey(prop_name))
				{
					for (int i = 0; i < this.object_list.Count; i++)
					{
						if (this.object_list[i].Key == prop_name)
						{
							this.object_list[i] = keyValuePair;
							break;
						}
					}
				}
				else
				{
					this.object_list.Add(keyValuePair);
				}
				this.inst_object[prop_name] = value;
				this.json = null;
			}
		}

		// Token: 0x170000EC RID: 236
		public JsonData this[int index]
		{
			get
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					return this.inst_array[index];
				}
				return this.object_list[index].Value;
			}
			set
			{
				this.EnsureCollection();
				if (this.type == JsonType.Array)
				{
					this.inst_array[index] = value;
				}
				else
				{
					KeyValuePair<string, JsonData> keyValuePair = this.object_list[index];
					KeyValuePair<string, JsonData> value2 = new KeyValuePair<string, JsonData>(keyValuePair.Key, value);
					this.object_list[index] = value2;
					this.inst_object[keyValuePair.Key] = value;
				}
				this.json = null;
			}
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00036949 File Offset: 0x00034D49
		public static implicit operator JsonData(bool data)
		{
			return new JsonData(data);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00036951 File Offset: 0x00034D51
		public static implicit operator JsonData(double data)
		{
			return new JsonData(data);
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x00036959 File Offset: 0x00034D59
		public static implicit operator JsonData(int data)
		{
			return new JsonData(data);
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00036961 File Offset: 0x00034D61
		public static implicit operator JsonData(long data)
		{
			return new JsonData(data);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00036969 File Offset: 0x00034D69
		public static implicit operator JsonData(string data)
		{
			return new JsonData(data);
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00036971 File Offset: 0x00034D71
		public static explicit operator bool(JsonData data)
		{
			if (data.type != JsonType.Boolean)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_boolean;
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00036990 File Offset: 0x00034D90
		public static explicit operator double(JsonData data)
		{
			if (data.type != JsonType.Double)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a double");
			}
			return data.inst_double;
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x000369AF File Offset: 0x00034DAF
		public static explicit operator int(JsonData data)
		{
			if (data.type != JsonType.Int)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_int;
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x000369CE File Offset: 0x00034DCE
		public static explicit operator long(JsonData data)
		{
			if (data.type != JsonType.Long)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold an int");
			}
			return data.inst_long;
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x000369ED File Offset: 0x00034DED
		public static explicit operator string(JsonData data)
		{
			if (data.type != JsonType.String)
			{
				throw new InvalidCastException("Instance of JsonData doesn't hold a string");
			}
			return data.inst_string;
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x00036A0C File Offset: 0x00034E0C
		void ICollection.CopyTo(Array array, int index)
		{
			this.EnsureCollection().CopyTo(array, index);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x00036A1C File Offset: 0x00034E1C
		void IDictionary.Add(object key, object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.EnsureDictionary().Add(key, value2);
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>((string)key, value2);
			this.object_list.Add(item);
			this.json = null;
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x00036A5F File Offset: 0x00034E5F
		void IDictionary.Clear()
		{
			this.EnsureDictionary().Clear();
			this.object_list.Clear();
			this.json = null;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00036A7E File Offset: 0x00034E7E
		bool IDictionary.Contains(object key)
		{
			return this.EnsureDictionary().Contains(key);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x00036A8C File Offset: 0x00034E8C
		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return ((IOrderedDictionary)this).GetEnumerator();
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00036A94 File Offset: 0x00034E94
		void IDictionary.Remove(object key)
		{
			this.EnsureDictionary().Remove(key);
			for (int i = 0; i < this.object_list.Count; i++)
			{
				if (this.object_list[i].Key == (string)key)
				{
					this.object_list.RemoveAt(i);
					break;
				}
			}
			this.json = null;
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00036B05 File Offset: 0x00034F05
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.EnsureCollection().GetEnumerator();
		}

		// Token: 0x060009DE RID: 2526 RVA: 0x00036B12 File Offset: 0x00034F12
		bool IJsonWrapper.GetBoolean()
		{
			if (this.type != JsonType.Boolean)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a boolean");
			}
			return this.inst_boolean;
		}

		// Token: 0x060009DF RID: 2527 RVA: 0x00036B31 File Offset: 0x00034F31
		double IJsonWrapper.GetDouble()
		{
			if (this.type != JsonType.Double)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a double");
			}
			return this.inst_double;
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x00036B50 File Offset: 0x00034F50
		int IJsonWrapper.GetInt()
		{
			if (this.type != JsonType.Int)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold an int");
			}
			return this.inst_int;
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x00036B6F File Offset: 0x00034F6F
		long IJsonWrapper.GetLong()
		{
			if (this.type != JsonType.Long)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a long");
			}
			return this.inst_long;
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00036B8E File Offset: 0x00034F8E
		string IJsonWrapper.GetString()
		{
			if (this.type != JsonType.String)
			{
				throw new InvalidOperationException("JsonData instance doesn't hold a string");
			}
			return this.inst_string;
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00036BAD File Offset: 0x00034FAD
		void IJsonWrapper.SetBoolean(bool val)
		{
			this.type = JsonType.Boolean;
			this.inst_boolean = val;
			this.json = null;
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00036BC4 File Offset: 0x00034FC4
		void IJsonWrapper.SetDouble(double val)
		{
			this.type = JsonType.Double;
			this.inst_double = val;
			this.json = null;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00036BDB File Offset: 0x00034FDB
		void IJsonWrapper.SetInt(int val)
		{
			this.type = JsonType.Int;
			this.inst_int = val;
			this.json = null;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00036BF2 File Offset: 0x00034FF2
		void IJsonWrapper.SetLong(long val)
		{
			this.type = JsonType.Long;
			this.inst_long = val;
			this.json = null;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00036C09 File Offset: 0x00035009
		void IJsonWrapper.SetString(string val)
		{
			this.type = JsonType.String;
			this.inst_string = val;
			this.json = null;
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00036C20 File Offset: 0x00035020
		string IJsonWrapper.ToJson()
		{
			return this.ToJson();
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00036C28 File Offset: 0x00035028
		void IJsonWrapper.ToJson(JsonWriter writer)
		{
			this.ToJson(writer);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00036C31 File Offset: 0x00035031
		int IList.Add(object value)
		{
			return this.Add(value);
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x00036C3A File Offset: 0x0003503A
		void IList.Clear()
		{
			this.EnsureList().Clear();
			this.json = null;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00036C4E File Offset: 0x0003504E
		bool IList.Contains(object value)
		{
			return this.EnsureList().Contains(value);
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x00036C5C File Offset: 0x0003505C
		int IList.IndexOf(object value)
		{
			return this.EnsureList().IndexOf(value);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00036C6A File Offset: 0x0003506A
		void IList.Insert(int index, object value)
		{
			this.EnsureList().Insert(index, value);
			this.json = null;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00036C80 File Offset: 0x00035080
		void IList.Remove(object value)
		{
			this.EnsureList().Remove(value);
			this.json = null;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00036C95 File Offset: 0x00035095
		void IList.RemoveAt(int index)
		{
			this.EnsureList().RemoveAt(index);
			this.json = null;
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x00036CAA File Offset: 0x000350AA
		IDictionaryEnumerator IOrderedDictionary.GetEnumerator()
		{
			this.EnsureDictionary();
			return new OrderedDictionaryEnumerator(this.object_list.GetEnumerator());
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00036CC4 File Offset: 0x000350C4
		void IOrderedDictionary.Insert(int idx, object key, object value)
		{
			string text = (string)key;
			JsonData value2 = this.ToJsonData(value);
			this[text] = value2;
			KeyValuePair<string, JsonData> item = new KeyValuePair<string, JsonData>(text, value2);
			this.object_list.Insert(idx, item);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00036D00 File Offset: 0x00035100
		void IOrderedDictionary.RemoveAt(int idx)
		{
			this.EnsureDictionary();
			this.inst_object.Remove(this.object_list[idx].Key);
			this.object_list.RemoveAt(idx);
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00036D40 File Offset: 0x00035140
		private ICollection EnsureCollection()
		{
			if (this.type == JsonType.Array)
			{
				return (ICollection)this.inst_array;
			}
			if (this.type == JsonType.Object)
			{
				return (ICollection)this.inst_object;
			}
			throw new InvalidOperationException("The JsonData instance has to be initialized first");
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00036D7C File Offset: 0x0003517C
		private IDictionary EnsureDictionary()
		{
			if (this.type == JsonType.Object)
			{
				return (IDictionary)this.inst_object;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a dictionary");
			}
			this.type = JsonType.Object;
			this.inst_object = new Dictionary<string, JsonData>();
			this.object_list = new List<KeyValuePair<string, JsonData>>();
			return (IDictionary)this.inst_object;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00036DE0 File Offset: 0x000351E0
		private IList EnsureList()
		{
			if (this.type == JsonType.Array)
			{
				return (IList)this.inst_array;
			}
			if (this.type != JsonType.None)
			{
				throw new InvalidOperationException("Instance of JsonData is not a list");
			}
			this.type = JsonType.Array;
			this.inst_array = new List<JsonData>();
			return (IList)this.inst_array;
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00036E38 File Offset: 0x00035238
		private JsonData ToJsonData(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is JsonData)
			{
				return (JsonData)obj;
			}
			return new JsonData(obj);
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00036E5C File Offset: 0x0003525C
		private static void WriteJson(IJsonWrapper obj, JsonWriter writer)
		{
			if (obj == null)
			{
				writer.Write(null);
				return;
			}
			if (obj.IsString)
			{
				writer.Write(obj.GetString());
				return;
			}
			if (obj.IsBoolean)
			{
				writer.Write(obj.GetBoolean());
				return;
			}
			if (obj.IsDouble)
			{
				writer.Write(obj.GetDouble());
				return;
			}
			if (obj.IsInt)
			{
				writer.Write(obj.GetInt());
				return;
			}
			if (obj.IsLong)
			{
				writer.Write(obj.GetLong());
				return;
			}
			if (obj.IsArray)
			{
				writer.WriteArrayStart();
				IEnumerator enumerator = obj.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						JsonData.WriteJson((JsonData)obj2, writer);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				writer.WriteArrayEnd();
				return;
			}
			if (obj.IsObject)
			{
				writer.WriteObjectStart();
				IDictionaryEnumerator enumerator2 = obj.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj3 = enumerator2.Current;
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
						writer.WritePropertyName((string)dictionaryEntry.Key);
						JsonData.WriteJson((JsonData)dictionaryEntry.Value, writer);
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				writer.WriteObjectEnd();
				return;
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x00036FE4 File Offset: 0x000353E4
		public int Add(object value)
		{
			JsonData value2 = this.ToJsonData(value);
			this.json = null;
			return this.EnsureList().Add(value2);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0003700C File Offset: 0x0003540C
		public void Clear()
		{
			if (this.IsObject)
			{
				((IDictionary)this).Clear();
				return;
			}
			if (this.IsArray)
			{
				((IList)this).Clear();
				return;
			}
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00037034 File Offset: 0x00035434
		public bool Equals(JsonData x)
		{
			if (x == null)
			{
				return false;
			}
			if (x.type != this.type)
			{
				return false;
			}
			switch (this.type)
			{
			case JsonType.None:
				return true;
			case JsonType.Object:
				return this.inst_object.Equals(x.inst_object);
			case JsonType.Array:
				return this.inst_array.Equals(x.inst_array);
			case JsonType.String:
				return this.inst_string.Equals(x.inst_string);
			case JsonType.Int:
				return this.inst_int.Equals(x.inst_int);
			case JsonType.Long:
				return this.inst_long.Equals(x.inst_long);
			case JsonType.Double:
				return this.inst_double.Equals(x.inst_double);
			case JsonType.Boolean:
				return this.inst_boolean.Equals(x.inst_boolean);
			default:
				return false;
			}
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0003710F File Offset: 0x0003550F
		public JsonType GetJsonType()
		{
			return this.type;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00037118 File Offset: 0x00035518
		public void SetJsonType(JsonType type)
		{
			if (this.type == type)
			{
				return;
			}
			switch (type)
			{
			case JsonType.Object:
				this.inst_object = new Dictionary<string, JsonData>();
				this.object_list = new List<KeyValuePair<string, JsonData>>();
				break;
			case JsonType.Array:
				this.inst_array = new List<JsonData>();
				break;
			case JsonType.String:
				this.inst_string = null;
				break;
			case JsonType.Int:
				this.inst_int = 0;
				break;
			case JsonType.Long:
				this.inst_long = 0L;
				break;
			case JsonType.Double:
				this.inst_double = 0.0;
				break;
			case JsonType.Boolean:
				this.inst_boolean = false;
				break;
			}
			this.type = type;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x000371DC File Offset: 0x000355DC
		public string ToJson()
		{
			if (this.json != null)
			{
				return this.json;
			}
			StringWriter stringWriter = new StringWriter();
			JsonData.WriteJson(this, new JsonWriter(stringWriter)
			{
				Validate = false
			});
			this.json = stringWriter.ToString();
			return this.json;
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00037228 File Offset: 0x00035628
		public void ToJson(JsonWriter writer)
		{
			bool validate = writer.Validate;
			writer.Validate = false;
			JsonData.WriteJson(this, writer);
			writer.Validate = validate;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00037254 File Offset: 0x00035654
		public override string ToString()
		{
			switch (this.type)
			{
			case JsonType.Object:
				return "JsonData object";
			case JsonType.Array:
				return "JsonData array";
			case JsonType.String:
				return this.inst_string;
			case JsonType.Int:
				return this.inst_int.ToString();
			case JsonType.Long:
				return this.inst_long.ToString();
			case JsonType.Double:
				return this.inst_double.ToString();
			case JsonType.Boolean:
				return this.inst_boolean.ToString();
			default:
				return "Uninitialized JsonData";
			}
		}

		// Token: 0x040007F6 RID: 2038
		private IList<JsonData> inst_array;

		// Token: 0x040007F7 RID: 2039
		private bool inst_boolean;

		// Token: 0x040007F8 RID: 2040
		private double inst_double;

		// Token: 0x040007F9 RID: 2041
		private int inst_int;

		// Token: 0x040007FA RID: 2042
		private long inst_long;

		// Token: 0x040007FB RID: 2043
		private IDictionary<string, JsonData> inst_object;

		// Token: 0x040007FC RID: 2044
		private string inst_string;

		// Token: 0x040007FD RID: 2045
		private string json;

		// Token: 0x040007FE RID: 2046
		private JsonType type;

		// Token: 0x040007FF RID: 2047
		private IList<KeyValuePair<string, JsonData>> object_list;
	}
}
