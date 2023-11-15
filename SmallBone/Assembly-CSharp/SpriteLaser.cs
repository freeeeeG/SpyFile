using System;
using Characters.Operations.Summon;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

// Token: 0x0200008D RID: 141
public class SpriteLaser : MonoBehaviour
{
	// Token: 0x060002B1 RID: 689 RVA: 0x0000A9F4 File Offset: 0x00008BF4
	private void Update()
	{
		this._pivot.Rotate(new Vector3(0f, 0f, 30f * Time.deltaTime));
		SpriteLaser._laycaster.contactFilter.SetLayerMask(this._terrainMask);
		Vector3 v = Quaternion.Euler(0f, 0f, this._pivot.rotation.eulerAngles.z) * Vector2.down;
		SpriteLaser._laycaster.RayCast(this._firePosition.position, v, this._maxDistance);
		ReadonlyBoundedList<RaycastHit2D> results = SpriteLaser._laycaster.results;
		if (results.Count <= 0)
		{
			this._elapsed = this._fireTerm;
			this._body.localScale = new Vector2(1f, this._maxDistance);
			this._hitEffect.SetActive(false);
			return;
		}
		int index = 0;
		float num = results[0].distance;
		for (int i = 1; i < results.Count; i++)
		{
			float distance = results[i].distance;
			if (distance < num)
			{
				num = distance;
				index = i;
			}
		}
		RaycastHit2D raycastHit2D = results[index];
		this._body.localScale = new Vector2(1f, Vector2.Distance(this._firePosition.transform.position, raycastHit2D.point));
		this._hitEffect.transform.position = raycastHit2D.point;
		this._hitEffect.SetActive(true);
		this._elapsed += Time.deltaTime;
		if (this._elapsed >= this._fireTerm)
		{
			this._summonOperationRunner.Run(Singleton<Service>.Instance.levelManager.player);
			this._elapsed -= this._fireTerm;
		}
	}

	// Token: 0x0400023D RID: 573
	[Header("Length")]
	[SerializeField]
	private float _minWidth = 2f;

	// Token: 0x0400023E RID: 574
	[SerializeField]
	private float _maxWidth = 40f;

	// Token: 0x0400023F RID: 575
	[SerializeField]
	private float _minHeight = 0.78125f;

	// Token: 0x04000240 RID: 576
	[SerializeField]
	private float _maxDistance = 30f;

	// Token: 0x04000241 RID: 577
	[SerializeField]
	[Header("Point")]
	private Transform _firePosition;

	// Token: 0x04000242 RID: 578
	[SerializeField]
	private Transform _pivot;

	// Token: 0x04000243 RID: 579
	[SerializeField]
	private Transform _body;

	// Token: 0x04000244 RID: 580
	[SerializeField]
	[Header("Hit")]
	private LayerMask _terrainMask;

	// Token: 0x04000245 RID: 581
	[SerializeField]
	private GameObject _hitEffect;

	// Token: 0x04000246 RID: 582
	[SerializeField]
	private float _fireTerm = 0.5f;

	// Token: 0x04000247 RID: 583
	[Subcomponent(typeof(SummonOperationRunner))]
	[SerializeField]
	private SummonOperationRunner _summonOperationRunner;

	// Token: 0x04000248 RID: 584
	private static NonAllocCaster _laycaster = new NonAllocCaster(15);

	// Token: 0x04000249 RID: 585
	private float _elapsed;
}
