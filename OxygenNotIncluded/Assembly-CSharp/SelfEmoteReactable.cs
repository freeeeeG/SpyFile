using System;
using UnityEngine;

// Token: 0x02000411 RID: 1041
public class SelfEmoteReactable : EmoteReactable
{
	// Token: 0x06001602 RID: 5634 RVA: 0x00073E30 File Offset: 0x00072030
	public SelfEmoteReactable(GameObject gameObject, HashedString id, ChoreType chore_type, float globalCooldown = 0f, float localCooldown = 20f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f) : base(gameObject, id, chore_type, 3, 3, globalCooldown, localCooldown, lifeSpan, max_initial_delay)
	{
	}

	// Token: 0x06001603 RID: 5635 RVA: 0x00073E50 File Offset: 0x00072050
	public override bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		if (reactor != this.gameObject)
		{
			return false;
		}
		Navigator component = reactor.GetComponent<Navigator>();
		return !(component == null) && component.IsMoving();
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x00073E88 File Offset: 0x00072088
	public void PairEmote(EmoteChore emoteChore)
	{
		this.chore = emoteChore;
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x00073E94 File Offset: 0x00072094
	protected override void InternalEnd()
	{
		if (this.chore != null && this.chore.driver != null)
		{
			this.chore.PairReactable(null);
			this.chore.Cancel("Reactable ended");
			this.chore = null;
		}
		base.InternalEnd();
	}

	// Token: 0x04000C50 RID: 3152
	private EmoteChore chore;
}
