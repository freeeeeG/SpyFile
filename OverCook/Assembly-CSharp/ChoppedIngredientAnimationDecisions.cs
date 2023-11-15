using System;
using UnityEngine;

// Token: 0x02000395 RID: 917
public class ChoppedIngredientAnimationDecisions : MonoBehaviour, IClientSurfacePlacementNotified
{
	// Token: 0x06001151 RID: 4433 RVA: 0x000638A4 File Offset: 0x00061CA4
	private void Awake()
	{
		this.m_animator = base.gameObject.RequestComponentRecursive<Animator>();
		this.m_gatherVariableHash = Animator.StringToHash(this.m_gatherVariable);
		if (this.m_animator == null)
		{
			UnityEngine.Object.Destroy(this);
		}
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x000638DF File Offset: 0x00061CDF
	public void OnSurfacePlacement(ClientAttachStation _station)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_gatherVariableHash, false);
		}
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x00063904 File Offset: 0x00061D04
	public void OnSurfaceDeplacement(ClientAttachStation _station)
	{
		if (this.m_animator != null)
		{
			this.m_animator.SetBool(this.m_gatherVariableHash, true);
		}
	}

	// Token: 0x04000D7B RID: 3451
	[SerializeField]
	private string m_gatherVariable = "Gather";

	// Token: 0x04000D7C RID: 3452
	private Animator m_animator;

	// Token: 0x04000D7D RID: 3453
	private int m_gatherVariableHash;
}
