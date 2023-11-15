using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020008F4 RID: 2292
public class ConveyorPrediction : IClientSidePredicted
{
	// Token: 0x06002CA7 RID: 11431 RVA: 0x000D3097 File Offset: 0x000D1497
	public void EnqueueDestination(ConveyorPrediction.Destination dest)
	{
		this.m_Destinations.Add(dest);
		if (this.m_Destinations.Count == 1)
		{
			ConveyorPrediction.OnStartedMovingToDestination(this.m_Destinations._items[0], this);
		}
	}

	// Token: 0x06002CA8 RID: 11432 RVA: 0x000D30D7 File Offset: 0x000D14D7
	public void Clear()
	{
		this.m_Destinations.Clear();
	}

	// Token: 0x06002CA9 RID: 11433 RVA: 0x000D30E4 File Offset: 0x000D14E4
	public void Update()
	{
		if (TimeManager.IsPaused(this.m_Transform.gameObject))
		{
			for (int i = 0; i < this.m_Destinations.Count; i++)
			{
				ConveyorPrediction.Destination[] items = this.m_Destinations._items;
				int num = i;
				items[num].arriveTime = items[num].arriveTime + ClientTime.DeltaTime();
			}
			return;
		}
		if (this.m_Destinations.Count > 0)
		{
			Vector3 position = this.m_Destinations._items[0].targetTransform.position;
			float num2 = ClientTime.Time();
			float num3 = this.m_Destinations._items[0].arriveTime - num2;
			if (num3 <= Time.deltaTime)
			{
				this.m_Transform.position = position;
				ConveyorPrediction.OnDestinationReached(this.m_Destinations._items[0], this);
				this.m_Destinations.RemoveAt(0);
				if (this.m_Destinations.Count > 0)
				{
					ConveyorPrediction.OnStartedMovingToDestination(this.m_Destinations._items[0], this);
				}
			}
			else
			{
				Vector3 a = position - this.m_Transform.position;
				float d = a.magnitude / num3;
				a.Normalize();
				Vector3 b = a * d * TimeManager.GetDeltaTime(this.m_Transform.gameObject);
				this.m_Transform.position += b;
			}
		}
	}

	// Token: 0x040023F6 RID: 9206
	public FastList<ConveyorPrediction.Destination> m_Destinations = new FastList<ConveyorPrediction.Destination>();

	// Token: 0x040023F7 RID: 9207
	public Transform m_Transform;

	// Token: 0x040023F8 RID: 9208
	public static GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction> OnStartedMovingToDestination = delegate(ConveyorPrediction.Destination dest, ConveyorPrediction prediction)
	{
	};

	// Token: 0x040023F9 RID: 9209
	public static GenericVoid<ConveyorPrediction.Destination, ConveyorPrediction> OnDestinationReached = delegate(ConveyorPrediction.Destination dest, ConveyorPrediction prediction)
	{
	};

	// Token: 0x040023FA RID: 9210
	private float m_RemainingMove;

	// Token: 0x040023FB RID: 9211
	private bool m_bCalculatedDistance;

	// Token: 0x020008F5 RID: 2293
	public struct Destination
	{
		// Token: 0x040023FC RID: 9212
		public uint targetEntityID;

		// Token: 0x040023FD RID: 9213
		public Transform targetTransform;

		// Token: 0x040023FE RID: 9214
		public float arriveTime;

		// Token: 0x040023FF RID: 9215
		public float attachTime;
	}
}
