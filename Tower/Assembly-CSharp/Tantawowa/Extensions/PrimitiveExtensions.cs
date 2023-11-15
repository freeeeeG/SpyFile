using System;
using UnityEngine;

namespace Tantawowa.Extensions
{
	// Token: 0x02000077 RID: 119
	public static class PrimitiveExtensions
	{
		// Token: 0x060001BB RID: 443 RVA: 0x00007DD0 File Offset: 0x00005FD0
		public static int ClampIndex(this int value, int min, int max)
		{
			if (value > max)
			{
				return 0;
			}
			if (value < min)
			{
				return max;
			}
			return value;
		}

		// Token: 0x060001BC RID: 444 RVA: 0x00007DE0 File Offset: 0x00005FE0
		public static bool IsValidAsType(this string input, Type type)
		{
			bool result = false;
			if (type == typeof(string))
			{
				result = true;
			}
			else if (type == typeof(float))
			{
				float num;
				result = float.TryParse(input, out num);
			}
			else if (type == typeof(int))
			{
				int num2;
				result = int.TryParse(input, out num2);
			}
			else if (type == typeof(bool))
			{
				bool flag;
				result = bool.TryParse(input, out flag);
			}
			return result;
		}

		// Token: 0x060001BD RID: 445 RVA: 0x00007E5C File Offset: 0x0000605C
		public static T ConvertToType<T>(this string input)
		{
			Type typeFromHandle = typeof(T);
			if (typeFromHandle == typeof(string))
			{
				return (T)((object)input);
			}
			bool flag;
			if (typeFromHandle == typeof(float))
			{
				float num;
				if (float.TryParse(input, out num))
				{
					return (T)((object)num);
				}
			}
			else if (typeFromHandle == typeof(int))
			{
				int num2;
				if (int.TryParse(input, out num2))
				{
					return (T)((object)num2);
				}
			}
			else if (typeFromHandle == typeof(bool) && bool.TryParse(input, out flag))
			{
				return (T)((object)flag);
			}
			return default(T);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x00007F0F File Offset: 0x0000610F
		public static double RoundUpToNearest(this double passednumber, double roundto)
		{
			if (roundto != 0.0)
			{
				return Math.Ceiling(passednumber / roundto) * roundto;
			}
			return passednumber;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00007F29 File Offset: 0x00006129
		public static double RoundDownToNearest(this double passednumber, double roundto)
		{
			if (roundto != 0.0)
			{
				return Math.Floor(passednumber / roundto) * roundto;
			}
			return passednumber;
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x00007F43 File Offset: 0x00006143
		public static float RoundUpToNearest(this float passednumber, float roundto)
		{
			if (roundto != 0f)
			{
				return Mathf.Ceil(passednumber / roundto) * roundto;
			}
			return passednumber;
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x00007F59 File Offset: 0x00006159
		public static float RoundDownToNearest(this float passednumber, float roundto)
		{
			if (roundto != 0f)
			{
				return Mathf.Floor(passednumber / roundto) * roundto;
			}
			return passednumber;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x00007F6F File Offset: 0x0000616F
		public static int RoundUpToNearest(this int passednumber, int roundto)
		{
			return (int)((roundto == 0) ? ((float)passednumber) : (Mathf.Ceil((float)(passednumber / roundto)) * (float)roundto));
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x00007F85 File Offset: 0x00006185
		public static int RoundDownToNearest(this int passednumber, int roundto)
		{
			return (int)((roundto == 0) ? ((float)passednumber) : (Mathf.Floor((float)(passednumber / roundto)) * (float)roundto));
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x00007F9B File Offset: 0x0000619B
		public static int Flip(this int value)
		{
			if (value != 0)
			{
				return 0;
			}
			return 1;
		}
	}
}
