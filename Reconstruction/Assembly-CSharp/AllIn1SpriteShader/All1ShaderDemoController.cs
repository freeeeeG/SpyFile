using System;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader
{
	// Token: 0x020002C3 RID: 707
	public class All1ShaderDemoController : MonoBehaviour
	{
		// Token: 0x06001141 RID: 4417 RVA: 0x00031280 File Offset: 0x0002F480
		private void Start()
		{
			this.currExpositor = 0;
			this.SetExpositorText();
			for (int i = 0; i < this.expositors.Length; i++)
			{
				this.expositors[i].transform.position = new Vector3(0f, this.expositorDistance * (float)i, 0f);
			}
			this.backgroundMat = this.background.GetComponent<Image>().material;
			this.targetColors = new Color[4];
			this.targetColors[0] = this.backgroundMat.GetColor("_GradTopLeftCol");
			this.targetColors[1] = this.backgroundMat.GetColor("_GradTopRightCol");
			this.targetColors[2] = this.backgroundMat.GetColor("_GradBotLeftCol");
			this.targetColors[3] = this.backgroundMat.GetColor("_GradBotRightCol");
			this.currentColors = (this.targetColors.Clone() as Color[]);
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00031380 File Offset: 0x0002F580
		private void Update()
		{
			this.GetInput();
			this.currentColors[0] = Color.Lerp(this.currentColors[0], this.targetColors[this.currExpositor % this.targetColors.Length], this.colorLerpSpeed * Time.deltaTime);
			this.currentColors[1] = Color.Lerp(this.currentColors[1], this.targetColors[(1 + this.currExpositor) % this.targetColors.Length], this.colorLerpSpeed * Time.deltaTime);
			this.currentColors[2] = Color.Lerp(this.currentColors[2], this.targetColors[(2 + this.currExpositor) % this.targetColors.Length], this.colorLerpSpeed * Time.deltaTime);
			this.currentColors[3] = Color.Lerp(this.currentColors[3], this.targetColors[(3 + this.currExpositor) % this.targetColors.Length], this.colorLerpSpeed * Time.deltaTime);
			this.backgroundMat.SetColor("_GradTopLeftCol", this.currentColors[0]);
			this.backgroundMat.SetColor("_GradTopRightCol", this.currentColors[1]);
			this.backgroundMat.SetColor("_GradBotLeftCol", this.currentColors[2]);
			this.backgroundMat.SetColor("_GradBotRightCol", this.currentColors[3]);
		}

		// Token: 0x06001143 RID: 4419 RVA: 0x00031518 File Offset: 0x0002F718
		private void GetInput()
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
			{
				this.expositors[this.currExpositor].ChangeTarget(-1);
				return;
			}
			if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
			{
				this.expositors[this.currExpositor].ChangeTarget(1);
				return;
			}
			if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
			{
				this.ChangeExpositor(-1);
				return;
			}
			if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
			{
				this.ChangeExpositor(1);
			}
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x000315B0 File Offset: 0x0002F7B0
		private void ChangeExpositor(int offset)
		{
			this.currExpositor += offset;
			if (this.currExpositor > this.expositors.Length - 1)
			{
				this.currExpositor = 0;
			}
			else if (this.currExpositor < 0)
			{
				this.currExpositor = this.expositors.Length - 1;
			}
			this.SetExpositorText();
		}

		// Token: 0x06001145 RID: 4421 RVA: 0x00031605 File Offset: 0x0002F805
		private void SetExpositorText()
		{
			this.expositorsTitle.text = this.expositors[this.currExpositor].name;
			this.expositorsTitleOutline.text = this.expositors[this.currExpositor].name;
		}

		// Token: 0x06001146 RID: 4422 RVA: 0x00031641 File Offset: 0x0002F841
		public int GetCurrExpositor()
		{
			return this.currExpositor;
		}

		// Token: 0x0400098A RID: 2442
		[SerializeField]
		private DemoCircleExpositor[] expositors;

		// Token: 0x0400098B RID: 2443
		[SerializeField]
		private Text expositorsTitle;

		// Token: 0x0400098C RID: 2444
		[SerializeField]
		private Text expositorsTitleOutline;

		// Token: 0x0400098D RID: 2445
		public float expositorDistance;

		// Token: 0x0400098E RID: 2446
		private int currExpositor;

		// Token: 0x0400098F RID: 2447
		[SerializeField]
		private GameObject background;

		// Token: 0x04000990 RID: 2448
		private Material backgroundMat;

		// Token: 0x04000991 RID: 2449
		[SerializeField]
		private float colorLerpSpeed;

		// Token: 0x04000992 RID: 2450
		private Color[] targetColors;

		// Token: 0x04000993 RID: 2451
		private Color[] currentColors;
	}
}
