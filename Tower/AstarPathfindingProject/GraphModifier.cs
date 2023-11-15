using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding
{
	// Token: 0x0200003A RID: 58
	[ExecuteInEditMode]
	public abstract class GraphModifier : VersionedMonoBehaviour
	{
		// Token: 0x0600029C RID: 668 RVA: 0x0000E9D8 File Offset: 0x0000CBD8
		protected static List<T> GetModifiersOfType<T>() where T : GraphModifier
		{
			GraphModifier graphModifier = GraphModifier.root;
			List<T> list = new List<T>();
			while (graphModifier != null)
			{
				T t = graphModifier as T;
				if (t != null)
				{
					list.Add(t);
				}
				graphModifier = graphModifier.next;
			}
			return list;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000EA28 File Offset: 0x0000CC28
		public static void FindAllModifiers()
		{
			GraphModifier[] array = Object.FindObjectsOfType(typeof(GraphModifier)) as GraphModifier[];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].enabled)
				{
					array[i].OnEnable();
				}
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000EA6C File Offset: 0x0000CC6C
		public static void TriggerEvent(GraphModifier.EventType type)
		{
			if (!Application.isPlaying)
			{
				GraphModifier.FindAllModifiers();
			}
			GraphModifier graphModifier = GraphModifier.root;
			if (type <= GraphModifier.EventType.PreUpdate)
			{
				switch (type)
				{
				case GraphModifier.EventType.PostScan:
					while (graphModifier != null)
					{
						graphModifier.OnPostScan();
						graphModifier = graphModifier.next;
					}
					return;
				case GraphModifier.EventType.PreScan:
					while (graphModifier != null)
					{
						graphModifier.OnPreScan();
						graphModifier = graphModifier.next;
					}
					return;
				case (GraphModifier.EventType)3:
					break;
				case GraphModifier.EventType.LatePostScan:
					while (graphModifier != null)
					{
						graphModifier.OnLatePostScan();
						graphModifier = graphModifier.next;
					}
					return;
				default:
					if (type != GraphModifier.EventType.PreUpdate)
					{
						return;
					}
					while (graphModifier != null)
					{
						graphModifier.OnGraphsPreUpdate();
						graphModifier = graphModifier.next;
					}
					return;
				}
			}
			else
			{
				if (type == GraphModifier.EventType.PostUpdate)
				{
					while (graphModifier != null)
					{
						graphModifier.OnGraphsPostUpdate();
						graphModifier = graphModifier.next;
					}
					return;
				}
				if (type != GraphModifier.EventType.PostCacheLoad)
				{
					return;
				}
				while (graphModifier != null)
				{
					graphModifier.OnPostCacheLoad();
					graphModifier = graphModifier.next;
				}
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000EB43 File Offset: 0x0000CD43
		protected virtual void OnEnable()
		{
			this.RemoveFromLinkedList();
			this.AddToLinkedList();
			this.ConfigureUniqueID();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000EB57 File Offset: 0x0000CD57
		protected virtual void OnDisable()
		{
			this.RemoveFromLinkedList();
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x0000EB5F File Offset: 0x0000CD5F
		protected override void Awake()
		{
			base.Awake();
			this.ConfigureUniqueID();
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000EB70 File Offset: 0x0000CD70
		private void ConfigureUniqueID()
		{
			GraphModifier x;
			if (GraphModifier.usedIDs.TryGetValue(this.uniqueID, out x) && x != this)
			{
				this.Reset();
			}
			GraphModifier.usedIDs[this.uniqueID] = this;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000EBB1 File Offset: 0x0000CDB1
		private void AddToLinkedList()
		{
			if (GraphModifier.root == null)
			{
				GraphModifier.root = this;
				return;
			}
			this.next = GraphModifier.root;
			GraphModifier.root.prev = this;
			GraphModifier.root = this;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000EBE4 File Offset: 0x0000CDE4
		private void RemoveFromLinkedList()
		{
			if (GraphModifier.root == this)
			{
				GraphModifier.root = this.next;
				if (GraphModifier.root != null)
				{
					GraphModifier.root.prev = null;
				}
			}
			else
			{
				if (this.prev != null)
				{
					this.prev.next = this.next;
				}
				if (this.next != null)
				{
					this.next.prev = this.prev;
				}
			}
			this.prev = null;
			this.next = null;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000EC6F File Offset: 0x0000CE6F
		protected virtual void OnDestroy()
		{
			GraphModifier.usedIDs.Remove(this.uniqueID);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x0000EC82 File Offset: 0x0000CE82
		public virtual void OnPostScan()
		{
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000EC84 File Offset: 0x0000CE84
		public virtual void OnPreScan()
		{
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000EC86 File Offset: 0x0000CE86
		public virtual void OnLatePostScan()
		{
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000EC88 File Offset: 0x0000CE88
		public virtual void OnPostCacheLoad()
		{
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000EC8A File Offset: 0x0000CE8A
		public virtual void OnGraphsPreUpdate()
		{
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000EC8C File Offset: 0x0000CE8C
		public virtual void OnGraphsPostUpdate()
		{
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000EC90 File Offset: 0x0000CE90
		protected override void Reset()
		{
			base.Reset();
			ulong num = (ulong)((long)Random.Range(0, int.MaxValue));
			ulong num2 = (ulong)((ulong)((long)Random.Range(0, int.MaxValue)) << 32);
			this.uniqueID = (num | num2);
			GraphModifier.usedIDs[this.uniqueID] = this;
		}

		// Token: 0x040001BB RID: 443
		private static GraphModifier root;

		// Token: 0x040001BC RID: 444
		private GraphModifier prev;

		// Token: 0x040001BD RID: 445
		private GraphModifier next;

		// Token: 0x040001BE RID: 446
		[SerializeField]
		[HideInInspector]
		protected ulong uniqueID;

		// Token: 0x040001BF RID: 447
		protected static Dictionary<ulong, GraphModifier> usedIDs = new Dictionary<ulong, GraphModifier>();

		// Token: 0x02000114 RID: 276
		public enum EventType
		{
			// Token: 0x040006BF RID: 1727
			PostScan = 1,
			// Token: 0x040006C0 RID: 1728
			PreScan,
			// Token: 0x040006C1 RID: 1729
			LatePostScan = 4,
			// Token: 0x040006C2 RID: 1730
			PreUpdate = 8,
			// Token: 0x040006C3 RID: 1731
			PostUpdate = 16,
			// Token: 0x040006C4 RID: 1732
			PostCacheLoad = 32
		}
	}
}
