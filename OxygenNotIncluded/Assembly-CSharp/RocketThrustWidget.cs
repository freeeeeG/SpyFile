using System;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000BDB RID: 3035
[AddComponentMenu("KMonoBehaviour/scripts/RocketThrustWidget")]
public class RocketThrustWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x06005FD6 RID: 24534 RVA: 0x002361C6 File Offset: 0x002343C6
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x06005FD7 RID: 24535 RVA: 0x002361C8 File Offset: 0x002343C8
	public void Draw(CommandModule commandModule)
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
		}
		this.commandModule = commandModule;
		this.totalWidth = this.rectTransform.rect.width;
		this.UpdateGraphDotPos(commandModule);
	}

	// Token: 0x06005FD8 RID: 24536 RVA: 0x00236220 File Offset: 0x00234420
	private void UpdateGraphDotPos(CommandModule rocket)
	{
		this.totalWidth = this.rectTransform.rect.width;
		float num = Mathf.Lerp(0f, this.totalWidth, rocket.rocketStats.GetTotalMass() / this.maxMass);
		num = Mathf.Clamp(num, 0f, this.totalWidth);
		this.graphDot.rectTransform.SetLocalPosition(new Vector3(num, 0f, 0f));
		this.graphDotText.text = "-" + Util.FormatWholeNumber(rocket.rocketStats.GetTotalThrust() - rocket.rocketStats.GetRocketMaxDistance()) + "km";
	}

	// Token: 0x06005FD9 RID: 24537 RVA: 0x002362D4 File Offset: 0x002344D4
	private void Update()
	{
		if (this.mouseOver)
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
			}
			Vector3 position = this.rectTransform.GetPosition();
			Vector2 size = this.rectTransform.rect.size;
			float num = KInputManager.GetMousePos().x - position.x + size.x / 2f;
			num = Mathf.Clamp(num, 0f, this.totalWidth);
			this.hoverMarker.rectTransform.SetLocalPosition(new Vector3(num, 0f, 0f));
			float num2 = Mathf.Lerp(0f, this.maxMass, num / this.totalWidth);
			float totalThrust = this.commandModule.rocketStats.GetTotalThrust();
			float rocketMaxDistance = this.commandModule.rocketStats.GetRocketMaxDistance();
			this.hoverTooltip.SetSimpleTooltip(string.Concat(new string[]
			{
				UI.STARMAP.ROCKETWEIGHT.MASS,
				GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"),
				"\n",
				UI.STARMAP.ROCKETWEIGHT.MASSPENALTY,
				Util.FormatWholeNumber(ROCKETRY.CalculateMassWithPenalty(num2)),
				UI.UNITSUFFIXES.DISTANCE.KILOMETER,
				"\n\n",
				UI.STARMAP.ROCKETWEIGHT.CURRENTMASS,
				GameUtil.GetFormattedMass(this.commandModule.rocketStats.GetTotalMass(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"),
				"\n",
				UI.STARMAP.ROCKETWEIGHT.CURRENTMASSPENALTY,
				Util.FormatWholeNumber(totalThrust - rocketMaxDistance),
				UI.UNITSUFFIXES.DISTANCE.KILOMETER
			}));
		}
	}

	// Token: 0x06005FDA RID: 24538 RVA: 0x0023648D File Offset: 0x0023468D
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOver = true;
		this.hoverMarker.SetAlpha(1f);
	}

	// Token: 0x06005FDB RID: 24539 RVA: 0x002364A6 File Offset: 0x002346A6
	public void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOver = false;
		this.hoverMarker.SetAlpha(0f);
	}

	// Token: 0x04004130 RID: 16688
	public Image graphBar;

	// Token: 0x04004131 RID: 16689
	public Image graphDot;

	// Token: 0x04004132 RID: 16690
	public LocText graphDotText;

	// Token: 0x04004133 RID: 16691
	public Image hoverMarker;

	// Token: 0x04004134 RID: 16692
	public ToolTip hoverTooltip;

	// Token: 0x04004135 RID: 16693
	public RectTransform markersContainer;

	// Token: 0x04004136 RID: 16694
	public Image markerTemplate;

	// Token: 0x04004137 RID: 16695
	private RectTransform rectTransform;

	// Token: 0x04004138 RID: 16696
	private float maxMass = 20000f;

	// Token: 0x04004139 RID: 16697
	private float totalWidth = 5f;

	// Token: 0x0400413A RID: 16698
	private bool mouseOver;

	// Token: 0x0400413B RID: 16699
	public CommandModule commandModule;
}
