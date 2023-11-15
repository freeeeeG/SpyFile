using System;
using BT;
using BT.SharedValues;
using Characters;
using Level.Traps;
using UnityEngine;

// Token: 0x0200005C RID: 92
public class RotatingBody : MonoBehaviour
{
	// Token: 0x060001B5 RID: 437 RVA: 0x00008188 File Offset: 0x00006388
	private void Awake()
	{
		float num = 0f;
		Orb[] child = this._child;
		for (int i = 0; i < child.Length; i++)
		{
			child[i].Initialize(num);
			num += 6.2831855f / (float)this._child.Length;
		}
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x000081CC File Offset: 0x000063CC
	private void OnEnable()
	{
		Context context = Context.Create();
		this._owner = base.GetComponentInParent<Character>();
		if (this._owner == null)
		{
			Debug.LogError(this.ToString() + " must have an owner");
		}
		context.Set<Character>(BT.Key.OwnerCharacter, new SharedValue<Character>(this._owner));
		context.Set<float>("radius", new SharedValue<float>(this._radius));
		context.Set<float>("speed", new SharedValue<float>(this._speed));
		context.Set<float>("currentChildCount", new SharedValue<float>(0f));
		this._bTRunner.Run(context);
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x00008274 File Offset: 0x00006474
	public void Rotate()
	{
		if (this._bTRunner.context.Get<Character>(BT.Key.OwnerCharacter) == null)
		{
			return;
		}
		float radious = this._bTRunner.context.Get<float>("radius");
		float amount = this._bTRunner.context.Get<float>("speed") * this._owner.chronometer.master.deltaTime;
		Orb[] child = this._child;
		for (int i = 0; i < child.Length; i++)
		{
			child[i].MoveCenteredOn(base.transform.position, radious, amount);
		}
	}

	// Token: 0x0400017B RID: 379
	[SerializeField]
	private BehaviourTreeRunner _bTRunner;

	// Token: 0x0400017C RID: 380
	[SerializeField]
	private float _radius;

	// Token: 0x0400017D RID: 381
	[SerializeField]
	private float _speed;

	// Token: 0x0400017E RID: 382
	[SerializeField]
	private int _maxChildCount = 3;

	// Token: 0x0400017F RID: 383
	[SerializeField]
	private Orb[] _child;

	// Token: 0x04000180 RID: 384
	private Character _owner;

	// Token: 0x04000181 RID: 385
	private const string RADIUS_KEY = "radius";

	// Token: 0x04000182 RID: 386
	private const string SPEED_KEY = "speed";

	// Token: 0x04000183 RID: 387
	private const string CUREENTCHILDCOUNT_KEY = "currentChildCount";
}
