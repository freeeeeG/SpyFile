using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000079 RID: 121
public class TestMoving : MonoBehaviour
{
	// Token: 0x0600022D RID: 557 RVA: 0x0000970C File Offset: 0x0000790C
	private void Start()
	{
		base.StartCoroutine(this.CProcess());
	}

	// Token: 0x0600022E RID: 558 RVA: 0x0000971B File Offset: 0x0000791B
	private void Update()
	{
		if (this._moving)
		{
			base.transform.Translate(this._direction * this._speed * Time.deltaTime);
		}
	}

	// Token: 0x0600022F RID: 559 RVA: 0x00009750 File Offset: 0x00007950
	private IEnumerator CProcess()
	{
		for (;;)
		{
			this._moving = false;
			yield return Chronometer.global.WaitForSeconds(this._waitTime);
			this.SetDirection();
			this._directionToggle = !this._directionToggle;
			this._moving = true;
			yield return Chronometer.global.WaitForSeconds(this._waitTime);
		}
		yield break;
	}

	// Token: 0x06000230 RID: 560 RVA: 0x00009760 File Offset: 0x00007960
	private void SetDirection()
	{
		float x = UnityEngine.Random.Range(this._boundary.transform.position.x - this._boundary.radius, this._boundary.transform.position.x + this._boundary.radius);
		float y = UnityEngine.Random.Range(this._boundary.transform.position.y - this._boundary.radius, this._boundary.transform.position.y + this._boundary.radius);
		this._direction = (new Vector2(x, y) - base.transform.position).normalized;
	}

	// Token: 0x040001E3 RID: 483
	[SerializeField]
	private CircleCollider2D _boundary;

	// Token: 0x040001E4 RID: 484
	[SerializeField]
	private RopeBridge _leftUp;

	// Token: 0x040001E5 RID: 485
	[SerializeField]
	private RopeBridge _rightUp;

	// Token: 0x040001E6 RID: 486
	[SerializeField]
	private RopeBridge _leftDown;

	// Token: 0x040001E7 RID: 487
	[SerializeField]
	private RopeBridge _rightDown;

	// Token: 0x040001E8 RID: 488
	[SerializeField]
	private float _waitTime;

	// Token: 0x040001E9 RID: 489
	[SerializeField]
	private float _moveTime;

	// Token: 0x040001EA RID: 490
	[SerializeField]
	private float _speed;

	// Token: 0x040001EB RID: 491
	private Vector2 _direction;

	// Token: 0x040001EC RID: 492
	private bool _moving;

	// Token: 0x040001ED RID: 493
	private bool _directionToggle;
}
