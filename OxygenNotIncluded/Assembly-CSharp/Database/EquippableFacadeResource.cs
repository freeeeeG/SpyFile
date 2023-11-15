using System;
using UnityEngine;

namespace Database
{
	// Token: 0x02000D01 RID: 3329
	public class EquippableFacadeResource : PermitResource
	{
		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x060069A3 RID: 27043 RVA: 0x0028CEC6 File Offset: 0x0028B0C6
		// (set) Token: 0x060069A4 RID: 27044 RVA: 0x0028CECE File Offset: 0x0028B0CE
		public string BuildOverride { get; private set; }

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x060069A5 RID: 27045 RVA: 0x0028CED7 File Offset: 0x0028B0D7
		// (set) Token: 0x060069A6 RID: 27046 RVA: 0x0028CEDF File Offset: 0x0028B0DF
		public string DefID { get; private set; }

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060069A7 RID: 27047 RVA: 0x0028CEE8 File Offset: 0x0028B0E8
		// (set) Token: 0x060069A8 RID: 27048 RVA: 0x0028CEF0 File Offset: 0x0028B0F0
		public KAnimFile AnimFile { get; private set; }

		// Token: 0x060069A9 RID: 27049 RVA: 0x0028CEF9 File Offset: 0x0028B0F9
		public EquippableFacadeResource(string id, string name, string buildOverride, string defID, string animFile) : base(id, name, "n/a", PermitCategory.Equipment, PermitRarity.Unknown)
		{
			this.DefID = defID;
			this.BuildOverride = buildOverride;
			this.AnimFile = Assets.GetAnim(animFile);
		}

		// Token: 0x060069AA RID: 27050 RVA: 0x0028CF2C File Offset: 0x0028B12C
		public global::Tuple<Sprite, Color> GetUISprite()
		{
			if (this.AnimFile == null)
			{
				global::Debug.LogError("Facade AnimFile is null: " + this.DefID);
			}
			Sprite uispriteFromMultiObjectAnim = Def.GetUISpriteFromMultiObjectAnim(this.AnimFile, "ui", false, "");
			return new global::Tuple<Sprite, Color>(uispriteFromMultiObjectAnim, (uispriteFromMultiObjectAnim != null) ? Color.white : Color.clear);
		}

		// Token: 0x060069AB RID: 27051 RVA: 0x0028CF8C File Offset: 0x0028B18C
		public override PermitPresentationInfo GetPermitPresentationInfo()
		{
			PermitPresentationInfo result = default(PermitPresentationInfo);
			result.sprite = this.GetUISprite().first;
			GameObject gameObject = Assets.TryGetPrefab(this.DefID);
			if (gameObject == null || !gameObject)
			{
				result.SetFacadeForPrefabID(this.DefID);
			}
			else
			{
				result.SetFacadeForPrefabName(gameObject.GetProperName());
			}
			return result;
		}
	}
}
