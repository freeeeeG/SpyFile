using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200057F RID: 1407
public class FireballSpawner : MonoBehaviour
{
	// Token: 0x06001AA8 RID: 6824 RVA: 0x0008588C File Offset: 0x00083C8C
	private void Update()
	{
		if (!TimeManager.IsPaused(base.gameObject))
		{
			if (this.FireCommmand && !this.m_firing)
			{
				this.SpawnFireball();
				this.m_firing = true;
				this.FireCommmand = false;
			}
			else if (this.m_firing && !this.FireCommmand)
			{
				this.m_firing = false;
			}
		}
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000858F8 File Offset: 0x00083CF8
	private void SpawnFireball()
	{
		GameObject gameObject = this.m_killObjectPrefab.InstantiateOnParent(null, true);
		gameObject.transform.localPosition = base.transform.position;
		gameObject.transform.localRotation = Quaternion.identity;
		SphereCollider sphereCollider = gameObject.RequireComponent<SphereCollider>();
		Collider[] array = Physics.OverlapSphere(gameObject.transform.TransformPoint(sphereCollider.center), sphereCollider.radius);
		for (int i = 0; i < array.Length; i++)
		{
			Physics.IgnoreCollision(array[i], sphereCollider);
		}
		ParticleSystem particleSystem = gameObject.RequestComponentRecursive<ParticleSystem>();
		if (particleSystem != null)
		{
			particleSystem.RestartPFX();
		}
		FireballSpawner.Mover mover = gameObject.AddComponent<FireballSpawner.Mover>();
		mover.Velocity = this.m_fireballSpeed * (this.m_target.position - gameObject.transform.position).SafeNormalised(Vector3.zero);
		this.m_fireballs.Add(gameObject);
	}

	// Token: 0x04001515 RID: 5397
	[SerializeField]
	private Transform m_target;

	// Token: 0x04001516 RID: 5398
	[SerializeField]
	private GameObject m_killObjectPrefab;

	// Token: 0x04001517 RID: 5399
	[SerializeField]
	private float m_fireballSpeed = 2f;

	// Token: 0x04001518 RID: 5400
	[Header("Animator Variables")]
	public bool FireCommmand;

	// Token: 0x04001519 RID: 5401
	private bool m_firing;

	// Token: 0x0400151A RID: 5402
	private List<GameObject> m_fireballs = new List<GameObject>();

	// Token: 0x0400151B RID: 5403
	private float m_timer;

	// Token: 0x02000580 RID: 1408
	private class Mover : MonoBehaviour
	{
		// Token: 0x06001AAB RID: 6827 RVA: 0x000859E9 File Offset: 0x00083DE9
		private void Update()
		{
			base.transform.position += TimeManager.GetDeltaTime(base.gameObject) * this.Velocity;
		}

		// Token: 0x0400151C RID: 5404
		public Vector3 Velocity;

		// Token: 0x0400151D RID: 5405
		private float m_prop;
	}
}
