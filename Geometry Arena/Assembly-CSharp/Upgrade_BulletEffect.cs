using System;
using UnityEngine;

// Token: 0x0200003D RID: 61
[Serializable]
public class Upgrade_BulletEffect
{
	// Token: 0x0600027E RID: 638 RVA: 0x0000F00C File Offset: 0x0000D20C
	public string BulletEffectFacToString()
	{
		string text = "";
		LanguageText inst = LanguageText.Inst;
		string[] factor = inst.factor;
		int num = 0;
		switch (this.bulletEffectType)
		{
		case Upgrade_BulletEffect.EnumBulletEffect.GROWING:
			text = text + inst.bulletEffectInfo.growing + "\n";
			break;
		case Upgrade_BulletEffect.EnumBulletEffect.SUDDEN:
			text = text + inst.bulletEffectInfo.sudden.Replace("rng", MyTool.DecimalToUnsignedPercentString((double)this.bulletEffectTrigRange).Colored(UI_Setting.Inst.skill.Color_FactorNumber)) + "\n";
			break;
		case Upgrade_BulletEffect.EnumBulletEffect.OTHER:
			Debug.Log("Other");
			return null;
		default:
			Debug.LogError("UNINITED??");
			return null;
		}
		foreach (Upgrade_BulletEffect.Fac fac in this.bulletEffectFacs)
		{
			if (fac.type >= 0)
			{
				int num2 = this.BulletEffectFacTypeToNormalFacType(fac.type);
				if (num > 0)
				{
					text += "\n";
				}
				text = text + factor[num2] + " ";
				text += MyTool.ColorfulSignedFactorMultiPercent(num2, fac.numMul);
				num++;
			}
		}
		return text;
	}

	// Token: 0x0600027F RID: 639 RVA: 0x0000F134 File Offset: 0x0000D334
	public int BulletEffectFacTypeToNormalFacType(int type)
	{
		switch (type)
		{
		case 0:
			return 4;
		case 1:
			return 6;
		case 2:
			return 9;
		default:
			Debug.LogError("typeError!");
			return 0;
		}
	}

	// Token: 0x04000246 RID: 582
	public string name;

	// Token: 0x04000247 RID: 583
	[Header("BulletEffect")]
	public float bulletEffectTrigRange = -1f;

	// Token: 0x04000248 RID: 584
	public Upgrade_BulletEffect.EnumBulletEffect bulletEffectType = Upgrade_BulletEffect.EnumBulletEffect.UNINITED;

	// Token: 0x04000249 RID: 585
	public Upgrade_BulletEffect.Fac[] bulletEffectFacs = new Upgrade_BulletEffect.Fac[3];

	// Token: 0x02000148 RID: 328
	public enum EnumBulletEffect
	{
		// Token: 0x040009A0 RID: 2464
		UNINITED = -1,
		// Token: 0x040009A1 RID: 2465
		GROWING,
		// Token: 0x040009A2 RID: 2466
		SUDDEN,
		// Token: 0x040009A3 RID: 2467
		OTHER
	}

	// Token: 0x02000149 RID: 329
	[Serializable]
	public class Fac
	{
		// Token: 0x040009A4 RID: 2468
		public int type = -1;

		// Token: 0x040009A5 RID: 2469
		public float numMul = -1f;
	}
}
