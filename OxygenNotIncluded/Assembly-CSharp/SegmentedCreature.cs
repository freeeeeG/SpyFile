using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000DE RID: 222
public class SegmentedCreature : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>
{
	// Token: 0x060003FD RID: 1021 RVA: 0x0001EC78 File Offset: 0x0001CE78
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.freeMovement.idle;
		this.root.Enter(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
		this.retracted.DefaultState(this.retracted.pre).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop, false, 0);
		}).Exit(new StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State.Callback(this.SetRetractedPath));
		this.retracted.pre.Update(new Action<SegmentedCreature.Instance, float>(this.UpdateRetractedPre), UpdateRate.SIM_EVERY_TICK, false);
		this.retracted.loop.ParamTransition<bool>(this.isRetracted, this.freeMovement, (SegmentedCreature.Instance smi, bool p) => !this.isRetracted.Get(smi)).Update(new Action<SegmentedCreature.Instance, float>(this.UpdateRetractedLoop), UpdateRate.SIM_EVERY_TICK, false);
		this.freeMovement.DefaultState(this.freeMovement.idle).ParamTransition<bool>(this.isRetracted, this.retracted, (SegmentedCreature.Instance smi, bool p) => this.isRetracted.Get(smi)).Update(new Action<SegmentedCreature.Instance, float>(this.UpdateFreeMovement), UpdateRate.SIM_EVERY_TICK, false);
		this.freeMovement.idle.Transition(this.freeMovement.moving, (SegmentedCreature.Instance smi) => smi.GetComponent<Navigator>().IsMoving(), UpdateRate.SIM_200ms).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "idle_loop", KAnim.PlayMode.Loop, true, 0);
		});
		this.freeMovement.moving.Transition(this.freeMovement.idle, (SegmentedCreature.Instance smi) => !smi.GetComponent<Navigator>().IsMoving(), UpdateRate.SIM_200ms).Enter(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "walking_pre", KAnim.PlayMode.Once, false, 0);
			this.PlayBodySegmentsAnim(smi, "walking_loop", KAnim.PlayMode.Loop, false, smi.def.animFrameOffset);
		}).Exit(delegate(SegmentedCreature.Instance smi)
		{
			this.PlayBodySegmentsAnim(smi, "walking_pst", KAnim.PlayMode.Once, true, 0);
		});
	}

	// Token: 0x060003FE RID: 1022 RVA: 0x0001EE30 File Offset: 0x0001D030
	private void PlayBodySegmentsAnim(SegmentedCreature.Instance smi, string animName, KAnim.PlayMode playMode, bool queue = false, int frameOffset = 0)
	{
		LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode();
		int num = 0;
		while (linkedListNode != null)
		{
			if (queue)
			{
				linkedListNode.Value.animController.Queue(animName, playMode, 1f, 0f);
			}
			else
			{
				linkedListNode.Value.animController.Play(animName, playMode, 1f, 0f);
			}
			if (frameOffset > 0)
			{
				float num2 = (float)linkedListNode.Value.animController.GetCurrentNumFrames();
				float elapsedTime = (float)num * ((float)frameOffset / num2);
				linkedListNode.Value.animController.SetElapsedTime(elapsedTime);
			}
			num++;
			linkedListNode = linkedListNode.Next;
		}
	}

	// Token: 0x060003FF RID: 1023 RVA: 0x0001EED8 File Offset: 0x0001D0D8
	private void UpdateRetractedPre(SegmentedCreature.Instance smi, float dt)
	{
		this.UpdateHeadPosition(smi);
		bool flag = true;
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.distanceToPreviousSegment = Mathf.Max(smi.def.minSegmentSpacing, linkedListNode.Value.distanceToPreviousSegment - dt * smi.def.retractionSegmentSpeed);
			if (linkedListNode.Value.distanceToPreviousSegment > smi.def.minSegmentSpacing)
			{
				flag = false;
			}
		}
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode2 = smi.path.First;
		Vector3 forward = value.Forward;
		Quaternion rotation = value.Rotation;
		int num = 0;
		while (linkedListNode2 != null)
		{
			Vector3 b = value.Position - smi.def.pathSpacing * (float)num * forward;
			linkedListNode2.Value.position = Vector3.Lerp(linkedListNode2.Value.position, b, dt * smi.def.retractionPathSpeed);
			linkedListNode2.Value.rotation = Quaternion.Slerp(linkedListNode2.Value.rotation, rotation, dt * smi.def.retractionPathSpeed);
			num++;
			linkedListNode2 = linkedListNode2.Next;
		}
		this.UpdateBodyPosition(smi);
		if (flag)
		{
			smi.GoTo(this.retracted.loop);
		}
	}

	// Token: 0x06000400 RID: 1024 RVA: 0x0001F025 File Offset: 0x0001D225
	private void UpdateRetractedLoop(SegmentedCreature.Instance smi, float dt)
	{
		this.UpdateHeadPosition(smi);
		this.SetRetractedPath(smi);
		this.UpdateBodyPosition(smi);
	}

	// Token: 0x06000401 RID: 1025 RVA: 0x0001F040 File Offset: 0x0001D240
	private void SetRetractedPath(SegmentedCreature.Instance smi)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode = smi.path.First;
		Vector3 position = value.Position;
		Quaternion rotation = value.Rotation;
		Vector3 forward = value.Forward;
		int num = 0;
		while (linkedListNode != null)
		{
			linkedListNode.Value.position = position - smi.def.pathSpacing * (float)num * forward;
			linkedListNode.Value.rotation = rotation;
			num++;
			linkedListNode = linkedListNode.Next;
		}
	}

	// Token: 0x06000402 RID: 1026 RVA: 0x0001F0C0 File Offset: 0x0001D2C0
	private void UpdateFreeMovement(SegmentedCreature.Instance smi, float dt)
	{
		float spacing = this.UpdateHeadPosition(smi);
		this.AdjustBodySegmentsSpacing(smi, spacing);
		this.UpdateBodyPosition(smi);
	}

	// Token: 0x06000403 RID: 1027 RVA: 0x0001F0E4 File Offset: 0x0001D2E4
	private float UpdateHeadPosition(SegmentedCreature.Instance smi)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		if (value.Position == smi.previousHeadPosition)
		{
			return 0f;
		}
		SegmentedCreature.PathNode value2 = smi.path.First.Value;
		SegmentedCreature.PathNode value3 = smi.path.First.Next.Value;
		float magnitude = (value2.position - value3.position).magnitude;
		float magnitude2 = (value.Position - value3.position).magnitude;
		float result = magnitude2 - magnitude;
		value2.position = value.Position;
		value2.rotation = value.Rotation;
		smi.previousHeadPosition = value2.position;
		Vector3 normalized = (value2.position - value3.position).normalized;
		int num = Mathf.FloorToInt(magnitude2 / smi.def.pathSpacing);
		for (int i = 0; i < num; i++)
		{
			Vector3 position = value3.position + normalized * smi.def.pathSpacing;
			LinkedListNode<SegmentedCreature.PathNode> last = smi.path.Last;
			last.Value.position = position;
			last.Value.rotation = value2.rotation;
			float num2 = magnitude2 - (float)i * smi.def.pathSpacing;
			float t = num2 - smi.def.pathSpacing / num2;
			last.Value.rotation = Quaternion.Lerp(value2.rotation, value3.rotation, t);
			smi.path.RemoveLast();
			smi.path.AddAfter(smi.path.First, last);
			value3 = last.Value;
		}
		return result;
	}

	// Token: 0x06000404 RID: 1028 RVA: 0x0001F2A8 File Offset: 0x0001D4A8
	private void AdjustBodySegmentsSpacing(SegmentedCreature.Instance smi, float spacing)
	{
		if (spacing == 0f)
		{
			return;
		}
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			linkedListNode.Value.distanceToPreviousSegment += spacing;
			if (linkedListNode.Value.distanceToPreviousSegment < smi.def.minSegmentSpacing)
			{
				spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.minSegmentSpacing;
				linkedListNode.Value.distanceToPreviousSegment = smi.def.minSegmentSpacing;
			}
			else
			{
				if (linkedListNode.Value.distanceToPreviousSegment <= smi.def.maxSegmentSpacing)
				{
					break;
				}
				spacing = linkedListNode.Value.distanceToPreviousSegment - smi.def.maxSegmentSpacing;
				linkedListNode.Value.distanceToPreviousSegment = smi.def.maxSegmentSpacing;
			}
		}
	}

	// Token: 0x06000405 RID: 1029 RVA: 0x0001F37C File Offset: 0x0001D57C
	private void UpdateBodyPosition(SegmentedCreature.Instance smi)
	{
		LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.GetFirstBodySegmentNode();
		LinkedListNode<SegmentedCreature.PathNode> linkedListNode2 = smi.path.First;
		float num = 0f;
		float num2 = smi.LengthPercentage();
		int num3 = 0;
		while (linkedListNode != null)
		{
			float num4 = linkedListNode.Value.distanceToPreviousSegment;
			float num5 = 0f;
			while (linkedListNode2.Next != null)
			{
				num5 = (linkedListNode2.Value.position - linkedListNode2.Next.Value.position).magnitude - num;
				if (num4 < num5)
				{
					break;
				}
				num4 -= num5;
				num = 0f;
				linkedListNode2 = linkedListNode2.Next;
			}
			if (linkedListNode2.Next == null)
			{
				linkedListNode.Value.SetPosition(linkedListNode2.Value.position);
				linkedListNode.Value.SetRotation(smi.path.Last.Value.rotation);
			}
			else
			{
				SegmentedCreature.PathNode value = linkedListNode2.Value;
				SegmentedCreature.PathNode value2 = linkedListNode2.Next.Value;
				linkedListNode.Value.SetPosition(linkedListNode2.Value.position + (linkedListNode2.Next.Value.position - linkedListNode2.Value.position).normalized * num4);
				linkedListNode.Value.SetRotation(Quaternion.Slerp(value.rotation, value2.rotation, num4 / num5));
				num = num4;
			}
			linkedListNode.Value.animController.FlipX = (linkedListNode.Previous.Value.Position.x < linkedListNode.Value.Position.x);
			linkedListNode.Value.animController.animScale = smi.baseAnimScale + smi.baseAnimScale * smi.def.compressedMaxScale * ((float)(smi.def.numBodySegments - num3) / (float)smi.def.numBodySegments) * (1f - num2);
			linkedListNode = linkedListNode.Next;
			num3++;
		}
	}

	// Token: 0x06000406 RID: 1030 RVA: 0x0001F578 File Offset: 0x0001D778
	private void DrawDebug(SegmentedCreature.Instance smi, float dt)
	{
		SegmentedCreature.CreatureSegment value = smi.GetHeadSegmentNode().Value;
		DrawUtil.Arrow(value.Position, value.Position + value.Up, 0.05f, Color.red, 0f);
		DrawUtil.Arrow(value.Position, value.Position + value.Forward * 0.06f, 0.05f, Color.cyan, 0f);
		int num = 0;
		foreach (SegmentedCreature.PathNode pathNode in smi.path)
		{
			Color color = Color.HSVToRGB((float)num / (float)smi.def.numPathNodes, 1f, 1f);
			DrawUtil.Gnomon(pathNode.position, 0.05f, Color.cyan, 0f);
			DrawUtil.Arrow(pathNode.position, pathNode.position + pathNode.rotation * Vector3.up * 0.5f, 0.025f, color, 0f);
			num++;
		}
		for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = smi.segments.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
		{
			DrawUtil.Circle(linkedListNode.Value.Position, 0.05f, Color.white, new Vector3?(Vector3.forward), 0f);
			DrawUtil.Gnomon(linkedListNode.Value.Position, 0.05f, Color.white, 0f);
		}
	}

	// Token: 0x040002A2 RID: 674
	public SegmentedCreature.RectractStates retracted;

	// Token: 0x040002A3 RID: 675
	public SegmentedCreature.FreeMovementStates freeMovement;

	// Token: 0x040002A4 RID: 676
	private StateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.BoolParameter isRetracted;

	// Token: 0x02000F20 RID: 3872
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040054FD RID: 21757
		public HashedString segmentTrackerSymbol;

		// Token: 0x040054FE RID: 21758
		public Vector3 headOffset = Vector3.zero;

		// Token: 0x040054FF RID: 21759
		public Vector3 bodyPivot = Vector3.zero;

		// Token: 0x04005500 RID: 21760
		public Vector3 tailPivot = Vector3.zero;

		// Token: 0x04005501 RID: 21761
		public int numBodySegments;

		// Token: 0x04005502 RID: 21762
		public float minSegmentSpacing;

		// Token: 0x04005503 RID: 21763
		public float maxSegmentSpacing;

		// Token: 0x04005504 RID: 21764
		public int numPathNodes;

		// Token: 0x04005505 RID: 21765
		public float pathSpacing;

		// Token: 0x04005506 RID: 21766
		public KAnimFile midAnim;

		// Token: 0x04005507 RID: 21767
		public KAnimFile tailAnim;

		// Token: 0x04005508 RID: 21768
		public string movingAnimName;

		// Token: 0x04005509 RID: 21769
		public string idleAnimName;

		// Token: 0x0400550A RID: 21770
		public float retractionSegmentSpeed = 1f;

		// Token: 0x0400550B RID: 21771
		public float retractionPathSpeed = 1f;

		// Token: 0x0400550C RID: 21772
		public float compressedMaxScale = 1.2f;

		// Token: 0x0400550D RID: 21773
		public int animFrameOffset;

		// Token: 0x0400550E RID: 21774
		public HashSet<HashedString> hideBoddyWhenStartingAnimNames = new HashSet<HashedString>
		{
			"rocket_biological"
		};

		// Token: 0x0400550F RID: 21775
		public HashSet<HashedString> retractWhenStartingAnimNames = new HashSet<HashedString>
		{
			"trapped",
			"trussed",
			"escape",
			"drown_pre",
			"drown_loop",
			"drown_pst",
			"rocket_biological"
		};

		// Token: 0x04005510 RID: 21776
		public HashSet<HashedString> retractWhenEndingAnimNames = new HashSet<HashedString>
		{
			"floor_floor_2_0",
			"grooming_pst",
			"fall"
		};
	}

	// Token: 0x02000F21 RID: 3873
	public class RectractStates : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
	{
		// Token: 0x04005511 RID: 21777
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State pre;

		// Token: 0x04005512 RID: 21778
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State loop;
	}

	// Token: 0x02000F22 RID: 3874
	public class FreeMovementStates : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State
	{
		// Token: 0x04005513 RID: 21779
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State idle;

		// Token: 0x04005514 RID: 21780
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State moving;

		// Token: 0x04005515 RID: 21781
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State layEgg;

		// Token: 0x04005516 RID: 21782
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State poop;

		// Token: 0x04005517 RID: 21783
		public GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.State dead;
	}

	// Token: 0x02000F23 RID: 3875
	public new class Instance : GameStateMachine<SegmentedCreature, SegmentedCreature.Instance, IStateMachineTarget, SegmentedCreature.Def>.GameInstance
	{
		// Token: 0x06007118 RID: 28952 RVA: 0x002BC338 File Offset: 0x002BA538
		public Instance(IStateMachineTarget master, SegmentedCreature.Def def) : base(master, def)
		{
			global::Debug.Assert((float)def.numBodySegments * def.maxSegmentSpacing < (float)def.numPathNodes * def.pathSpacing);
			this.CreateSegments();
		}

		// Token: 0x06007119 RID: 28953 RVA: 0x002BC38C File Offset: 0x002BA58C
		private void CreateSegments()
		{
			float num = (float)SegmentedCreature.Instance.creatureBatchSlot * 0.01f;
			SegmentedCreature.Instance.creatureBatchSlot = (SegmentedCreature.Instance.creatureBatchSlot + 1) % 10;
			SegmentedCreature.CreatureSegment value = this.segments.AddFirst(new SegmentedCreature.CreatureSegment(base.GetComponent<KBatchedAnimController>(), base.gameObject, num, base.smi.def.headOffset, Vector3.zero)).Value;
			base.gameObject.SetActive(false);
			value.animController = base.GetComponent<KBatchedAnimController>();
			value.animController.SetSymbolVisiblity(base.smi.def.segmentTrackerSymbol, false);
			value.symbol = base.smi.def.segmentTrackerSymbol;
			value.SetPosition(base.transform.position);
			base.gameObject.SetActive(true);
			this.baseAnimScale = value.animController.animScale;
			value.animController.onAnimEnter += this.AnimEntered;
			value.animController.onAnimComplete += this.AnimComplete;
			for (int i = 0; i < base.def.numBodySegments; i++)
			{
				GameObject gameObject = new GameObject(base.gameObject.GetProperName() + string.Format(" Segment {0}", i));
				gameObject.SetActive(false);
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = value.Position;
				KAnimFile kanimFile = base.def.midAnim;
				Vector3 pivot = base.def.bodyPivot;
				if (i == base.def.numBodySegments - 1)
				{
					kanimFile = base.def.tailAnim;
					pivot = base.def.tailPivot;
				}
				KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
				kbatchedAnimController.AnimFiles = new KAnimFile[]
				{
					kanimFile
				};
				kbatchedAnimController.isMovable = true;
				kbatchedAnimController.SetSymbolVisiblity(base.smi.def.segmentTrackerSymbol, false);
				kbatchedAnimController.sceneLayer = value.animController.sceneLayer;
				SegmentedCreature.CreatureSegment creatureSegment = new SegmentedCreature.CreatureSegment(value.animController, gameObject, num + (float)(i + 1) * 0.0001f, Vector3.zero, pivot);
				creatureSegment.animController = kbatchedAnimController;
				creatureSegment.symbol = base.smi.def.segmentTrackerSymbol;
				creatureSegment.distanceToPreviousSegment = base.smi.def.minSegmentSpacing;
				creatureSegment.animLink = new KAnimLink(value.animController, kbatchedAnimController);
				this.segments.AddLast(creatureSegment);
				gameObject.SetActive(true);
			}
			for (int j = 0; j < base.def.numPathNodes; j++)
			{
				this.path.AddLast(new SegmentedCreature.PathNode(value.Position));
			}
		}

		// Token: 0x0600711A RID: 28954 RVA: 0x002BC64C File Offset: 0x002BA84C
		public void AnimEntered(HashedString name)
		{
			if (base.smi.def.retractWhenStartingAnimNames.Contains(name))
			{
				base.smi.sm.isRetracted.Set(true, base.smi, false);
			}
			else
			{
				base.smi.sm.isRetracted.Set(false, base.smi, false);
			}
			if (base.smi.def.hideBoddyWhenStartingAnimNames.Contains(name))
			{
				this.SetBodySegmentsVisibility(false);
				return;
			}
			this.SetBodySegmentsVisibility(true);
		}

		// Token: 0x0600711B RID: 28955 RVA: 0x002BC6D8 File Offset: 0x002BA8D8
		public void SetBodySegmentsVisibility(bool visible)
		{
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = base.smi.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value.animController.SetVisiblity(visible);
			}
		}

		// Token: 0x0600711C RID: 28956 RVA: 0x002BC70E File Offset: 0x002BA90E
		public void AnimComplete(HashedString name)
		{
			if (base.smi.def.retractWhenEndingAnimNames.Contains(name))
			{
				base.smi.sm.isRetracted.Set(true, base.smi, false);
			}
		}

		// Token: 0x0600711D RID: 28957 RVA: 0x002BC746 File Offset: 0x002BA946
		public LinkedListNode<SegmentedCreature.CreatureSegment> GetHeadSegmentNode()
		{
			return base.smi.segments.First;
		}

		// Token: 0x0600711E RID: 28958 RVA: 0x002BC758 File Offset: 0x002BA958
		public LinkedListNode<SegmentedCreature.CreatureSegment> GetFirstBodySegmentNode()
		{
			return base.smi.segments.First.Next;
		}

		// Token: 0x0600711F RID: 28959 RVA: 0x002BC770 File Offset: 0x002BA970
		public float LengthPercentage()
		{
			float num = 0f;
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				num += linkedListNode.Value.distanceToPreviousSegment;
			}
			float num2 = this.MinLength();
			float num3 = this.MaxLength();
			return Mathf.Clamp(num - num2, 0f, num3) / (num3 - num2);
		}

		// Token: 0x06007120 RID: 28960 RVA: 0x002BC7C4 File Offset: 0x002BA9C4
		public float MinLength()
		{
			return base.smi.def.minSegmentSpacing * (float)base.smi.def.numBodySegments;
		}

		// Token: 0x06007121 RID: 28961 RVA: 0x002BC7E8 File Offset: 0x002BA9E8
		public float MaxLength()
		{
			return base.smi.def.maxSegmentSpacing * (float)base.smi.def.numBodySegments;
		}

		// Token: 0x06007122 RID: 28962 RVA: 0x002BC80C File Offset: 0x002BAA0C
		protected override void OnCleanUp()
		{
			this.GetHeadSegmentNode().Value.animController.onAnimEnter -= this.AnimEntered;
			this.GetHeadSegmentNode().Value.animController.onAnimComplete -= this.AnimComplete;
			for (LinkedListNode<SegmentedCreature.CreatureSegment> linkedListNode = this.GetFirstBodySegmentNode(); linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value.CleanUp();
			}
		}

		// Token: 0x04005518 RID: 21784
		private const int NUM_CREATURE_SLOTS = 10;

		// Token: 0x04005519 RID: 21785
		private static int creatureBatchSlot;

		// Token: 0x0400551A RID: 21786
		public float baseAnimScale;

		// Token: 0x0400551B RID: 21787
		public Vector3 previousHeadPosition;

		// Token: 0x0400551C RID: 21788
		public float previousDist;

		// Token: 0x0400551D RID: 21789
		public LinkedList<SegmentedCreature.PathNode> path = new LinkedList<SegmentedCreature.PathNode>();

		// Token: 0x0400551E RID: 21790
		public LinkedList<SegmentedCreature.CreatureSegment> segments = new LinkedList<SegmentedCreature.CreatureSegment>();
	}

	// Token: 0x02000F24 RID: 3876
	public class PathNode
	{
		// Token: 0x06007123 RID: 28963 RVA: 0x002BC879 File Offset: 0x002BAA79
		public PathNode(Vector3 position)
		{
			this.position = position;
			this.rotation = Quaternion.identity;
		}

		// Token: 0x0400551F RID: 21791
		public Vector3 position;

		// Token: 0x04005520 RID: 21792
		public Quaternion rotation;
	}

	// Token: 0x02000F25 RID: 3877
	public class CreatureSegment
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06007124 RID: 28964 RVA: 0x002BC893 File Offset: 0x002BAA93
		public float ZOffset
		{
			get
			{
				return Grid.GetLayerZ(this.head.sceneLayer) + this.zRelativeOffset;
			}
		}

		// Token: 0x06007125 RID: 28965 RVA: 0x002BC8AC File Offset: 0x002BAAAC
		public CreatureSegment(KBatchedAnimController head, GameObject go, float zRelativeOffset, Vector3 offset, Vector3 pivot)
		{
			this.head = head;
			this.m_transform = go.transform;
			this.zRelativeOffset = zRelativeOffset;
			this.offset = offset;
			this.pivot = pivot;
			this.SetPosition(go.transform.position);
		}

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06007126 RID: 28966 RVA: 0x002BC8FC File Offset: 0x002BAAFC
		public Vector3 Position
		{
			get
			{
				Vector3 vector = this.offset;
				vector.x *= (float)(this.animController.FlipX ? -1 : 1);
				if (vector != Vector3.zero)
				{
					vector = this.Rotation * vector;
				}
				if (this.symbol.IsValid)
				{
					bool flag;
					Vector3 a = this.animController.GetSymbolTransform(this.symbol, out flag).GetColumn(3);
					a.z = this.ZOffset;
					return a + vector;
				}
				return this.m_transform.position + vector;
			}
		}

		// Token: 0x06007127 RID: 28967 RVA: 0x002BC9A0 File Offset: 0x002BABA0
		public void SetPosition(Vector3 value)
		{
			bool flag = false;
			if (this.animController != null && this.animController.sceneLayer != this.head.sceneLayer)
			{
				this.animController.SetSceneLayer(this.head.sceneLayer);
				flag = true;
			}
			value.z = this.ZOffset;
			this.m_transform.position = value;
			if (flag)
			{
				this.animController.enabled = false;
				this.animController.enabled = true;
			}
		}

		// Token: 0x06007128 RID: 28968 RVA: 0x002BCA21 File Offset: 0x002BAC21
		public void SetRotation(Quaternion rotation)
		{
			this.m_transform.rotation = rotation;
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06007129 RID: 28969 RVA: 0x002BCA30 File Offset: 0x002BAC30
		public Quaternion Rotation
		{
			get
			{
				if (this.symbol.IsValid)
				{
					bool flag;
					Vector3 toDirection = this.animController.GetSymbolLocalTransform(this.symbol, out flag).MultiplyVector(Vector3.right);
					if (!this.animController.FlipX)
					{
						toDirection.y *= -1f;
					}
					return Quaternion.FromToRotation(Vector3.right, toDirection);
				}
				return this.m_transform.rotation;
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x0600712A RID: 28970 RVA: 0x002BCA9F File Offset: 0x002BAC9F
		public Vector3 Forward
		{
			get
			{
				return this.Rotation * (this.animController.FlipX ? Vector3.left : Vector3.right);
			}
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x0600712B RID: 28971 RVA: 0x002BCAC5 File Offset: 0x002BACC5
		public Vector3 Up
		{
			get
			{
				return this.Rotation * Vector3.up;
			}
		}

		// Token: 0x0600712C RID: 28972 RVA: 0x002BCAD7 File Offset: 0x002BACD7
		public void CleanUp()
		{
			UnityEngine.Object.Destroy(this.m_transform.gameObject);
		}

		// Token: 0x04005521 RID: 21793
		public KBatchedAnimController animController;

		// Token: 0x04005522 RID: 21794
		public KAnimLink animLink;

		// Token: 0x04005523 RID: 21795
		public float distanceToPreviousSegment;

		// Token: 0x04005524 RID: 21796
		public HashedString symbol;

		// Token: 0x04005525 RID: 21797
		public Vector3 offset;

		// Token: 0x04005526 RID: 21798
		public Vector3 pivot;

		// Token: 0x04005527 RID: 21799
		public KBatchedAnimController head;

		// Token: 0x04005528 RID: 21800
		private float zRelativeOffset;

		// Token: 0x04005529 RID: 21801
		private Transform m_transform;
	}
}
