using System;
using UnityEngine;

// Token: 0x02000102 RID: 258
[CreateAssetMenu(menuName = "Factory/NonEnemyFactory", fileName = "nonEnemyFactory")]
public class NonEnemyFactory : GameObjectFactory
{
	// Token: 0x06000679 RID: 1657 RVA: 0x000117F8 File Offset: 0x0000F9F8
	public AirAttacker GetAirAttacker()
	{
		return Singleton<ObjectPool>.Instance.Spawn(this.aircraftPrefab) as AirAttacker;
	}

	// Token: 0x0600067A RID: 1658 RVA: 0x0001180F File Offset: 0x0000FA0F
	public AirProtector GetAirProtector()
	{
		return Singleton<ObjectPool>.Instance.Spawn(this.strongerAircraftPrefab) as AirProtector;
	}

	// Token: 0x040002F8 RID: 760
	[SerializeField]
	private AirAttacker aircraftPrefab;

	// Token: 0x040002F9 RID: 761
	[SerializeField]
	private AirProtector strongerAircraftPrefab;
}
