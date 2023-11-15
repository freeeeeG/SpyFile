using System;
using UnityEngine;

namespace TwoDLaserPack
{
	// Token: 0x02001657 RID: 5719
	public class BloodEffect : MonoBehaviour
	{
		// Token: 0x06006CFB RID: 27899 RVA: 0x001377E4 File Offset: 0x001359E4
		private void Awake()
		{
			this.sprite = base.gameObject.GetComponent<SpriteRenderer>();
		}

		// Token: 0x06006CFC RID: 27900 RVA: 0x00002191 File Offset: 0x00000391
		private void OnEnable()
		{
		}

		// Token: 0x06006CFD RID: 27901 RVA: 0x001377F8 File Offset: 0x001359F8
		private void OnDisable()
		{
			this.spriteColor = new Color(this.sprite.GetComponent<Renderer>().material.color.r, this.sprite.GetComponent<Renderer>().material.color.g, this.sprite.GetComponent<Renderer>().material.color.b, 1f);
		}

		// Token: 0x06006CFE RID: 27902 RVA: 0x00002191 File Offset: 0x00000391
		private void Start()
		{
		}

		// Token: 0x06006CFF RID: 27903 RVA: 0x00137864 File Offset: 0x00135A64
		private void Update()
		{
			this.elapsedTimeBeforeFadeStarts += Time.deltaTime;
			if (this.elapsedTimeBeforeFadeStarts >= this.timeBeforeFadeStarts)
			{
				this.spriteColor = new Color(this.sprite.GetComponent<Renderer>().material.color.r, this.sprite.GetComponent<Renderer>().material.color.g, this.sprite.GetComponent<Renderer>().material.color.b, Mathf.Lerp(this.sprite.GetComponent<Renderer>().material.color.a, 0f, Time.deltaTime * this.fadespeed));
				this.sprite.GetComponent<Renderer>().material.color = this.spriteColor;
				if (this.sprite.material.color.a <= 0f)
				{
					base.gameObject.SetActive(false);
				}
			}
		}

		// Token: 0x040058C1 RID: 22721
		public float fadespeed = 2f;

		// Token: 0x040058C2 RID: 22722
		public float timeBeforeFadeStarts = 1f;

		// Token: 0x040058C3 RID: 22723
		private float elapsedTimeBeforeFadeStarts;

		// Token: 0x040058C4 RID: 22724
		private SpriteRenderer sprite;

		// Token: 0x040058C5 RID: 22725
		private Color spriteColor;
	}
}
