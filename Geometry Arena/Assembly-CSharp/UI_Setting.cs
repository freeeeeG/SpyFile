using System;
using UnityEngine;

// Token: 0x0200000C RID: 12
[CreateAssetMenu(fileName = "UI_Setting", menuName = "CreateAsset/UI_Setting")]
public class UI_Setting : ScriptableObject
{
	// Token: 0x1700005C RID: 92
	// (get) Token: 0x0600008E RID: 142 RVA: 0x00004893 File Offset: 0x00002A93
	public static UI_Setting Inst
	{
		get
		{
			if (AssetManager.inst != null)
			{
				return AssetManager.inst.UI_Setting;
			}
			return Resources.Load<UI_Setting>("Assets/UI_Setting");
		}
	}

	// Token: 0x040000CD RID: 205
	public float ToolTip_HeaderSize = 35f;

	// Token: 0x040000CE RID: 206
	public float ToolTip_NormalSize = 25f;

	// Token: 0x040000CF RID: 207
	public float ToolTip_SmallSize = 15f;

	// Token: 0x040000D0 RID: 208
	public Color color_Input;

	// Token: 0x040000D1 RID: 209
	public Color ToolTip_SmallColor = Color.gray;

	// Token: 0x040000D2 RID: 210
	public int main_TitleSize = 40;

	// Token: 0x040000D3 RID: 211
	[Header("Resolutions")]
	public global::Resolution[] resolutions = new global::Resolution[0];

	// Token: 0x040000D4 RID: 212
	[Header("Main")]
	public UI_Setting.UpdateLog updateLog = new UI_Setting.UpdateLog();

	// Token: 0x040000D5 RID: 213
	public UI_Setting.InfinityMode infinityMode = new UI_Setting.InfinityMode();

	// Token: 0x040000D6 RID: 214
	public UI_Setting.CommonSets commonSets = new UI_Setting.CommonSets();

	// Token: 0x040000D7 RID: 215
	[Header("Outline")]
	public Color outline_ColorAbove = Color.blue;

	// Token: 0x040000D8 RID: 216
	public Color outline_ColorSelected = Color.yellow;

	// Token: 0x040000D9 RID: 217
	public float outline_Distance = 8f;

	// Token: 0x040000DA RID: 218
	[Header("MyColor")]
	public Color myRed = Color.red;

	// Token: 0x040000DB RID: 219
	public Color myGreen = Color.green;

	// Token: 0x040000DC RID: 220
	public Color myYellow = Color.yellow;

	// Token: 0x040000DD RID: 221
	public Color[] rankColors = new Color[4];

	// Token: 0x040000DE RID: 222
	[Header("Button")]
	public Color button_Highlight = Color.yellow;

	// Token: 0x040000DF RID: 223
	[Header("Shop")]
	[SerializeField]
	public UI_Setting.Shop shop = new UI_Setting.Shop();

	// Token: 0x040000E0 RID: 224
	[Header("Skill")]
	[SerializeField]
	public UI_Setting.Skill skill = new UI_Setting.Skill();

	// Token: 0x040000E1 RID: 225
	[Header("CommonLog")]
	[SerializeField]
	public UI_Setting.CommonLog commonLog = new UI_Setting.CommonLog();

	// Token: 0x040000E2 RID: 226
	[Header("RankList")]
	[SerializeField]
	public UI_Setting.RankList rankList = new UI_Setting.RankList();

	// Token: 0x040000E3 RID: 227
	[Header("UpgradeIcon")]
	[SerializeField]
	public UI_Setting.UpgradeIcon upgradeIcon = new UI_Setting.UpgradeIcon();

	// Token: 0x040000E4 RID: 228
	[Header("Addition")]
	[SerializeField]
	public UI_Setting.Addition addition = new UI_Setting.Addition();

	// Token: 0x02000120 RID: 288
	[Serializable]
	public class Shop
	{
		// Token: 0x04000902 RID: 2306
		public int size_Name = 40;

		// Token: 0x04000903 RID: 2307
		public int size_RareLine = 25;

		// Token: 0x04000904 RID: 2308
		public int size_SmallTile = 30;

		// Token: 0x04000905 RID: 2309
		public int size_Info = 25;

		// Token: 0x04000906 RID: 2310
		public Color color_SoldOut = Color.red;

		// Token: 0x04000907 RID: 2311
		public Color color_RankLine;

		// Token: 0x04000908 RID: 2312
		public Color color_SmallTitle;

		// Token: 0x04000909 RID: 2313
		public Color color_SmallTitleOnShow;

		// Token: 0x0400090A RID: 2314
		public Color color_Price;

		// Token: 0x0400090B RID: 2315
		public Color color_Buy;
	}

	// Token: 0x02000121 RID: 289
	[Serializable]
	public class Skill
	{
		// Token: 0x0400090C RID: 2316
		public int size_Name = 40;

		// Token: 0x0400090D RID: 2317
		public int size_Type = 25;

		// Token: 0x0400090E RID: 2318
		public int size_SmallTile = 30;

		// Token: 0x0400090F RID: 2319
		public int size_Info = 20;

		// Token: 0x04000910 RID: 2320
		public Color Color_Type;

		// Token: 0x04000911 RID: 2321
		public Color Color_SmallTitle;

		// Token: 0x04000912 RID: 2322
		public Color Color_FactorType;

		// Token: 0x04000913 RID: 2323
		public Color Color_FactorNumber;

		// Token: 0x04000914 RID: 2324
		public Color Color_Red;
	}

	// Token: 0x02000122 RID: 290
	[Serializable]
	public class UpdateLog
	{
		// Token: 0x04000915 RID: 2325
		public TextSet setTitle = new TextSet();

		// Token: 0x04000916 RID: 2326
		public TextSet setVersion = new TextSet();

		// Token: 0x04000917 RID: 2327
		public TextSet setSmallTitle = new TextSet();

		// Token: 0x04000918 RID: 2328
		public TextSet setNormal = new TextSet();

		// Token: 0x04000919 RID: 2329
		public TextSet setButton = new TextSet();
	}

	// Token: 0x02000123 RID: 291
	[Serializable]
	public class InfinityMode
	{
		// Token: 0x0400091A RID: 2330
		public TextSet setTitle = new TextSet();

		// Token: 0x0400091B RID: 2331
		public TextSet lockedOrOff = new TextSet();

		// Token: 0x0400091C RID: 2332
		public TextSet open = new TextSet();

		// Token: 0x0400091D RID: 2333
		public TextSet info = new TextSet();
	}

	// Token: 0x02000124 RID: 292
	[Serializable]
	public class CommonSets
	{
		// Token: 0x0400091E RID: 2334
		public TextSet blueTitle = new TextSet();

		// Token: 0x0400091F RID: 2335
		public TextSet blueSmallTile = new TextSet();

		// Token: 0x04000920 RID: 2336
		public TextSet factorInfo = new TextSet();
	}

	// Token: 0x02000125 RID: 293
	[Serializable]
	public class CommonLog
	{
		// Token: 0x04000921 RID: 2337
		public TextSet smallTitle = new TextSet();

		// Token: 0x04000922 RID: 2338
		public TextSet normalText = new TextSet();

		// Token: 0x04000923 RID: 2339
		public TextSet specialText = new TextSet();
	}

	// Token: 0x02000126 RID: 294
	[Serializable]
	public class RankList
	{
		// Token: 0x04000924 RID: 2340
		public Color colorMyRank;

		// Token: 0x04000925 RID: 2341
		public Color color011;
	}

	// Token: 0x02000127 RID: 295
	[Serializable]
	public class UpgradeIcon
	{
		// Token: 0x04000926 RID: 2342
		public Color shadeColor;

		// Token: 0x04000927 RID: 2343
		public float shadeDeltaY;
	}

	// Token: 0x02000128 RID: 296
	[Serializable]
	public class Addition
	{
		// Token: 0x04000928 RID: 2344
		public int title_Size = 25;

		// Token: 0x04000929 RID: 2345
		public TextSet info_Set = new TextSet();
	}
}
