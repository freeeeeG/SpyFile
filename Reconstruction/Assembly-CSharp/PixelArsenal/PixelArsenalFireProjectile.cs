using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PixelArsenal
{
	// Token: 0x020002AE RID: 686
	public class PixelArsenalFireProjectile : MonoBehaviour
	{
		// Token: 0x060010EB RID: 4331 RVA: 0x0002F7F7 File Offset: 0x0002D9F7
		private void Start()
		{
			this.selectedProjectileButton = GameObject.Find("Button").GetComponent<PixelArsenalButtonScript>();
		}

		// Token: 0x060010EC RID: 4332 RVA: 0x0002F810 File Offset: 0x0002DA10
		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				this.nextEffect();
			}
			if (Input.GetKeyDown(KeyCode.D))
			{
				this.nextEffect();
			}
			if (Input.GetKeyDown(KeyCode.A))
			{
				this.previousEffect();
			}
			else if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				this.previousEffect();
			}
			if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out this.hit, 100f))
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.projectiles[this.currentProjectile], this.spawnPosition.position, Quaternion.identity);
				gameObject.transform.LookAt(this.hit.point);
				gameObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * this.speed);
			}
			Debug.DrawRay(Camera.main.ScreenPointToRay(Input.mousePosition).origin, Camera.main.ScreenPointToRay(Input.mousePosition).direction * 100f, Color.yellow);
		}

		// Token: 0x060010ED RID: 4333 RVA: 0x0002F939 File Offset: 0x0002DB39
		public void nextEffect()
		{
			if (this.currentProjectile < this.projectiles.Length - 1)
			{
				this.currentProjectile++;
			}
			else
			{
				this.currentProjectile = 0;
			}
			this.selectedProjectileButton.getProjectileNames();
		}

		// Token: 0x060010EE RID: 4334 RVA: 0x0002F96F File Offset: 0x0002DB6F
		public void previousEffect()
		{
			if (this.currentProjectile > 0)
			{
				this.currentProjectile--;
			}
			else
			{
				this.currentProjectile = this.projectiles.Length - 1;
			}
			this.selectedProjectileButton.getProjectileNames();
		}

		// Token: 0x060010EF RID: 4335 RVA: 0x0002F9A5 File Offset: 0x0002DBA5
		public void AdjustSpeed(float newSpeed)
		{
			this.speed = newSpeed;
		}

		// Token: 0x04000922 RID: 2338
		[SerializeField]
		public GameObject[] projectiles;

		// Token: 0x04000923 RID: 2339
		[Header("Missile spawns at attached game object")]
		public Transform spawnPosition;

		// Token: 0x04000924 RID: 2340
		[HideInInspector]
		public int currentProjectile;

		// Token: 0x04000925 RID: 2341
		public float speed = 1000f;

		// Token: 0x04000926 RID: 2342
		private PixelArsenalButtonScript selectedProjectileButton;

		// Token: 0x04000927 RID: 2343
		private RaycastHit hit;
	}
}
