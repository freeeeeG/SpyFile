using System;
using UnityEngine;

// Token: 0x02000AD8 RID: 2776
public class LeaveLobbyButton : MonoBehaviour
{
	// Token: 0x06003825 RID: 14373 RVA: 0x00108908 File Offset: 0x00106D08
	public void LeaveLobby()
	{
		ClientLobbyFlowController component = this.flowGO.GetComponent<ClientLobbyFlowController>();
		component.ShowLeaveDialog();
	}

	// Token: 0x04002CE2 RID: 11490
	[SerializeField]
	private GameObject flowGO;
}
