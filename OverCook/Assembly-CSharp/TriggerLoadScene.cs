using System;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000184 RID: 388
public class TriggerLoadScene : MonoBehaviour
{
	// Token: 0x060006B8 RID: 1720 RVA: 0x0002D7F9 File Offset: 0x0002BBF9
	private void OnTrigger(string msg)
	{
		if (this.m_changeSceneMessage == msg)
		{
			GameUtils.LoadScene(this.m_sceneName, LoadSceneMode.Single);
		}
	}

	// Token: 0x04000596 RID: 1430
	[SerializeField]
	private string m_changeSceneMessage = "change scene";

	// Token: 0x04000597 RID: 1431
	[SerializeField]
	private string m_sceneName = "default";
}
