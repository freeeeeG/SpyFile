using System;

// Token: 0x02000A45 RID: 2629
internal class HasSetSafeArea : BoolOption
{
	// Token: 0x170003AC RID: 940
	// (get) Token: 0x060033FB RID: 13307 RVA: 0x000F39EE File Offset: 0x000F1DEE
	public override string Label
	{
		get
		{
			return "HasSetSafeArea";
		}
	}

	// Token: 0x170003AD RID: 941
	// (get) Token: 0x060033FC RID: 13308 RVA: 0x000F39F5 File Offset: 0x000F1DF5
	public override OptionsData.Categories Category
	{
		get
		{
			return OptionsData.Categories.SafeArea;
		}
	}

	// Token: 0x060033FD RID: 13309 RVA: 0x000F39F8 File Offset: 0x000F1DF8
	public override void Commit()
	{
	}
}
