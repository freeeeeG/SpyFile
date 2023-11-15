using System;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

// Token: 0x02000073 RID: 115
public static class MyTool
{
	// Token: 0x0600044D RID: 1101 RVA: 0x0001B3CD File Offset: 0x000195CD
	public static float Vec2toAngle180(Vector2 direction)
	{
		return Mathf.Atan2(direction.y, direction.x) / 3.1415927f * 180f;
	}

	// Token: 0x0600044E RID: 1102 RVA: 0x0001B3EC File Offset: 0x000195EC
	public static Vector2 AngleToVec2(float angle)
	{
		float f = angle / 180f * 3.1415927f;
		return new Vector2(Mathf.Cos(f), Mathf.Sin(f));
	}

	// Token: 0x0600044F RID: 1103 RVA: 0x0001B418 File Offset: 0x00019618
	public static Vector2 MouseToPlayerVec2()
	{
		Vector2 a = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 b = Player.inst.gameObject.transform.position;
		return a - b;
	}

	// Token: 0x06000450 RID: 1104 RVA: 0x0001B459 File Offset: 0x00019659
	public static Vector2 MousePos()
	{
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	// Token: 0x06000451 RID: 1105 RVA: 0x0001B470 File Offset: 0x00019670
	public static float MouseToPlayerAngle()
	{
		Vector2 vector = MyTool.MouseToPlayerVec2();
		return Mathf.Atan2(vector.y, vector.x) / 3.1415927f * 180f;
	}

	// Token: 0x06000452 RID: 1106 RVA: 0x0001B4A0 File Offset: 0x000196A0
	public static float MouseToPoint0()
	{
		Vector2 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return Mathf.Atan2(vector.y, vector.x) / 3.1415927f * 180f;
	}

	// Token: 0x06000453 RID: 1107 RVA: 0x0001B4E0 File Offset: 0x000196E0
	public static int DecimalToInt(float num)
	{
		if (num < 0f)
		{
			Debug.LogError("Decimal<0?");
		}
		int num2 = Mathf.FloorToInt(num);
		num -= (float)num2;
		float num3 = UnityEngine.Random.Range(0f, 1f);
		if (num > num3)
		{
			num2++;
		}
		return num2;
	}

	// Token: 0x06000454 RID: 1108 RVA: 0x0001B528 File Offset: 0x00019728
	public static long DoubleToLong(double num)
	{
		if (num < 0.0)
		{
			Debug.LogError("Decimal<0?" + num);
		}
		long num2 = (long)math.round(num);
		num -= (double)num2;
		float num3 = UnityEngine.Random.Range(0f, 1f);
		if (num > (double)num3)
		{
			num2 += 1L;
		}
		return num2;
	}

	// Token: 0x06000455 RID: 1109 RVA: 0x0001B580 File Offset: 0x00019780
	public static string DecimalToPercentString(double num)
	{
		num = (double)((float)((int)math.round(num * 1000.0)) / 1000f);
		string str = "";
		if (num >= 0.0)
		{
			str += "+";
		}
		return str + num.ToSmartString_Percent();
	}

	// Token: 0x06000456 RID: 1110 RVA: 0x0001B5D4 File Offset: 0x000197D4
	public static string DecimalToUnsignedPercentString(double num)
	{
		return num.ToSmartString_Percent();
	}

	// Token: 0x06000457 RID: 1111 RVA: 0x0001B5DC File Offset: 0x000197DC
	public static float GetScaleWithSizeShape(EnumShapeType shape, float size)
	{
		return Mathf.Sqrt(size);
	}

	// Token: 0x06000458 RID: 1112 RVA: 0x0001B5E4 File Offset: 0x000197E4
	public static bool ifIsAddFactorID(int id)
	{
		return (id >= 10 && id <= 13) || id == 1;
	}

	// Token: 0x06000459 RID: 1113 RVA: 0x0001B5F7 File Offset: 0x000197F7
	public static string DecimalToMultiPercentString(double num)
	{
		return "x" + num.ToSmartString_Percent();
	}

	// Token: 0x0600045A RID: 1114 RVA: 0x0001B609 File Offset: 0x00019809
	public static bool DecimalToBool(float deci)
	{
		if (deci > 1f)
		{
			return true;
		}
		if (deci > 5f)
		{
			Debug.LogWarning("decimal>5!");
			return true;
		}
		return UnityEngine.Random.Range(0f, 1f) <= deci;
	}

	// Token: 0x0600045B RID: 1115 RVA: 0x0001B640 File Offset: 0x00019840
	public static string ColorfulSignedFactorMultiPercent(int type, float num)
	{
		UI_Setting inst = UI_Setting.Inst;
		if (type == 5 || type == 12)
		{
			return null;
		}
		int num2 = 1;
		if (type >= 10 && type <= 13)
		{
			num2 = 0;
		}
		float num3 = num - (float)num2;
		if (num3 == 0f)
		{
			return null;
		}
		Color color = ((float)((type == 14 || type == 8) ? -1 : 1) * num3 > 0f) ? inst.myGreen : inst.myRed;
		if (MyTool.ifIsAddFactorID(type))
		{
			return MyTool.DecimalToPercentString((double)num).Colored(color);
		}
		return MyTool.DecimalToMultiPercentString((double)num).Colored(color);
	}

	// Token: 0x0600045C RID: 1116 RVA: 0x0001B6C4 File Offset: 0x000198C4
	public static string ColorfulSignedFactorPlusPercent(int type, float num)
	{
		UI_Setting inst = UI_Setting.Inst;
		if (type == 1)
		{
			if (num > 0f)
			{
				return ("+" + num).Colored(inst.myGreen);
			}
			return num.ToString().Colored(inst.myRed);
		}
		else
		{
			if (type == 5 || type == 12)
			{
				return null;
			}
			if (num == 0f)
			{
				return null;
			}
			Color color = ((float)((type == 14 || type == 8) ? -1 : 1) * num > 0f) ? inst.myGreen : inst.myRed;
			return MyTool.DecimalToPercentString((double)num).Colored(color);
		}
	}

	// Token: 0x0600045D RID: 1117 RVA: 0x0001B75C File Offset: 0x0001995C
	public static string DecimalToAutoPercentString(double num, int type)
	{
		if (type >= 200)
		{
			type -= 200;
		}
		else if (type >= 100)
		{
			type -= 100;
		}
		if (!MyTool.ifIsAddFactorID(type))
		{
			return MyTool.DecimalToMultiPercentString(num);
		}
		if (type == 1)
		{
			return MyTool.DecimalToAddNumber(num).ToString();
		}
		return MyTool.DecimalToPercentString(num);
	}

	// Token: 0x0600045E RID: 1118 RVA: 0x0001B7AC File Offset: 0x000199AC
	public static string DecimalToAutoPercentString_Colorful(double num, int type, bool ifFactorBattle)
	{
		UI_Setting inst = UI_Setting.Inst;
		if (type >= 200)
		{
			type -= 200;
		}
		else if (type >= 100)
		{
			type -= 100;
		}
		int num2;
		if (!ifFactorBattle)
		{
			num2 = ((type == 14 || type == 8) ? -1 : 1);
		}
		else
		{
			num2 = ((type == 0 || type == 2 || type == 3 || type == 4 || type == 9 || type == 10 || type == 11) ? 1 : -1);
		}
		double num3 = ifFactorBattle ? (num - 1.0) : (num - (double)MyTool.factorOrigin[type]);
		Color color = ((double)num2 * num3 >= 0.0) ? inst.myGreen : inst.myRed;
		if (!ifFactorBattle)
		{
			return MyTool.DecimalToAutoPercentString(num, type).Colored(color);
		}
		return MyTool.DecimalToMultiPercentString(num).Colored(color);
	}

	// Token: 0x0600045F RID: 1119 RVA: 0x0001B86B File Offset: 0x00019A6B
	public static string DecimalToAddNumber(double num)
	{
		if (num > 0.0)
		{
			return "+" + num;
		}
		return num.ToString();
	}

	// Token: 0x06000460 RID: 1120 RVA: 0x0001B891 File Offset: 0x00019A91
	public static float BounceMirror(float angleIn, float angleWallNormal)
	{
		return angleWallNormal - (angleIn - angleWallNormal) + 180f;
	}

	// Token: 0x06000461 RID: 1121 RVA: 0x0001B8A0 File Offset: 0x00019AA0
	public static Vector2 BounceMirror(Vector2 vec2In, Vector2 vec2WallNormal)
	{
		float angleIn = MyTool.Vec2toAngle180(vec2In);
		float angleWallNormal = MyTool.Vec2toAngle180(vec2WallNormal);
		return MyTool.AngleToVec2(MyTool.BounceMirror(angleIn, angleWallNormal));
	}

	// Token: 0x06000462 RID: 1122 RVA: 0x0001B8C5 File Offset: 0x00019AC5
	public static bool ifSimiliar(float a, float b)
	{
		return a > 0.99f * b && b > 0.99f * a;
	}

	// Token: 0x06000463 RID: 1123 RVA: 0x0001B8E0 File Offset: 0x00019AE0
	public static string GetColorfulStringWithTypeAndNum(int bigType, int smallType, double num, bool ifAllPlus, float deciDigi, bool ifColored = true)
	{
		UI_Setting inst = UI_Setting.Inst;
		double num2;
		if (ifAllPlus)
		{
			num2 = 0.0;
		}
		else if (bigType == 1 || bigType == 2)
		{
			num2 = (double)MyTool.factorOrigin[smallType];
		}
		else
		{
			num2 = 1.0;
		}
		bool flag;
		if (num == num2)
		{
			flag = true;
		}
		else if (bigType == 1 || bigType == 2)
		{
			if (smallType == 8)
			{
				flag = false;
			}
			else
			{
				flag = (smallType != 14);
				if (bigType == 2)
				{
					flag = !flag;
				}
			}
		}
		else
		{
			flag = (smallType == 0 || smallType == 2 || smallType == 3 || smallType == 4 || smallType == 9 || smallType == 10 || smallType == 11 || smallType == 12);
		}
		bool flag2 = num >= num2;
		Color color = ((flag && flag2) || (!flag & !flag2)) ? inst.myGreen : inst.myRed;
		string str;
		if (num2 == 0.0)
		{
			if (num >= num2)
			{
				str = "+";
			}
			else
			{
				str = "";
			}
		}
		else
		{
			str = "x";
		}
		string str2;
		if (bigType != 0 && (smallType == 1 || smallType == 5))
		{
			str2 = num.ToString();
		}
		else
		{
			str2 = num.ToSmartString_Percent();
		}
		if (ifColored)
		{
			return (str + str2).Colored(color);
		}
		return str + str2;
	}

	// Token: 0x06000464 RID: 1124 RVA: 0x0001BA1C File Offset: 0x00019C1C
	public static string GetVersion(int vInt)
	{
		int num = vInt / 10000;
		int num2 = (vInt - num * 10000) / 100;
		int num3 = vInt % 100;
		return string.Format("v{0}.{1}.{2}", num, num2, num3);
	}

	// Token: 0x06000465 RID: 1125 RVA: 0x0001BA60 File Offset: 0x00019C60
	public static Vector2 Cross(Vector2 posOrigin, Vector2 direction, float n, bool ifY)
	{
		if (!ifY)
		{
			if (direction.x == 0f)
			{
				Debug.LogError("Error_xDir==0 无交点");
				return Vector2.zero;
			}
			float num = n;
			if (direction.x < 0f)
			{
				num = -n;
			}
			float d = (num - posOrigin.x) / direction.x;
			return posOrigin + d * direction;
		}
		else
		{
			if (direction.y == 0f)
			{
				Debug.LogError("Error_yDir==0 无交点");
				return Vector2.zero;
			}
			float num = n;
			if (direction.y < 0f)
			{
				num = -n;
			}
			float d = (num - posOrigin.y) / direction.y;
			return posOrigin + d * direction;
		}
	}

	// Token: 0x06000466 RID: 1126 RVA: 0x0001BB0C File Offset: 0x00019D0C
	public static string BoolsToStringDO(bool[] bools)
	{
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < bools.Length; i++)
		{
			stringBuilder.Append(bools[i] ? 1 : 0);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x06000467 RID: 1127 RVA: 0x0001BB44 File Offset: 0x00019D44
	public static bool[] StringDOToBools(string stringDO, int length)
	{
		bool[] array = new bool[length];
		for (int i = 0; i < math.min(array.Length, stringDO.Length); i++)
		{
			array[i] = (stringDO[i] == '\u0001');
		}
		return array;
	}

	// Token: 0x04000393 RID: 915
	public static float[] factorOrigin = new float[]
	{
		1f,
		0f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		1f,
		0f,
		0f,
		0f,
		0f,
		1f,
		1f
	};
}
