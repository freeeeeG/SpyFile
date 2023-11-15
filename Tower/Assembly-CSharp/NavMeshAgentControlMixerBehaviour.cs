using System;
using UnityEngine.AI;
using UnityEngine.Playables;

// Token: 0x0200000B RID: 11
public class NavMeshAgentControlMixerBehaviour : PlayableBehaviour
{
	// Token: 0x06000018 RID: 24 RVA: 0x0000278C File Offset: 0x0000098C
	public override void ProcessFrame(Playable playable, FrameData info, object playerData)
	{
		NavMeshAgent navMeshAgent = playerData as NavMeshAgent;
		if (!navMeshAgent)
		{
			return;
		}
		int inputCount = playable.GetInputCount<Playable>();
		for (int i = 0; i < inputCount; i++)
		{
			float inputWeight = playable.GetInputWeight(i);
			NavMeshAgentControlBehaviour behaviour = ((ScriptPlayable<T>)playable.GetInput(i)).GetBehaviour();
			if (inputWeight > 0.5f && !behaviour.destinationSet && behaviour.destination && navMeshAgent.isOnNavMesh)
			{
				navMeshAgent.SetDestination(behaviour.destination.position);
				behaviour.destinationSet = true;
			}
		}
	}
}
