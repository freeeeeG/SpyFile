using System;
using System.Collections;
using System.Linq;
using Characters.Actions.Constraints;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions
{
	// Token: 0x02000943 RID: 2371
	public class Motion : MonoBehaviour
	{
		// Token: 0x1400008D RID: 141
		// (add) Token: 0x060032EF RID: 13039 RVA: 0x000974D4 File Offset: 0x000956D4
		// (remove) Token: 0x060032F0 RID: 13040 RVA: 0x0009750C File Offset: 0x0009570C
		public event Action onStart;

		// Token: 0x1400008E RID: 142
		// (add) Token: 0x060032F1 RID: 13041 RVA: 0x00097544 File Offset: 0x00095744
		// (remove) Token: 0x060032F2 RID: 13042 RVA: 0x0009757C File Offset: 0x0009577C
		public event Action onApply;

		// Token: 0x1400008F RID: 143
		// (add) Token: 0x060032F3 RID: 13043 RVA: 0x000975B4 File Offset: 0x000957B4
		// (remove) Token: 0x060032F4 RID: 13044 RVA: 0x000975EC File Offset: 0x000957EC
		public event Action onEnd;

		// Token: 0x14000090 RID: 144
		// (add) Token: 0x060032F5 RID: 13045 RVA: 0x00097624 File Offset: 0x00095824
		// (remove) Token: 0x060032F6 RID: 13046 RVA: 0x0009765C File Offset: 0x0009585C
		public event Action onCancel;

		// Token: 0x17000AFA RID: 2810
		// (get) Token: 0x060032F7 RID: 13047 RVA: 0x00097691 File Offset: 0x00095891
		public Character owner
		{
			get
			{
				return this.action.owner;
			}
		}

		// Token: 0x17000AFB RID: 2811
		// (get) Token: 0x060032F8 RID: 13048 RVA: 0x0009769E File Offset: 0x0009589E
		// (set) Token: 0x060032F9 RID: 13049 RVA: 0x000976A6 File Offset: 0x000958A6
		public Action action { get; set; }

		// Token: 0x17000AFC RID: 2812
		// (get) Token: 0x060032FA RID: 13050 RVA: 0x000976AF File Offset: 0x000958AF
		public string key
		{
			get
			{
				return this._key;
			}
		}

		// Token: 0x17000AFD RID: 2813
		// (get) Token: 0x060032FB RID: 13051 RVA: 0x000976B7 File Offset: 0x000958B7
		// (set) Token: 0x060032FC RID: 13052 RVA: 0x000976BF File Offset: 0x000958BF
		public CharacterAnimationController.AnimationInfo animationInfo
		{
			get
			{
				return this._animationInfo;
			}
			set
			{
				this._animationInfo = this.animationInfo;
			}
		}

		// Token: 0x17000AFE RID: 2814
		// (get) Token: 0x060032FD RID: 13053 RVA: 0x000976CD File Offset: 0x000958CD
		public bool blockMovement
		{
			get
			{
				return this._blockMovement;
			}
		}

		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x060032FE RID: 13054 RVA: 0x000976D5 File Offset: 0x000958D5
		public bool blockLook
		{
			get
			{
				return this._blockLook;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x060032FF RID: 13055 RVA: 0x000976DD File Offset: 0x000958DD
		public bool stay
		{
			get
			{
				return this._stay;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x06003300 RID: 13056 RVA: 0x000976E5 File Offset: 0x000958E5
		// (set) Token: 0x06003301 RID: 13057 RVA: 0x000976ED File Offset: 0x000958ED
		public float length { get; set; }

		// Token: 0x17000B02 RID: 2818
		// (get) Token: 0x06003302 RID: 13058 RVA: 0x000976F6 File Offset: 0x000958F6
		public float speed
		{
			get
			{
				return this._speed;
			}
		}

		// Token: 0x17000B03 RID: 2819
		// (get) Token: 0x06003303 RID: 13059 RVA: 0x000976FE File Offset: 0x000958FE
		// (set) Token: 0x06003304 RID: 13060 RVA: 0x00097706 File Offset: 0x00095906
		public float time { get; protected set; }

		// Token: 0x17000B04 RID: 2820
		// (get) Token: 0x06003305 RID: 13061 RVA: 0x0009770F File Offset: 0x0009590F
		public float normalizedTime
		{
			get
			{
				return this.time / this.length;
			}
		}

		// Token: 0x17000B05 RID: 2821
		// (get) Token: 0x06003306 RID: 13062 RVA: 0x0009771E File Offset: 0x0009591E
		public Motion.SpeedMultiplierSource speedMultiplierSource
		{
			get
			{
				return this._speedMultiplierSource;
			}
		}

		// Token: 0x17000B06 RID: 2822
		// (get) Token: 0x06003307 RID: 13063 RVA: 0x00097726 File Offset: 0x00095926
		public float speedMultiplierFactor
		{
			get
			{
				return this._speedMultiplierFactor;
			}
		}

		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06003308 RID: 13064 RVA: 0x0009772E File Offset: 0x0009592E
		// (set) Token: 0x06003309 RID: 13065 RVA: 0x00097736 File Offset: 0x00095936
		public bool running { get; set; }

		// Token: 0x0600330A RID: 13066 RVA: 0x0009773F File Offset: 0x0009593F
		public static Motion AddComponent(GameObject gameObject, Action action, bool blockLook, bool blockMovement)
		{
			Motion motion = gameObject.AddComponent<Motion>();
			motion.action = action;
			motion._blockLook = blockLook;
			motion._blockMovement = blockMovement;
			motion._constraints = new Constraint.Subcomponents();
			motion.Initialize(action);
			return motion;
		}

		// Token: 0x0600330B RID: 13067 RVA: 0x00097770 File Offset: 0x00095970
		private void Awake()
		{
			if (this._animationInfo == null)
			{
				this._animationInfo = new CharacterAnimationController.AnimationInfo(Array.Empty<CharacterAnimationController.AnimationInfo.KeyClip>());
			}
			if (this._operations == null)
			{
				this._operationInfos = new OperationInfo[0];
			}
			else if (this._action == null)
			{
				this._operationInfos = (from operation in this._operations.components
				orderby operation.timeToTrigger
				select operation).ToArray<OperationInfo>();
			}
			else
			{
				this._operationInfos = (from operation in this._action.operations.Concat(this._operations.components)
				orderby operation.timeToTrigger
				select operation).ToArray<OperationInfo>();
			}
			this.length = this.CaculateRealLength();
			for (int i = 0; i < this._operationInfos.Length; i++)
			{
				this._operationInfos[i].operation.Initialize();
			}
		}

		// Token: 0x0600330C RID: 13068 RVA: 0x00097874 File Offset: 0x00095A74
		private void OnDestroy()
		{
			this._animationInfo.Dispose();
			this._action = null;
			for (int i = 0; i < this._operationInfos.Length; i++)
			{
				this._operationInfos[i] = null;
			}
			this._operationInfos = null;
			this._operations = null;
		}

		// Token: 0x0600330D RID: 13069 RVA: 0x000978BD File Offset: 0x00095ABD
		private void OnDisable()
		{
			this.CancelBehaviour();
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x000978C8 File Offset: 0x00095AC8
		private void StopAllOperations()
		{
			for (int i = 0; i < this._operationInfos.Length; i++)
			{
				if (!this._operationInfos[i].stay)
				{
					this._operationInfos[i].operation.Stop();
				}
			}
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x0009790C File Offset: 0x00095B0C
		public void Initialize(Action action)
		{
			this.action = action;
			for (int i = 0; i < this._constraints.components.Length; i++)
			{
				this._constraints.components[i].Initilaize(action);
			}
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x0009794C File Offset: 0x00095B4C
		public float CaculateRealLength()
		{
			if (this._length > 0f)
			{
				return this._length;
			}
			float num = 0f;
			for (int i = 0; i < this._animationInfo.values.Length; i++)
			{
				if (this._animationInfo.values[i].clip != null && this._animationInfo.values[i].clip.length > num)
				{
					num = this._animationInfo.values[i].clip.length;
				}
			}
			return num;
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x000979D8 File Offset: 0x00095BD8
		public void StartBehaviour(float speed)
		{
			if (this.running)
			{
				return;
			}
			this._runSpeed = speed;
			this.running = true;
			Action action = this.onStart;
			if (action != null)
			{
				action();
			}
			this._cRunOperations = this.StartCoroutineWithReference(this.CRunOperations());
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x00097A14 File Offset: 0x00095C14
		public void EndBehaviour()
		{
			if (!this.running)
			{
				return;
			}
			this.time = this.length;
			this.StopAllOperations();
			this.running = false;
			this._cRunOperations.Stop();
			Action action = this.onEnd;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x00097A53 File Offset: 0x00095C53
		public void CancelBehaviour()
		{
			if (!this.running)
			{
				return;
			}
			this.StopAllOperations();
			this.running = false;
			this._cRunOperations.Stop();
			Action action = this.onCancel;
			if (action == null)
			{
				return;
			}
			action();
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x00097A88 File Offset: 0x00095C88
		public override string ToString()
		{
			if (this._animationInfo == null || this._animationInfo.values == null || this._animationInfo.values.Length == 0 || this._animationInfo.values[0].clip == null)
			{
				return base.ToString();
			}
			return "Motion (" + this._animationInfo.values[0].clip.name + ")";
		}

		// Token: 0x06003315 RID: 13077 RVA: 0x00097AFF File Offset: 0x00095CFF
		public IEnumerator CWaitForEndOfRunning()
		{
			while (this.running)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x06003316 RID: 13078 RVA: 0x00097B0E File Offset: 0x00095D0E
		private IEnumerator CRunOperations()
		{
			int operationIndex = 0;
			this.time = 0f;
			for (;;)
			{
				if (operationIndex >= this._operationInfos.Length || this.time < this._operationInfos[operationIndex].timeToTrigger)
				{
					yield return new WaitForEndOfFrame();
					this.time += this.owner.chronometer.animation.deltaTime * this._runSpeed;
				}
				else
				{
					this._operationInfos[operationIndex].operation.runSpeed = this._runSpeed;
					this._operationInfos[operationIndex].operation.Run(this.owner);
					int num = operationIndex;
					operationIndex = num + 1;
				}
			}
			yield break;
		}

		// Token: 0x06003317 RID: 13079 RVA: 0x00097B1D File Offset: 0x00095D1D
		internal bool PassConstraints()
		{
			return this._constraints.components.Pass();
		}

		// Token: 0x06003318 RID: 13080 RVA: 0x00097B2F File Offset: 0x00095D2F
		internal void ConsumeConstraints()
		{
			this.action.ConsumeConstraints();
			this._constraints.components.Consume();
		}

		// Token: 0x04002984 RID: 10628
		[SerializeField]
		[GetComponentInParent(true)]
		private Action _action;

		// Token: 0x04002985 RID: 10629
		[SerializeField]
		private string _key;

		// Token: 0x04002986 RID: 10630
		[SerializeField]
		private CharacterAnimationController.AnimationInfo _animationInfo;

		// Token: 0x04002987 RID: 10631
		[SerializeField]
		[Constraint.SubcomponentAttribute]
		private Constraint.Subcomponents _constraints;

		// Token: 0x04002988 RID: 10632
		[SerializeField]
		[Information("재생 중 이동불가 여부", InformationAttribute.InformationType.Info, false)]
		private bool _blockMovement = true;

		// Token: 0x04002989 RID: 10633
		[Information("재생 중 방향 변경 불가 여부", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private bool _blockLook = true;

		// Token: 0x0400298A RID: 10634
		[SerializeField]
		[Information("체크시 재생이 끝난 후 마지막 프레임 상태로 유지, 반복되는 애니메이션은 반복, 지속 시간은 Length를 따라감", InformationAttribute.InformationType.Info, false)]
		private bool _stay;

		// Token: 0x0400298B RID: 10635
		[SerializeField]
		[Information("애니메이션 재생 시간, 0이면 CharacterBody 애니메이션의 길이만큼 지속", InformationAttribute.InformationType.Info, false)]
		private float _length;

		// Token: 0x0400298C RID: 10636
		[SerializeField]
		[Information("애니메이션 재생 속도", InformationAttribute.InformationType.Info, false)]
		private float _speed = 1f;

		// Token: 0x0400298D RID: 10637
		[Information("어떤 속도 스탯을 사용할지", InformationAttribute.InformationType.Info, false)]
		[SerializeField]
		private Motion.SpeedMultiplierSource _speedMultiplierSource;

		// Token: 0x0400298E RID: 10638
		[SerializeField]
		[Range(0f, 1f)]
		[Information("속도 스탯에 영향받는 정도", InformationAttribute.InformationType.Info, false)]
		private float _speedMultiplierFactor = 1f;

		// Token: 0x0400298F RID: 10639
		private float _runSpeed;

		// Token: 0x04002990 RID: 10640
		[Subcomponent(typeof(OperationInfo))]
		[SerializeField]
		private OperationInfo.Subcomponents _operations;

		// Token: 0x04002991 RID: 10641
		private OperationInfo[] _operationInfos;

		// Token: 0x04002992 RID: 10642
		private CoroutineReference _cRunOperations;

		// Token: 0x02000944 RID: 2372
		public enum SpeedMultiplierSource
		{
			// Token: 0x0400299C RID: 10652
			Default,
			// Token: 0x0400299D RID: 10653
			ForceBasic,
			// Token: 0x0400299E RID: 10654
			ForceSkill,
			// Token: 0x0400299F RID: 10655
			ForceMovement,
			// Token: 0x040029A0 RID: 10656
			ForceCharging,
			// Token: 0x040029A1 RID: 10657
			ForceBasicAndCharging,
			// Token: 0x040029A2 RID: 10658
			ForceSkillAndCharging
		}

		// Token: 0x02000945 RID: 2373
		[Serializable]
		public class Subcomponents : SubcomponentArray<Motion>
		{
			// Token: 0x0600331A RID: 13082 RVA: 0x00097B78 File Offset: 0x00095D78
			public void EndBehaviour()
			{
				Motion[] components = base.components;
				for (int i = 0; i < components.Length; i++)
				{
					components[i].EndBehaviour();
				}
			}
		}
	}
}
