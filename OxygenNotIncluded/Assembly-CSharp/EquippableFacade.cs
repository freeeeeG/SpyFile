using System;
using KSerialization;

// Token: 0x020007A2 RID: 1954
public class EquippableFacade : KMonoBehaviour
{
	// Token: 0x0600364C RID: 13900 RVA: 0x0012576C File Offset: 0x0012396C
	public static void AddFacadeToEquippable(Equippable equippable, string facadeID)
	{
		EquippableFacade equippableFacade = equippable.gameObject.AddOrGet<EquippableFacade>();
		equippableFacade.FacadeID = facadeID;
		equippableFacade.BuildOverride = Db.GetEquippableFacades().Get(facadeID).BuildOverride;
		equippableFacade.ApplyAnimOverride();
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x0012579B File Offset: 0x0012399B
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.OverrideName();
		this.ApplyAnimOverride();
	}

	// Token: 0x170003F1 RID: 1009
	// (get) Token: 0x0600364E RID: 13902 RVA: 0x001257AF File Offset: 0x001239AF
	// (set) Token: 0x0600364F RID: 13903 RVA: 0x001257B7 File Offset: 0x001239B7
	public string FacadeID
	{
		get
		{
			return this._facadeID;
		}
		private set
		{
			this._facadeID = value;
			this.OverrideName();
		}
	}

	// Token: 0x06003650 RID: 13904 RVA: 0x001257C6 File Offset: 0x001239C6
	public void ApplyAnimOverride()
	{
		if (this.FacadeID.IsNullOrWhiteSpace())
		{
			return;
		}
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			Db.GetEquippableFacades().Get(this.FacadeID).AnimFile
		});
	}

	// Token: 0x06003651 RID: 13905 RVA: 0x001257FF File Offset: 0x001239FF
	private void OverrideName()
	{
		base.GetComponent<KSelectable>().SetName(EquippableFacade.GetNameOverride(base.GetComponent<Equippable>().def.Id, this.FacadeID));
	}

	// Token: 0x06003652 RID: 13906 RVA: 0x00125827 File Offset: 0x00123A27
	public static string GetNameOverride(string defID, string facadeID)
	{
		if (facadeID.IsNullOrWhiteSpace())
		{
			return Strings.Get("STRINGS.EQUIPMENT.PREFABS." + defID.ToUpper() + ".NAME");
		}
		return Db.GetEquippableFacades().Get(facadeID).Name;
	}

	// Token: 0x04002128 RID: 8488
	[Serialize]
	private string _facadeID;

	// Token: 0x04002129 RID: 8489
	[Serialize]
	public string BuildOverride;
}
