using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020003EA RID: 1002
public class StickerBomber : GameStateMachine<StickerBomber, StickerBomber.Instance>
{
	// Token: 0x06001523 RID: 5411 RVA: 0x0006F948 File Offset: 0x0006DB48
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.neutral;
		this.root.TagTransition(GameTags.Dead, null, false);
		this.neutral.TagTransition(GameTags.Overjoyed, this.overjoyed, false).Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		});
		this.overjoyed.TagTransition(GameTags.Overjoyed, this.neutral, true).DefaultState(this.overjoyed.idle).ToggleStatusItem(Db.Get().DuplicantStatusItems.JoyResponse_StickerBombing, null);
		this.overjoyed.idle.Transition(this.overjoyed.place_stickers, (StickerBomber.Instance smi) => GameClock.Instance.GetTime() >= smi.nextStickerBomb, UpdateRate.SIM_200ms);
		this.overjoyed.place_stickers.Exit(delegate(StickerBomber.Instance smi)
		{
			smi.nextStickerBomb = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.TIME_PER_STICKER_BOMB;
		}).ToggleReactable((StickerBomber.Instance smi) => smi.CreateReactable()).OnSignal(this.doneStickerBomb, this.overjoyed.idle);
	}

	// Token: 0x04000B73 RID: 2931
	public StateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.Signal doneStickerBomb;

	// Token: 0x04000B74 RID: 2932
	public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State neutral;

	// Token: 0x04000B75 RID: 2933
	public StickerBomber.OverjoyedStates overjoyed;

	// Token: 0x02001073 RID: 4211
	public class OverjoyedStates : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x04005918 RID: 22808
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State idle;

		// Token: 0x04005919 RID: 22809
		public GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.State place_stickers;
	}

	// Token: 0x02001074 RID: 4212
	public new class Instance : GameStateMachine<StickerBomber, StickerBomber.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x060075BB RID: 30139 RVA: 0x002CD7E7 File Offset: 0x002CB9E7
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x060075BC RID: 30140 RVA: 0x002CD7F0 File Offset: 0x002CB9F0
		public Reactable CreateReactable()
		{
			return new StickerBomber.Instance.StickerBombReactable(base.master.gameObject, base.smi);
		}

		// Token: 0x0400591A RID: 22810
		[Serialize]
		public float nextStickerBomb;

		// Token: 0x02002003 RID: 8195
		private class StickerBombReactable : Reactable
		{
			// Token: 0x0600A466 RID: 42086 RVA: 0x0036987C File Offset: 0x00367A7C
			public StickerBombReactable(GameObject gameObject, StickerBomber.Instance stickerBomber) : base(gameObject, "StickerBombReactable", Db.Get().ChoreTypes.Build, 2, 1, false, 0f, 0f, float.PositiveInfinity, 0f, ObjectLayer.NumLayers)
			{
				this.preventChoreInterruption = true;
				this.stickerBomber = stickerBomber;
			}

			// Token: 0x0600A467 RID: 42087 RVA: 0x0036995C File Offset: 0x00367B5C
			public override bool InternalCanBegin(GameObject new_reactor, Navigator.ActiveTransition transition)
			{
				if (this.reactor != null)
				{
					return false;
				}
				if (new_reactor == null)
				{
					return false;
				}
				if (this.gameObject != new_reactor)
				{
					return false;
				}
				Navigator component = new_reactor.GetComponent<Navigator>();
				return !(component == null) && component.CurrentNavType != NavType.Tube && component.CurrentNavType != NavType.Ladder && component.CurrentNavType != NavType.Pole;
			}

			// Token: 0x0600A468 RID: 42088 RVA: 0x003699C4 File Offset: 0x00367BC4
			protected override void InternalBegin()
			{
				this.stickersToPlace = UnityEngine.Random.Range(4, 6);
				this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				this.placementCell = this.FindPlacementCell();
				if (this.placementCell == 0)
				{
					base.End();
					return;
				}
				this.kbac = this.reactor.GetComponent<KBatchedAnimController>();
				this.kbac.AddAnimOverrides(this.animset, 0f);
				this.kbac.Play(this.pre_anim, KAnim.PlayMode.Once, 1f, 0f);
				this.kbac.Queue(this.loop_anim, KAnim.PlayMode.Loop, 1f, 0f);
			}

			// Token: 0x0600A469 RID: 42089 RVA: 0x00369A64 File Offset: 0x00367C64
			public override void Update(float dt)
			{
				this.STICKER_PLACE_TIMER -= dt;
				if (this.STICKER_PLACE_TIMER <= 0f)
				{
					this.PlaceSticker();
					this.STICKER_PLACE_TIMER = this.TIME_PER_STICKER_PLACED;
				}
				if (this.stickersPlaced >= this.stickersToPlace)
				{
					this.kbac.Play(this.pst_anim, KAnim.PlayMode.Once, 1f, 0f);
					base.End();
				}
			}

			// Token: 0x0600A46A RID: 42090 RVA: 0x00369AD0 File Offset: 0x00367CD0
			protected override void InternalEnd()
			{
				if (this.kbac != null)
				{
					this.kbac.RemoveAnimOverrides(this.animset);
					this.kbac = null;
				}
				this.stickerBomber.sm.doneStickerBomb.Trigger(this.stickerBomber);
				this.stickersPlaced = 0;
			}

			// Token: 0x0600A46B RID: 42091 RVA: 0x00369B28 File Offset: 0x00367D28
			private int FindPlacementCell()
			{
				int cell = Grid.PosToCell(this.reactor.transform.GetPosition() + Vector3.up);
				ListPool<int, PathFinder>.PooledList pooledList = ListPool<int, PathFinder>.Allocate();
				ListPool<int, PathFinder>.PooledList pooledList2 = ListPool<int, PathFinder>.Allocate();
				QueuePool<GameUtil.FloodFillInfo, Comet>.PooledQueue pooledQueue = QueuePool<GameUtil.FloodFillInfo, Comet>.Allocate();
				pooledQueue.Enqueue(new GameUtil.FloodFillInfo
				{
					cell = cell,
					depth = 0
				});
				GameUtil.FloodFillConditional(pooledQueue, this.canPlaceStickerCb, pooledList, pooledList2, 2);
				if (pooledList2.Count > 0)
				{
					int random = pooledList2.GetRandom<int>();
					pooledList.Recycle();
					pooledList2.Recycle();
					pooledQueue.Recycle();
					return random;
				}
				pooledList.Recycle();
				pooledList2.Recycle();
				pooledQueue.Recycle();
				return 0;
			}

			// Token: 0x0600A46C RID: 42092 RVA: 0x00369BCC File Offset: 0x00367DCC
			private void PlaceSticker()
			{
				this.stickersPlaced++;
				Vector3 a = Grid.CellToPos(this.placementCell);
				int i = 10;
				while (i > 0)
				{
					i--;
					Vector3 position = a + new Vector3(UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), UnityEngine.Random.Range(-this.tile_random_range, this.tile_random_range), -2.5f);
					if (StickerBomb.CanPlaceSticker(StickerBomb.BuildCellOffsets(position)))
					{
						GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("StickerBomb".ToTag()), position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-this.tile_random_rotation, this.tile_random_rotation)), null, null, true, 0);
						StickerBomb component = gameObject.GetComponent<StickerBomb>();
						string stickerType = this.reactor.GetComponent<MinionIdentity>().stickerType;
						component.SetStickerType(stickerType);
						gameObject.SetActive(true);
						i = 0;
					}
				}
			}

			// Token: 0x0600A46D RID: 42093 RVA: 0x00369CA7 File Offset: 0x00367EA7
			protected override void InternalCleanup()
			{
			}

			// Token: 0x0400901A RID: 36890
			private int stickersToPlace;

			// Token: 0x0400901B RID: 36891
			private int stickersPlaced;

			// Token: 0x0400901C RID: 36892
			private int placementCell;

			// Token: 0x0400901D RID: 36893
			private float tile_random_range = 1f;

			// Token: 0x0400901E RID: 36894
			private float tile_random_rotation = 90f;

			// Token: 0x0400901F RID: 36895
			private float TIME_PER_STICKER_PLACED = 0.66f;

			// Token: 0x04009020 RID: 36896
			private float STICKER_PLACE_TIMER;

			// Token: 0x04009021 RID: 36897
			private KBatchedAnimController kbac;

			// Token: 0x04009022 RID: 36898
			private KAnimFile animset = Assets.GetAnim("anim_stickers_kanim");

			// Token: 0x04009023 RID: 36899
			private HashedString pre_anim = "working_pre";

			// Token: 0x04009024 RID: 36900
			private HashedString loop_anim = "working_loop";

			// Token: 0x04009025 RID: 36901
			private HashedString pst_anim = "working_pst";

			// Token: 0x04009026 RID: 36902
			private StickerBomber.Instance stickerBomber;

			// Token: 0x04009027 RID: 36903
			private Func<int, bool> canPlaceStickerCb = (int cell) => !Grid.Solid[cell] && (!Grid.IsValidCell(Grid.CellLeft(cell)) || !Grid.Solid[Grid.CellLeft(cell)]) && (!Grid.IsValidCell(Grid.CellRight(cell)) || !Grid.Solid[Grid.CellRight(cell)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, 1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, 1)]) && (!Grid.IsValidCell(Grid.OffsetCell(cell, 0, -1)) || !Grid.Solid[Grid.OffsetCell(cell, 0, -1)]) && !Grid.IsCellOpenToSpace(cell);
		}
	}
}
