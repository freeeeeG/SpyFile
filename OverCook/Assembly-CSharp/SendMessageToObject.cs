using System;
using UnityEngine;

// Token: 0x0200010A RID: 266
public class SendMessageToObject : StateMachineBehaviour
{
	// Token: 0x060004F0 RID: 1264 RVA: 0x00028C5C File Offset: 0x0002705C
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (this.m_objectName != string.Empty)
		{
			GameObject gameObject = GameObject.Find(this.m_objectName);
			if (gameObject != null)
			{
				gameObject.SendMessage(this.m_message, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	// Token: 0x04000454 RID: 1108
	[SerializeField]
	private string m_objectName;

	// Token: 0x04000455 RID: 1109
	[SerializeField]
	private string m_message;
}
