using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EpicToonFX
{
	// Token: 0x02000062 RID: 98
	public class ETFXFireProjectile : MonoBehaviour
	{
		// Token: 0x06000140 RID: 320 RVA: 0x00006462 File Offset: 0x00004662
		private void Start()
		{
			this.selectedProjectileButton = GameObject.Find("Button").GetComponent<ETFXButtonScript>();
		}

		// Token: 0x06000141 RID: 321 RVA: 0x0000647C File Offset: 0x0000467C
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

		// Token: 0x06000142 RID: 322 RVA: 0x000065A5 File Offset: 0x000047A5
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

		// Token: 0x06000143 RID: 323 RVA: 0x000065DB File Offset: 0x000047DB
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

		// Token: 0x06000144 RID: 324 RVA: 0x00006611 File Offset: 0x00004811
		public void AdjustSpeed(float newSpeed)
		{
			this.speed = newSpeed;
		}

		// Token: 0x0400014A RID: 330
		[SerializeField]
		public GameObject[] projectiles;

		// Token: 0x0400014B RID: 331
		[Header("Missile spawns at attached game object")]
		public Transform spawnPosition;

		// Token: 0x0400014C RID: 332
		[HideInInspector]
		public int currentProjectile;

		// Token: 0x0400014D RID: 333
		public float speed = 500f;

		// Token: 0x0400014E RID: 334
		private ETFXButtonScript selectedProjectileButton;

		// Token: 0x0400014F RID: 335
		private RaycastHit hit;
	}
}
