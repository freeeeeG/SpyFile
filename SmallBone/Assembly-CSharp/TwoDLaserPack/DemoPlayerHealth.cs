using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	// Token: 0x02001659 RID: 5721
	public class DemoPlayerHealth : MonoBehaviour
	{
		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06006D06 RID: 27910 RVA: 0x00137A68 File Offset: 0x00135C68
		// (set) Token: 0x06006D07 RID: 27911 RVA: 0x00137A70 File Offset: 0x00135C70
		public int HealthPoints
		{
			get
			{
				return this._healthPoints;
			}
			set
			{
				this._healthPoints = value;
				if (this._healthPoints <= 0)
				{
					if (this.bloodSplatPrefab != null)
					{
						UnityEngine.Object.Instantiate<GameObject>(this.bloodSplatPrefab, base.transform.position, Quaternion.identity);
					}
					this.healthText.text = "Health: 0";
					base.gameObject.GetComponent<Renderer>().enabled = false;
					base.gameObject.GetComponent<PlayerMovement>().enabled = false;
					this.restartButton.gameObject.SetActive(true);
					foreach (LineBasedLaser lineBasedLaser in this.allLasersInScene)
					{
						lineBasedLaser.OnLaserHitTriggered -= this.LaserOnOnLaserHitTriggered;
						lineBasedLaser.SetLaserState(false);
					}
					return;
				}
				this.healthText.text = "Health: " + this._healthPoints.ToString();
			}
		}

		// Token: 0x06006D08 RID: 27912 RVA: 0x00137B50 File Offset: 0x00135D50
		private void Start()
		{
			this._healthPoints = 10;
			if (this.restartButton == null)
			{
				this.restartButton = UnityEngine.Object.FindObjectsOfType<Button>().FirstOrDefault((Button b) => b.name == "ButtonReplay");
			}
			this.healthText = UnityEngine.Object.FindObjectsOfType<Text>().FirstOrDefault((Text t) => t.name == "TextHealth");
			this.healthText.text = "Health: 10";
			this.allLasersInScene = UnityEngine.Object.FindObjectsOfType<LineBasedLaser>();
			this.restartButton.onClick.RemoveAllListeners();
			this.restartButton.onClick.AddListener(new UnityAction(this.OnRestartButtonClick));
			if (this.allLasersInScene.Any<LineBasedLaser>())
			{
				foreach (LineBasedLaser lineBasedLaser in this.allLasersInScene)
				{
					lineBasedLaser.OnLaserHitTriggered += this.LaserOnOnLaserHitTriggered;
					lineBasedLaser.SetLaserState(true);
					lineBasedLaser.targetGo = base.gameObject;
				}
			}
			base.gameObject.GetComponent<PlayerMovement>().enabled = true;
			base.gameObject.GetComponent<Renderer>().enabled = true;
			this.restartButton.gameObject.SetActive(false);
		}

		// Token: 0x06006D09 RID: 27913 RVA: 0x00137C94 File Offset: 0x00135E94
		private void OnRestartButtonClick()
		{
			this.CreateNewPlayer();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06006D0A RID: 27914 RVA: 0x00137CA8 File Offset: 0x00135EA8
		private void CreateNewPlayer()
		{
			GameObject targetGo = UnityEngine.Object.Instantiate<GameObject>(this.playerPrefab, new Vector2(6.26f, -2.8f), Quaternion.identity);
			LineBasedLaser[] array = this.allLasersInScene;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].targetGo = targetGo;
			}
		}

		// Token: 0x06006D0B RID: 27915 RVA: 0x00137CF8 File Offset: 0x00135EF8
		private void LaserOnOnLaserHitTriggered(RaycastHit2D hitInfo)
		{
			if (hitInfo.collider.gameObject == base.gameObject && this.bloodParticleSystem != null)
			{
				this.bloodParticleSystem.Play();
				int healthPoints = this.HealthPoints;
				this.HealthPoints = healthPoints - 1;
			}
		}

		// Token: 0x06006D0C RID: 27916 RVA: 0x00002191 File Offset: 0x00000391
		private void Update()
		{
		}

		// Token: 0x040058CD RID: 22733
		public GameObject bloodSplatPrefab;

		// Token: 0x040058CE RID: 22734
		public GameObject playerPrefab;

		// Token: 0x040058CF RID: 22735
		public Button restartButton;

		// Token: 0x040058D0 RID: 22736
		public Text healthText;

		// Token: 0x040058D1 RID: 22737
		private LineBasedLaser[] allLasersInScene;

		// Token: 0x040058D2 RID: 22738
		public ParticleSystem bloodParticleSystem;

		// Token: 0x040058D3 RID: 22739
		[SerializeField]
		private int _healthPoints;
	}
}
