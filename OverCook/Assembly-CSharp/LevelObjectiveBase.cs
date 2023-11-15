using System;
using UnityEngine;

// Token: 0x02000A60 RID: 2656
[Serializable]
public abstract class LevelObjectiveBase : ScriptableObject, IObjective
{
	// Token: 0x06003463 RID: 13411
	public abstract void Initialise();

	// Token: 0x06003464 RID: 13412
	public abstract void CleanUp();

	// Token: 0x06003465 RID: 13413 RVA: 0x000F61CD File Offset: 0x000F45CD
	public virtual bool IsObjectiveComplete()
	{
		return true;
	}
}
