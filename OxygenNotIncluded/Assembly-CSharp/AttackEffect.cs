using System;

// Token: 0x020006E4 RID: 1764
[Serializable]
public class AttackEffect
{
	// Token: 0x06003058 RID: 12376 RVA: 0x000FFB4B File Offset: 0x000FDD4B
	public AttackEffect(string ID, float probability)
	{
		this.effectID = ID;
		this.effectProbability = probability;
	}

	// Token: 0x04001C8A RID: 7306
	public string effectID;

	// Token: 0x04001C8B RID: 7307
	public float effectProbability;
}
