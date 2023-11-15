using System;
using UnityEngine;
using UnityEngine.Playables;

// Token: 0x02000009 RID: 9
[Serializable]
public class NavMeshAgentControlBehaviour : PlayableBehaviour
{
	// Token: 0x06000013 RID: 19 RVA: 0x00002723 File Offset: 0x00000923
	public override void OnPlayableCreate(Playable playable)
	{
		this.destinationSet = false;
	}

	// Token: 0x04000021 RID: 33
	public Transform destination;

	// Token: 0x04000022 RID: 34
	public bool destinationSet;
}
