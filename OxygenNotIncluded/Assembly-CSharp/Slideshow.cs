using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C68 RID: 3176
[AddComponentMenu("KMonoBehaviour/scripts/Slideshow")]
public class Slideshow : KMonoBehaviour
{
	// Token: 0x06006516 RID: 25878 RVA: 0x00258BE0 File Offset: 0x00256DE0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.timeUntilNextSlide = this.timePerSlide;
		if (this.transparentIfEmpty && this.sprites != null && this.sprites.Length == 0)
		{
			this.imageTarget.color = Color.clear;
		}
		if (this.isExpandable)
		{
			this.button = base.GetComponent<KButton>();
			this.button.onClick += delegate()
			{
				if (this.onBeforePlay != null)
				{
					this.onBeforePlay();
				}
				SlideshowUpdateType slideshowUpdateType = this.updateType;
				if (slideshowUpdateType == SlideshowUpdateType.preloadedSprites)
				{
					VideoScreen.Instance.PlaySlideShow(this.sprites);
					return;
				}
				if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
				{
					return;
				}
				VideoScreen.Instance.PlaySlideShow(this.files);
			};
		}
		if (this.nextButton != null)
		{
			this.nextButton.onClick += delegate()
			{
				this.nextSlide();
			};
		}
		if (this.prevButton != null)
		{
			this.prevButton.onClick += delegate()
			{
				this.prevSlide();
			};
		}
		if (this.pauseButton != null)
		{
			this.pauseButton.onClick += delegate()
			{
				this.SetPaused(!this.paused);
			};
		}
		if (this.closeButton != null)
		{
			this.closeButton.onClick += delegate()
			{
				VideoScreen.Instance.Stop();
				if (this.onEndingPlay != null)
				{
					this.onEndingPlay();
				}
			};
		}
	}

	// Token: 0x06006517 RID: 25879 RVA: 0x00258CE8 File Offset: 0x00256EE8
	public void SetPaused(bool state)
	{
		this.paused = state;
		if (this.pauseIcon != null)
		{
			this.pauseIcon.gameObject.SetActive(!this.paused);
		}
		if (this.unpauseIcon != null)
		{
			this.unpauseIcon.gameObject.SetActive(this.paused);
		}
		if (this.prevButton != null)
		{
			this.prevButton.gameObject.SetActive(this.paused);
		}
		if (this.nextButton != null)
		{
			this.nextButton.gameObject.SetActive(this.paused);
		}
	}

	// Token: 0x06006518 RID: 25880 RVA: 0x00258D90 File Offset: 0x00256F90
	private void resetSlide(bool enable)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		this.currentSlide = 0;
		if (enable)
		{
			this.imageTarget.color = Color.white;
			return;
		}
		if (this.transparentIfEmpty)
		{
			this.imageTarget.color = Color.clear;
		}
	}

	// Token: 0x06006519 RID: 25881 RVA: 0x00258DDC File Offset: 0x00256FDC
	private Sprite loadSlide(string file)
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		Texture2D texture2D = new Texture2D(512, 768);
		texture2D.filterMode = FilterMode.Point;
		texture2D.LoadImage(File.ReadAllBytes(file));
		return Sprite.Create(texture2D, new Rect(Vector2.zero, new Vector2((float)texture2D.width, (float)texture2D.height)), new Vector2(0.5f, 0.5f), 100f, 0U, SpriteMeshType.FullRect);
	}

	// Token: 0x0600651A RID: 25882 RVA: 0x00258E4C File Offset: 0x0025704C
	public void SetFiles(string[] files, int loadFrame = -1)
	{
		if (files == null)
		{
			return;
		}
		this.files = files;
		bool flag = files.Length != 0 && files[0] != null;
		this.resetSlide(flag);
		if (flag)
		{
			int num = (loadFrame != -1) ? loadFrame : (files.Length - 1);
			string file = files[num];
			Sprite slide = this.loadSlide(file);
			this.setSlide(slide);
			this.currentSlideImage = slide;
		}
	}

	// Token: 0x0600651B RID: 25883 RVA: 0x00258EA4 File Offset: 0x002570A4
	public void updateSize(Sprite sprite)
	{
		Vector2 fittedSize = this.GetFittedSize(sprite, 960f, 960f);
		base.GetComponent<RectTransform>().sizeDelta = fittedSize;
	}

	// Token: 0x0600651C RID: 25884 RVA: 0x00258ECF File Offset: 0x002570CF
	public void SetSprites(Sprite[] sprites)
	{
		if (sprites == null)
		{
			return;
		}
		this.sprites = sprites;
		this.resetSlide(sprites.Length != 0 && sprites[0] != null);
		if (sprites.Length != 0 && sprites[0] != null)
		{
			this.setSlide(sprites[0]);
		}
	}

	// Token: 0x0600651D RID: 25885 RVA: 0x00258F0C File Offset: 0x0025710C
	public Vector2 GetFittedSize(Sprite sprite, float maxWidth, float maxHeight)
	{
		if (sprite == null || sprite.texture == null)
		{
			return Vector2.zero;
		}
		int width = sprite.texture.width;
		int height = sprite.texture.height;
		float num = maxWidth / (float)width;
		float num2 = maxHeight / (float)height;
		if (num < num2)
		{
			return new Vector2((float)width * num, (float)height * num);
		}
		return new Vector2((float)width * num2, (float)height * num2);
	}

	// Token: 0x0600651E RID: 25886 RVA: 0x00258F77 File Offset: 0x00257177
	public void setSlide(Sprite slide)
	{
		if (slide == null)
		{
			return;
		}
		this.imageTarget.texture = slide.texture;
		this.updateSize(slide);
	}

	// Token: 0x0600651F RID: 25887 RVA: 0x00258F9B File Offset: 0x0025719B
	public void nextSlide()
	{
		this.setSlideIndex(this.currentSlide + 1);
	}

	// Token: 0x06006520 RID: 25888 RVA: 0x00258FAB File Offset: 0x002571AB
	public void prevSlide()
	{
		this.setSlideIndex(this.currentSlide - 1);
	}

	// Token: 0x06006521 RID: 25889 RVA: 0x00258FBC File Offset: 0x002571BC
	private void setSlideIndex(int slideIndex)
	{
		this.timeUntilNextSlide = this.timePerSlide;
		SlideshowUpdateType slideshowUpdateType = this.updateType;
		if (slideshowUpdateType != SlideshowUpdateType.preloadedSprites)
		{
			if (slideshowUpdateType != SlideshowUpdateType.loadOnDemand)
			{
				return;
			}
			if (slideIndex < 0)
			{
				slideIndex = this.files.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.files.Length;
			if (this.currentSlide == this.files.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				if (this.currentSlideImage != null)
				{
					UnityEngine.Object.Destroy(this.currentSlideImage.texture);
					UnityEngine.Object.Destroy(this.currentSlideImage);
					GC.Collect();
				}
				this.currentSlideImage = this.loadSlide(this.files[this.currentSlide]);
				this.setSlide(this.currentSlideImage);
			}
		}
		else
		{
			if (slideIndex < 0)
			{
				slideIndex = this.sprites.Length + slideIndex;
			}
			this.currentSlide = slideIndex % this.sprites.Length;
			if (this.currentSlide == this.sprites.Length - 1)
			{
				this.timeUntilNextSlide *= this.timeFactorForLastSlide;
			}
			if (this.playInThumbnail)
			{
				this.setSlide(this.sprites[this.currentSlide]);
				return;
			}
		}
	}

	// Token: 0x06006522 RID: 25890 RVA: 0x002590E8 File Offset: 0x002572E8
	private void Update()
	{
		if (this.updateType == SlideshowUpdateType.preloadedSprites && (this.sprites == null || this.sprites.Length == 0))
		{
			return;
		}
		if (this.updateType == SlideshowUpdateType.loadOnDemand && (this.files == null || this.files.Length == 0))
		{
			return;
		}
		if (this.paused)
		{
			return;
		}
		this.timeUntilNextSlide -= Time.unscaledDeltaTime;
		if (this.timeUntilNextSlide <= 0f)
		{
			this.nextSlide();
		}
	}

	// Token: 0x04004553 RID: 17747
	public RawImage imageTarget;

	// Token: 0x04004554 RID: 17748
	private string[] files;

	// Token: 0x04004555 RID: 17749
	private Sprite currentSlideImage;

	// Token: 0x04004556 RID: 17750
	private Sprite[] sprites;

	// Token: 0x04004557 RID: 17751
	public float timePerSlide = 1f;

	// Token: 0x04004558 RID: 17752
	public float timeFactorForLastSlide = 3f;

	// Token: 0x04004559 RID: 17753
	private int currentSlide;

	// Token: 0x0400455A RID: 17754
	private float timeUntilNextSlide;

	// Token: 0x0400455B RID: 17755
	private bool paused;

	// Token: 0x0400455C RID: 17756
	public bool playInThumbnail;

	// Token: 0x0400455D RID: 17757
	public SlideshowUpdateType updateType;

	// Token: 0x0400455E RID: 17758
	[SerializeField]
	private bool isExpandable;

	// Token: 0x0400455F RID: 17759
	[SerializeField]
	private KButton button;

	// Token: 0x04004560 RID: 17760
	[SerializeField]
	private bool transparentIfEmpty = true;

	// Token: 0x04004561 RID: 17761
	[SerializeField]
	private KButton closeButton;

	// Token: 0x04004562 RID: 17762
	[SerializeField]
	private KButton prevButton;

	// Token: 0x04004563 RID: 17763
	[SerializeField]
	private KButton nextButton;

	// Token: 0x04004564 RID: 17764
	[SerializeField]
	private KButton pauseButton;

	// Token: 0x04004565 RID: 17765
	[SerializeField]
	private Image pauseIcon;

	// Token: 0x04004566 RID: 17766
	[SerializeField]
	private Image unpauseIcon;

	// Token: 0x04004567 RID: 17767
	public Slideshow.onBeforeAndEndPlayDelegate onBeforePlay;

	// Token: 0x04004568 RID: 17768
	public Slideshow.onBeforeAndEndPlayDelegate onEndingPlay;

	// Token: 0x02001B9E RID: 7070
	// (Invoke) Token: 0x06009A87 RID: 39559
	public delegate void onBeforeAndEndPlayDelegate();
}
