using System;

// Token: 0x02000033 RID: 51
[Serializable]
public class RuneGood
{
	// Token: 0x0600024D RID: 589 RVA: 0x0000DBF4 File Offset: 0x0000BDF4
	public void InitRandom()
	{
		this.theRune = Rune.InitSpecial_RuneStore(ref this.thePrice, -1);
		int int_RecyclePrice = this.theRune.GetInt_RecyclePrice();
		if (this.thePrice <= int_RecyclePrice)
		{
			this.thePrice = int_RecyclePrice;
		}
	}

	// Token: 0x0600024E RID: 590 RVA: 0x0000DC30 File Offset: 0x0000BE30
	public void TryBuyThisRuneGood()
	{
		if (GameData.inst.GeometryCoin < (long)this.thePrice)
		{
			UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.lackOfGeometryCoin);
			return;
		}
		GameData.inst.GeometryCoin_Use((long)this.thePrice);
		Rune.AddNewRune(this.theRune);
		this.indexInGood = -1;
		UI_FloatTextControl.inst.Special_AnyString(LanguageText.Inst.floatText.rune_BuyRune.Replace("sPrice", this.thePrice.ToString()).Replace("sRune", this.theRune.Get_Lang_RuneName()));
		UI_Panel_Rune_RuneDetail.inst.ClearRuneGood();
		UI_Panel_Rune_RuneDetail.inst.UpdatePreview();
		UI_Panel_Main_RunePanel.inst.Open();
	}

	// Token: 0x040001FE RID: 510
	public int indexInGood = -1;

	// Token: 0x040001FF RID: 511
	public Rune theRune;

	// Token: 0x04000200 RID: 512
	public int thePrice;
}
