using System;
using System.Text;
using UnityEngine.UI;

// Token: 0x0200035D RID: 861
public class ThaiFontProcessor : ITextProcessor
{
	// Token: 0x06001074 RID: 4212 RVA: 0x0005ED7B File Offset: 0x0005D17B
	private static bool IsBase(char checkChar)
	{
		return (checkChar >= 'ก' && checkChar <= 'ฯ') || checkChar == 'ะ' || checkChar == 'เ' || checkChar == 'แ';
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x0005EDB4 File Offset: 0x0005D1B4
	private static bool IsBaseDesc(char checkChar)
	{
		return checkChar == 'ฎ' || checkChar == 'ฏ';
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x0005EDCC File Offset: 0x0005D1CC
	private static bool IsBaseAsc(char checkChar)
	{
		return checkChar == 'ป' || checkChar == 'ฝ' || checkChar == 'ฟ' || checkChar == 'ฬ';
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x0005EDFA File Offset: 0x0005D1FA
	private static bool IsTop(char checkChar)
	{
		return checkChar >= '่' && checkChar <= '์';
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x0005EE15 File Offset: 0x0005D215
	private static bool IsLower(char checkChar)
	{
		return checkChar >= 'ุ' && checkChar <= 'ฺ';
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x0005EE30 File Offset: 0x0005D230
	private static bool IsUpper(char checkChar)
	{
		return checkChar == 'ั' || checkChar == 'ิ' || checkChar == 'ี' || checkChar == 'ึ' || checkChar == 'ื' || checkChar == '็' || checkChar == 'ํ';
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x0005EE8A File Offset: 0x0005D28A
	public bool OnPopulateMesh(VertexHelper _helper)
	{
		return false;
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x0005EE8D File Offset: 0x0005D28D
	public bool HasEmbeddedImages(string inputString)
	{
		return false;
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x0005EE90 File Offset: 0x0005D290
	public bool ProcessText(ref string thaiString)
	{
		int length = thaiString.Length;
		ThaiFontProcessor.s_FixedString.Length = 0;
		int i = 0;
		while (i < length)
		{
			char c = thaiString[i];
			char c2 = c;
			char c3 = c;
			if (i > 0)
			{
				c2 = thaiString[i - 1];
			}
			if (i > 1)
			{
				c3 = thaiString[i - 2];
			}
			if (ThaiFontProcessor.IsTop(c) && i > 0)
			{
				char checkChar = c2;
				if (ThaiFontProcessor.IsLower(checkChar) && i > 1)
				{
					checkChar = c3;
				}
				if (ThaiFontProcessor.IsBase(checkChar))
				{
					bool flag = i < length - 1 && (c2 == 'ำ' || c2 == 'ํ');
					if (ThaiFontProcessor.IsBaseAsc(checkChar))
					{
						if (flag)
						{
							c += '';
							ThaiFontProcessor.s_FixedString.Append('');
							ThaiFontProcessor.s_FixedString.Append(c);
							if (c2 == 'ำ')
							{
								ThaiFontProcessor.s_FixedString.Append('า');
							}
							i++;
							goto IL_28D;
						}
						c += '';
					}
					else if (!flag)
					{
						c += '';
					}
				}
				if (i > 1 && ThaiFontProcessor.IsUpper(c2) && ThaiFontProcessor.IsBaseAsc(c3))
				{
					c += '';
				}
				goto IL_281;
			}
			if (ThaiFontProcessor.IsUpper(c) && i > 0 && ThaiFontProcessor.IsBaseAsc(c2))
			{
				ThaiFontProcessor.THAICHARACTERS thaicharacters = (ThaiFontProcessor.THAICHARACTERS)c;
				switch (thaicharacters)
				{
				case ThaiFontProcessor.THAICHARACTERS.MAI_HANAKAT:
					c = '';
					break;
				default:
					if (thaicharacters != ThaiFontProcessor.THAICHARACTERS.MAITAIKHU)
					{
						if (thaicharacters == ThaiFontProcessor.THAICHARACTERS.NIKHAHIT)
						{
							c = '';
						}
					}
					else
					{
						c = '';
					}
					break;
				case ThaiFontProcessor.THAICHARACTERS.SARA_I:
					c = '';
					break;
				case ThaiFontProcessor.THAICHARACTERS.SARA_II:
					c = '';
					break;
				case ThaiFontProcessor.THAICHARACTERS.SARA_UE:
					c = '';
					break;
				case ThaiFontProcessor.THAICHARACTERS.SARA_UEE:
					c = '';
					break;
				}
				goto IL_281;
			}
			if (ThaiFontProcessor.IsLower(c) && i > 0 && ThaiFontProcessor.IsBaseDesc(c2))
			{
				c += '';
				goto IL_281;
			}
			if (c == 'ญ' && i < length - 1 && ThaiFontProcessor.IsLower(c2))
			{
				c = '';
				goto IL_281;
			}
			if (c == 'ฐ' && i < length - 1 && ThaiFontProcessor.IsLower(c2))
			{
				c = '';
				goto IL_281;
			}
			goto IL_281;
			IL_28D:
			i++;
			continue;
			IL_281:
			ThaiFontProcessor.s_FixedString.Append(c);
			goto IL_28D;
		}
		thaiString = ThaiFontProcessor.s_FixedString.ToString();
		ThaiFontProcessor.s_FixedString.Length = 0;
		return true;
	}

	// Token: 0x04000C85 RID: 3205
	private static StringBuilder s_FixedString = new StringBuilder();

	// Token: 0x04000C86 RID: 3206
	private const char m_kFirstThaiChar = '฀';

	// Token: 0x04000C87 RID: 3207
	private const char m_kLastThaiChar = '๿';

	// Token: 0x0200035E RID: 862
	private enum THAICHARACTERS
	{
		// Token: 0x04000C89 RID: 3209
		KO_KAI = 3585,
		// Token: 0x04000C8A RID: 3210
		YO_YING = 3597,
		// Token: 0x04000C8B RID: 3211
		DO_CHADA,
		// Token: 0x04000C8C RID: 3212
		TO_PATAK,
		// Token: 0x04000C8D RID: 3213
		THO_THAN,
		// Token: 0x04000C8E RID: 3214
		PO_PLA = 3611,
		// Token: 0x04000C8F RID: 3215
		FO_FA = 3613,
		// Token: 0x04000C90 RID: 3216
		FO_FAN = 3615,
		// Token: 0x04000C91 RID: 3217
		LO_CHULA = 3628,
		// Token: 0x04000C92 RID: 3218
		PAIYANNOI = 3631,
		// Token: 0x04000C93 RID: 3219
		SARA_A,
		// Token: 0x04000C94 RID: 3220
		MAI_HANAKAT,
		// Token: 0x04000C95 RID: 3221
		SARA_AA,
		// Token: 0x04000C96 RID: 3222
		SARA_AM,
		// Token: 0x04000C97 RID: 3223
		SARA_I,
		// Token: 0x04000C98 RID: 3224
		SARA_II,
		// Token: 0x04000C99 RID: 3225
		SARA_UE,
		// Token: 0x04000C9A RID: 3226
		SARA_UEE,
		// Token: 0x04000C9B RID: 3227
		SARA_U,
		// Token: 0x04000C9C RID: 3228
		PHINTHU = 3642,
		// Token: 0x04000C9D RID: 3229
		SARA_E = 3648,
		// Token: 0x04000C9E RID: 3230
		SARA_AE,
		// Token: 0x04000C9F RID: 3231
		MAITAIKHU = 3655,
		// Token: 0x04000CA0 RID: 3232
		MAI_EK,
		// Token: 0x04000CA1 RID: 3233
		THANTHAKHAT = 3660,
		// Token: 0x04000CA2 RID: 3234
		NIKHAHIT,
		// Token: 0x04000CA3 RID: 3235
		THO_THAN_DESCLESS = 63232,
		// Token: 0x04000CA4 RID: 3236
		SARA_I_LEFT,
		// Token: 0x04000CA5 RID: 3237
		SARA_II_LEFT,
		// Token: 0x04000CA6 RID: 3238
		SARA_UE_LEFT,
		// Token: 0x04000CA7 RID: 3239
		SARA_UEE_LEFT,
		// Token: 0x04000CA8 RID: 3240
		MAI_EK_LOW_LEFT,
		// Token: 0x04000CA9 RID: 3241
		MAI_EK_LOW = 63242,
		// Token: 0x04000CAA RID: 3242
		YO_YING_DESCLESS = 63247,
		// Token: 0x04000CAB RID: 3243
		MAI_HANAKAT_LEFT,
		// Token: 0x04000CAC RID: 3244
		SARA_AA_LEFT,
		// Token: 0x04000CAD RID: 3245
		SARA_AM_LEFT,
		// Token: 0x04000CAE RID: 3246
		MAI_EK_LEFT,
		// Token: 0x04000CAF RID: 3247
		SARA_U_LOW = 63256
	}
}
