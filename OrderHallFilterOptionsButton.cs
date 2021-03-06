using System;
using UnityEngine;
using UnityEngine.UI;
using WowStatConstants;

public class OrderHallFilterOptionsButton : MonoBehaviour
{
	public Image m_normalImage;

	public Image m_selectedImage;

	public string m_labelTag;

	public string m_labelFallbackText;

	public GameObject m_labelArea;

	public Text m_label;

	public LayoutElement m_holderLayoutElement;

	private float m_selectedSize = 54f;

	private float m_normalSize = 40f;

	public float m_resizeDuration;

	public GameObject m_selectionGlowRoot;

	public MapFilterType m_mapFilterType;

	private UiAnimMgr.UiAnimHandle m_glowSpinHandle;

	private UiAnimMgr.UiAnimHandle m_glowPulseHandle;

	private bool m_isSelected;

	private void Start()
	{
		this.m_label.set_font(GeneralHelpers.LoadStandardFont());
		this.m_label.set_text(StaticDB.GetString(this.m_labelTag, this.m_labelFallbackText));
	}

	private void OnEnable()
	{
		Main expr_05 = Main.instance;
		expr_05.OrderHallfilterOptionsButtonSelectedAction = (Action<OrderHallFilterOptionsButton>)Delegate.Combine(expr_05.OrderHallfilterOptionsButtonSelectedAction, new Action<OrderHallFilterOptionsButton>(this.HandleOrderHallfilterOptionsButtonSelected));
	}

	private void OnDisable()
	{
		Main expr_05 = Main.instance;
		expr_05.OrderHallfilterOptionsButtonSelectedAction = (Action<OrderHallFilterOptionsButton>)Delegate.Remove(expr_05.OrderHallfilterOptionsButtonSelectedAction, new Action<OrderHallFilterOptionsButton>(this.HandleOrderHallfilterOptionsButtonSelected));
	}

	private void StopGlowEffect()
	{
		if (this.m_glowSpinHandle != null)
		{
			UiAnimation anim = this.m_glowSpinHandle.GetAnim();
			if (anim != null)
			{
				anim.Stop(0.5f);
			}
			this.m_glowSpinHandle = null;
		}
		if (this.m_glowPulseHandle != null)
		{
			UiAnimation anim2 = this.m_glowPulseHandle.GetAnim();
			if (anim2 != null)
			{
				anim2.Stop(0.5f);
			}
			this.m_glowPulseHandle = null;
		}
	}

	private void OnResizeUpdate(float newSize)
	{
		this.m_holderLayoutElement.set_minWidth(newSize);
		this.m_holderLayoutElement.set_minHeight(newSize);
	}

	private void OnResizeUpComplete()
	{
		this.m_holderLayoutElement.set_minWidth(this.m_selectedSize);
		this.m_holderLayoutElement.set_minHeight(this.m_selectedSize);
	}

	private void OnResizeDownComplete()
	{
		this.m_holderLayoutElement.set_minWidth(this.m_normalSize);
		this.m_holderLayoutElement.set_minHeight(this.m_normalSize);
	}

	private void ResizeForSelect()
	{
		iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"name",
			"ScaleUpForSelect",
			"from",
			this.m_normalSize,
			"to",
			this.m_selectedSize,
			"time",
			this.m_resizeDuration,
			"onupdate",
			"OnResizeUpdate",
			"oncomplete",
			"OnResizeUpComplete"
		}));
	}

	private void ResizeForDeselect()
	{
		iTween.ValueTo(base.get_gameObject(), iTween.Hash(new object[]
		{
			"name",
			"ScaleUpForDeselect",
			"from",
			this.m_selectedSize,
			"to",
			this.m_normalSize,
			"time",
			this.m_resizeDuration,
			"onupdate",
			"OnResizeUpdate",
			"oncomplete",
			"OnResizeDownComplete"
		}));
	}

	private void HandleOrderHallfilterOptionsButtonSelected(OrderHallFilterOptionsButton navButton)
	{
		if (navButton == this)
		{
			if (!this.m_isSelected)
			{
				this.m_normalImage.set_enabled(false);
				this.m_selectedImage.set_enabled(true);
				this.StopGlowEffect();
				this.m_glowSpinHandle = UiAnimMgr.instance.PlayAnim("PrestigeSpin", this.m_selectionGlowRoot.get_transform(), Vector3.get_zero(), 1.245f, 0f);
				this.m_glowPulseHandle = UiAnimMgr.instance.PlayAnim("PrestigePulse", this.m_selectionGlowRoot.get_transform(), Vector3.get_zero(), 1.245f, 0f);
				UiAnimMgr.instance.PlayAnim("MinimapPulseAnim", base.get_transform(), Vector3.get_zero(), 1.5f, 0f);
				this.m_labelArea.SetActive(true);
				this.ResizeForSelect();
				this.m_isSelected = true;
			}
		}
		else
		{
			this.m_labelArea.SetActive(false);
			this.m_normalImage.set_enabled(true);
			this.m_selectedImage.set_enabled(false);
			if (this.m_isSelected)
			{
				this.StopGlowEffect();
				this.ResizeForDeselect();
				this.m_isSelected = false;
			}
		}
	}

	public void SelectMe()
	{
		Main.instance.SelectOrderHallFilterOptionsButton(this);
	}
}
