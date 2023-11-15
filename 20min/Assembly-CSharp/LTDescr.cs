using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000019 RID: 25
public class LTDescr
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x060000A1 RID: 161 RVA: 0x00008040 File Offset: 0x00006240
	// (set) Token: 0x060000A2 RID: 162 RVA: 0x00008048 File Offset: 0x00006248
	public Vector3 from
	{
		get
		{
			return this.fromInternal;
		}
		set
		{
			this.fromInternal = value;
		}
	}

	// Token: 0x17000002 RID: 2
	// (get) Token: 0x060000A3 RID: 163 RVA: 0x00008051 File Offset: 0x00006251
	// (set) Token: 0x060000A4 RID: 164 RVA: 0x00008059 File Offset: 0x00006259
	public Vector3 to
	{
		get
		{
			return this.toInternal;
		}
		set
		{
			this.toInternal = value;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x060000A5 RID: 165 RVA: 0x00008062 File Offset: 0x00006262
	// (set) Token: 0x060000A6 RID: 166 RVA: 0x0000806A File Offset: 0x0000626A
	public LTDescr.ActionMethodDelegate easeInternal { get; set; }

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x060000A7 RID: 167 RVA: 0x00008073 File Offset: 0x00006273
	// (set) Token: 0x060000A8 RID: 168 RVA: 0x0000807B File Offset: 0x0000627B
	public LTDescr.ActionMethodDelegate initInternal { get; set; }

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x060000A9 RID: 169 RVA: 0x00008084 File Offset: 0x00006284
	public Transform toTrans
	{
		get
		{
			return this.optional.toTrans;
		}
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00008094 File Offset: 0x00006294
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			(this.trans != null) ? ("name:" + this.trans.gameObject.name) : "gameObject:null",
			" toggle:",
			this.toggle.ToString(),
			" passed:",
			this.passed,
			" time:",
			this.time,
			" delay:",
			this.delay,
			" direction:",
			this.direction,
			" from:",
			this.from,
			" to:",
			this.to,
			" diff:",
			this.diff,
			" type:",
			this.type,
			" ease:",
			this.easeType,
			" useEstimatedTime:",
			this.useEstimatedTime.ToString(),
			" id:",
			this.id,
			" hasInitiliazed:",
			this.hasInitiliazed.ToString()
		});
	}

	// Token: 0x060000AC RID: 172 RVA: 0x00008229 File Offset: 0x00006429
	[Obsolete("Use 'LeanTween.cancel( id )' instead")]
	public LTDescr cancel(GameObject gameObject)
	{
		if (gameObject == this.trans.gameObject)
		{
			LeanTween.removeTween((int)this._id, this.uniqueId);
		}
		return this;
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x060000AD RID: 173 RVA: 0x00008250 File Offset: 0x00006450
	public int uniqueId
	{
		get
		{
			return (int)(this._id | this.counter << 16);
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x060000AE RID: 174 RVA: 0x00008262 File Offset: 0x00006462
	public int id
	{
		get
		{
			return this.uniqueId;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x060000AF RID: 175 RVA: 0x0000826A File Offset: 0x0000646A
	// (set) Token: 0x060000B0 RID: 176 RVA: 0x00008272 File Offset: 0x00006472
	public LTDescrOptional optional
	{
		get
		{
			return this._optional;
		}
		set
		{
			this._optional = value;
		}
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x0000827C File Offset: 0x0000647C
	public void reset()
	{
		this.toggle = (this.useRecursion = (this.usesNormalDt = true));
		this.trans = null;
		this.spriteRen = null;
		this.passed = (this.delay = (this.lastVal = 0f));
		this.hasUpdateCallback = (this.useEstimatedTime = (this.useFrames = (this.hasInitiliazed = (this.onCompleteOnRepeat = (this.destroyOnComplete = (this.onCompleteOnStart = (this.useManualTime = (this.hasExtraOnCompletes = false))))))));
		this.easeType = LeanTweenType.linear;
		this.loopType = LeanTweenType.once;
		this.loopCount = 0;
		this.direction = (this.directionLast = (this.overshoot = (this.scale = 1f)));
		this.period = 0.3f;
		this.speed = -1f;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeLinear);
		this.from = (this.to = Vector3.zero);
		this._optional.reset();
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0000839F File Offset: 0x0000659F
	public LTDescr setFollow()
	{
		this.type = TweenAction.FOLLOW;
		return this;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000083AA File Offset: 0x000065AA
	public LTDescr setMoveX()
	{
		this.type = TweenAction.MOVE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.position.x;
		};
		this.easeInternal = delegate()
		{
			this.trans.position = new Vector3(this.easeMethod().x, this.trans.position.y, this.trans.position.z);
		};
		return this;
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x000083D8 File Offset: 0x000065D8
	public LTDescr setMoveY()
	{
		this.type = TweenAction.MOVE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.position.y;
		};
		this.easeInternal = delegate()
		{
			this.trans.position = new Vector3(this.trans.position.x, this.easeMethod().x, this.trans.position.z);
		};
		return this;
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x00008406 File Offset: 0x00006606
	public LTDescr setMoveZ()
	{
		this.type = TweenAction.MOVE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.position.z;
		};
		this.easeInternal = delegate()
		{
			this.trans.position = new Vector3(this.trans.position.x, this.trans.position.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00008434 File Offset: 0x00006634
	public LTDescr setMoveLocalX()
	{
		this.type = TweenAction.MOVE_LOCAL_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localPosition.x;
		};
		this.easeInternal = delegate()
		{
			this.trans.localPosition = new Vector3(this.easeMethod().x, this.trans.localPosition.y, this.trans.localPosition.z);
		};
		return this;
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00008462 File Offset: 0x00006662
	public LTDescr setMoveLocalY()
	{
		this.type = TweenAction.MOVE_LOCAL_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localPosition.y;
		};
		this.easeInternal = delegate()
		{
			this.trans.localPosition = new Vector3(this.trans.localPosition.x, this.easeMethod().x, this.trans.localPosition.z);
		};
		return this;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00008490 File Offset: 0x00006690
	public LTDescr setMoveLocalZ()
	{
		this.type = TweenAction.MOVE_LOCAL_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localPosition.z;
		};
		this.easeInternal = delegate()
		{
			this.trans.localPosition = new Vector3(this.trans.localPosition.x, this.trans.localPosition.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x000084BE File Offset: 0x000066BE
	private void initFromInternal()
	{
		this.fromInternal.x = 0f;
	}

	// Token: 0x060000BA RID: 186 RVA: 0x000084D0 File Offset: 0x000066D0
	public LTDescr setOffset(Vector3 offset)
	{
		this.toInternal = offset;
		return this;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x000084DA File Offset: 0x000066DA
	public LTDescr setMoveCurved()
	{
		this.type = TweenAction.MOVE_CURVED;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.path.orientToPath)
			{
				this.trans.position = this._optional.path.point(LTDescr.val);
				return;
			}
			if (this._optional.path.orientToPath2d)
			{
				this._optional.path.place2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.path.place(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00008508 File Offset: 0x00006708
	public LTDescr setMoveCurvedLocal()
	{
		this.type = TweenAction.MOVE_CURVED_LOCAL;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.path.orientToPath)
			{
				this.trans.localPosition = this._optional.path.point(LTDescr.val);
				return;
			}
			if (this._optional.path.orientToPath2d)
			{
				this._optional.path.placeLocal2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.path.placeLocal(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00008536 File Offset: 0x00006736
	public LTDescr setMoveSpline()
	{
		this.type = TweenAction.MOVE_SPLINE;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.spline.orientToPath)
			{
				this.trans.position = this._optional.spline.point(LTDescr.val);
				return;
			}
			if (this._optional.spline.orientToPath2d)
			{
				this._optional.spline.place2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.spline.place(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00008564 File Offset: 0x00006764
	public LTDescr setMoveSplineLocal()
	{
		this.type = TweenAction.MOVE_SPLINE_LOCAL;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initFromInternal);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (!this._optional.spline.orientToPath)
			{
				this.trans.localPosition = this._optional.spline.point(LTDescr.val);
				return;
			}
			if (this._optional.spline.orientToPath2d)
			{
				this._optional.spline.placeLocal2d(this.trans, LTDescr.val);
				return;
			}
			this._optional.spline.placeLocal(this.trans, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00008593 File Offset: 0x00006793
	public LTDescr setScaleX()
	{
		this.type = TweenAction.SCALE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localScale.x;
		};
		this.easeInternal = delegate()
		{
			this.trans.localScale = new Vector3(this.easeMethod().x, this.trans.localScale.y, this.trans.localScale.z);
		};
		return this;
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x000085C2 File Offset: 0x000067C2
	public LTDescr setScaleY()
	{
		this.type = TweenAction.SCALE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localScale.y;
		};
		this.easeInternal = delegate()
		{
			this.trans.localScale = new Vector3(this.trans.localScale.x, this.easeMethod().x, this.trans.localScale.z);
		};
		return this;
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x000085F1 File Offset: 0x000067F1
	public LTDescr setScaleZ()
	{
		this.type = TweenAction.SCALE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.localScale.z;
		};
		this.easeInternal = delegate()
		{
			this.trans.localScale = new Vector3(this.trans.localScale.x, this.trans.localScale.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060000C2 RID: 194 RVA: 0x00008620 File Offset: 0x00006820
	public LTDescr setRotateX()
	{
		this.type = TweenAction.ROTATE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.eulerAngles.x;
			this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
		};
		this.easeInternal = delegate()
		{
			this.trans.eulerAngles = new Vector3(this.easeMethod().x, this.trans.eulerAngles.y, this.trans.eulerAngles.z);
		};
		return this;
	}

	// Token: 0x060000C3 RID: 195 RVA: 0x0000864F File Offset: 0x0000684F
	public LTDescr setRotateY()
	{
		this.type = TweenAction.ROTATE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.eulerAngles.y;
			this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
		};
		this.easeInternal = delegate()
		{
			this.trans.eulerAngles = new Vector3(this.trans.eulerAngles.x, this.easeMethod().x, this.trans.eulerAngles.z);
		};
		return this;
	}

	// Token: 0x060000C4 RID: 196 RVA: 0x0000867E File Offset: 0x0000687E
	public LTDescr setRotateZ()
	{
		this.type = TweenAction.ROTATE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.eulerAngles.z;
			this.toInternal.x = LeanTween.closestRot(this.fromInternal.x, this.toInternal.x);
		};
		this.easeInternal = delegate()
		{
			this.trans.eulerAngles = new Vector3(this.trans.eulerAngles.x, this.trans.eulerAngles.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060000C5 RID: 197 RVA: 0x000086AD File Offset: 0x000068AD
	public LTDescr setRotateAround()
	{
		this.type = TweenAction.ROTATE_AROUND;
		this.initInternal = delegate()
		{
			this.fromInternal.x = 0f;
			this._optional.origRotation = this.trans.rotation;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Vector3 localPosition = this.trans.localPosition;
			Vector3 point = this.trans.TransformPoint(this._optional.point);
			this.trans.RotateAround(point, this._optional.axis, -this._optional.lastVal);
			Vector3 b = localPosition - this.trans.localPosition;
			this.trans.localPosition = localPosition - b;
			this.trans.rotation = this._optional.origRotation;
			point = this.trans.TransformPoint(this._optional.point);
			this.trans.RotateAround(point, this._optional.axis, LTDescr.val);
			this._optional.lastVal = LTDescr.val;
		};
		return this;
	}

	// Token: 0x060000C6 RID: 198 RVA: 0x000086DC File Offset: 0x000068DC
	public LTDescr setRotateAroundLocal()
	{
		this.type = TweenAction.ROTATE_AROUND_LOCAL;
		this.initInternal = delegate()
		{
			this.fromInternal.x = 0f;
			this._optional.origRotation = this.trans.localRotation;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Vector3 localPosition = this.trans.localPosition;
			this.trans.RotateAround(this.trans.TransformPoint(this._optional.point), this.trans.TransformDirection(this._optional.axis), -this._optional.lastVal);
			Vector3 b = localPosition - this.trans.localPosition;
			this.trans.localPosition = localPosition - b;
			this.trans.localRotation = this._optional.origRotation;
			Vector3 point = this.trans.TransformPoint(this._optional.point);
			this.trans.RotateAround(point, this.trans.TransformDirection(this._optional.axis), LTDescr.val);
			this._optional.lastVal = LTDescr.val;
		};
		return this;
	}

	// Token: 0x060000C7 RID: 199 RVA: 0x0000870B File Offset: 0x0000690B
	public LTDescr setAlpha()
	{
		this.type = TweenAction.ALPHA;
		this.initInternal = delegate()
		{
			SpriteRenderer component = this.trans.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				this.fromInternal.x = component.color.a;
			}
			else if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				this.fromInternal.x = this.trans.GetComponent<Renderer>().material.color.a;
			}
			else if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				Color color = this.trans.GetComponent<Renderer>().material.GetColor("_TintColor");
				this.fromInternal.x = color.a;
			}
			else if (this.trans.childCount > 0)
			{
				foreach (object obj in this.trans)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.GetComponent<Renderer>() != null)
					{
						Color color2 = transform.gameObject.GetComponent<Renderer>().material.color;
						this.fromInternal.x = color2.a;
						break;
					}
				}
			}
			this.easeInternal = delegate()
			{
				LTDescr.val = this.easeMethod().x;
				if (this.spriteRen != null)
				{
					this.spriteRen.color = new Color(this.spriteRen.color.r, this.spriteRen.color.g, this.spriteRen.color.b, LTDescr.val);
					LTDescr.alphaRecursiveSprite(this.trans, LTDescr.val);
					return;
				}
				LTDescr.alphaRecursive(this.trans, LTDescr.val, this.useRecursion);
			};
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (this.spriteRen != null)
			{
				this.spriteRen.color = new Color(this.spriteRen.color.r, this.spriteRen.color.g, this.spriteRen.color.b, LTDescr.val);
				LTDescr.alphaRecursiveSprite(this.trans, LTDescr.val);
				return;
			}
			LTDescr.alphaRecursive(this.trans, LTDescr.val, this.useRecursion);
		};
		return this;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x0000873A File Offset: 0x0000693A
	public LTDescr setTextAlpha()
	{
		this.type = TweenAction.TEXT_ALPHA;
		this.initInternal = delegate()
		{
			this.uiText = this.trans.GetComponent<Text>();
			this.fromInternal.x = ((this.uiText != null) ? this.uiText.color.a : 1f);
		};
		this.easeInternal = delegate()
		{
			LTDescr.textAlphaRecursive(this.trans, this.easeMethod().x, this.useRecursion);
		};
		return this;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00008769 File Offset: 0x00006969
	public LTDescr setAlphaVertex()
	{
		this.type = TweenAction.ALPHA_VERTEX;
		this.initInternal = delegate()
		{
			this.fromInternal.x = (float)this.trans.GetComponent<MeshFilter>().mesh.colors32[0].a;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Mesh mesh = this.trans.GetComponent<MeshFilter>().mesh;
			Vector3[] vertices = mesh.vertices;
			Color32[] array = new Color32[vertices.Length];
			if (array.Length == 0)
			{
				Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
				array = new Color32[mesh.vertices.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = color;
				}
				mesh.colors32 = array;
			}
			Color32 color2 = mesh.colors32[0];
			color2 = new Color((float)color2.r, (float)color2.g, (float)color2.b, LTDescr.val);
			for (int j = 0; j < vertices.Length; j++)
			{
				array[j] = color2;
			}
			mesh.colors32 = array;
		};
		return this;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00008798 File Offset: 0x00006998
	public LTDescr setColor()
	{
		this.type = TweenAction.COLOR;
		this.initInternal = delegate()
		{
			SpriteRenderer component = this.trans.GetComponent<SpriteRenderer>();
			if (component != null)
			{
				this.setFromColor(component.color);
				return;
			}
			if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_Color"))
			{
				Color color = this.trans.GetComponent<Renderer>().material.color;
				this.setFromColor(color);
				return;
			}
			if (this.trans.GetComponent<Renderer>() != null && this.trans.GetComponent<Renderer>().material.HasProperty("_TintColor"))
			{
				Color color2 = this.trans.GetComponent<Renderer>().material.GetColor("_TintColor");
				this.setFromColor(color2);
				return;
			}
			if (this.trans.childCount > 0)
			{
				foreach (object obj in this.trans)
				{
					Transform transform = (Transform)obj;
					if (transform.gameObject.GetComponent<Renderer>() != null)
					{
						Color color3 = transform.gameObject.GetComponent<Renderer>().material.color;
						this.setFromColor(color3);
						break;
					}
				}
			}
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.spriteRen != null)
			{
				this.spriteRen.color = color;
				LTDescr.colorRecursiveSprite(this.trans, color);
			}
			else if (this.type == TweenAction.COLOR)
			{
				LTDescr.colorRecursive(this.trans, color, this.useRecursion);
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
				return;
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColorObject != null)
			{
				this._optional.onUpdateColorObject(color, this._optional.onUpdateParam);
			}
		};
		return this;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x000087C7 File Offset: 0x000069C7
	public LTDescr setCallbackColor()
	{
		this.type = TweenAction.CALLBACK_COLOR;
		this.initInternal = delegate()
		{
			this.diff = new Vector3(1f, 0f, 0f);
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.spriteRen != null)
			{
				this.spriteRen.color = color;
				LTDescr.colorRecursiveSprite(this.trans, color);
			}
			else if (this.type == TweenAction.COLOR)
			{
				LTDescr.colorRecursive(this.trans, color, this.useRecursion);
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
				return;
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColorObject != null)
			{
				this._optional.onUpdateColorObject(color, this._optional.onUpdateParam);
			}
		};
		return this;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x000087F6 File Offset: 0x000069F6
	public LTDescr setTextColor()
	{
		this.type = TweenAction.TEXT_COLOR;
		this.initInternal = delegate()
		{
			this.uiText = this.trans.GetComponent<Text>();
			this.setFromColor((this.uiText != null) ? this.uiText.color : Color.white);
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			this.uiText.color = color;
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
			}
			if (this.useRecursion && this.trans.childCount > 0)
			{
				LTDescr.textColorRecursive(this.trans, color);
			}
		};
		return this;
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00008825 File Offset: 0x00006A25
	public LTDescr setCanvasAlpha()
	{
		this.type = TweenAction.CANVAS_ALPHA;
		this.initInternal = delegate()
		{
			this.uiImage = this.trans.GetComponent<Image>();
			if (this.uiImage != null)
			{
				this.fromInternal.x = this.uiImage.color.a;
				return;
			}
			this.rawImage = this.trans.GetComponent<RawImage>();
			if (this.rawImage != null)
			{
				this.fromInternal.x = this.rawImage.color.a;
				return;
			}
			this.fromInternal.x = 1f;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			if (this.uiImage != null)
			{
				Color color = this.uiImage.color;
				color.a = LTDescr.val;
				this.uiImage.color = color;
			}
			else if (this.rawImage != null)
			{
				Color color2 = this.rawImage.color;
				color2.a = LTDescr.val;
				this.rawImage.color = color2;
			}
			if (this.useRecursion)
			{
				LTDescr.alphaRecursive(this.rectTransform, LTDescr.val, 0);
				LTDescr.textAlphaChildrenRecursive(this.rectTransform, LTDescr.val, true);
			}
		};
		return this;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00008854 File Offset: 0x00006A54
	public LTDescr setCanvasGroupAlpha()
	{
		this.type = TweenAction.CANVASGROUP_ALPHA;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.trans.GetComponent<CanvasGroup>().alpha;
		};
		this.easeInternal = delegate()
		{
			this.trans.GetComponent<CanvasGroup>().alpha = this.easeMethod().x;
		};
		return this;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00008883 File Offset: 0x00006A83
	public LTDescr setCanvasColor()
	{
		this.type = TweenAction.CANVAS_COLOR;
		this.initInternal = delegate()
		{
			this.uiImage = this.trans.GetComponent<Image>();
			if (this.uiImage == null)
			{
				this.rawImage = this.trans.GetComponent<RawImage>();
				this.setFromColor((this.rawImage != null) ? this.rawImage.color : Color.white);
				return;
			}
			this.setFromColor(this.uiImage.color);
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			Color color = LTDescr.tweenColor(this, LTDescr.val);
			if (this.uiImage != null)
			{
				this.uiImage.color = color;
			}
			else if (this.rawImage != null)
			{
				this.rawImage.color = color;
			}
			if (LTDescr.dt != 0f && this._optional.onUpdateColor != null)
			{
				this._optional.onUpdateColor(color);
			}
			if (this.useRecursion)
			{
				LTDescr.colorRecursive(this.rectTransform, color);
			}
		};
		return this;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x000088B2 File Offset: 0x00006AB2
	public LTDescr setCanvasMoveX()
	{
		this.type = TweenAction.CANVAS_MOVE_X;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.rectTransform.anchoredPosition3D.x;
		};
		this.easeInternal = delegate()
		{
			Vector3 anchoredPosition3D = this.rectTransform.anchoredPosition3D;
			this.rectTransform.anchoredPosition3D = new Vector3(this.easeMethod().x, anchoredPosition3D.y, anchoredPosition3D.z);
		};
		return this;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x000088E1 File Offset: 0x00006AE1
	public LTDescr setCanvasMoveY()
	{
		this.type = TweenAction.CANVAS_MOVE_Y;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.rectTransform.anchoredPosition3D.y;
		};
		this.easeInternal = delegate()
		{
			Vector3 anchoredPosition3D = this.rectTransform.anchoredPosition3D;
			this.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D.x, this.easeMethod().x, anchoredPosition3D.z);
		};
		return this;
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00008910 File Offset: 0x00006B10
	public LTDescr setCanvasMoveZ()
	{
		this.type = TweenAction.CANVAS_MOVE_Z;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this.rectTransform.anchoredPosition3D.z;
		};
		this.easeInternal = delegate()
		{
			Vector3 anchoredPosition3D = this.rectTransform.anchoredPosition3D;
			this.rectTransform.anchoredPosition3D = new Vector3(anchoredPosition3D.x, anchoredPosition3D.y, this.easeMethod().x);
		};
		return this;
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x0000893F File Offset: 0x00006B3F
	private void initCanvasRotateAround()
	{
		this.lastVal = 0f;
		this.fromInternal.x = 0f;
		this._optional.origRotation = this.rectTransform.rotation;
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00008972 File Offset: 0x00006B72
	public LTDescr setCanvasRotateAround()
	{
		this.type = TweenAction.CANVAS_ROTATEAROUND;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initCanvasRotateAround);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			RectTransform rectTransform = this.rectTransform;
			Vector3 localPosition = rectTransform.localPosition;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), this._optional.axis, -LTDescr.val);
			Vector3 b = localPosition - rectTransform.localPosition;
			rectTransform.localPosition = localPosition - b;
			rectTransform.rotation = this._optional.origRotation;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), this._optional.axis, LTDescr.val);
		};
		return this;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x000089A1 File Offset: 0x00006BA1
	public LTDescr setCanvasRotateAroundLocal()
	{
		this.type = TweenAction.CANVAS_ROTATEAROUND_LOCAL;
		this.initInternal = new LTDescr.ActionMethodDelegate(this.initCanvasRotateAround);
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			RectTransform rectTransform = this.rectTransform;
			Vector3 localPosition = rectTransform.localPosition;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), rectTransform.TransformDirection(this._optional.axis), -LTDescr.val);
			Vector3 b = localPosition - rectTransform.localPosition;
			rectTransform.localPosition = localPosition - b;
			rectTransform.rotation = this._optional.origRotation;
			rectTransform.RotateAround(rectTransform.TransformPoint(this._optional.point), rectTransform.TransformDirection(this._optional.axis), LTDescr.val);
		};
		return this;
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x000089D0 File Offset: 0x00006BD0
	public LTDescr setCanvasPlaySprite()
	{
		this.type = TweenAction.CANVAS_PLAYSPRITE;
		this.initInternal = delegate()
		{
			this.uiImage = this.trans.GetComponent<Image>();
			this.fromInternal.x = 0f;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			LTDescr.val = LTDescr.newVect.x;
			int num = (int)Mathf.Round(LTDescr.val);
			this.uiImage.sprite = this.sprites[num];
		};
		return this;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x000089FF File Offset: 0x00006BFF
	public LTDescr setCanvasMove()
	{
		this.type = TweenAction.CANVAS_MOVE;
		this.initInternal = delegate()
		{
			this.fromInternal = this.rectTransform.anchoredPosition3D;
		};
		this.easeInternal = delegate()
		{
			this.rectTransform.anchoredPosition3D = this.easeMethod();
		};
		return this;
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x00008A2E File Offset: 0x00006C2E
	public LTDescr setCanvasScale()
	{
		this.type = TweenAction.CANVAS_SCALE;
		this.initInternal = delegate()
		{
			this.from = this.rectTransform.localScale;
		};
		this.easeInternal = delegate()
		{
			this.rectTransform.localScale = this.easeMethod();
		};
		return this;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x00008A5D File Offset: 0x00006C5D
	public LTDescr setCanvasSizeDelta()
	{
		this.type = TweenAction.CANVAS_SIZEDELTA;
		this.initInternal = delegate()
		{
			this.from = this.rectTransform.sizeDelta;
		};
		this.easeInternal = delegate()
		{
			this.rectTransform.sizeDelta = this.easeMethod();
		};
		return this;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x00008A8C File Offset: 0x00006C8C
	private void callback()
	{
		LTDescr.newVect = this.easeMethod();
		LTDescr.val = LTDescr.newVect.x;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00008AB0 File Offset: 0x00006CB0
	public LTDescr setCallback()
	{
		this.type = TweenAction.CALLBACK;
		this.initInternal = delegate()
		{
		};
		this.easeInternal = new LTDescr.ActionMethodDelegate(this.callback);
		return this;
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00008B00 File Offset: 0x00006D00
	public LTDescr setValue3()
	{
		this.type = TweenAction.VALUE3;
		this.initInternal = delegate()
		{
		};
		this.easeInternal = new LTDescr.ActionMethodDelegate(this.callback);
		return this;
	}

	// Token: 0x060000DD RID: 221 RVA: 0x00008B4D File Offset: 0x00006D4D
	public LTDescr setMove()
	{
		this.type = TweenAction.MOVE;
		this.initInternal = delegate()
		{
			this.from = this.trans.position;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.position = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00008B7C File Offset: 0x00006D7C
	public LTDescr setMoveLocal()
	{
		this.type = TweenAction.MOVE_LOCAL;
		this.initInternal = delegate()
		{
			this.from = this.trans.localPosition;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.localPosition = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x00008BAB File Offset: 0x00006DAB
	public LTDescr setMoveToTransform()
	{
		this.type = TweenAction.MOVE_TO_TRANSFORM;
		this.initInternal = delegate()
		{
			this.from = this.trans.position;
		};
		this.easeInternal = delegate()
		{
			this.to = this._optional.toTrans.position;
			this.diff = this.to - this.from;
			this.diffDiv2 = this.diff * 0.5f;
			LTDescr.newVect = this.easeMethod();
			this.trans.position = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x00008BDA File Offset: 0x00006DDA
	public LTDescr setRotate()
	{
		this.type = TweenAction.ROTATE;
		this.initInternal = delegate()
		{
			this.from = this.trans.eulerAngles;
			this.to = new Vector3(LeanTween.closestRot(this.fromInternal.x, this.toInternal.x), LeanTween.closestRot(this.from.y, this.to.y), LeanTween.closestRot(this.from.z, this.to.z));
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.eulerAngles = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00008C09 File Offset: 0x00006E09
	public LTDescr setRotateLocal()
	{
		this.type = TweenAction.ROTATE_LOCAL;
		this.initInternal = delegate()
		{
			this.from = this.trans.localEulerAngles;
			this.to = new Vector3(LeanTween.closestRot(this.fromInternal.x, this.toInternal.x), LeanTween.closestRot(this.from.y, this.to.y), LeanTween.closestRot(this.from.z, this.to.z));
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.localEulerAngles = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x00008C38 File Offset: 0x00006E38
	public LTDescr setScale()
	{
		this.type = TweenAction.SCALE;
		this.initInternal = delegate()
		{
			this.from = this.trans.localScale;
		};
		this.easeInternal = delegate()
		{
			LTDescr.newVect = this.easeMethod();
			this.trans.localScale = LTDescr.newVect;
		};
		return this;
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00008C67 File Offset: 0x00006E67
	public LTDescr setGUIMove()
	{
		this.type = TweenAction.GUI_MOVE;
		this.initInternal = delegate()
		{
			this.from = new Vector3(this._optional.ltRect.rect.x, this._optional.ltRect.rect.y, 0f);
		};
		this.easeInternal = delegate()
		{
			Vector3 vector = this.easeMethod();
			this._optional.ltRect.rect = new Rect(vector.x, vector.y, this._optional.ltRect.rect.width, this._optional.ltRect.rect.height);
		};
		return this;
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x00008C96 File Offset: 0x00006E96
	public LTDescr setGUIMoveMargin()
	{
		this.type = TweenAction.GUI_MOVE_MARGIN;
		this.initInternal = delegate()
		{
			this.from = new Vector2(this._optional.ltRect.margin.x, this._optional.ltRect.margin.y);
		};
		this.easeInternal = delegate()
		{
			Vector3 vector = this.easeMethod();
			this._optional.ltRect.margin = new Vector2(vector.x, vector.y);
		};
		return this;
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00008CC5 File Offset: 0x00006EC5
	public LTDescr setGUIScale()
	{
		this.type = TweenAction.GUI_SCALE;
		this.initInternal = delegate()
		{
			this.from = new Vector3(this._optional.ltRect.rect.width, this._optional.ltRect.rect.height, 0f);
		};
		this.easeInternal = delegate()
		{
			Vector3 vector = this.easeMethod();
			this._optional.ltRect.rect = new Rect(this._optional.ltRect.rect.x, this._optional.ltRect.rect.y, vector.x, vector.y);
		};
		return this;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x00008CF4 File Offset: 0x00006EF4
	public LTDescr setGUIAlpha()
	{
		this.type = TweenAction.GUI_ALPHA;
		this.initInternal = delegate()
		{
			this.fromInternal.x = this._optional.ltRect.alpha;
		};
		this.easeInternal = delegate()
		{
			this._optional.ltRect.alpha = this.easeMethod().x;
		};
		return this;
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00008D23 File Offset: 0x00006F23
	public LTDescr setGUIRotate()
	{
		this.type = TweenAction.GUI_ROTATE;
		this.initInternal = delegate()
		{
			if (!this._optional.ltRect.rotateEnabled)
			{
				this._optional.ltRect.rotateEnabled = true;
				this._optional.ltRect.resetForRotation();
			}
			this.fromInternal.x = this._optional.ltRect.rotation;
		};
		this.easeInternal = delegate()
		{
			this._optional.ltRect.rotation = this.easeMethod().x;
		};
		return this;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00008D52 File Offset: 0x00006F52
	public LTDescr setDelayedSound()
	{
		this.type = TweenAction.DELAYED_SOUND;
		this.initInternal = delegate()
		{
			this.hasExtraOnCompletes = true;
		};
		this.easeInternal = new LTDescr.ActionMethodDelegate(this.callback);
		return this;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00008D81 File Offset: 0x00006F81
	public LTDescr setTarget(Transform trans)
	{
		this.optional.toTrans = trans;
		return this;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00008D90 File Offset: 0x00006F90
	private void init()
	{
		this.hasInitiliazed = true;
		this.usesNormalDt = (!this.useEstimatedTime && !this.useManualTime && !this.useFrames);
		if (this.useFrames)
		{
			this.optional.initFrameCount = Time.frameCount;
		}
		if (this.time <= 0f)
		{
			this.time = Mathf.Epsilon;
		}
		if (this.initInternal != null)
		{
			this.initInternal();
		}
		this.diff = this.to - this.from;
		this.diffDiv2 = this.diff * 0.5f;
		if (this._optional.onStart != null)
		{
			this._optional.onStart();
		}
		if (this.onCompleteOnStart)
		{
			this.callOnCompletes();
		}
		if (this.speed >= 0f)
		{
			this.initSpeed();
		}
	}

	// Token: 0x060000EB RID: 235 RVA: 0x00008E74 File Offset: 0x00007074
	private void initSpeed()
	{
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			this.time = this._optional.path.distance / this.speed;
			return;
		}
		if (this.type == TweenAction.MOVE_SPLINE || this.type == TweenAction.MOVE_SPLINE_LOCAL)
		{
			this.time = this._optional.spline.distance / this.speed;
			return;
		}
		this.time = (this.to - this.from).magnitude / this.speed;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x00008F08 File Offset: 0x00007108
	public LTDescr updateNow()
	{
		this.updateInternal();
		return this;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x00008F14 File Offset: 0x00007114
	public bool updateInternal()
	{
		float num = this.direction;
		if (this.usesNormalDt)
		{
			LTDescr.dt = LeanTween.dtActual;
		}
		else if (this.useEstimatedTime)
		{
			LTDescr.dt = LeanTween.dtEstimated;
		}
		else if (this.useFrames)
		{
			LTDescr.dt = (float)((this.optional.initFrameCount == 0) ? 0 : 1);
			this.optional.initFrameCount = Time.frameCount;
		}
		else if (this.useManualTime)
		{
			LTDescr.dt = LeanTween.dtManual;
		}
		if (this.delay <= 0f && num != 0f)
		{
			if (this.trans == null)
			{
				return true;
			}
			if (!this.hasInitiliazed)
			{
				this.init();
			}
			LTDescr.dt *= num;
			this.passed += LTDescr.dt;
			this.passed = Mathf.Clamp(this.passed, 0f, this.time);
			this.ratioPassed = this.passed / this.time;
			this.easeInternal();
			if (this.hasUpdateCallback)
			{
				this._optional.callOnUpdate(LTDescr.val, this.ratioPassed);
			}
			if ((num > 0f) ? (this.passed >= this.time) : (this.passed <= 0f))
			{
				this.loopCount--;
				if (this.loopType == LeanTweenType.pingPong)
				{
					this.direction = 0f - num;
				}
				else
				{
					this.passed = Mathf.Epsilon;
				}
				bool flag = this.loopCount == 0 || this.loopType == LeanTweenType.once;
				if (!flag && this.onCompleteOnRepeat && this.hasExtraOnCompletes)
				{
					this.callOnCompletes();
				}
				return flag;
			}
		}
		else
		{
			this.delay -= LTDescr.dt;
		}
		return false;
	}

	// Token: 0x060000EE RID: 238 RVA: 0x000090E4 File Offset: 0x000072E4
	public void callOnCompletes()
	{
		if (this.type == TweenAction.GUI_ROTATE)
		{
			this._optional.ltRect.rotateFinished = true;
		}
		if (this.type == TweenAction.DELAYED_SOUND)
		{
			AudioSource.PlayClipAtPoint((AudioClip)this._optional.onCompleteParam, this.to, this.from.x);
		}
		if (this._optional.onComplete != null)
		{
			this._optional.onComplete();
			return;
		}
		if (this._optional.onCompleteObject != null)
		{
			this._optional.onCompleteObject(this._optional.onCompleteParam);
		}
	}

	// Token: 0x060000EF RID: 239 RVA: 0x00009184 File Offset: 0x00007384
	public LTDescr setFromColor(Color col)
	{
		this.from = new Vector3(0f, col.a, 0f);
		this.diff = new Vector3(1f, 0f, 0f);
		this._optional.axis = new Vector3(col.r, col.g, col.b);
		return this;
	}

	// Token: 0x060000F0 RID: 240 RVA: 0x000091EC File Offset: 0x000073EC
	private static void alphaRecursive(Transform transform, float val, bool useRecursion = true)
	{
		Renderer component = transform.gameObject.GetComponent<Renderer>();
		if (component != null)
		{
			foreach (Material material in component.materials)
			{
				if (material.HasProperty("_Color"))
				{
					material.color = new Color(material.color.r, material.color.g, material.color.b, val);
				}
				else if (material.HasProperty("_TintColor"))
				{
					Color color = material.GetColor("_TintColor");
					material.SetColor("_TintColor", new Color(color.r, color.g, color.b, val));
				}
			}
		}
		if (useRecursion && transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				LTDescr.alphaRecursive((Transform)obj, val, true);
			}
		}
	}

	// Token: 0x060000F1 RID: 241 RVA: 0x00009308 File Offset: 0x00007508
	private static void colorRecursive(Transform transform, Color toColor, bool useRecursion = true)
	{
		Renderer component = transform.gameObject.GetComponent<Renderer>();
		if (component != null)
		{
			Material[] materials = component.materials;
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].color = toColor;
			}
		}
		if (useRecursion && transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				LTDescr.colorRecursive((Transform)obj, toColor, true);
			}
		}
	}

	// Token: 0x060000F2 RID: 242 RVA: 0x000093A0 File Offset: 0x000075A0
	private static void alphaRecursive(RectTransform rectTransform, float val, int recursiveLevel = 0)
	{
		if (rectTransform.childCount > 0)
		{
			foreach (object obj in rectTransform)
			{
				RectTransform rectTransform2 = (RectTransform)obj;
				MaskableGraphic component = rectTransform2.GetComponent<Image>();
				if (component != null)
				{
					Color color = component.color;
					color.a = val;
					component.color = color;
				}
				else
				{
					component = rectTransform2.GetComponent<RawImage>();
					if (component != null)
					{
						Color color2 = component.color;
						color2.a = val;
						component.color = color2;
					}
				}
				LTDescr.alphaRecursive(rectTransform2, val, recursiveLevel + 1);
			}
		}
	}

	// Token: 0x060000F3 RID: 243 RVA: 0x00009458 File Offset: 0x00007658
	private static void alphaRecursiveSprite(Transform transform, float val)
	{
		if (transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				SpriteRenderer component = transform2.GetComponent<SpriteRenderer>();
				if (component != null)
				{
					component.color = new Color(component.color.r, component.color.g, component.color.b, val);
				}
				LTDescr.alphaRecursiveSprite(transform2, val);
			}
		}
	}

	// Token: 0x060000F4 RID: 244 RVA: 0x000094F0 File Offset: 0x000076F0
	private static void colorRecursiveSprite(Transform transform, Color toColor)
	{
		if (transform.childCount > 0)
		{
			foreach (object obj in transform)
			{
				Transform transform2 = (Transform)obj;
				SpriteRenderer component = transform.gameObject.GetComponent<SpriteRenderer>();
				if (component != null)
				{
					component.color = toColor;
				}
				LTDescr.colorRecursiveSprite(transform2, toColor);
			}
		}
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x00009568 File Offset: 0x00007768
	private static void colorRecursive(RectTransform rectTransform, Color toColor)
	{
		if (rectTransform.childCount > 0)
		{
			foreach (object obj in rectTransform)
			{
				RectTransform rectTransform2 = (RectTransform)obj;
				MaskableGraphic component = rectTransform2.GetComponent<Image>();
				if (component != null)
				{
					component.color = toColor;
				}
				else
				{
					component = rectTransform2.GetComponent<RawImage>();
					if (component != null)
					{
						component.color = toColor;
					}
				}
				LTDescr.colorRecursive(rectTransform2, toColor);
			}
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000095F8 File Offset: 0x000077F8
	private static void textAlphaChildrenRecursive(Transform trans, float val, bool useRecursion = true)
	{
		if (useRecursion && trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				Transform transform = (Transform)obj;
				Text component = transform.GetComponent<Text>();
				if (component != null)
				{
					Color color = component.color;
					color.a = val;
					component.color = color;
				}
				LTDescr.textAlphaChildrenRecursive(transform, val, true);
			}
		}
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00009680 File Offset: 0x00007880
	private static void textAlphaRecursive(Transform trans, float val, bool useRecursion = true)
	{
		Text component = trans.GetComponent<Text>();
		if (component != null)
		{
			Color color = component.color;
			color.a = val;
			component.color = color;
		}
		if (useRecursion && trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				LTDescr.textAlphaRecursive((Transform)obj, val, true);
			}
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x00009708 File Offset: 0x00007908
	private static void textColorRecursive(Transform trans, Color toColor)
	{
		if (trans.childCount > 0)
		{
			foreach (object obj in trans)
			{
				Transform transform = (Transform)obj;
				Text component = transform.GetComponent<Text>();
				if (component != null)
				{
					component.color = toColor;
				}
				LTDescr.textColorRecursive(transform, toColor);
			}
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x0000977C File Offset: 0x0000797C
	private static Color tweenColor(LTDescr tween, float val)
	{
		Vector3 vector = tween._optional.point - tween._optional.axis;
		float num = tween.to.y - tween.from.y;
		return new Color(tween._optional.axis.x + vector.x * val, tween._optional.axis.y + vector.y * val, tween._optional.axis.z + vector.z * val, tween.from.y + num * val);
	}

	// Token: 0x060000FA RID: 250 RVA: 0x0000981C File Offset: 0x00007A1C
	public LTDescr pause()
	{
		if (this.direction != 0f)
		{
			this.directionLast = this.direction;
			this.direction = 0f;
		}
		return this;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00009843 File Offset: 0x00007A43
	public LTDescr resume()
	{
		this.direction = this.directionLast;
		return this;
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00009852 File Offset: 0x00007A52
	public LTDescr setAxis(Vector3 axis)
	{
		this._optional.axis = axis;
		return this;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00009861 File Offset: 0x00007A61
	public LTDescr setDelay(float delay)
	{
		this.delay = delay;
		return this;
	}

	// Token: 0x060000FE RID: 254 RVA: 0x0000986C File Offset: 0x00007A6C
	public LTDescr setEase(LeanTweenType easeType)
	{
		switch (easeType)
		{
		case LeanTweenType.linear:
			this.setEaseLinear();
			break;
		case LeanTweenType.easeOutQuad:
			this.setEaseOutQuad();
			break;
		case LeanTweenType.easeInQuad:
			this.setEaseInQuad();
			break;
		case LeanTweenType.easeInOutQuad:
			this.setEaseInOutQuad();
			break;
		case LeanTweenType.easeInCubic:
			this.setEaseInCubic();
			break;
		case LeanTweenType.easeOutCubic:
			this.setEaseOutCubic();
			break;
		case LeanTweenType.easeInOutCubic:
			this.setEaseInOutCubic();
			break;
		case LeanTweenType.easeInQuart:
			this.setEaseInQuart();
			break;
		case LeanTweenType.easeOutQuart:
			this.setEaseOutQuart();
			break;
		case LeanTweenType.easeInOutQuart:
			this.setEaseInOutQuart();
			break;
		case LeanTweenType.easeInQuint:
			this.setEaseInQuint();
			break;
		case LeanTweenType.easeOutQuint:
			this.setEaseOutQuint();
			break;
		case LeanTweenType.easeInOutQuint:
			this.setEaseInOutQuint();
			break;
		case LeanTweenType.easeInSine:
			this.setEaseInSine();
			break;
		case LeanTweenType.easeOutSine:
			this.setEaseOutSine();
			break;
		case LeanTweenType.easeInOutSine:
			this.setEaseInOutSine();
			break;
		case LeanTweenType.easeInExpo:
			this.setEaseInExpo();
			break;
		case LeanTweenType.easeOutExpo:
			this.setEaseOutExpo();
			break;
		case LeanTweenType.easeInOutExpo:
			this.setEaseInOutExpo();
			break;
		case LeanTweenType.easeInCirc:
			this.setEaseInCirc();
			break;
		case LeanTweenType.easeOutCirc:
			this.setEaseOutCirc();
			break;
		case LeanTweenType.easeInOutCirc:
			this.setEaseInOutCirc();
			break;
		case LeanTweenType.easeInBounce:
			this.setEaseInBounce();
			break;
		case LeanTweenType.easeOutBounce:
			this.setEaseOutBounce();
			break;
		case LeanTweenType.easeInOutBounce:
			this.setEaseInOutBounce();
			break;
		case LeanTweenType.easeInBack:
			this.setEaseInBack();
			break;
		case LeanTweenType.easeOutBack:
			this.setEaseOutBack();
			break;
		case LeanTweenType.easeInOutBack:
			this.setEaseInOutBack();
			break;
		case LeanTweenType.easeInElastic:
			this.setEaseInElastic();
			break;
		case LeanTweenType.easeOutElastic:
			this.setEaseOutElastic();
			break;
		case LeanTweenType.easeInOutElastic:
			this.setEaseInOutElastic();
			break;
		case LeanTweenType.easeSpring:
			this.setEaseSpring();
			break;
		case LeanTweenType.easeShake:
			this.setEaseShake();
			break;
		case LeanTweenType.punch:
			this.setEasePunch();
			break;
		default:
			this.setEaseLinear();
			break;
		}
		return this;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00009A84 File Offset: 0x00007C84
	public LTDescr setEaseLinear()
	{
		this.easeType = LeanTweenType.linear;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeLinear);
		return this;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00009AA0 File Offset: 0x00007CA0
	public LTDescr setEaseSpring()
	{
		this.easeType = LeanTweenType.easeSpring;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeSpring);
		return this;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00009ABD File Offset: 0x00007CBD
	public LTDescr setEaseInQuad()
	{
		this.easeType = LeanTweenType.easeInQuad;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInQuad);
		return this;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00009AD9 File Offset: 0x00007CD9
	public LTDescr setEaseOutQuad()
	{
		this.easeType = LeanTweenType.easeOutQuad;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutQuad);
		return this;
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00009AF5 File Offset: 0x00007CF5
	public LTDescr setEaseInOutQuad()
	{
		this.easeType = LeanTweenType.easeInOutQuad;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutQuad);
		return this;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00009B11 File Offset: 0x00007D11
	public LTDescr setEaseInCubic()
	{
		this.easeType = LeanTweenType.easeInCubic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInCubic);
		return this;
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00009B2D File Offset: 0x00007D2D
	public LTDescr setEaseOutCubic()
	{
		this.easeType = LeanTweenType.easeOutCubic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutCubic);
		return this;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00009B49 File Offset: 0x00007D49
	public LTDescr setEaseInOutCubic()
	{
		this.easeType = LeanTweenType.easeInOutCubic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutCubic);
		return this;
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00009B65 File Offset: 0x00007D65
	public LTDescr setEaseInQuart()
	{
		this.easeType = LeanTweenType.easeInQuart;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInQuart);
		return this;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00009B81 File Offset: 0x00007D81
	public LTDescr setEaseOutQuart()
	{
		this.easeType = LeanTweenType.easeOutQuart;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutQuart);
		return this;
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00009B9E File Offset: 0x00007D9E
	public LTDescr setEaseInOutQuart()
	{
		this.easeType = LeanTweenType.easeInOutQuart;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutQuart);
		return this;
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00009BBB File Offset: 0x00007DBB
	public LTDescr setEaseInQuint()
	{
		this.easeType = LeanTweenType.easeInQuint;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInQuint);
		return this;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00009BD8 File Offset: 0x00007DD8
	public LTDescr setEaseOutQuint()
	{
		this.easeType = LeanTweenType.easeOutQuint;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutQuint);
		return this;
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00009BF5 File Offset: 0x00007DF5
	public LTDescr setEaseInOutQuint()
	{
		this.easeType = LeanTweenType.easeInOutQuint;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutQuint);
		return this;
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00009C12 File Offset: 0x00007E12
	public LTDescr setEaseInSine()
	{
		this.easeType = LeanTweenType.easeInSine;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInSine);
		return this;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00009C2F File Offset: 0x00007E2F
	public LTDescr setEaseOutSine()
	{
		this.easeType = LeanTweenType.easeOutSine;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutSine);
		return this;
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00009C4C File Offset: 0x00007E4C
	public LTDescr setEaseInOutSine()
	{
		this.easeType = LeanTweenType.easeInOutSine;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutSine);
		return this;
	}

	// Token: 0x06000110 RID: 272 RVA: 0x00009C69 File Offset: 0x00007E69
	public LTDescr setEaseInExpo()
	{
		this.easeType = LeanTweenType.easeInExpo;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInExpo);
		return this;
	}

	// Token: 0x06000111 RID: 273 RVA: 0x00009C86 File Offset: 0x00007E86
	public LTDescr setEaseOutExpo()
	{
		this.easeType = LeanTweenType.easeOutExpo;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutExpo);
		return this;
	}

	// Token: 0x06000112 RID: 274 RVA: 0x00009CA3 File Offset: 0x00007EA3
	public LTDescr setEaseInOutExpo()
	{
		this.easeType = LeanTweenType.easeInOutExpo;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutExpo);
		return this;
	}

	// Token: 0x06000113 RID: 275 RVA: 0x00009CC0 File Offset: 0x00007EC0
	public LTDescr setEaseInCirc()
	{
		this.easeType = LeanTweenType.easeInCirc;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInCirc);
		return this;
	}

	// Token: 0x06000114 RID: 276 RVA: 0x00009CDD File Offset: 0x00007EDD
	public LTDescr setEaseOutCirc()
	{
		this.easeType = LeanTweenType.easeOutCirc;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutCirc);
		return this;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00009CFA File Offset: 0x00007EFA
	public LTDescr setEaseInOutCirc()
	{
		this.easeType = LeanTweenType.easeInOutCirc;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutCirc);
		return this;
	}

	// Token: 0x06000116 RID: 278 RVA: 0x00009D17 File Offset: 0x00007F17
	public LTDescr setEaseInBounce()
	{
		this.easeType = LeanTweenType.easeInBounce;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInBounce);
		return this;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00009D34 File Offset: 0x00007F34
	public LTDescr setEaseOutBounce()
	{
		this.easeType = LeanTweenType.easeOutBounce;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutBounce);
		return this;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x00009D51 File Offset: 0x00007F51
	public LTDescr setEaseInOutBounce()
	{
		this.easeType = LeanTweenType.easeInOutBounce;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutBounce);
		return this;
	}

	// Token: 0x06000119 RID: 281 RVA: 0x00009D6E File Offset: 0x00007F6E
	public LTDescr setEaseInBack()
	{
		this.easeType = LeanTweenType.easeInBack;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInBack);
		return this;
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00009D8B File Offset: 0x00007F8B
	public LTDescr setEaseOutBack()
	{
		this.easeType = LeanTweenType.easeOutBack;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutBack);
		return this;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00009DA8 File Offset: 0x00007FA8
	public LTDescr setEaseInOutBack()
	{
		this.easeType = LeanTweenType.easeInOutBack;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutBack);
		return this;
	}

	// Token: 0x0600011C RID: 284 RVA: 0x00009DC5 File Offset: 0x00007FC5
	public LTDescr setEaseInElastic()
	{
		this.easeType = LeanTweenType.easeInElastic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInElastic);
		return this;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x00009DE2 File Offset: 0x00007FE2
	public LTDescr setEaseOutElastic()
	{
		this.easeType = LeanTweenType.easeOutElastic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeOutElastic);
		return this;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00009DFF File Offset: 0x00007FFF
	public LTDescr setEaseInOutElastic()
	{
		this.easeType = LeanTweenType.easeInOutElastic;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.easeInOutElastic);
		return this;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00009E1C File Offset: 0x0000801C
	public LTDescr setEasePunch()
	{
		this._optional.animationCurve = LeanTween.punch;
		this.toInternal.x = this.from.x + this.to.x;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.tweenOnCurve);
		return this;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x00009E70 File Offset: 0x00008070
	public LTDescr setEaseShake()
	{
		this._optional.animationCurve = LeanTween.shake;
		this.toInternal.x = this.from.x + this.to.x;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.tweenOnCurve);
		return this;
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00009EC4 File Offset: 0x000080C4
	private Vector3 tweenOnCurve()
	{
		return new Vector3(this.from.x + this.diff.x * this._optional.animationCurve.Evaluate(this.ratioPassed), this.from.y + this.diff.y * this._optional.animationCurve.Evaluate(this.ratioPassed), this.from.z + this.diff.z * this._optional.animationCurve.Evaluate(this.ratioPassed));
	}

	// Token: 0x06000122 RID: 290 RVA: 0x00009F60 File Offset: 0x00008160
	private Vector3 easeInOutQuad()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val *= LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val = (1f - LTDescr.val) * (LTDescr.val - 3f) + 1f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000123 RID: 291 RVA: 0x0000A078 File Offset: 0x00008278
	private Vector3 easeInQuad()
	{
		LTDescr.val = this.ratioPassed * this.ratioPassed;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000124 RID: 292 RVA: 0x0000A0F3 File Offset: 0x000082F3
	private Vector3 easeOutQuad()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val = -LTDescr.val * (LTDescr.val - 2f);
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x06000125 RID: 293 RVA: 0x0000A134 File Offset: 0x00008334
	private Vector3 easeLinear()
	{
		LTDescr.val = this.ratioPassed;
		return new Vector3(this.from.x + this.diff.x * LTDescr.val, this.from.y + this.diff.y * LTDescr.val, this.from.z + this.diff.z * LTDescr.val);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x0000A1A8 File Offset: 0x000083A8
	private Vector3 easeSpring()
	{
		LTDescr.val = Mathf.Clamp01(this.ratioPassed);
		LTDescr.val = (Mathf.Sin(LTDescr.val * 3.1415927f * (0.2f + 2.5f * LTDescr.val * LTDescr.val * LTDescr.val)) * Mathf.Pow(1f - LTDescr.val, 2.2f) + LTDescr.val) * (1f + 1.2f * (1f - LTDescr.val));
		return this.from + this.diff * LTDescr.val;
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0000A248 File Offset: 0x00008448
	private Vector3 easeInCubic()
	{
		LTDescr.val = this.ratioPassed * this.ratioPassed * this.ratioPassed;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000128 RID: 296 RVA: 0x0000A2CC File Offset: 0x000084CC
	private Vector3 easeOutCubic()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val + 1f;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000129 RID: 297 RVA: 0x0000A364 File Offset: 0x00008564
	private Vector3 easeInOutCubic()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val + 2f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0000A489 File Offset: 0x00008689
	private Vector3 easeInQuart()
	{
		LTDescr.val = this.ratioPassed * this.ratioPassed * this.ratioPassed * this.ratioPassed;
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x0600012B RID: 299 RVA: 0x0000A4C8 File Offset: 0x000086C8
	private Vector3 easeOutQuart()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = -(LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val - 1f);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600012C RID: 300 RVA: 0x0000A568 File Offset: 0x00008768
	private Vector3 easeInOutQuart()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		return -this.diffDiv2 * (LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val - 2f) + this.from;
	}

	// Token: 0x0600012D RID: 301 RVA: 0x0000A654 File Offset: 0x00008854
	private Vector3 easeInQuint()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600012E RID: 302 RVA: 0x0000A6EC File Offset: 0x000088EC
	private Vector3 easeOutQuint()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val + 1f;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x0600012F RID: 303 RVA: 0x0000A790 File Offset: 0x00008990
	private Vector3 easeInOutQuint()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val;
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		LTDescr.val = LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val * LTDescr.val + 2f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000130 RID: 304 RVA: 0x0000A8D0 File Offset: 0x00008AD0
	private Vector3 easeInSine()
	{
		LTDescr.val = -Mathf.Cos(this.ratioPassed * LeanTween.PI_DIV2);
		return new Vector3(this.diff.x * LTDescr.val + this.diff.x + this.from.x, this.diff.y * LTDescr.val + this.diff.y + this.from.y, this.diff.z * LTDescr.val + this.diff.z + this.from.z);
	}

	// Token: 0x06000131 RID: 305 RVA: 0x0000A974 File Offset: 0x00008B74
	private Vector3 easeOutSine()
	{
		LTDescr.val = Mathf.Sin(this.ratioPassed * LeanTween.PI_DIV2);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000132 RID: 306 RVA: 0x0000A9F4 File Offset: 0x00008BF4
	private Vector3 easeInOutSine()
	{
		LTDescr.val = -(Mathf.Cos(3.1415927f * this.ratioPassed) - 1f);
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000133 RID: 307 RVA: 0x0000AA7C File Offset: 0x00008C7C
	private Vector3 easeInExpo()
	{
		LTDescr.val = Mathf.Pow(2f, 10f * (this.ratioPassed - 1f));
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000134 RID: 308 RVA: 0x0000AB08 File Offset: 0x00008D08
	private Vector3 easeOutExpo()
	{
		LTDescr.val = -Mathf.Pow(2f, -10f * this.ratioPassed) + 1f;
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000135 RID: 309 RVA: 0x0000AB94 File Offset: 0x00008D94
	private Vector3 easeInOutExpo()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			return this.diffDiv2 * Mathf.Pow(2f, 10f * (LTDescr.val - 1f)) + this.from;
		}
		LTDescr.val -= 1f;
		return this.diffDiv2 * (-Mathf.Pow(2f, -10f * LTDescr.val) + 2f) + this.from;
	}

	// Token: 0x06000136 RID: 310 RVA: 0x0000AC34 File Offset: 0x00008E34
	private Vector3 easeInCirc()
	{
		LTDescr.val = -(Mathf.Sqrt(1f - this.ratioPassed * this.ratioPassed) - 1f);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000137 RID: 311 RVA: 0x0000ACC4 File Offset: 0x00008EC4
	private Vector3 easeOutCirc()
	{
		LTDescr.val = this.ratioPassed - 1f;
		LTDescr.val = Mathf.Sqrt(1f - LTDescr.val * LTDescr.val);
		return new Vector3(this.diff.x * LTDescr.val + this.from.x, this.diff.y * LTDescr.val + this.from.y, this.diff.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000138 RID: 312 RVA: 0x0000AD5C File Offset: 0x00008F5C
	private Vector3 easeInOutCirc()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			LTDescr.val = -(Mathf.Sqrt(1f - LTDescr.val * LTDescr.val) - 1f);
			return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
		}
		LTDescr.val -= 2f;
		LTDescr.val = Mathf.Sqrt(1f - LTDescr.val * LTDescr.val) + 1f;
		return new Vector3(this.diffDiv2.x * LTDescr.val + this.from.x, this.diffDiv2.y * LTDescr.val + this.from.y, this.diffDiv2.z * LTDescr.val + this.from.z);
	}

	// Token: 0x06000139 RID: 313 RVA: 0x0000AE94 File Offset: 0x00009094
	private Vector3 easeInBounce()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val = 1f - LTDescr.val;
		return new Vector3(this.diff.x - LeanTween.easeOutBounce(0f, this.diff.x, LTDescr.val) + this.from.x, this.diff.y - LeanTween.easeOutBounce(0f, this.diff.y, LTDescr.val) + this.from.y, this.diff.z - LeanTween.easeOutBounce(0f, this.diff.z, LTDescr.val) + this.from.z);
	}

	// Token: 0x0600013A RID: 314 RVA: 0x0000AF58 File Offset: 0x00009158
	private Vector3 easeOutBounce()
	{
		LTDescr.val = this.ratioPassed;
		float num;
		float num2;
		if (LTDescr.val < (num = 1f - 1.75f * this.overshoot / 2.75f))
		{
			LTDescr.val = 1f / num / num * LTDescr.val * LTDescr.val;
		}
		else if (LTDescr.val < (num2 = 1f - 0.75f * this.overshoot / 2.75f))
		{
			LTDescr.val -= (num + num2) / 2f;
			LTDescr.val = 7.5625f * LTDescr.val * LTDescr.val + 1f - 0.25f * this.overshoot * this.overshoot;
		}
		else if (LTDescr.val < (num = 1f - 0.25f * this.overshoot / 2.75f))
		{
			LTDescr.val -= (num + num2) / 2f;
			LTDescr.val = 7.5625f * LTDescr.val * LTDescr.val + 1f - 0.0625f * this.overshoot * this.overshoot;
		}
		else
		{
			LTDescr.val -= (num + 1f) / 2f;
			LTDescr.val = 7.5625f * LTDescr.val * LTDescr.val + 1f - 0.015625f * this.overshoot * this.overshoot;
		}
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x0600013B RID: 315 RVA: 0x0000B0E4 File Offset: 0x000092E4
	private Vector3 easeInOutBounce()
	{
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			return new Vector3(LeanTween.easeInBounce(0f, this.diff.x, LTDescr.val) * 0.5f + this.from.x, LeanTween.easeInBounce(0f, this.diff.y, LTDescr.val) * 0.5f + this.from.y, LeanTween.easeInBounce(0f, this.diff.z, LTDescr.val) * 0.5f + this.from.z);
		}
		LTDescr.val -= 1f;
		return new Vector3(LeanTween.easeOutBounce(0f, this.diff.x, LTDescr.val) * 0.5f + this.diffDiv2.x + this.from.x, LeanTween.easeOutBounce(0f, this.diff.y, LTDescr.val) * 0.5f + this.diffDiv2.y + this.from.y, LeanTween.easeOutBounce(0f, this.diff.z, LTDescr.val) * 0.5f + this.diffDiv2.z + this.from.z);
	}

	// Token: 0x0600013C RID: 316 RVA: 0x0000B258 File Offset: 0x00009458
	private Vector3 easeInBack()
	{
		LTDescr.val = this.ratioPassed;
		LTDescr.val /= 1f;
		float num = 1.70158f * this.overshoot;
		return this.diff * LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val - num) + this.from;
	}

	// Token: 0x0600013D RID: 317 RVA: 0x0000B2C8 File Offset: 0x000094C8
	private Vector3 easeOutBack()
	{
		float num = 1.70158f * this.overshoot;
		LTDescr.val = this.ratioPassed / 1f - 1f;
		LTDescr.val = LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val + num) + 1f;
		return this.diff * LTDescr.val + this.from;
	}

	// Token: 0x0600013E RID: 318 RVA: 0x0000B33C File Offset: 0x0000953C
	private Vector3 easeInOutBack()
	{
		float num = 1.70158f * this.overshoot;
		LTDescr.val = this.ratioPassed * 2f;
		if (LTDescr.val < 1f)
		{
			num *= 1.525f * this.overshoot;
			return this.diffDiv2 * (LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val - num)) + this.from;
		}
		LTDescr.val -= 2f;
		num *= 1.525f * this.overshoot;
		LTDescr.val = LTDescr.val * LTDescr.val * ((num + 1f) * LTDescr.val + num) + 2f;
		return this.diffDiv2 * LTDescr.val + this.from;
	}

	// Token: 0x0600013F RID: 319 RVA: 0x0000B414 File Offset: 0x00009614
	private Vector3 easeInElastic()
	{
		return new Vector3(LeanTween.easeInElastic(this.from.x, this.to.x, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInElastic(this.from.y, this.to.y, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInElastic(this.from.z, this.to.z, this.ratioPassed, this.overshoot, this.period));
	}

	// Token: 0x06000140 RID: 320 RVA: 0x0000B4B0 File Offset: 0x000096B0
	private Vector3 easeOutElastic()
	{
		return new Vector3(LeanTween.easeOutElastic(this.from.x, this.to.x, this.ratioPassed, this.overshoot, this.period), LeanTween.easeOutElastic(this.from.y, this.to.y, this.ratioPassed, this.overshoot, this.period), LeanTween.easeOutElastic(this.from.z, this.to.z, this.ratioPassed, this.overshoot, this.period));
	}

	// Token: 0x06000141 RID: 321 RVA: 0x0000B54C File Offset: 0x0000974C
	private Vector3 easeInOutElastic()
	{
		return new Vector3(LeanTween.easeInOutElastic(this.from.x, this.to.x, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInOutElastic(this.from.y, this.to.y, this.ratioPassed, this.overshoot, this.period), LeanTween.easeInOutElastic(this.from.z, this.to.z, this.ratioPassed, this.overshoot, this.period));
	}

	// Token: 0x06000142 RID: 322 RVA: 0x0000B5E5 File Offset: 0x000097E5
	public LTDescr setOvershoot(float overshoot)
	{
		this.overshoot = overshoot;
		return this;
	}

	// Token: 0x06000143 RID: 323 RVA: 0x0000B5EF File Offset: 0x000097EF
	public LTDescr setPeriod(float period)
	{
		this.period = period;
		return this;
	}

	// Token: 0x06000144 RID: 324 RVA: 0x0000B5F9 File Offset: 0x000097F9
	public LTDescr setScale(float scale)
	{
		this.scale = scale;
		return this;
	}

	// Token: 0x06000145 RID: 325 RVA: 0x0000B603 File Offset: 0x00009803
	public LTDescr setEase(AnimationCurve easeCurve)
	{
		this._optional.animationCurve = easeCurve;
		this.easeMethod = new LTDescr.EaseTypeDelegate(this.tweenOnCurve);
		this.easeType = LeanTweenType.animationCurve;
		return this;
	}

	// Token: 0x06000146 RID: 326 RVA: 0x0000B62C File Offset: 0x0000982C
	public LTDescr setTo(Vector3 to)
	{
		if (this.hasInitiliazed)
		{
			this.to = to;
			this.diff = to - this.from;
		}
		else
		{
			this.to = to;
		}
		return this;
	}

	// Token: 0x06000147 RID: 327 RVA: 0x0000B659 File Offset: 0x00009859
	public LTDescr setTo(Transform to)
	{
		this._optional.toTrans = to;
		return this;
	}

	// Token: 0x06000148 RID: 328 RVA: 0x0000B668 File Offset: 0x00009868
	public LTDescr setFrom(Vector3 from)
	{
		if (this.trans)
		{
			this.init();
		}
		this.from = from;
		this.diff = this.to - this.from;
		this.diffDiv2 = this.diff * 0.5f;
		return this;
	}

	// Token: 0x06000149 RID: 329 RVA: 0x0000B6BD File Offset: 0x000098BD
	public LTDescr setFrom(float from)
	{
		return this.setFrom(new Vector3(from, 0f, 0f));
	}

	// Token: 0x0600014A RID: 330 RVA: 0x0000B6D5 File Offset: 0x000098D5
	public LTDescr setDiff(Vector3 diff)
	{
		this.diff = diff;
		return this;
	}

	// Token: 0x0600014B RID: 331 RVA: 0x0000B6DF File Offset: 0x000098DF
	public LTDescr setHasInitialized(bool has)
	{
		this.hasInitiliazed = has;
		return this;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x0000B6E9 File Offset: 0x000098E9
	public LTDescr setId(uint id, uint global_counter)
	{
		this._id = id;
		this.counter = global_counter;
		return this;
	}

	// Token: 0x0600014D RID: 333 RVA: 0x0000B6FA File Offset: 0x000098FA
	public LTDescr setPassed(float passed)
	{
		this.passed = passed;
		return this;
	}

	// Token: 0x0600014E RID: 334 RVA: 0x0000B704 File Offset: 0x00009904
	public LTDescr setTime(float time)
	{
		float num = this.passed / this.time;
		this.passed = time * num;
		this.time = time;
		return this;
	}

	// Token: 0x0600014F RID: 335 RVA: 0x0000B730 File Offset: 0x00009930
	public LTDescr setSpeed(float speed)
	{
		this.speed = speed;
		if (this.hasInitiliazed)
		{
			this.initSpeed();
		}
		return this;
	}

	// Token: 0x06000150 RID: 336 RVA: 0x0000B748 File Offset: 0x00009948
	public LTDescr setRepeat(int repeat)
	{
		this.loopCount = repeat;
		if ((repeat > 1 && this.loopType == LeanTweenType.once) || (repeat < 0 && this.loopType == LeanTweenType.once))
		{
			this.loopType = LeanTweenType.clamp;
		}
		if (this.type == TweenAction.CALLBACK || this.type == TweenAction.CALLBACK_COLOR)
		{
			this.setOnCompleteOnRepeat(true);
		}
		return this;
	}

	// Token: 0x06000151 RID: 337 RVA: 0x0000B79D File Offset: 0x0000999D
	public LTDescr setLoopType(LeanTweenType loopType)
	{
		this.loopType = loopType;
		return this;
	}

	// Token: 0x06000152 RID: 338 RVA: 0x0000B7A7 File Offset: 0x000099A7
	public LTDescr setUseEstimatedTime(bool useEstimatedTime)
	{
		this.useEstimatedTime = useEstimatedTime;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000153 RID: 339 RVA: 0x0000B7A7 File Offset: 0x000099A7
	public LTDescr setIgnoreTimeScale(bool useUnScaledTime)
	{
		this.useEstimatedTime = useUnScaledTime;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000154 RID: 340 RVA: 0x0000B7B8 File Offset: 0x000099B8
	public LTDescr setUseFrames(bool useFrames)
	{
		this.useFrames = useFrames;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000155 RID: 341 RVA: 0x0000B7C9 File Offset: 0x000099C9
	public LTDescr setUseManualTime(bool useManualTime)
	{
		this.useManualTime = useManualTime;
		this.usesNormalDt = false;
		return this;
	}

	// Token: 0x06000156 RID: 342 RVA: 0x0000B7DA File Offset: 0x000099DA
	public LTDescr setLoopCount(int loopCount)
	{
		this.loopType = LeanTweenType.clamp;
		this.loopCount = loopCount;
		return this;
	}

	// Token: 0x06000157 RID: 343 RVA: 0x0000B7EC File Offset: 0x000099EC
	public LTDescr setLoopOnce()
	{
		this.loopType = LeanTweenType.once;
		return this;
	}

	// Token: 0x06000158 RID: 344 RVA: 0x0000B7F7 File Offset: 0x000099F7
	public LTDescr setLoopClamp()
	{
		this.loopType = LeanTweenType.clamp;
		if (this.loopCount == 0)
		{
			this.loopCount = -1;
		}
		return this;
	}

	// Token: 0x06000159 RID: 345 RVA: 0x0000B811 File Offset: 0x00009A11
	public LTDescr setLoopClamp(int loops)
	{
		this.loopCount = loops;
		return this;
	}

	// Token: 0x0600015A RID: 346 RVA: 0x0000B81B File Offset: 0x00009A1B
	public LTDescr setLoopPingPong()
	{
		this.loopType = LeanTweenType.pingPong;
		if (this.loopCount == 0)
		{
			this.loopCount = -1;
		}
		return this;
	}

	// Token: 0x0600015B RID: 347 RVA: 0x0000B835 File Offset: 0x00009A35
	public LTDescr setLoopPingPong(int loops)
	{
		this.loopType = LeanTweenType.pingPong;
		this.loopCount = ((loops == -1) ? loops : (loops * 2));
		return this;
	}

	// Token: 0x0600015C RID: 348 RVA: 0x0000B850 File Offset: 0x00009A50
	public LTDescr setOnComplete(Action onComplete)
	{
		this._optional.onComplete = onComplete;
		this.hasExtraOnCompletes = true;
		return this;
	}

	// Token: 0x0600015D RID: 349 RVA: 0x0000B866 File Offset: 0x00009A66
	public LTDescr setOnComplete(Action<object> onComplete)
	{
		this._optional.onCompleteObject = onComplete;
		this.hasExtraOnCompletes = true;
		return this;
	}

	// Token: 0x0600015E RID: 350 RVA: 0x0000B87C File Offset: 0x00009A7C
	public LTDescr setOnComplete(Action<object> onComplete, object onCompleteParam)
	{
		this._optional.onCompleteObject = onComplete;
		this.hasExtraOnCompletes = true;
		if (onCompleteParam != null)
		{
			this._optional.onCompleteParam = onCompleteParam;
		}
		return this;
	}

	// Token: 0x0600015F RID: 351 RVA: 0x0000B8A1 File Offset: 0x00009AA1
	public LTDescr setOnCompleteParam(object onCompleteParam)
	{
		this._optional.onCompleteParam = onCompleteParam;
		this.hasExtraOnCompletes = true;
		return this;
	}

	// Token: 0x06000160 RID: 352 RVA: 0x0000B8B7 File Offset: 0x00009AB7
	public LTDescr setOnUpdate(Action<float> onUpdate)
	{
		this._optional.onUpdateFloat = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000161 RID: 353 RVA: 0x0000B8CD File Offset: 0x00009ACD
	public LTDescr setOnUpdateRatio(Action<float, float> onUpdate)
	{
		this._optional.onUpdateFloatRatio = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000162 RID: 354 RVA: 0x0000B8E3 File Offset: 0x00009AE3
	public LTDescr setOnUpdateObject(Action<float, object> onUpdate)
	{
		this._optional.onUpdateFloatObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000163 RID: 355 RVA: 0x0000B8F9 File Offset: 0x00009AF9
	public LTDescr setOnUpdateVector2(Action<Vector2> onUpdate)
	{
		this._optional.onUpdateVector2 = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000164 RID: 356 RVA: 0x0000B90F File Offset: 0x00009B0F
	public LTDescr setOnUpdateVector3(Action<Vector3> onUpdate)
	{
		this._optional.onUpdateVector3 = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000165 RID: 357 RVA: 0x0000B925 File Offset: 0x00009B25
	public LTDescr setOnUpdateColor(Action<Color> onUpdate)
	{
		this._optional.onUpdateColor = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000166 RID: 358 RVA: 0x0000B93B File Offset: 0x00009B3B
	public LTDescr setOnUpdateColor(Action<Color, object> onUpdate)
	{
		this._optional.onUpdateColorObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000167 RID: 359 RVA: 0x0000B925 File Offset: 0x00009B25
	public LTDescr setOnUpdate(Action<Color> onUpdate)
	{
		this._optional.onUpdateColor = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000168 RID: 360 RVA: 0x0000B93B File Offset: 0x00009B3B
	public LTDescr setOnUpdate(Action<Color, object> onUpdate)
	{
		this._optional.onUpdateColorObject = onUpdate;
		this.hasUpdateCallback = true;
		return this;
	}

	// Token: 0x06000169 RID: 361 RVA: 0x0000B951 File Offset: 0x00009B51
	public LTDescr setOnUpdate(Action<float, object> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateFloatObject = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x0600016A RID: 362 RVA: 0x0000B976 File Offset: 0x00009B76
	public LTDescr setOnUpdate(Action<Vector3, object> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateVector3Object = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x0600016B RID: 363 RVA: 0x0000B99B File Offset: 0x00009B9B
	public LTDescr setOnUpdate(Action<Vector2> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateVector2 = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x0600016C RID: 364 RVA: 0x0000B9C0 File Offset: 0x00009BC0
	public LTDescr setOnUpdate(Action<Vector3> onUpdate, object onUpdateParam = null)
	{
		this._optional.onUpdateVector3 = onUpdate;
		this.hasUpdateCallback = true;
		if (onUpdateParam != null)
		{
			this._optional.onUpdateParam = onUpdateParam;
		}
		return this;
	}

	// Token: 0x0600016D RID: 365 RVA: 0x0000B9E5 File Offset: 0x00009BE5
	public LTDescr setOnUpdateParam(object onUpdateParam)
	{
		this._optional.onUpdateParam = onUpdateParam;
		return this;
	}

	// Token: 0x0600016E RID: 366 RVA: 0x0000B9F4 File Offset: 0x00009BF4
	public LTDescr setOrientToPath(bool doesOrient)
	{
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			if (this._optional.path == null)
			{
				this._optional.path = new LTBezierPath();
			}
			this._optional.path.orientToPath = doesOrient;
		}
		else
		{
			this._optional.spline.orientToPath = doesOrient;
		}
		return this;
	}

	// Token: 0x0600016F RID: 367 RVA: 0x0000BA58 File Offset: 0x00009C58
	public LTDescr setOrientToPath2d(bool doesOrient2d)
	{
		this.setOrientToPath(doesOrient2d);
		if (this.type == TweenAction.MOVE_CURVED || this.type == TweenAction.MOVE_CURVED_LOCAL)
		{
			this._optional.path.orientToPath2d = doesOrient2d;
		}
		else
		{
			this._optional.spline.orientToPath2d = doesOrient2d;
		}
		return this;
	}

	// Token: 0x06000170 RID: 368 RVA: 0x0000BAA4 File Offset: 0x00009CA4
	public LTDescr setRect(LTRect rect)
	{
		this._optional.ltRect = rect;
		return this;
	}

	// Token: 0x06000171 RID: 369 RVA: 0x0000BAB3 File Offset: 0x00009CB3
	public LTDescr setRect(Rect rect)
	{
		this._optional.ltRect = new LTRect(rect);
		return this;
	}

	// Token: 0x06000172 RID: 370 RVA: 0x0000BAC7 File Offset: 0x00009CC7
	public LTDescr setPath(LTBezierPath path)
	{
		this._optional.path = path;
		return this;
	}

	// Token: 0x06000173 RID: 371 RVA: 0x0000BAD6 File Offset: 0x00009CD6
	public LTDescr setPoint(Vector3 point)
	{
		this._optional.point = point;
		return this;
	}

	// Token: 0x06000174 RID: 372 RVA: 0x0000BAE5 File Offset: 0x00009CE5
	public LTDescr setDestroyOnComplete(bool doesDestroy)
	{
		this.destroyOnComplete = doesDestroy;
		return this;
	}

	// Token: 0x06000175 RID: 373 RVA: 0x0000BAEF File Offset: 0x00009CEF
	public LTDescr setAudio(object audio)
	{
		this._optional.onCompleteParam = audio;
		return this;
	}

	// Token: 0x06000176 RID: 374 RVA: 0x0000BAFE File Offset: 0x00009CFE
	public LTDescr setOnCompleteOnRepeat(bool isOn)
	{
		this.onCompleteOnRepeat = isOn;
		return this;
	}

	// Token: 0x06000177 RID: 375 RVA: 0x0000BB08 File Offset: 0x00009D08
	public LTDescr setOnCompleteOnStart(bool isOn)
	{
		this.onCompleteOnStart = isOn;
		return this;
	}

	// Token: 0x06000178 RID: 376 RVA: 0x0000BB12 File Offset: 0x00009D12
	public LTDescr setRect(RectTransform rect)
	{
		this.rectTransform = rect;
		return this;
	}

	// Token: 0x06000179 RID: 377 RVA: 0x0000BB1C File Offset: 0x00009D1C
	public LTDescr setSprites(Sprite[] sprites)
	{
		this.sprites = sprites;
		return this;
	}

	// Token: 0x0600017A RID: 378 RVA: 0x0000BB26 File Offset: 0x00009D26
	public LTDescr setFrameRate(float frameRate)
	{
		this.time = (float)this.sprites.Length / frameRate;
		return this;
	}

	// Token: 0x0600017B RID: 379 RVA: 0x0000BB3A File Offset: 0x00009D3A
	public LTDescr setOnStart(Action onStart)
	{
		this._optional.onStart = onStart;
		return this;
	}

	// Token: 0x0600017C RID: 380 RVA: 0x0000BB4C File Offset: 0x00009D4C
	public LTDescr setDirection(float direction)
	{
		if (this.direction != -1f && this.direction != 1f)
		{
			Debug.LogWarning("You have passed an incorrect direction of '" + direction + "', direction must be -1f or 1f");
			return this;
		}
		if (this.direction != direction)
		{
			if (this.hasInitiliazed)
			{
				this.direction = direction;
			}
			else if (this._optional.path != null)
			{
				this._optional.path = new LTBezierPath(LTUtility.reverse(this._optional.path.pts));
			}
			else if (this._optional.spline != null)
			{
				this._optional.spline = new LTSpline(LTUtility.reverse(this._optional.spline.pts));
			}
		}
		return this;
	}

	// Token: 0x0600017D RID: 381 RVA: 0x0000BC10 File Offset: 0x00009E10
	public LTDescr setRecursive(bool useRecursion)
	{
		this.useRecursion = useRecursion;
		return this;
	}

	// Token: 0x0400009F RID: 159
	public bool toggle;

	// Token: 0x040000A0 RID: 160
	public bool useEstimatedTime;

	// Token: 0x040000A1 RID: 161
	public bool useFrames;

	// Token: 0x040000A2 RID: 162
	public bool useManualTime;

	// Token: 0x040000A3 RID: 163
	public bool usesNormalDt;

	// Token: 0x040000A4 RID: 164
	public bool hasInitiliazed;

	// Token: 0x040000A5 RID: 165
	public bool hasExtraOnCompletes;

	// Token: 0x040000A6 RID: 166
	public bool hasPhysics;

	// Token: 0x040000A7 RID: 167
	public bool onCompleteOnRepeat;

	// Token: 0x040000A8 RID: 168
	public bool onCompleteOnStart;

	// Token: 0x040000A9 RID: 169
	public bool useRecursion;

	// Token: 0x040000AA RID: 170
	public float ratioPassed;

	// Token: 0x040000AB RID: 171
	public float passed;

	// Token: 0x040000AC RID: 172
	public float delay;

	// Token: 0x040000AD RID: 173
	public float time;

	// Token: 0x040000AE RID: 174
	public float speed;

	// Token: 0x040000AF RID: 175
	public float lastVal;

	// Token: 0x040000B0 RID: 176
	private uint _id;

	// Token: 0x040000B1 RID: 177
	public int loopCount;

	// Token: 0x040000B2 RID: 178
	public uint counter = uint.MaxValue;

	// Token: 0x040000B3 RID: 179
	public float direction;

	// Token: 0x040000B4 RID: 180
	public float directionLast;

	// Token: 0x040000B5 RID: 181
	public float overshoot;

	// Token: 0x040000B6 RID: 182
	public float period;

	// Token: 0x040000B7 RID: 183
	public float scale;

	// Token: 0x040000B8 RID: 184
	public bool destroyOnComplete;

	// Token: 0x040000B9 RID: 185
	public Transform trans;

	// Token: 0x040000BA RID: 186
	internal Vector3 fromInternal;

	// Token: 0x040000BB RID: 187
	internal Vector3 toInternal;

	// Token: 0x040000BC RID: 188
	internal Vector3 diff;

	// Token: 0x040000BD RID: 189
	internal Vector3 diffDiv2;

	// Token: 0x040000BE RID: 190
	public TweenAction type;

	// Token: 0x040000BF RID: 191
	private LeanTweenType easeType;

	// Token: 0x040000C0 RID: 192
	public LeanTweenType loopType;

	// Token: 0x040000C1 RID: 193
	public bool hasUpdateCallback;

	// Token: 0x040000C2 RID: 194
	public LTDescr.EaseTypeDelegate easeMethod;

	// Token: 0x040000C5 RID: 197
	public SpriteRenderer spriteRen;

	// Token: 0x040000C6 RID: 198
	public RectTransform rectTransform;

	// Token: 0x040000C7 RID: 199
	public Text uiText;

	// Token: 0x040000C8 RID: 200
	public Image uiImage;

	// Token: 0x040000C9 RID: 201
	public RawImage rawImage;

	// Token: 0x040000CA RID: 202
	public Sprite[] sprites;

	// Token: 0x040000CB RID: 203
	public LTDescrOptional _optional = new LTDescrOptional();

	// Token: 0x040000CC RID: 204
	public static float val;

	// Token: 0x040000CD RID: 205
	public static float dt;

	// Token: 0x040000CE RID: 206
	public static Vector3 newVect;

	// Token: 0x02000280 RID: 640
	// (Invoke) Token: 0x06000D8E RID: 3470
	public delegate Vector3 EaseTypeDelegate();

	// Token: 0x02000281 RID: 641
	// (Invoke) Token: 0x06000D92 RID: 3474
	public delegate void ActionMethodDelegate();
}
