using System;
using System.Collections;
using System.Collections.Specialized;

namespace LitJson
{
	// Token: 0x0200023B RID: 571
	public interface IJsonWrapper : IList, IOrderedDictionary, IEnumerable, ICollection, IDictionary
	{
		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600098E RID: 2446
		bool IsArray { get; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600098F RID: 2447
		bool IsBoolean { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000990 RID: 2448
		bool IsDouble { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000991 RID: 2449
		bool IsInt { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000992 RID: 2450
		bool IsLong { get; }

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000993 RID: 2451
		bool IsObject { get; }

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000994 RID: 2452
		bool IsString { get; }

		// Token: 0x06000995 RID: 2453
		bool GetBoolean();

		// Token: 0x06000996 RID: 2454
		double GetDouble();

		// Token: 0x06000997 RID: 2455
		int GetInt();

		// Token: 0x06000998 RID: 2456
		JsonType GetJsonType();

		// Token: 0x06000999 RID: 2457
		long GetLong();

		// Token: 0x0600099A RID: 2458
		string GetString();

		// Token: 0x0600099B RID: 2459
		void SetBoolean(bool val);

		// Token: 0x0600099C RID: 2460
		void SetDouble(double val);

		// Token: 0x0600099D RID: 2461
		void SetInt(int val);

		// Token: 0x0600099E RID: 2462
		void SetJsonType(JsonType type);

		// Token: 0x0600099F RID: 2463
		void SetLong(long val);

		// Token: 0x060009A0 RID: 2464
		void SetString(string val);

		// Token: 0x060009A1 RID: 2465
		string ToJson();

		// Token: 0x060009A2 RID: 2466
		void ToJson(JsonWriter writer);
	}
}
