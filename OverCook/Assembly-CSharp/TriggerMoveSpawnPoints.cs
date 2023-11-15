using System;
using UnityEngine;

// Token: 0x020005E6 RID: 1510
public class TriggerMoveSpawnPoints : MonoBehaviour
{
	// Token: 0x0400166F RID: 5743
	[SerializeField]
	public string m_trigger = string.Empty;

	// Token: 0x04001670 RID: 5744
	[SerializeField]
	public bool m_movePlayersImmediately;

	// Token: 0x04001671 RID: 5745
	[SerializeField]
	public Transform[] m_spawnPoints = new Transform[0];
}
