using System;
using UnityEngine;

// Token: 0x02000933 RID: 2355
[AddComponentMenu("KMonoBehaviour/scripts/Reservable")]
public class Reservable : KMonoBehaviour
{
	// Token: 0x170004CA RID: 1226
	// (get) Token: 0x06004464 RID: 17508 RVA: 0x0017FC2D File Offset: 0x0017DE2D
	public GameObject ReservedBy
	{
		get
		{
			return this.reservedBy;
		}
	}

	// Token: 0x170004CB RID: 1227
	// (get) Token: 0x06004465 RID: 17509 RVA: 0x0017FC35 File Offset: 0x0017DE35
	public bool isReserved
	{
		get
		{
			return !(this.reservedBy == null);
		}
	}

	// Token: 0x06004466 RID: 17510 RVA: 0x0017FC46 File Offset: 0x0017DE46
	public bool Reserve(GameObject reserver)
	{
		if (this.reservedBy == null)
		{
			this.reservedBy = reserver;
			return true;
		}
		return false;
	}

	// Token: 0x06004467 RID: 17511 RVA: 0x0017FC60 File Offset: 0x0017DE60
	public void ClearReservation(GameObject reserver)
	{
		if (this.reservedBy == reserver)
		{
			this.reservedBy = null;
		}
	}

	// Token: 0x04002D55 RID: 11605
	private GameObject reservedBy;
}
