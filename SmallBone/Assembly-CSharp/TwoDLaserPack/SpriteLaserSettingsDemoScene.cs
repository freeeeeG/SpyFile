using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	// Token: 0x02001661 RID: 5729
	public class SpriteLaserSettingsDemoScene : MonoBehaviour
	{
		// Token: 0x06006D37 RID: 27959 RVA: 0x00138A38 File Offset: 0x00136C38
		private void Start()
		{
			if (this.SpriteBasedLaser == null)
			{
				Debug.LogError("You need to reference a valid LineBasedLaser on this script.");
			}
			this.toggleisActive.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserActiveChanged));
			this.toggleignoreCollisions.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserToggleCollisionsChanged));
			this.togglelaserRotationEnabled.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserAllowRotationChanged));
			this.togglelerpLaserRotation.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserLerpRotationChanged));
			this.toggleuseArc.onValueChanged.AddListener(new UnityAction<bool>(this.OnUseArcValueChanged));
			this.toggleOscillateLaser.onValueChanged.AddListener(new UnityAction<bool>(this.OnOscillateLaserChanged));
			this.sliderlaserArcMaxYDown.onValueChanged.AddListener(new UnityAction<float>(this.OnArcMaxYDownValueChanged));
			this.sliderlaserArcMaxYUp.onValueChanged.AddListener(new UnityAction<float>(this.OnArcMaxYUpValueChanged));
			this.slidermaxLaserRaycastDistance.onValueChanged.AddListener(new UnityAction<float>(this.OnLaserRaycastDistanceChanged));
			this.sliderturningRate.onValueChanged.AddListener(new UnityAction<float>(this.OnLaserTurningRateChanged));
			this.sliderOscillationThreshold.onValueChanged.AddListener(new UnityAction<float>(this.OnOscillationThresholdChanged));
			this.sliderOscillationSpeed.onValueChanged.AddListener(new UnityAction<float>(this.OnOscillationSpeedChanged));
			this.buttonSwitch.onClick.AddListener(new UnityAction(this.OnButtonClick));
			this.selectedMaterialIndex = 1;
			this.maxSelectedIndex = this.LaserMaterials.Length - 1;
		}

		// Token: 0x06006D38 RID: 27960 RVA: 0x00138BE0 File Offset: 0x00136DE0
		private void OnOscillationSpeedChanged(float oscillationSpeed)
		{
			this.SpriteBasedLaser.oscillationSpeed = oscillationSpeed;
		}

		// Token: 0x06006D39 RID: 27961 RVA: 0x00138BEE File Offset: 0x00136DEE
		private void OnOscillationThresholdChanged(float oscillationThreshold)
		{
			this.SpriteBasedLaser.oscillationThreshold = oscillationThreshold;
		}

		// Token: 0x06006D3A RID: 27962 RVA: 0x00138BFC File Offset: 0x00136DFC
		private void OnOscillateLaserChanged(bool oscillateLaser)
		{
			this.SpriteBasedLaser.oscillateLaser = oscillateLaser;
		}

		// Token: 0x06006D3B RID: 27963 RVA: 0x00138C0C File Offset: 0x00136E0C
		private void OnButtonClick()
		{
			if (this.selectedMaterialIndex < this.maxSelectedIndex)
			{
				this.selectedMaterialIndex++;
				this.SpriteBasedLaser.laserLineRendererArc.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.SpriteBasedLaser.hitSparkParticleSystem.GetComponent<Renderer>().material = this.LaserMaterials[this.selectedMaterialIndex];
				this.SpriteBasedLaser.laserStartPiece = this.laserStartPieceRed;
				this.SpriteBasedLaser.laserMiddlePiece = this.laserMidPieceRed;
				this.SpriteBasedLaser.laserEndPiece = this.laserEndPieceRed;
			}
			else
			{
				this.selectedMaterialIndex = 0;
				this.SpriteBasedLaser.laserLineRendererArc.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.SpriteBasedLaser.hitSparkParticleSystem.GetComponent<Renderer>().material = this.LaserMaterials[this.selectedMaterialIndex];
				this.SpriteBasedLaser.laserStartPiece = this.laserStartPieceBlue;
				this.SpriteBasedLaser.laserMiddlePiece = this.laserMidPieceBlue;
				this.SpriteBasedLaser.laserEndPiece = this.laserEndPieceBlue;
			}
			this.SpriteBasedLaser.DisableLaserGameObjectComponents();
		}

		// Token: 0x06006D3C RID: 27964 RVA: 0x00138D30 File Offset: 0x00136F30
		private void OnLaserTurningRateChanged(float turningRate)
		{
			this.SpriteBasedLaser.turningRate = turningRate;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser turning rate: " + Math.Round((double)turningRate, 2).ToString();
		}

		// Token: 0x06006D3D RID: 27965 RVA: 0x00138D80 File Offset: 0x00136F80
		private void OnLaserRaycastDistanceChanged(float raycastDistance)
		{
			this.SpriteBasedLaser.maxLaserRaycastDistance = raycastDistance;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser raycast max distance: " + Math.Round((double)raycastDistance, 2).ToString();
		}

		// Token: 0x06006D3E RID: 27966 RVA: 0x00138DD0 File Offset: 0x00136FD0
		private void OnArcMaxYUpValueChanged(float maxYValueUp)
		{
			this.SpriteBasedLaser.laserArcMaxYUp = maxYValueUp;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc maximum up arc height: " + Math.Round((double)maxYValueUp, 2).ToString();
		}

		// Token: 0x06006D3F RID: 27967 RVA: 0x00138E20 File Offset: 0x00137020
		private void OnArcMaxYDownValueChanged(float maxYValueDown)
		{
			this.SpriteBasedLaser.laserArcMaxYDown = maxYValueDown;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc maximum down arc height: " + Math.Round((double)maxYValueDown, 2).ToString();
		}

		// Token: 0x06006D40 RID: 27968 RVA: 0x00138E70 File Offset: 0x00137070
		private void OnUseArcValueChanged(bool useArc)
		{
			this.SpriteBasedLaser.useArc = useArc;
			this.sliderlaserArcMaxYDown.interactable = useArc;
			this.sliderlaserArcMaxYUp.interactable = useArc;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc enabled: " + useArc.ToString();
		}

		// Token: 0x06006D41 RID: 27969 RVA: 0x00138ED0 File Offset: 0x001370D0
		private void OnLaserLerpRotationChanged(bool lerpLaserRotation)
		{
			this.SpriteBasedLaser.lerpLaserRotation = lerpLaserRotation;
			this.sliderturningRate.interactable = lerpLaserRotation;
			this.textValue.color = Color.white;
			this.textValue.text = "Lerp laser rotation: " + lerpLaserRotation.ToString();
		}

		// Token: 0x06006D42 RID: 27970 RVA: 0x00138F24 File Offset: 0x00137124
		private void OnLaserAllowRotationChanged(bool allowRotation)
		{
			this.SpriteBasedLaser.laserRotationEnabled = allowRotation;
			this.togglelerpLaserRotation.interactable = allowRotation;
			this.sliderturningRate.interactable = allowRotation;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser rotation enabled: " + allowRotation.ToString();
		}

		// Token: 0x06006D43 RID: 27971 RVA: 0x00138F81 File Offset: 0x00137181
		private void OnLaserToggleCollisionsChanged(bool ignoreCollisions)
		{
			this.SpriteBasedLaser.ignoreCollisions = ignoreCollisions;
			this.textValue.color = Color.white;
			this.textValue.text = "Ignore laser collisions: " + ignoreCollisions.ToString();
		}

		// Token: 0x06006D44 RID: 27972 RVA: 0x00138FBB File Offset: 0x001371BB
		private void OnLaserActiveChanged(bool state)
		{
			this.SpriteBasedLaser.SetLaserState(state);
			this.textValue.color = Color.white;
			this.textValue.text = "Laser active: " + state.ToString();
		}

		// Token: 0x06006D45 RID: 27973 RVA: 0x00002191 File Offset: 0x00000391
		private void Update()
		{
		}

		// Token: 0x04005902 RID: 22786
		public SpriteBasedLaser SpriteBasedLaser;

		// Token: 0x04005903 RID: 22787
		public Toggle toggleisActive;

		// Token: 0x04005904 RID: 22788
		public Toggle toggleignoreCollisions;

		// Token: 0x04005905 RID: 22789
		public Toggle togglelaserRotationEnabled;

		// Token: 0x04005906 RID: 22790
		public Toggle togglelerpLaserRotation;

		// Token: 0x04005907 RID: 22791
		public Toggle toggleuseArc;

		// Token: 0x04005908 RID: 22792
		public Toggle toggleOscillateLaser;

		// Token: 0x04005909 RID: 22793
		public Slider sliderlaserArcMaxYDown;

		// Token: 0x0400590A RID: 22794
		public Slider sliderlaserArcMaxYUp;

		// Token: 0x0400590B RID: 22795
		public Slider slidermaxLaserRaycastDistance;

		// Token: 0x0400590C RID: 22796
		public Slider sliderturningRate;

		// Token: 0x0400590D RID: 22797
		public Slider sliderOscillationThreshold;

		// Token: 0x0400590E RID: 22798
		public Slider sliderOscillationSpeed;

		// Token: 0x0400590F RID: 22799
		public Button buttonSwitch;

		// Token: 0x04005910 RID: 22800
		public Text textValue;

		// Token: 0x04005911 RID: 22801
		public Material[] LaserMaterials;

		// Token: 0x04005912 RID: 22802
		public GameObject laserStartPieceBlue;

		// Token: 0x04005913 RID: 22803
		public GameObject laserStartPieceRed;

		// Token: 0x04005914 RID: 22804
		public GameObject laserMidPieceBlue;

		// Token: 0x04005915 RID: 22805
		public GameObject laserMidPieceRed;

		// Token: 0x04005916 RID: 22806
		public GameObject laserEndPieceBlue;

		// Token: 0x04005917 RID: 22807
		public GameObject laserEndPieceRed;

		// Token: 0x04005918 RID: 22808
		private int selectedMaterialIndex;

		// Token: 0x04005919 RID: 22809
		private int maxSelectedIndex;
	}
}
