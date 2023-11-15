using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000789 RID: 1929
public class TutorialIntroFlowroutine : CutsceneIntroFlowRoutine
{
	// Token: 0x06002550 RID: 9552 RVA: 0x000B0C7C File Offset: 0x000AF07C
	protected override void Awake()
	{
		if (this.m_iconController != null)
		{
			this.m_iconController.enabled = false;
		}
	}

	// Token: 0x06002551 RID: 9553 RVA: 0x000B0C9C File Offset: 0x000AF09C
	public override IEnumerator Run()
	{
		IEnumerator routine = base.Run();
		while (routine.MoveNext())
		{
			object obj = routine.Current;
			yield return obj;
		}
		if (this.m_iconController != null)
		{
			this.m_iconController.enabled = true;
		}
		yield break;
	}

	// Token: 0x04001CE9 RID: 7401
	[SerializeField]
	private TutorialIconController m_iconController;
}
