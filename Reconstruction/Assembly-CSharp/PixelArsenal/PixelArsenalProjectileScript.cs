using System;
using UnityEngine;

namespace PixelArsenal
{
	// Token: 0x020002AF RID: 687
	public class PixelArsenalProjectileScript : MonoBehaviour
	{
		// Token: 0x060010F1 RID: 4337 RVA: 0x0002F9C4 File Offset: 0x0002DBC4
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

		// Token: 0x060010F2 RID: 4338 RVA: 0x0002FA54 File Offset: 0x0002DC54
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

		// Token: 0x04000928 RID: 2344
		public GameObject impactParticle;

		// Token: 0x04000929 RID: 2345
		public GameObject projectileParticle;

		// Token: 0x0400092A RID: 2346
		public GameObject muzzleParticle;

		// Token: 0x0400092B RID: 2347
		[Header("Adjust if not using Sphere Collider")]
		public float colliderRadius = 1f;

		// Token: 0x0400092C RID: 2348
		[Range(0f, 1f)]
		public float collideOffset = 0.15f;
	}
}
