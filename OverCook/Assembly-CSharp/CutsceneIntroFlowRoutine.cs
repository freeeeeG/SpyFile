using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200067D RID: 1661
public class CutsceneIntroFlowRoutine : LevelIntroFlowroutine
{
	// Token: 0x06001FE2 RID: 8162 RVA: 0x0009AFD0 File Offset: 0x000993D0
	protected override void Awake()
	{
		if (this.m_cutscene != null)
		{
			this.m_cutscene.gameObject.SetActive(false);
		}
	}

	// Token: 0x06001FE3 RID: 8163 RVA: 0x0009AFF4 File Offset: 0x000993F4
	public override void Setup(CallbackVoid _startRoundCallback)
	{
		base.Setup(_startRoundCallback);
		this.m_cutsceneController = this.m_cutscene.gameObject.RequireComponent<ClientCutsceneController>();
		CutsceneController.SetupData setupData = new CutsceneController.SetupData();
		setupData.skippable = true;
		setupData.postplaybackUIEnabled = true;
		this.m_cutsceneRoutine = this.m_cutsceneController.StartCutscene(setupData);
	}

	// Token: 0x06001FE4 RID: 8164 RVA: 0x0009B044 File Offset: 0x00099444
	public override IEnumerator Run()
	{
		if (this.m_cutsceneController != null)
		{
			this.m_cutsceneController.gameObject.SetActive(true);
			while (this.m_cutsceneRoutine != null && this.m_cutsceneRoutine.MoveNext())
			{
				yield return this.m_cutsceneRoutine.Current;
			}
			this.m_cutsceneController.Shutdown();
			this.m_cutsceneController.gameObject.SetActive(false);
		}
		IEnumerator routine = base.Run();
		while (routine.MoveNext())
		{
			object obj = routine.Current;
			yield return obj;
		}
		yield break;
	}

	// Token: 0x04001842 RID: 6210
	[SerializeField]
	private CutsceneController m_cutscene;

	// Token: 0x04001843 RID: 6211
	private ClientCutsceneController m_cutsceneController;

	// Token: 0x04001844 RID: 6212
	private IEnumerator m_cutsceneRoutine;
}
