using System;
using UnityEngine;

// Token: 0x02000A8E RID: 2702
[Serializable]
public class EmoteWheelOptions : ScriptableObject
{
	// Token: 0x0600358B RID: 13707 RVA: 0x000FA4B8 File Offset: 0x000F88B8
	protected virtual void Awake()
	{
		for (int i = 0; i < this.m_options.Length; i++)
		{
			this.m_options[i].m_animTriggerHash = Animator.StringToHash(this.m_options[i].m_animTrigger);
		}
	}

	// Token: 0x0600358C RID: 13708 RVA: 0x000FA4FD File Offset: 0x000F88FD
	public EmoteWheelOption.Connection[] ConnectionsForButton(int _buttonIdx)
	{
		if (_buttonIdx < 0 || _buttonIdx > 7)
		{
			return this.m_originConnections;
		}
		return this.m_options[_buttonIdx].m_connections;
	}

	// Token: 0x04002AED RID: 10989
	public const int c_MaxEmotes = 6;

	// Token: 0x04002AEE RID: 10990
	[SerializeField]
	public float m_radius = 100f;

	// Token: 0x04002AEF RID: 10991
	[SerializeField]
	public GameObject m_wheelPrefab;

	// Token: 0x04002AF0 RID: 10992
	[SerializeField]
	public EmoteWheelOption[] m_options = new EmoteWheelOption[6];

	// Token: 0x04002AF1 RID: 10993
	[SerializeField]
	public EmoteWheelOption.Connection[] m_originConnections = new EmoteWheelOption.Connection[8];
}
