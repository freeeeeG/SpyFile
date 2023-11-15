using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D08 RID: 3336
	public class MonumentPartResource : PermitResource
	{
		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060069C0 RID: 27072 RVA: 0x00290463 File Offset: 0x0028E663
		// (set) Token: 0x060069C1 RID: 27073 RVA: 0x0029046B File Offset: 0x0028E66B
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060069C2 RID: 27074 RVA: 0x00290474 File Offset: 0x0028E674
		// (set) Token: 0x060069C3 RID: 27075 RVA: 0x0029047C File Offset: 0x0028E67C
		public string SymbolName { get; private set; }

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060069C4 RID: 27076 RVA: 0x00290485 File Offset: 0x0028E685
		// (set) Token: 0x060069C5 RID: 27077 RVA: 0x0029048D File Offset: 0x0028E68D
		public string State { get; private set; }

		// Token: 0x060069C6 RID: 27078 RVA: 0x00290496 File Offset: 0x0028E696
		public MonumentPartResource(string id, string animFilename, string state, string symbolName, MonumentPartResource.Part part) : base(id, "TODO:DbMonumentParts", "TODO:DbMonumentParts", PermitCategory.Artwork, PermitRarity.Unknown)
		{
			this.AnimFile = Assets.GetAnim(animFilename);
			this.SymbolName = symbolName;
			this.State = state;
			this.part = part;
		}

		// Token: 0x060069C7 RID: 27079 RVA: 0x002904D4 File Offset: 0x0028E6D4
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			Sprite sprite = Assets.GetSprite("unknown");
			return new global::Tuple<Sprite, Color>(sprite, (sprite != null) ? Color.white : Color.clear);
		}

		// Token: 0x060069C8 RID: 27080 RVA: 0x00290500 File Offset: 0x0028E700
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = this.GetUISprite().first;
			result.SetFacadeForText("_monument part");
			return result;
		}

		// Token: 0x04004C53 RID: 19539
		public MonumentPartResource.Part part;

		// Token: 0x02001C32 RID: 7218
		public enum Part
		{
			// Token: 0x04008004 RID: 32772
			Bottom,
			// Token: 0x04008005 RID: 32773
			Middle,
			// Token: 0x04008006 RID: 32774
			Top
		}
	}
}
