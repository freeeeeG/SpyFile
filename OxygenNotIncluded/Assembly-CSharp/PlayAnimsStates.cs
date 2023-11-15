using System;

// Token: 0x020000B2 RID: 178
public class PlayAnimsStates : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>
{
	// Token: 0x06000328 RID: 808 RVA: 0x00019180 File Offset: 0x00017380
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.animating;
		this.root.ToggleStatusItem("Unused", "Unused", "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemName, (string str, PlayAnimsStates.Instance smi) => smi.def.statusItemTooltip, Db.Get().StatusItemCategories.Main);
		this.animating.Enter("PlayAnims", delegate(PlayAnimsStates.Instance smi)
		{
			smi.PlayAnims();
		}).OnAnimQueueComplete(this.done).EventHandler(GameHashes.TagsChanged, delegate(PlayAnimsStates.Instance smi, object obj)
		{
			smi.HandleTagsChanged(obj);
		});
		this.done.PlayAnim("idle_loop", KAnim.PlayMode.Loop).BehaviourComplete((PlayAnimsStates.Instance smi) => smi.def.tag, false);
	}

	// Token: 0x04000224 RID: 548
	public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State animating;

	// Token: 0x04000225 RID: 549
	public GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.State done;

	// Token: 0x02000E95 RID: 3733
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x06006FCC RID: 28620 RVA: 0x002B9D4C File Offset: 0x002B7F4C
		public Def(Tag tag, bool loop, string anim, string status_item_name, string status_item_tooltip) : this(tag, loop, new string[]
		{
			anim
		}, status_item_name, status_item_tooltip)
		{
		}

		// Token: 0x06006FCD RID: 28621 RVA: 0x002B9D64 File Offset: 0x002B7F64
		public Def(Tag tag, bool loop, string[] anims, string status_item_name, string status_item_tooltip)
		{
			this.tag = tag;
			this.loop = loop;
			this.anims = anims;
			this.statusItemName = status_item_name;
			this.statusItemTooltip = status_item_tooltip;
		}

		// Token: 0x06006FCE RID: 28622 RVA: 0x002B9D91 File Offset: 0x002B7F91
		public override string ToString()
		{
			return this.tag.ToString() + "(PlayAnimsStates)";
		}

		// Token: 0x040053D8 RID: 21464
		public Tag tag;

		// Token: 0x040053D9 RID: 21465
		public string[] anims;

		// Token: 0x040053DA RID: 21466
		public bool loop;

		// Token: 0x040053DB RID: 21467
		public string statusItemName;

		// Token: 0x040053DC RID: 21468
		public string statusItemTooltip;
	}

	// Token: 0x02000E96 RID: 3734
	public new class Instance : GameStateMachine<PlayAnimsStates, PlayAnimsStates.Instance, IStateMachineTarget, PlayAnimsStates.Def>.GameInstance
	{
		// Token: 0x06006FCF RID: 28623 RVA: 0x002B9DAE File Offset: 0x002B7FAE
		public Instance(Chore<PlayAnimsStates.Instance> chore, PlayAnimsStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, def.tag);
		}

		// Token: 0x06006FD0 RID: 28624 RVA: 0x002B9DD4 File Offset: 0x002B7FD4
		public void PlayAnims()
		{
			if (base.def.anims == null || base.def.anims.Length == 0)
			{
				return;
			}
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			for (int i = 0; i < base.def.anims.Length; i++)
			{
				KAnim.PlayMode mode = KAnim.PlayMode.Once;
				if (base.def.loop && i == base.def.anims.Length - 1)
				{
					mode = KAnim.PlayMode.Loop;
				}
				if (i == 0)
				{
					component.Play(base.def.anims[i], mode, 1f, 0f);
				}
				else
				{
					component.Queue(base.def.anims[i], mode, 1f, 0f);
				}
			}
		}

		// Token: 0x06006FD1 RID: 28625 RVA: 0x002B9E8D File Offset: 0x002B808D
		public void HandleTagsChanged(object obj)
		{
			if (!base.smi.HasTag(base.smi.def.tag))
			{
				base.smi.GoTo(null);
			}
		}
	}
}
