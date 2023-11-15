using System;
using UnityEngine;

// Token: 0x02000021 RID: 33
public class LeanTest
{
	// Token: 0x06000241 RID: 577 RVA: 0x0000ECBD File Offset: 0x0000CEBD
	public static void debug(string name, bool didPass, string failExplaination = null)
	{
		LeanTest.expect(didPass, name, failExplaination);
	}

	// Token: 0x06000242 RID: 578 RVA: 0x0000ECC8 File Offset: 0x0000CEC8
	public static void expect(bool didPass, string definition, string failExplaination = null)
	{
		float num = LeanTest.printOutLength(definition);
		int totalWidth = 40 - (int)(num * 1.05f);
		string text = "".PadRight(totalWidth, "_"[0]);
		string text2 = string.Concat(new string[]
		{
			LeanTest.formatB(definition),
			" ",
			text,
			" [ ",
			didPass ? LeanTest.formatC("pass", "green") : LeanTest.formatC("fail", "red"),
			" ]"
		});
		if (!didPass && failExplaination != null)
		{
			text2 = text2 + " - " + failExplaination;
		}
		Debug.Log(text2);
		if (didPass)
		{
			LeanTest.passes++;
		}
		LeanTest.tests++;
		if (LeanTest.tests == LeanTest.expected && !LeanTest.testsFinished)
		{
			LeanTest.overview();
		}
		else if (LeanTest.tests > LeanTest.expected)
		{
			Debug.Log(LeanTest.formatB("Too many tests for a final report!") + " set LeanTest.expected = " + LeanTest.tests);
		}
		if (!LeanTest.timeoutStarted)
		{
			LeanTest.timeoutStarted = true;
			GameObject gameObject = new GameObject();
			gameObject.name = "~LeanTest";
			(gameObject.AddComponent(typeof(LeanTester)) as LeanTester).timeout = LeanTest.timeout;
			gameObject.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	// Token: 0x06000243 RID: 579 RVA: 0x0000EE18 File Offset: 0x0000D018
	public static string padRight(int len)
	{
		string text = "";
		for (int i = 0; i < len; i++)
		{
			text += "_";
		}
		return text;
	}

	// Token: 0x06000244 RID: 580 RVA: 0x0000EE44 File Offset: 0x0000D044
	public static float printOutLength(string str)
	{
		float num = 0f;
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == "I"[0])
			{
				num += 0.5f;
			}
			else if (str[i] == "J"[0])
			{
				num += 0.85f;
			}
			else
			{
				num += 1f;
			}
		}
		return num;
	}

	// Token: 0x06000245 RID: 581 RVA: 0x0000EEAD File Offset: 0x0000D0AD
	public static string formatBC(string str, string color)
	{
		return LeanTest.formatC(LeanTest.formatB(str), color);
	}

	// Token: 0x06000246 RID: 582 RVA: 0x0000EEBB File Offset: 0x0000D0BB
	public static string formatB(string str)
	{
		return "<b>" + str + "</b>";
	}

	// Token: 0x06000247 RID: 583 RVA: 0x0000EECD File Offset: 0x0000D0CD
	public static string formatC(string str, string color)
	{
		return string.Concat(new string[]
		{
			"<color=",
			color,
			">",
			str,
			"</color>"
		});
	}

	// Token: 0x06000248 RID: 584 RVA: 0x0000EEFC File Offset: 0x0000D0FC
	public static void overview()
	{
		LeanTest.testsFinished = true;
		int num = LeanTest.expected - LeanTest.passes;
		string text = (num > 0) ? LeanTest.formatBC(string.Concat(num), "red") : string.Concat(num);
		Debug.Log(string.Concat(new string[]
		{
			LeanTest.formatB("Final Report:"),
			" _____________________ PASSED: ",
			LeanTest.formatBC(string.Concat(LeanTest.passes), "green"),
			" FAILED: ",
			text,
			" "
		}));
	}

	// Token: 0x04000101 RID: 257
	public static int expected = 0;

	// Token: 0x04000102 RID: 258
	private static int tests = 0;

	// Token: 0x04000103 RID: 259
	private static int passes = 0;

	// Token: 0x04000104 RID: 260
	public static float timeout = 15f;

	// Token: 0x04000105 RID: 261
	public static bool timeoutStarted = false;

	// Token: 0x04000106 RID: 262
	public static bool testsFinished = false;
}
