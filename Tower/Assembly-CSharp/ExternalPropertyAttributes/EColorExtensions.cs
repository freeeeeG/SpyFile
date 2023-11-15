using System;
using UnityEngine;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000046 RID: 70
	public static class EColorExtensions
	{
		// Token: 0x060000BC RID: 188 RVA: 0x000038C4 File Offset: 0x00001AC4
		public static Color GetColor(this EColor color)
		{
			switch (color)
			{
			case EColor.Clear:
				return new Color32(0, 0, 0, 0);
			case EColor.White:
				return new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
			case EColor.Black:
				return new Color32(0, 0, 0, byte.MaxValue);
			case EColor.Gray:
				return new Color32(128, 128, 128, byte.MaxValue);
			case EColor.Red:
				return new Color32(byte.MaxValue, 0, 63, byte.MaxValue);
			case EColor.Pink:
				return new Color32(byte.MaxValue, 152, 203, byte.MaxValue);
			case EColor.Orange:
				return new Color32(byte.MaxValue, 128, 0, byte.MaxValue);
			case EColor.Yellow:
				return new Color32(byte.MaxValue, 211, 0, byte.MaxValue);
			case EColor.Green:
				return new Color32(98, 200, 79, byte.MaxValue);
			case EColor.Blue:
				return new Color32(0, 135, 189, byte.MaxValue);
			case EColor.Indigo:
				return new Color32(75, 0, 130, byte.MaxValue);
			case EColor.Violet:
				return new Color32(128, 0, byte.MaxValue, byte.MaxValue);
			default:
				return new Color32(0, 0, 0, byte.MaxValue);
			}
		}
	}
}
