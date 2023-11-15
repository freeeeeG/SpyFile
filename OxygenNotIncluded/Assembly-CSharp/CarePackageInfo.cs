using System;
using UnityEngine;

// Token: 0x02000805 RID: 2053
public class CarePackageInfo : ITelepadDeliverable
{
	// Token: 0x06003ADC RID: 15068 RVA: 0x00146E1E File Offset: 0x0014501E
	public CarePackageInfo(string ID, float amount, Func<bool> requirement)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
	}

	// Token: 0x06003ADD RID: 15069 RVA: 0x00146E3B File Offset: 0x0014503B
	public CarePackageInfo(string ID, float amount, Func<bool> requirement, string facadeID)
	{
		this.id = ID;
		this.quantity = amount;
		this.requirement = requirement;
		this.facadeID = facadeID;
	}

	// Token: 0x06003ADE RID: 15070 RVA: 0x00146E60 File Offset: 0x00145060
	public GameObject Deliver(Vector3 location)
	{
		location += Vector3.right / 2f;
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(CarePackageConfig.ID), location);
		gameObject.SetActive(true);
		gameObject.GetComponent<CarePackage>().SetInfo(this);
		return gameObject;
	}

	// Token: 0x040026FA RID: 9978
	public readonly string id;

	// Token: 0x040026FB RID: 9979
	public readonly float quantity;

	// Token: 0x040026FC RID: 9980
	public readonly Func<bool> requirement;

	// Token: 0x040026FD RID: 9981
	public readonly string facadeID;
}
