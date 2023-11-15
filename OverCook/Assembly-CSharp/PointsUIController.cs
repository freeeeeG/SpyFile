using System;
using UnityEngine;

// Token: 0x02000B40 RID: 2880
public class PointsUIController : DisplayIntUIController
{
	// Token: 0x06003A7A RID: 14970 RVA: 0x001169A8 File Offset: 0x00114DA8
	protected override void Awake()
	{
		base.Awake();
		this.m_animator = base.gameObject.RequireComponentRecursive<Animator>();
	}

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06003A7B RID: 14971 RVA: 0x001169C1 File Offset: 0x00114DC1
	// (set) Token: 0x06003A7C RID: 14972 RVA: 0x001169C9 File Offset: 0x00114DC9
	public override int Value
	{
		get
		{
			return base.Value;
		}
		set
		{
			if (this.m_animator != null)
			{
				this.m_animator.SetTrigger(PointsUIController.m_iScore);
			}
			base.Value = value;
		}
	}

	// Token: 0x06003A7D RID: 14973 RVA: 0x001169F3 File Offset: 0x00114DF3
	public TeamID GetTeam()
	{
		return this.m_scoringTeam;
	}

	// Token: 0x04002F6D RID: 12141
	[SerializeField]
	private TeamID m_scoringTeam;

	// Token: 0x04002F6E RID: 12142
	private Animator m_animator;

	// Token: 0x04002F6F RID: 12143
	private static readonly int m_iScore = Animator.StringToHash("Score");
}
