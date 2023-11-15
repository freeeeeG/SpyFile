using System;
using UnityEngine;

namespace EpicToonFX
{
	// Token: 0x02000065 RID: 101
	public class ETFXProjectileScript : MonoBehaviour
	{
		// Token: 0x06000153 RID: 339 RVA: 0x000069EC File Offset: 0x00004BEC
		private void Start()
		{
			this.projectileParticle = Object.Instantiate<GameObject>(this.projectileParticle, base.transform.position, base.transform.rotation);
			this.projectileParticle.transform.parent = base.transform;
			if (this.muzzleParticle)
			{
				this.muzzleParticle = Object.Instantiate<GameObject>(this.muzzleParticle, base.transform.position, base.transform.rotation);
				Object.Destroy(this.muzzleParticle, 1.5f);
			}
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00006A7C File Offset: 0x00004C7C
		private void FixedUpdate()
		{
			if (base.GetComponent<Rigidbody>().velocity.magnitude != 0f)
			{
				base.transform.rotation = Quaternion.LookRotation(base.GetComponent<Rigidbody>().velocity);
			}
			float radius;
			if (base.transform.GetComponent<SphereCollider>())
			{
				radius = base.transform.GetComponent<SphereCollider>().radius;
			}
			else
			{
				radius = this.colliderRadius;
			}
			Vector3 vector = base.transform.GetComponent<Rigidbody>().velocity;
			if (base.transform.GetComponent<Rigidbody>().useGravity)
			{
				vector += Physics.gravity * Time.deltaTime;
			}
			vector = vector.normalized;
			float maxDistance = base.transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime;
			RaycastHit raycastHit;
			if (Physics.SphereCast(base.transform.position, radius, vector, out raycastHit, maxDistance))
			{
				base.transform.position = raycastHit.point + raycastHit.normal * this.collideOffset;
				GameObject obj = Object.Instantiate<GameObject>(this.impactParticle, base.transform.position, Quaternion.FromToRotation(Vector3.up, raycastHit.normal));
				ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
				for (int i = 1; i < componentsInChildren.Length; i++)
				{
					ParticleSystem particleSystem = componentsInChildren[i];
					if (particleSystem.gameObject.name.Contains("Trail"))
					{
						particleSystem.transform.SetParent(null);
						Object.Destroy(particleSystem.gameObject, 2f);
					}
				}
				Object.Destroy(this.projectileParticle, 3f);
				Object.Destroy(obj, 3.5f);
				Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04000166 RID: 358
		public GameObject impactParticle;

		// Token: 0x04000167 RID: 359
		public GameObject projectileParticle;

		// Token: 0x04000168 RID: 360
		public GameObject muzzleParticle;

		// Token: 0x04000169 RID: 361
		[Header("Adjust if not using Sphere Collider")]
		public float colliderRadius = 1f;

		// Token: 0x0400016A RID: 362
		[Range(0f, 1f)]
		public float collideOffset = 0.15f;
	}
}
