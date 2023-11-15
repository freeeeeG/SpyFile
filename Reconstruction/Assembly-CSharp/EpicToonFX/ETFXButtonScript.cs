using System;
using UnityEngine;
using UnityEngine.UI;

namespace EpicToonFX
{
	// Token: 0x020002BA RID: 698
	public class ETFXButtonScript : MonoBehaviour
	{
		// Token: 0x0600111E RID: 4382 RVA: 0x000309B4 File Offset: 0x0002EBB4
		private void Start()
		{
			this.effectScript = GameObject.Find("ETFXFireProjectile").GetComponent<ETFXFireProjectile>();
			this.getProjectileNames();
			this.MyButtonText = this.Button.transform.Find("Text").GetComponent<Text>();
			this.MyButtonText.text = this.projectileParticleName;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x00030A0D File Offset: 0x0002EC0D
		private void Update()
		{
			this.MyButtonText.text = this.projectileParticleName;
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00030A20 File Offset: 0x0002EC20
		public void getProjectileNames()
		{
			this.projectileScript = this.effectScript.projectiles[this.effectScript.currentProjectile].GetComponent<ETFXProjectileScript>();
			this.projectileParticleName = this.projectileScript.projectileParticle.name;
		}

		// Token: 0x06001121 RID: 4385 RVA: 0x00030A5C File Offset: 0x0002EC5C
		public bool overButton()
		{
			Rect rect = new Rect(this.buttonsX, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
			Rect rect2 = new Rect(this.buttonsX + this.buttonsDistance, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
			return rect.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)) || rect2.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y));
		}

		// Token: 0x0400095C RID: 2396
		public GameObject Button;

		// Token: 0x0400095D RID: 2397
		private Text MyButtonText;

		// Token: 0x0400095E RID: 2398
		private string projectileParticleName;

		// Token: 0x0400095F RID: 2399
		private ETFXFireProjectile effectScript;

		// Token: 0x04000960 RID: 2400
		private ETFXProjectileScript projectileScript;

		// Token: 0x04000961 RID: 2401
		public float buttonsX;

		// Token: 0x04000962 RID: 2402
		public float buttonsY;

		// Token: 0x04000963 RID: 2403
		public float buttonsSizeX;

		// Token: 0x04000964 RID: 2404
		public float buttonsSizeY;

		// Token: 0x04000965 RID: 2405
		public float buttonsDistance;
	}
}
