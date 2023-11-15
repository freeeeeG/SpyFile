using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Database;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x02000697 RID: 1687
public class StickerBomb : StateMachineComponent<StickerBomb.StatesInstance>
{
	// Token: 0x06002D48 RID: 11592 RVA: 0x000F0808 File Offset: 0x000EEA08
	protected override void OnSpawn()
	{
		if (this.stickerName.IsNullOrWhiteSpace())
		{
			global::Debug.LogError("Missing sticker db entry for " + this.stickerType);
		}
		else
		{
			DbStickerBomb dbStickerBomb = Db.GetStickerBombs().Get(this.stickerName);
			base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
			{
				dbStickerBomb.animFile
			});
		}
		this.cellOffsets = StickerBomb.BuildCellOffsets(base.transform.GetPosition());
		base.smi.destroyTime = GameClock.Instance.GetTime() + TRAITS.JOY_REACTIONS.STICKER_BOMBER.STICKER_DURATION;
		base.smi.StartSM();
		Extents extents = base.GetComponent<OccupyArea>().GetExtents();
		Extents extents2 = new Extents(extents.x - 1, extents.y - 1, extents.width + 2, extents.height + 2);
		this.partitionerEntry = GameScenePartitioner.Instance.Add("StickerBomb.OnSpawn", base.gameObject, extents2, GameScenePartitioner.Instance.objectLayers[2], new Action<object>(this.OnFoundationCellChanged));
		base.OnSpawn();
	}

	// Token: 0x06002D49 RID: 11593 RVA: 0x000F0910 File Offset: 0x000EEB10
	[OnDeserialized]
	public void OnDeserialized()
	{
		if (this.stickerName.IsNullOrWhiteSpace() && !this.stickerType.IsNullOrWhiteSpace())
		{
			string[] array = this.stickerType.Split(new char[]
			{
				'_'
			});
			if (array.Length == 2)
			{
				this.stickerName = array[1];
			}
		}
	}

	// Token: 0x06002D4A RID: 11594 RVA: 0x000F095D File Offset: 0x000EEB5D
	protected override void OnCleanUp()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		base.OnCleanUp();
	}

	// Token: 0x06002D4B RID: 11595 RVA: 0x000F0975 File Offset: 0x000EEB75
	private void OnFoundationCellChanged(object data)
	{
		if (!StickerBomb.CanPlaceSticker(this.cellOffsets))
		{
			Util.KDestroyGameObject(base.gameObject);
		}
	}

	// Token: 0x06002D4C RID: 11596 RVA: 0x000F0990 File Offset: 0x000EEB90
	public static List<int> BuildCellOffsets(Vector3 position)
	{
		List<int> list = new List<int>();
		bool flag = position.x % 1f < 0.5f;
		bool flag2 = position.y % 1f > 0.5f;
		int num = Grid.PosToCell(position);
		list.Add(num);
		if (flag)
		{
			list.Add(Grid.CellLeft(num));
			if (flag2)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellUpLeft(num));
			}
			else
			{
				list.Add(Grid.CellBelow(num));
				list.Add(Grid.CellDownLeft(num));
			}
		}
		else
		{
			list.Add(Grid.CellRight(num));
			if (flag2)
			{
				list.Add(Grid.CellAbove(num));
				list.Add(Grid.CellUpRight(num));
			}
			else
			{
				list.Add(Grid.CellBelow(num));
				list.Add(Grid.CellDownRight(num));
			}
		}
		return list;
	}

	// Token: 0x06002D4D RID: 11597 RVA: 0x000F0A60 File Offset: 0x000EEC60
	public static bool CanPlaceSticker(List<int> offsets)
	{
		using (List<int>.Enumerator enumerator = offsets.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (Grid.IsCellOpenToSpace(enumerator.Current))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002D4E RID: 11598 RVA: 0x000F0AB4 File Offset: 0x000EECB4
	public void SetStickerType(string newStickerType)
	{
		if (newStickerType == null)
		{
			newStickerType = "sticker";
		}
		DbStickerBomb randomSticker = Db.GetStickerBombs().GetRandomSticker();
		this.stickerName = randomSticker.Id;
		this.stickerType = string.Format("{0}_{1}", newStickerType, randomSticker.Id);
		base.GetComponent<KBatchedAnimController>().SwapAnims(new KAnimFile[]
		{
			randomSticker.animFile
		});
	}

	// Token: 0x04001ACE RID: 6862
	[Serialize]
	public string stickerType;

	// Token: 0x04001ACF RID: 6863
	[Serialize]
	public string stickerName;

	// Token: 0x04001AD0 RID: 6864
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001AD1 RID: 6865
	private List<int> cellOffsets;

	// Token: 0x020013BB RID: 5051
	public class StatesInstance : GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.GameInstance
	{
		// Token: 0x06008208 RID: 33288 RVA: 0x002F7BD7 File Offset: 0x002F5DD7
		public StatesInstance(StickerBomb master) : base(master)
		{
		}

		// Token: 0x06008209 RID: 33289 RVA: 0x002F7BE0 File Offset: 0x002F5DE0
		public string GetStickerAnim(string type)
		{
			return string.Format("{0}_{1}", type, base.master.stickerType);
		}

		// Token: 0x0400633D RID: 25405
		[Serialize]
		public float destroyTime;
	}

	// Token: 0x020013BC RID: 5052
	public class States : GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb>
	{
		// Token: 0x0600820A RID: 33290 RVA: 0x002F7BF8 File Offset: 0x002F5DF8
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
			this.root.Transition(this.destroy, (StickerBomb.StatesInstance smi) => GameClock.Instance.GetTime() >= smi.destroyTime, UpdateRate.SIM_200ms).DefaultState(this.idle);
			this.idle.PlayAnim((StickerBomb.StatesInstance smi) => smi.GetStickerAnim("idle"), KAnim.PlayMode.Once).ScheduleGoTo((StickerBomb.StatesInstance smi) => (float)UnityEngine.Random.Range(20, 30), this.sparkle);
			this.sparkle.PlayAnim((StickerBomb.StatesInstance smi) => smi.GetStickerAnim("sparkle"), KAnim.PlayMode.Once).OnAnimQueueComplete(this.idle);
			this.destroy.Enter(delegate(StickerBomb.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master);
			});
		}

		// Token: 0x0400633E RID: 25406
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State destroy;

		// Token: 0x0400633F RID: 25407
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State sparkle;

		// Token: 0x04006340 RID: 25408
		public GameStateMachine<StickerBomb.States, StickerBomb.StatesInstance, StickerBomb, object>.State idle;
	}
}
