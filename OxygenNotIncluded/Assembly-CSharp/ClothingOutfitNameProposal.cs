using System;
using System.Runtime.CompilerServices;

// Token: 0x020006D3 RID: 1747
public readonly struct ClothingOutfitNameProposal
{
	// Token: 0x06002FAF RID: 12207 RVA: 0x000FC239 File Offset: 0x000FA439
	private ClothingOutfitNameProposal(string candidateName, ClothingOutfitNameProposal.Result result)
	{
		this.candidateName = candidateName;
		this.result = result;
	}

	// Token: 0x06002FB0 RID: 12208 RVA: 0x000FC24C File Offset: 0x000FA44C
	public static ClothingOutfitNameProposal ForNewOutfit(string candidateName)
	{
		ClothingOutfitNameProposal.<>c__DisplayClass3_0 CS$<>8__locals1;
		CS$<>8__locals1.candidateName = candidateName;
		if (string.IsNullOrEmpty(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.Error_NoInputName, ref CS$<>8__locals1);
		}
		if (ClothingOutfitTarget.DoesTemplateExist(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists, ref CS$<>8__locals1);
		}
		return ClothingOutfitNameProposal.<ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result.NewOutfit, ref CS$<>8__locals1);
	}

	// Token: 0x06002FB1 RID: 12209 RVA: 0x000FC298 File Offset: 0x000FA498
	public static ClothingOutfitNameProposal FromExistingOutfit(string candidateName, ClothingOutfitTarget existingOutfit, bool isSameNameAllowed)
	{
		ClothingOutfitNameProposal.<>c__DisplayClass4_0 CS$<>8__locals1;
		CS$<>8__locals1.candidateName = candidateName;
		if (string.IsNullOrEmpty(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_NoInputName, ref CS$<>8__locals1);
		}
		if (!ClothingOutfitTarget.DoesTemplateExist(CS$<>8__locals1.candidateName))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.NewOutfit, ref CS$<>8__locals1);
		}
		if (!isSameNameAllowed || !(CS$<>8__locals1.candidateName == existingOutfit.ReadName()))
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_NameAlreadyExists, ref CS$<>8__locals1);
		}
		if (existingOutfit.CanWriteName)
		{
			return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.SameOutfit, ref CS$<>8__locals1);
		}
		return ClothingOutfitNameProposal.<FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result.Error_SameOutfitReadonly, ref CS$<>8__locals1);
	}

	// Token: 0x06002FB2 RID: 12210 RVA: 0x000FC313 File Offset: 0x000FA513
	[CompilerGenerated]
	internal static ClothingOutfitNameProposal <ForNewOutfit>g__Make|3_0(ClothingOutfitNameProposal.Result result, ref ClothingOutfitNameProposal.<>c__DisplayClass3_0 A_1)
	{
		return new ClothingOutfitNameProposal(A_1.candidateName, result);
	}

	// Token: 0x06002FB3 RID: 12211 RVA: 0x000FC321 File Offset: 0x000FA521
	[CompilerGenerated]
	internal static ClothingOutfitNameProposal <FromExistingOutfit>g__Make|4_0(ClothingOutfitNameProposal.Result result, ref ClothingOutfitNameProposal.<>c__DisplayClass4_0 A_1)
	{
		return new ClothingOutfitNameProposal(A_1.candidateName, result);
	}

	// Token: 0x04001C47 RID: 7239
	public readonly string candidateName;

	// Token: 0x04001C48 RID: 7240
	public readonly ClothingOutfitNameProposal.Result result;

	// Token: 0x02001411 RID: 5137
	public enum Result
	{
		// Token: 0x04006440 RID: 25664
		None,
		// Token: 0x04006441 RID: 25665
		NewOutfit,
		// Token: 0x04006442 RID: 25666
		SameOutfit,
		// Token: 0x04006443 RID: 25667
		Error_NoInputName,
		// Token: 0x04006444 RID: 25668
		Error_NameAlreadyExists,
		// Token: 0x04006445 RID: 25669
		Error_SameOutfitReadonly
	}
}
