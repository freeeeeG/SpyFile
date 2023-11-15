using System;
using System.Collections.Generic;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Pope
{
	// Token: 0x020011E3 RID: 4579
	public sealed class Navigation : MonoBehaviour
	{
		// Token: 0x170011E8 RID: 4584
		// (get) Token: 0x060059E3 RID: 23011 RVA: 0x0010B2F7 File Offset: 0x001094F7
		// (set) Token: 0x060059E4 RID: 23012 RVA: 0x0010B2FF File Offset: 0x001094FF
		public Transform destination { get; set; }

		// Token: 0x170011E9 RID: 4585
		// (get) Token: 0x060059E5 RID: 23013 RVA: 0x0010B308 File Offset: 0x00109508
		// (set) Token: 0x060059E6 RID: 23014 RVA: 0x0010B310 File Offset: 0x00109510
		public Point.Tag destinationTag
		{
			get
			{
				return this._destinationTag;
			}
			set
			{
				switch (value)
				{
				case Point.Tag.None:
					this.destination = this._pointContainer.GetChild(UnityEngine.Random.Range(0, this._pointContainer.childCount - 1));
					return;
				case Point.Tag.Top:
					this.destination = this._top.transform;
					return;
				case Point.Tag.Center:
					this.destination = this._center.transform;
					return;
				case Point.Tag.Opposition:
				{
					Character player = Singleton<Service>.Instance.levelManager.player;
					int floor = this.GetFloor(player.transform.position.y);
					foreach (Point point in this._pointContainer.GetComponentsInChildren<Point>())
					{
						if (point.tag == Point.Tag.Opposition && point.floor == floor && Mathf.Sign(Map.Instance.bounds.center.x - player.transform.position.x) != Mathf.Sign(Map.Instance.bounds.center.x - point.transform.position.x))
						{
							this.destination = point.transform;
							return;
						}
					}
					return;
				}
				case Point.Tag.Inner:
					this.destination = this._inners.Random<Point>().transform;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x060059E7 RID: 23015 RVA: 0x0010B468 File Offset: 0x00109668
		private int GetFloor(float target)
		{
			Bounds bounds = this._platformArea.bounds;
			float num = (bounds.max.y - bounds.min.y) / 5f;
			if (target < bounds.min.y + num)
			{
				return 1;
			}
			if (target < bounds.min.y + num * 2f)
			{
				return 2;
			}
			if (target < bounds.min.y + num * 3f)
			{
				return 3;
			}
			if (target < bounds.min.y + num * 4f)
			{
				return 4;
			}
			return 5;
		}

		// Token: 0x060059E8 RID: 23016 RVA: 0x0010B500 File Offset: 0x00109700
		private void Awake()
		{
			foreach (Point point in this._pointContainer.GetComponentsInChildren<Point>())
			{
				if (point.tag == Point.Tag.Top)
				{
					this._top = point;
				}
				else if (point.tag == Point.Tag.Center)
				{
					this._center = point;
				}
				else if (point.tag == Point.Tag.Inner)
				{
					this._inners.Add(point);
				}
			}
		}

		// Token: 0x04004897 RID: 18583
		[SerializeField]
		private Transform _pointContainer;

		// Token: 0x04004898 RID: 18584
		[SerializeField]
		private Collider2D _platformArea;

		// Token: 0x04004899 RID: 18585
		private Point.Tag _destinationTag;

		// Token: 0x0400489A RID: 18586
		private Point _top;

		// Token: 0x0400489B RID: 18587
		private Point _center;

		// Token: 0x0400489C RID: 18588
		private List<Point> _inners = new List<Point>();
	}
}
