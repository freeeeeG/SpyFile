using System;

// Token: 0x0200025B RID: 603
public class ArtifactTier
{
	// Token: 0x06000C24 RID: 3108 RVA: 0x00044A0D File Offset: 0x00042C0D
	public ArtifactTier(StringKey str_key, EffectorValues values, float payload_drop_chance)
	{
		this.decorValues = values;
		this.name_key = str_key;
		this.payloadDropChance = payload_drop_chance;
	}

	// Token: 0x04000735 RID: 1845
	public EffectorValues decorValues;

	// Token: 0x04000736 RID: 1846
	public StringKey name_key;

	// Token: 0x04000737 RID: 1847
	public float payloadDropChance;
}
