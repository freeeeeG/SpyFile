using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000673 RID: 1651
public class GraveyardAnimatedDynamicTransition : AnimatedDynamicTransition
{
	// Token: 0x06001F83 RID: 8067 RVA: 0x00099964 File Offset: 0x00097D64
	public override IEnumerator Run()
	{
		Suppressor serverSuppressor = null;
		Suppressor clientSuppressor = null;
		FlowControllerBase flowController = GameUtils.RequireManager<FlowControllerBase>();
		ServerKitchenFlowControllerBase serverFlow = flowController.gameObject.RequestComponent<ServerKitchenFlowControllerBase>();
		if (serverFlow != null && serverFlow.RoundTimer != null)
		{
			serverSuppressor = serverFlow.RoundTimer.Suppressor.AddSuppressor(this);
		}
		ClientKitchenFlowControllerBase clientFlow = flowController.gameObject.RequestComponent<ClientKitchenFlowControllerBase>();
		if (clientFlow != null && clientFlow.RoundTimer != null)
		{
			clientSuppressor = clientFlow.RoundTimer.Suppressor.AddSuppressor(this);
		}
		Canvas hoverIconCanvas = GameUtils.GetNamedCanvas("HoverIconCanvas").RequireComponent<Canvas>();
		hoverIconCanvas.enabled = false;
		foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
		{
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				IAttachment[] componentsInChildren = gameObject.GetComponentsInChildren<IAttachment>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					if (componentsInChildren[j] != null && !(componentsInChildren[j].AccessGameObject() == null))
					{
						if (!componentsInChildren[j].IsAttached())
						{
							NetworkUtils.DestroyObject(componentsInChildren[j].AccessGameObject());
						}
					}
				}
			}
		}
		IEnumerator transitionRoutine = base.Run();
		while (transitionRoutine.MoveNext())
		{
			object obj = transitionRoutine.Current;
			yield return obj;
		}
		hoverIconCanvas.enabled = true;
		if (serverSuppressor != null)
		{
			serverSuppressor.Release();
		}
		if (clientSuppressor != null)
		{
			clientSuppressor.Release();
		}
		BossLevelOutroFlowroutine bossOutro = flowController.gameObject.RequireComponent<BossLevelOutroFlowroutine>();
		bossOutro.SetOutroDirectors(this.m_newSuccessCutscene, this.m_newFailureCutscene);
		yield break;
	}

	// Token: 0x04001805 RID: 6149
	[Header("Outro Cutscenes")]
	[SerializeField]
	private CutsceneController m_newSuccessCutscene;

	// Token: 0x04001806 RID: 6150
	[SerializeField]
	private CutsceneController m_newFailureCutscene;
}
