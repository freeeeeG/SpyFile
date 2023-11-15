using System;
using UnityEngine;

// Token: 0x02000780 RID: 1920
public abstract class IconTutorialBase : MonoBehaviour
{
	// Token: 0x04001CA0 RID: 7328
	[SerializeField]
	public GameObject m_iconPrefab;

	// Token: 0x04001CA1 RID: 7329
	[SerializeField]
	[AssignResource("AttentionDrawerUI", Editorbility.NonEditable)]
	public GameObject m_attentionDrawer;
}
