using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EpicToonFX
{
	// Token: 0x020002BB RID: 699
	public class ETFXFireProjectile : MonoBehaviour
	{
		// Token: 0x06001123 RID: 4387 RVA: 0x00030B0B File Offset: 0x0002ED0B
		private void Start()
		{
			this.selectedProjectileButton = GameObject.Find("Button").GetComponent<ETFXButtonScript>();
		}

		// Token: 0x06001124 RID: 4388 RVA: 0x00030B24 File Offset: 0x0002ED24
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

		// Token: 0x06001125 RID: 4389 RVA: 0x00030C4D File Offset: 0x0002EE4D
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

		// Token: 0x06001126 RID: 4390 RVA: 0x00030C83 File Offset: 0x0002EE83
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

		// Token: 0x06001127 RID: 4391 RVA: 0x00030CB9 File Offset: 0x0002EEB9
		public void AdjustSpeed(float newSpeed)
		{
			this.speed = newSpeed;
		}

		// Token: 0x04000966 RID: 2406
		[SerializeField]
		public GameObject[] projectiles;

		// Token: 0x04000967 RID: 2407
		[Header("Missile spawns at attached game object")]
		public Transform spawnPosition;

		// Token: 0x04000968 RID: 2408
		[HideInInspector]
		public int currentProjectile;

		// Token: 0x04000969 RID: 2409
		public float speed = 500f;

		// Token: 0x0400096A RID: 2410
		private ETFXButtonScript selectedProjectileButton;

		// Token: 0x0400096B RID: 2411
		private RaycastHit hit;
	}
}
