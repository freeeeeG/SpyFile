using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters
{
	// Token: 0x02000743 RID: 1859
	public class WitchSettings : ScriptableObject
	{
		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x060025D6 RID: 9686 RVA: 0x000722A4 File Offset: 0x000704A4
		public static WitchSettings instance
		{
			get
			{
				if (WitchSettings._instance == null)
				{
					WitchSettings._instance = Resources.Load<WitchSettings>("WitchSettings");
					WitchSettings._instance.Initialize();
				}
				return WitchSettings._instance;
			}
		}

		// Token: 0x060025D7 RID: 9687 RVA: 0x00002191 File Offset: 0x00000391
		private void Initialize()
		{
		}

		// Token: 0x04001FFE RID: 8190
		private static WitchSettings _instance;

		// Token: 0x04001FFF RID: 8191
		[Header("두개골")]
		[SerializeField]
		public int[] 골수이식_비용;

		// Token: 0x04002000 RID: 8192
		[SerializeField]
		public int 골수이식_마법공격력p;

		// Token: 0x04002001 RID: 8193
		[SerializeField]
		public int[] 신속한탈골_비용;

		// Token: 0x04002002 RID: 8194
		[SerializeField]
		public float 신속한탈골_교대대기시간가속;

		// Token: 0x04002003 RID: 8195
		[Space]
		[SerializeField]
		[FormerlySerializedAs("칼슘투여_비용")]
		public int[] 영양공급_비용;

		// Token: 0x04002004 RID: 8196
		[SerializeField]
		public float 영양공급_스킬쿨다운p;

		// Token: 0x04002005 RID: 8197
		[SerializeField]
		[Space]
		public int[] 외골격강화_비용;

		// Token: 0x04002006 RID: 8198
		[SerializeField]
		public float 외골격강화_보호막;

		// Token: 0x04002007 RID: 8199
		[SerializeField]
		[Information("고정", InformationAttribute.InformationType.Info, false)]
		public float 외골격강화_보호막지속시간;

		// Token: 0x04002008 RID: 8200
		[SerializeField]
		[Information("고정", InformationAttribute.InformationType.Info, false)]
		public float 외골격강화_교대대기시간감소;

		// Token: 0x04002009 RID: 8201
		[SerializeField]
		[Header("뼈")]
		public int[] 통뼈_비용;

		// Token: 0x0400200A RID: 8202
		[SerializeField]
		public int 통뼈_물리공격력p;

		// Token: 0x0400200B RID: 8203
		[SerializeField]
		[Space]
		public int[] 골절상면역_비용;

		// Token: 0x0400200C RID: 8204
		[SerializeField]
		public int 골절상면역_체력증가;

		// Token: 0x0400200D RID: 8205
		[Space]
		[FormerlySerializedAs("유연한척추_비용")]
		[SerializeField]
		public int[] 육중한뼈대_비용;

		// Token: 0x0400200E RID: 8206
		[SerializeField]
		public float 육중한뼈대_받는피해;

		// Token: 0x0400200F RID: 8207
		[Space]
		[SerializeField]
		public int[] 재조립_비용;

		// Token: 0x04002010 RID: 8208
		[SerializeField]
		public int 재조립_체력회복p;

		// Token: 0x04002011 RID: 8209
		[Space]
		[Header("영혼")]
		[SerializeField]
		public int[] 영혼가속_비용;

		// Token: 0x04002012 RID: 8210
		[SerializeField]
		public float 영혼가속_치명타확률p;

		// Token: 0x04002013 RID: 8211
		[Space]
		[SerializeField]
		public int[] 선조의의지_비용;

		// Token: 0x04002014 RID: 8212
		[SerializeField]
		public int 선조의의지_정수쿨다운가속p;

		// Token: 0x04002015 RID: 8213
		[SerializeField]
		[Space]
		public int[] 날카로운정신_비용;

		// Token: 0x04002016 RID: 8214
		[SerializeField]
		public int 날카로운정신_공격속도p;

		// Token: 0x04002017 RID: 8215
		[SerializeField]
		public int 날카로운정신_이동속도p;

		// Token: 0x04002018 RID: 8216
		[Space]
		[SerializeField]
		public int[] 고대연금술_비용;

		// Token: 0x04002019 RID: 8217
		[Header("아이템")]
		[SerializeField]
		public int 고대연금술_골드량_커먼;

		// Token: 0x0400201A RID: 8218
		[SerializeField]
		public int 고대연금술_골드량_레어;

		// Token: 0x0400201B RID: 8219
		[SerializeField]
		public int 고대연금술_골드량_유니크;

		// Token: 0x0400201C RID: 8220
		[SerializeField]
		public int 고대연금술_골드량_레전더리;

		// Token: 0x0400201D RID: 8221
		[SerializeField]
		[Header("정수")]
		public int 고대연금술_골드량_정수_커먼;

		// Token: 0x0400201E RID: 8222
		[SerializeField]
		public int 고대연금술_골드량_정수_레어;

		// Token: 0x0400201F RID: 8223
		[SerializeField]
		public int 고대연금술_골드량_정수_유니크;

		// Token: 0x04002020 RID: 8224
		[SerializeField]
		public int 고대연금술_골드량_정수_레전더리;
	}
}
