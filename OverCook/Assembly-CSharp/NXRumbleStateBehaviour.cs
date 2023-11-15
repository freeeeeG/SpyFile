using System;
using UnityEngine;

// Token: 0x02000106 RID: 262
public class NXRumbleStateBehaviour : StateMachineBehaviour
{
	// Token: 0x060004E5 RID: 1253 RVA: 0x00028AEC File Offset: 0x00026EEC
	public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
	}

	// Token: 0x060004E6 RID: 1254 RVA: 0x00028AEE File Offset: 0x00026EEE
	public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
	{
		this.m_finished = true;
	}

	// Token: 0x04000441 RID: 1089
	[SerializeField]
	private float m_lowAmplitude = 0.5f;

	// Token: 0x04000442 RID: 1090
	[SerializeField]
	private float m_lowFreq = 50f;

	// Token: 0x04000443 RID: 1091
	[SerializeField]
	private float m_highAmplitude = 0.5f;

	// Token: 0x04000444 RID: 1092
	[SerializeField]
	private float m_highFreq = 60f;

	// Token: 0x04000445 RID: 1093
	private NXRumbleManager m_NXRumbleManager;

	// Token: 0x04000446 RID: 1094
	private bool m_finished;
}
