using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace TwoDLaserPack
{
	// Token: 0x0200165D RID: 5725
	public class LaserSettingsDemoScene : MonoBehaviour
	{
		// Token: 0x06006D1A RID: 27930 RVA: 0x00137E3C File Offset: 0x0013603C
		private void Start()
		{
			if (this.LineBasedLaser == null)
			{
				Debug.LogError("You need to reference a valid LineBasedLaser on this script.");
			}
			this.toggleisActive.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserActiveChanged));
			this.toggleignoreCollisions.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserToggleCollisionsChanged));
			this.togglelaserRotationEnabled.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserAllowRotationChanged));
			this.togglelerpLaserRotation.onValueChanged.AddListener(new UnityAction<bool>(this.OnLaserLerpRotationChanged));
			this.toggleuseArc.onValueChanged.AddListener(new UnityAction<bool>(this.OnUseArcValueChanged));
			this.toggleTargetMouse.onValueChanged.AddListener(new UnityAction<bool>(this.OnToggleFollowMouse));
			this.slidertexOffsetSpeed.onValueChanged.AddListener(new UnityAction<float>(this.OnTextureOffsetSpeedChanged));
			this.sliderlaserArcMaxYDown.onValueChanged.AddListener(new UnityAction<float>(this.OnArcMaxYDownValueChanged));
			this.sliderlaserArcMaxYUp.onValueChanged.AddListener(new UnityAction<float>(this.OnArcMaxYUpValueChanged));
			this.slidermaxLaserRaycastDistance.onValueChanged.AddListener(new UnityAction<float>(this.OnLaserRaycastDistanceChanged));
			this.sliderturningRate.onValueChanged.AddListener(new UnityAction<float>(this.OnLaserTurningRateChanged));
			this.buttonSwitch.onClick.AddListener(new UnityAction(this.OnButtonClick));
			this.selectedMaterialIndex = 1;
			this.maxSelectedIndex = this.LaserMaterials.Length - 1;
		}

		// Token: 0x06006D1B RID: 27931 RVA: 0x00137FC8 File Offset: 0x001361C8
		private void OnToggleFollowMouse(bool followMouse)
		{
			this.targetShouldTrackMouse = followMouse;
			if (this.targetShouldTrackMouse)
			{
				this.FollowScript.enabled = false;
				return;
			}
			this.FollowScript.enabled = true;
		}

		// Token: 0x06006D1C RID: 27932 RVA: 0x00137FF4 File Offset: 0x001361F4
		private void OnButtonClick()
		{
			if (this.selectedMaterialIndex < this.maxSelectedIndex)
			{
				this.selectedMaterialIndex++;
				this.LineBasedLaser.laserLineRenderer.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.LineBasedLaser.laserLineRendererArc.material = this.LaserMaterials[this.selectedMaterialIndex];
				this.LineBasedLaser.hitSparkParticleSystem.GetComponent<Renderer>().material = this.LaserMaterials[this.selectedMaterialIndex];
				return;
			}
			this.selectedMaterialIndex = 0;
			this.LineBasedLaser.laserLineRenderer.material = this.LaserMaterials[this.selectedMaterialIndex];
			this.LineBasedLaser.laserLineRendererArc.material = this.LaserMaterials[this.selectedMaterialIndex];
			this.LineBasedLaser.hitSparkParticleSystem.GetComponent<Renderer>().material = this.LaserMaterials[this.selectedMaterialIndex];
		}

		// Token: 0x06006D1D RID: 27933 RVA: 0x001380E0 File Offset: 0x001362E0
		private void OnLaserTurningRateChanged(float turningRate)
		{
			this.LineBasedLaser.turningRate = turningRate;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser turning rate: " + Math.Round((double)turningRate, 2).ToString();
		}

		// Token: 0x06006D1E RID: 27934 RVA: 0x00138130 File Offset: 0x00136330
		private void OnLaserRaycastDistanceChanged(float raycastDistance)
		{
			this.LineBasedLaser.maxLaserRaycastDistance = raycastDistance;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser raycast max distance: " + Math.Round((double)raycastDistance, 2).ToString();
		}

		// Token: 0x06006D1F RID: 27935 RVA: 0x00138180 File Offset: 0x00136380
		private void OnArcMaxYUpValueChanged(float maxYValueUp)
		{
			this.LineBasedLaser.laserArcMaxYUp = maxYValueUp;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc maximum up arc height: " + Math.Round((double)maxYValueUp, 2).ToString();
		}

		// Token: 0x06006D20 RID: 27936 RVA: 0x001381D0 File Offset: 0x001363D0
		private void OnArcMaxYDownValueChanged(float maxYValueDown)
		{
			this.LineBasedLaser.laserArcMaxYDown = maxYValueDown;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc maximum down arc height: " + Math.Round((double)maxYValueDown, 2).ToString();
		}

		// Token: 0x06006D21 RID: 27937 RVA: 0x00138220 File Offset: 0x00136420
		private void OnTextureOffsetSpeedChanged(float offsetSpeed)
		{
			this.LineBasedLaser.laserTexOffsetSpeed = offsetSpeed;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser texture offset speed: " + Math.Round((double)offsetSpeed, 2).ToString();
		}

		// Token: 0x06006D22 RID: 27938 RVA: 0x00138270 File Offset: 0x00136470
		private void OnUseArcValueChanged(bool useArc)
		{
			this.LineBasedLaser.useArc = useArc;
			this.sliderlaserArcMaxYDown.interactable = useArc;
			this.sliderlaserArcMaxYUp.interactable = useArc;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser arc enabled: " + useArc.ToString();
		}

		// Token: 0x06006D23 RID: 27939 RVA: 0x001382D0 File Offset: 0x001364D0
		private void OnLaserLerpRotationChanged(bool lerpLaserRotation)
		{
			this.LineBasedLaser.lerpLaserRotation = lerpLaserRotation;
			this.sliderturningRate.interactable = lerpLaserRotation;
			this.textValue.color = Color.white;
			this.textValue.text = "Lerp laser rotation: " + lerpLaserRotation.ToString();
		}

		// Token: 0x06006D24 RID: 27940 RVA: 0x00138324 File Offset: 0x00136524
		private void OnLaserAllowRotationChanged(bool allowRotation)
		{
			this.LineBasedLaser.laserRotationEnabled = allowRotation;
			this.togglelerpLaserRotation.interactable = allowRotation;
			this.sliderturningRate.interactable = allowRotation;
			this.textValue.color = Color.white;
			this.textValue.text = "Laser rotation enabled: " + allowRotation.ToString();
		}

		// Token: 0x06006D25 RID: 27941 RVA: 0x00138381 File Offset: 0x00136581
		private void OnLaserToggleCollisionsChanged(bool ignoreCollisions)
		{
			this.LineBasedLaser.ignoreCollisions = ignoreCollisions;
			this.textValue.color = Color.white;
			this.textValue.text = "Ignore laser collisions: " + ignoreCollisions.ToString();
		}

		// Token: 0x06006D26 RID: 27942 RVA: 0x001383BB File Offset: 0x001365BB
		private void OnLaserActiveChanged(bool state)
		{
			this.LineBasedLaser.SetLaserState(state);
			this.textValue.color = Color.white;
			this.textValue.text = "Laser active: " + state.ToString();
		}

		// Token: 0x06006D27 RID: 27943 RVA: 0x001383F8 File Offset: 0x001365F8
		private void Update()
		{
			if (this.targetShouldTrackMouse)
			{
				Vector3 vector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Vector2 v = new Vector2(vector.x, vector.y);
				this.LineBasedLaser.targetGo.transform.position = v;
			}
		}

		// Token: 0x040058DA RID: 22746
		public LineBasedLaser LineBasedLaser;

		// Token: 0x040058DB RID: 22747
		public DemoFollowScript FollowScript;

		// Token: 0x040058DC RID: 22748
		public Toggle toggleisActive;

		// Token: 0x040058DD RID: 22749
		public Toggle toggleignoreCollisions;

		// Token: 0x040058DE RID: 22750
		public Toggle togglelaserRotationEnabled;

		// Token: 0x040058DF RID: 22751
		public Toggle togglelerpLaserRotation;

		// Token: 0x040058E0 RID: 22752
		public Toggle toggleuseArc;

		// Token: 0x040058E1 RID: 22753
		public Toggle toggleTargetMouse;

		// Token: 0x040058E2 RID: 22754
		public Slider slidertexOffsetSpeed;

		// Token: 0x040058E3 RID: 22755
		public Slider sliderlaserArcMaxYDown;

		// Token: 0x040058E4 RID: 22756
		public Slider sliderlaserArcMaxYUp;

		// Token: 0x040058E5 RID: 22757
		public Slider slidermaxLaserRaycastDistance;

		// Token: 0x040058E6 RID: 22758
		public Slider sliderturningRate;

		// Token: 0x040058E7 RID: 22759
		public Button buttonSwitch;

		// Token: 0x040058E8 RID: 22760
		public Text textValue;

		// Token: 0x040058E9 RID: 22761
		public Material[] LaserMaterials;

		// Token: 0x040058EA RID: 22762
		private int selectedMaterialIndex;

		// Token: 0x040058EB RID: 22763
		private int maxSelectedIndex;

		// Token: 0x040058EC RID: 22764
		private bool targetShouldTrackMouse;
	}
}
