using System;
using KSerialization;
using STRINGS;

// Token: 0x02000B7E RID: 2942
public class GeneticAnalysisCompleteMessage : Message
{
	// Token: 0x06005B5E RID: 23390 RVA: 0x00218DAF File Offset: 0x00216FAF
	public GeneticAnalysisCompleteMessage()
	{
	}

	// Token: 0x06005B5F RID: 23391 RVA: 0x00218DB7 File Offset: 0x00216FB7
	public GeneticAnalysisCompleteMessage(Tag subSpeciesID)
	{
		this.subSpeciesID = subSpeciesID;
	}

	// Token: 0x06005B60 RID: 23392 RVA: 0x00218DC6 File Offset: 0x00216FC6
	public override string GetSound()
	{
		return "";
	}

	// Token: 0x06005B61 RID: 23393 RVA: 0x00218DD0 File Offset: 0x00216FD0
	public override string GetMessageBody()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.MESSAGEBODY.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName()).Replace("{Subspecies}", subSpeciesInfo.GetNameWithMutations(subSpeciesInfo.speciesID.ProperName(), true, false)).Replace("{Info}", subSpeciesInfo.GetMutationsTooltip());
	}

	// Token: 0x06005B62 RID: 23394 RVA: 0x00218E37 File Offset: 0x00217037
	public override string GetTitle()
	{
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.NAME;
	}

	// Token: 0x06005B63 RID: 23395 RVA: 0x00218E44 File Offset: 0x00217044
	public override string GetTooltip()
	{
		PlantSubSpeciesCatalog.SubSpeciesInfo subSpeciesInfo = PlantSubSpeciesCatalog.Instance.FindSubSpecies(this.subSpeciesID);
		return MISC.NOTIFICATIONS.GENETICANALYSISCOMPLETE.TOOLTIP.Replace("{Plant}", subSpeciesInfo.speciesID.ProperName());
	}

	// Token: 0x06005B64 RID: 23396 RVA: 0x00218E7C File Offset: 0x0021707C
	public override bool IsValid()
	{
		return this.subSpeciesID.IsValid;
	}

	// Token: 0x04003DA7 RID: 15783
	[Serialize]
	public Tag subSpeciesID;
}
