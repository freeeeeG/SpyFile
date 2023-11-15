using System;
using UnityEngine;

namespace GameResources
{
	// Token: 0x0200018B RID: 395
	public class MaterialResource : ScriptableObject
	{
		// Token: 0x170001EC RID: 492
		// (get) Token: 0x0600088E RID: 2190 RVA: 0x00018C96 File Offset: 0x00016E96
		// (set) Token: 0x0600088F RID: 2191 RVA: 0x00018C9D File Offset: 0x00016E9D
		public static Material color { get; private set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000890 RID: 2192 RVA: 0x00018CA5 File Offset: 0x00016EA5
		// (set) Token: 0x06000891 RID: 2193 RVA: 0x00018CAC File Offset: 0x00016EAC
		public static Material colorOverlay { get; private set; }

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000892 RID: 2194 RVA: 0x00018CB4 File Offset: 0x00016EB4
		// (set) Token: 0x06000893 RID: 2195 RVA: 0x00018CBB File Offset: 0x00016EBB
		public static Material character { get; private set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000894 RID: 2196 RVA: 0x00018CC3 File Offset: 0x00016EC3
		// (set) Token: 0x06000895 RID: 2197 RVA: 0x00018CCA File Offset: 0x00016ECA
		public static Material effect { get; private set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000896 RID: 2198 RVA: 0x00018CD2 File Offset: 0x00016ED2
		// (set) Token: 0x06000897 RID: 2199 RVA: 0x00018CD9 File Offset: 0x00016ED9
		public static Material effect_darken { get; private set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00018CE1 File Offset: 0x00016EE1
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x00018CE8 File Offset: 0x00016EE8
		public static Material effect_lighten { get; private set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00018CF0 File Offset: 0x00016EF0
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x00018CF7 File Offset: 0x00016EF7
		public static Material effect_linearBurn { get; private set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00018CFF File Offset: 0x00016EFF
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00018D06 File Offset: 0x00016F06
		public static Material effect_linearDodge { get; private set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600089E RID: 2206 RVA: 0x00018D0E File Offset: 0x00016F0E
		// (set) Token: 0x0600089F RID: 2207 RVA: 0x00018D15 File Offset: 0x00016F15
		public static Material minimap { get; private set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060008A0 RID: 2208 RVA: 0x00018D1D File Offset: 0x00016F1D
		// (set) Token: 0x060008A1 RID: 2209 RVA: 0x00018D24 File Offset: 0x00016F24
		public static Material ui_grayScale { get; private set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060008A2 RID: 2210 RVA: 0x00018D2C File Offset: 0x00016F2C
		// (set) Token: 0x060008A3 RID: 2211 RVA: 0x00018D33 File Offset: 0x00016F33
		public static Material darkEnemy { get; private set; }

		// Token: 0x060008A4 RID: 2212 RVA: 0x00018D3C File Offset: 0x00016F3C
		public void Initialize()
		{
			base.hideFlags |= HideFlags.DontUnloadUnusedAsset;
			MaterialResource.color = this._color;
			MaterialResource.colorOverlay = this._colorOverlay;
			MaterialResource.character = this._character;
			MaterialResource.effect = this._effect;
			MaterialResource.effect_darken = this._effect_darken;
			MaterialResource.effect_lighten = this._effect_lighten;
			MaterialResource.effect_linearBurn = this._effect_linearBurn;
			MaterialResource.effect_linearDodge = this._effect_linearDodge;
			MaterialResource.minimap = this._minimap;
			MaterialResource.ui_grayScale = this._ui_grayScale;
			MaterialResource.darkEnemy = this._darkEnemy;
		}

		// Token: 0x040006E4 RID: 1764
		[SerializeField]
		private Material _color;

		// Token: 0x040006E5 RID: 1765
		[SerializeField]
		private Material _colorOverlay;

		// Token: 0x040006E6 RID: 1766
		[SerializeField]
		private Material _character;

		// Token: 0x040006E7 RID: 1767
		[SerializeField]
		private Material _effect;

		// Token: 0x040006E8 RID: 1768
		[SerializeField]
		private Material _effect_darken;

		// Token: 0x040006E9 RID: 1769
		[SerializeField]
		private Material _effect_lighten;

		// Token: 0x040006EA RID: 1770
		[SerializeField]
		private Material _effect_linearBurn;

		// Token: 0x040006EB RID: 1771
		[SerializeField]
		private Material _effect_linearDodge;

		// Token: 0x040006EC RID: 1772
		[SerializeField]
		private Material _minimap;

		// Token: 0x040006ED RID: 1773
		[SerializeField]
		private Material _ui_grayScale;

		// Token: 0x040006EE RID: 1774
		[SerializeField]
		private Material _darkEnemy;
	}
}
