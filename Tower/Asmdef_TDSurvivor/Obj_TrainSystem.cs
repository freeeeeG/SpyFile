using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000D2 RID: 210
public class Obj_TrainSystem : MonoBehaviour
{
	// Token: 0x060004EA RID: 1258 RVA: 0x000139F8 File Offset: 0x00011BF8
	private void Start()
	{
		this.list_StartIndex = new List<int>();
		foreach (Obj_TrainSystem.CartData cartData in this.list_CartData)
		{
			cartData.t = cartData.startT;
			cartData.currentIndex = Mathf.RoundToInt((float)this.trainRails.Count * cartData.t);
			cartData.trainCart.transform.position = this.trainRails[cartData.currentIndex].MidPoint;
			this.list_StartIndex.Add(cartData.currentIndex);
		}
		EventMgr.SendEvent<eTutorialType>(eGameEvents.QueueTutorialForGameStart, eTutorialType.TRAIN_CART);
	}

	// Token: 0x060004EB RID: 1259 RVA: 0x00013AC0 File Offset: 0x00011CC0
	private void Update()
	{
		if (Singleton<GameStateController>.Instance.IsInBattle)
		{
			foreach (Obj_TrainSystem.CartData cart in this.list_CartData)
			{
				this.UpdateCartMovement(cart, Time.deltaTime);
			}
			if (this.isLastFrameInBattle)
			{
				goto IL_141;
			}
			using (List<Obj_TrainSystem.CartData>.Enumerator enumerator = this.list_CartData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Obj_TrainSystem.CartData cartData = enumerator.Current;
					cartData.trainCart.ToggleCartAnimation(true);
				}
				goto IL_141;
			}
		}
		if (this.doStopAtBase)
		{
			for (int i = 0; i < this.list_CartData.Count; i++)
			{
				int num = this.list_StartIndex[i];
				Obj_TrainSystem.CartData cartData2 = this.list_CartData[i];
				if (cartData2.currentIndex != num)
				{
					this.UpdateCartMovement(cartData2, Time.deltaTime);
				}
				else if (cartData2.trainCart.IsAnimPlaying)
				{
					cartData2.trainCart.ToggleCartAnimation(false);
				}
			}
		}
		else if (this.isLastFrameInBattle)
		{
			foreach (Obj_TrainSystem.CartData cartData3 in this.list_CartData)
			{
				cartData3.trainCart.ToggleCartAnimation(false);
			}
		}
		IL_141:
		this.isLastFrameInBattle = Singleton<GameStateController>.Instance.IsInBattle;
	}

	// Token: 0x060004EC RID: 1260 RVA: 0x00013C48 File Offset: 0x00011E48
	private void UpdateCartMovement(Obj_TrainSystem.CartData cart, float deltaTime)
	{
		Obj_TrainRail obj_TrainRail = this.trainRails[cart.currentIndex];
		cart.speed = Mathf.Min(this.maxSpeed, cart.speed + deltaTime * this.accleration);
		cart.t += Time.deltaTime * cart.speed;
		cart.trainCart.transform.position = this.CalculateBezierPoint(cart.t, obj_TrainRail.StartPoint, obj_TrainRail.MidPoint, obj_TrainRail.EndPoint);
		if (cart.t >= 1f)
		{
			cart.t = 0f;
			cart.currentIndex = (cart.currentIndex + 1) % this.trainRails.Count;
		}
		cart.trainCart.UpdateCartDirection();
	}

	// Token: 0x060004ED RID: 1261 RVA: 0x00013D0C File Offset: 0x00011F0C
	private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		float num = 1f - t;
		float d = t * t;
		return num * num * p0 + 2f * num * t * p1 + d * p2;
	}

	// Token: 0x060004EE RID: 1262 RVA: 0x00013D50 File Offset: 0x00011F50
	public void FetchTrainCarts()
	{
		this.list_CartData.Clear();
		foreach (Obj_TrainCart cart in base.GetComponentsInChildren<Obj_TrainCart>())
		{
			this.list_CartData.Add(new Obj_TrainSystem.CartData(cart));
		}
		for (int j = 0; j < this.list_CartData.Count; j++)
		{
			this.list_CartData[j].startT = (float)j / (float)this.list_CartData.Count;
		}
	}

	// Token: 0x060004EF RID: 1263 RVA: 0x00013DC8 File Offset: 0x00011FC8
	public void FetchTrainRails()
	{
		this.trainRails.Clear();
		this.trainRails.AddRange(base.GetComponentsInChildren<Obj_TrainRail>());
		this.SortTrainRails();
	}

	// Token: 0x060004F0 RID: 1264 RVA: 0x00013DEC File Offset: 0x00011FEC
	public void SortTrainRails()
	{
		if (this.trainRails.Count == 0)
		{
			return;
		}
		List<Obj_TrainRail> list = new List<Obj_TrainRail>();
		Obj_TrainRail item = this.trainRails[0];
		list.Add(item);
		this.trainRails.Remove(item);
		while (this.trainRails.Count > 0)
		{
			Obj_TrainRail obj_TrainRail = list[list.Count - 1];
			Obj_TrainRail closestRail = this.GetClosestRail(obj_TrainRail.EndPoint, this.trainRails);
			list.Add(closestRail);
			this.trainRails.Remove(closestRail);
		}
		this.trainRails = list;
	}

	// Token: 0x060004F1 RID: 1265 RVA: 0x00013E7C File Offset: 0x0001207C
	private Obj_TrainRail GetClosestRail(Vector3 position, List<Obj_TrainRail> rails)
	{
		Obj_TrainRail result = null;
		float num = float.PositiveInfinity;
		foreach (Obj_TrainRail obj_TrainRail in rails)
		{
			float num2 = Vector3.Distance(position, obj_TrainRail.StartPoint);
			if (num2 < num)
			{
				result = obj_TrainRail;
				num = num2;
			}
		}
		return result;
	}

	// Token: 0x040004B6 RID: 1206
	[SerializeField]
	private List<Obj_TrainSystem.CartData> list_CartData;

	// Token: 0x040004B7 RID: 1207
	[SerializeField]
	private List<Obj_TrainRail> trainRails;

	// Token: 0x040004B8 RID: 1208
	[SerializeField]
	private bool doStopAtBase = true;

	// Token: 0x040004B9 RID: 1209
	[SerializeField]
	private float accleration = 5f;

	// Token: 0x040004BA RID: 1210
	[SerializeField]
	private float maxSpeed = 1f;

	// Token: 0x040004BB RID: 1211
	[SerializeField]
	private float stopLerpSpeed = 10f;

	// Token: 0x040004BC RID: 1212
	private List<int> list_StartIndex;

	// Token: 0x040004BD RID: 1213
	private bool isLastFrameInBattle;

	// Token: 0x0200023E RID: 574
	[Serializable]
	public class CartData
	{
		// Token: 0x06000DD4 RID: 3540 RVA: 0x000349F7 File Offset: 0x00032BF7
		public CartData()
		{
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x00034A0A File Offset: 0x00032C0A
		public CartData(Obj_TrainCart cart)
		{
			this.trainCart = cart;
		}

		// Token: 0x04000B19 RID: 2841
		public Obj_TrainCart trainCart;

		// Token: 0x04000B1A RID: 2842
		[Range(0f, 1f)]
		public float startT;

		// Token: 0x04000B1B RID: 2843
		[HideInInspector]
		public float speed = 1f;

		// Token: 0x04000B1C RID: 2844
		[HideInInspector]
		public int currentIndex;

		// Token: 0x04000B1D RID: 2845
		[HideInInspector]
		public float t;
	}
}
