using System;
using UnityEngine;
using UnityEngine.UI;

namespace EpicToonFX
{
	// Token: 0x0200005E RID: 94
	public class ETFXButtonScript : MonoBehaviour
	{
		// Token: 0x06000123 RID: 291 RVA: 0x00005E98 File Offset: 0x00004098
		private void Start()
		{
			this.effectScript = GameObject.Find("ETFXFireProjectile").GetComponent<ETFXFireProjectile>();
			this.getProjectileNames();
			this.MyButtonText = this.Button.transform.Find("Text").GetComponent<Text>();
			this.MyButtonText.text = this.projectileParticleName;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00005EF1 File Offset: 0x000040F1
		private void Update()
		{
			this.MyButtonText.text = this.projectileParticleName;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00005F04 File Offset: 0x00004104
		public void getProjectileNames()
		{
			this.projectileScript = this.effectScript.projectiles[this.effectScript.currentProjectile].GetComponent<ETFXProjectileScript>();
			this.projectileParticleName = this.projectileScript.projectileParticle.name;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00005F40 File Offset: 0x00004140
		public bool overButton()
		{
			Rect rect = new Rect(this.buttonsX, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
			Rect rect2 = new Rect(this.buttonsX + this.buttonsDistance, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
			return rect.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)) || rect2.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y));
		}

		// Token: 0x0400011F RID: 287
		public GameObject Button;

		// Token: 0x04000120 RID: 288
		private Text MyButtonText;

		// Token: 0x04000121 RID: 289
		private string projectileParticleName;

		// Token: 0x04000122 RID: 290
		private ETFXFireProjectile effectScript;

		// Token: 0x04000123 RID: 291
		private ETFXProjectileScript projectileScript;

		// Token: 0x04000124 RID: 292
		public float buttonsX;

		// Token: 0x04000125 RID: 293
		public float buttonsY;

		// Token: 0x04000126 RID: 294
		public float buttonsSizeX;

		// Token: 0x04000127 RID: 295
		public float buttonsSizeY;

		// Token: 0x04000128 RID: 296
		public float buttonsDistance;
	}
}
