using System;
using System.Collections.Generic;

// Token: 0x0200089A RID: 2202
public class SuitWearer : GameStateMachine<SuitWearer, SuitWearer.Instance>
{
	// Token: 0x06003FF0 RID: 16368 RVA: 0x00165DC0 File Offset: 0x00163FC0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventHandler(GameHashes.PathAdvanced, delegate(SuitWearer.Instance smi, object data)
		{
			smi.OnPathAdvanced(data);
		}).EventHandler(GameHashes.Died, delegate(SuitWearer.Instance smi, object data)
		{
			smi.UnreserveSuits();
		}).DoNothing();
		this.suit.DoNothing();
		this.nosuit.DoNothing();
	}

	// Token: 0x0400299D RID: 10653
	public GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.State suit;

	// Token: 0x0400299E RID: 10654
	public GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.State nosuit;

	// Token: 0x020016C2 RID: 5826
	public new class Instance : GameStateMachine<SuitWearer, SuitWearer.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C35 RID: 35893 RVA: 0x00317FD0 File Offset: 0x003161D0
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.navigator = master.GetComponent<Navigator>();
			this.navigator.SetFlags(PathFinder.PotentialPath.Flags.PerformSuitChecks);
			this.prefabInstanceID = this.navigator.GetComponent<KPrefabID>().InstanceID;
		}

		// Token: 0x06008C36 RID: 35894 RVA: 0x00318028 File Offset: 0x00316228
		public void OnPathAdvanced(object data)
		{
			if (this.navigator.CurrentNavType == NavType.Hover && (this.navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) <= PathFinder.PotentialPath.Flags.None)
			{
				this.navigator.SetCurrentNavType(NavType.Floor);
			}
			this.UnreserveSuits();
			this.ReserveSuits();
		}

		// Token: 0x06008C37 RID: 35895 RVA: 0x00318064 File Offset: 0x00316264
		public void ReserveSuits()
		{
			PathFinder.Path path = this.navigator.path;
			if (path.nodes == null)
			{
				return;
			}
			bool flag = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasAtmoSuit) > PathFinder.PotentialPath.Flags.None;
			bool flag2 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
			bool flag3 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasOxygenMask) > PathFinder.PotentialPath.Flags.None;
			bool flag4 = (this.navigator.flags & PathFinder.PotentialPath.Flags.HasLeadSuit) > PathFinder.PotentialPath.Flags.None;
			for (int i = 0; i < path.nodes.Count - 1; i++)
			{
				int cell = path.nodes[i].cell;
				Grid.SuitMarker.Flags flags = (Grid.SuitMarker.Flags)0;
				PathFinder.PotentialPath.Flags flags2 = PathFinder.PotentialPath.Flags.None;
				if (Grid.TryGetSuitMarkerFlags(cell, out flags, out flags2))
				{
					bool flag5 = (flags2 & PathFinder.PotentialPath.Flags.HasAtmoSuit) > PathFinder.PotentialPath.Flags.None;
					bool flag6 = (flags2 & PathFinder.PotentialPath.Flags.HasJetPack) > PathFinder.PotentialPath.Flags.None;
					bool flag7 = (flags2 & PathFinder.PotentialPath.Flags.HasOxygenMask) > PathFinder.PotentialPath.Flags.None;
					bool flag8 = (flags2 & PathFinder.PotentialPath.Flags.HasLeadSuit) > PathFinder.PotentialPath.Flags.None;
					bool flag9 = flag2 || flag || flag3 || flag4;
					bool flag10 = flag5 == flag && flag6 == flag2 && flag7 == flag3 && flag8 == flag4;
					bool flag11 = SuitMarker.DoesTraversalDirectionRequireSuit(cell, path.nodes[i + 1].cell, flags);
					if (flag11 && !flag9)
					{
						if (Grid.ReserveSuit(cell, this.prefabInstanceID, true))
						{
							this.suitReservations.Add(cell);
							if (flag5)
							{
								flag = true;
							}
							if (flag6)
							{
								flag2 = true;
							}
							if (flag7)
							{
								flag3 = true;
							}
							if (flag8)
							{
								flag4 = true;
							}
						}
					}
					else if (!flag11 && flag10 && Grid.HasEmptyLocker(cell, this.prefabInstanceID) && Grid.ReserveEmptyLocker(cell, this.prefabInstanceID, true))
					{
						this.emptyLockerReservations.Add(cell);
						if (flag5)
						{
							flag = false;
						}
						if (flag6)
						{
							flag2 = false;
						}
						if (flag7)
						{
							flag3 = false;
						}
						if (flag8)
						{
							flag4 = false;
						}
					}
				}
			}
		}

		// Token: 0x06008C38 RID: 35896 RVA: 0x00318210 File Offset: 0x00316410
		public void UnreserveSuits()
		{
			foreach (int num in this.suitReservations)
			{
				if (Grid.HasSuitMarker[num])
				{
					Grid.ReserveSuit(num, this.prefabInstanceID, false);
				}
			}
			this.suitReservations.Clear();
			foreach (int num2 in this.emptyLockerReservations)
			{
				if (Grid.HasSuitMarker[num2])
				{
					Grid.ReserveEmptyLocker(num2, this.prefabInstanceID, false);
				}
			}
			this.emptyLockerReservations.Clear();
		}

		// Token: 0x06008C39 RID: 35897 RVA: 0x003182E4 File Offset: 0x003164E4
		protected override void OnCleanUp()
		{
			this.UnreserveSuits();
		}

		// Token: 0x04006CBF RID: 27839
		private List<int> suitReservations = new List<int>();

		// Token: 0x04006CC0 RID: 27840
		private List<int> emptyLockerReservations = new List<int>();

		// Token: 0x04006CC1 RID: 27841
		private Navigator navigator;

		// Token: 0x04006CC2 RID: 27842
		private int prefabInstanceID;
	}
}
