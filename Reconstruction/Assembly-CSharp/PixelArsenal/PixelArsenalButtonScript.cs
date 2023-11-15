using System;
using UnityEngine;
using UnityEngine.UI;

namespace PixelArsenal
{
	// Token: 0x020002AC RID: 684
	public class PixelArsenalButtonScript : MonoBehaviour
	{
		// Token: 0x060010E2 RID: 4322 RVA: 0x0002F400 File Offset: 0x0002D600
		private void Start()
		{
			this.effectScript = GameObject.Find("PixelArsenalFireProjectile").GetComponent<PixelArsenalFireProjectile>();
			this.getProjectileNames();
			this.MyButtonText = this.Button.transform.Find("Text").GetComponent<Text>();
			this.MyButtonText.text = this.projectileParticleName;
		}

		// Token: 0x060010E3 RID: 4323 RVA: 0x0002F459 File Offset: 0x0002D659
		private void Update()
		{
			this.MyButtonText.text = this.projectileParticleName;
		}

		// Token: 0x060010E4 RID: 4324 RVA: 0x0002F46C File Offset: 0x0002D66C
		public void getProjectileNames()
		{
			this.projectileScript = this.effectScript.projectiles[this.effectScript.currentProjectile].GetComponent<PixelArsenalProjectileScript>();
			this.projectileParticleName = this.projectileScript.projectileParticle.name;
		}

		// Token: 0x060010E5 RID: 4325 RVA: 0x0002F4A8 File Offset: 0x0002D6A8
		public bool overButton()
		{
			Rect rect = new Rect(this.buttonsX, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
			Rect rect2 = new Rect(this.buttonsX + this.buttonsDistance, this.buttonsY, this.buttonsSizeX, this.buttonsSizeY);
			return rect.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y)) || rect2.Contains(new Vector2(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y));
		}

		// Token: 0x0400090B RID: 2315
		public GameObject Button;

		// Token: 0x0400090C RID: 2316
		private Text MyButtonText;

		// Token: 0x0400090D RID: 2317
		private string projectileParticleName;

		// Token: 0x0400090E RID: 2318
		private PixelArsenalFireProjectile effectScript;

		// Token: 0x0400090F RID: 2319
		private PixelArsenalProjectileScript projectileScript;

		// Token: 0x04000910 RID: 2320
		public float buttonsX;

		// Token: 0x04000911 RID: 2321
		public float buttonsY;

		// Token: 0x04000912 RID: 2322
		public float buttonsSizeX;

		// Token: 0x04000913 RID: 2323
		public float buttonsSizeY;

		// Token: 0x04000914 RID: 2324
		public float buttonsDistance;
	}
}
