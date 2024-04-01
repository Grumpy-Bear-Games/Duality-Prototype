//======================================================================================
// This VisualElement is based on AspectRatioPadding from
// https://github.com/plyoung/UIElements
//
// All credit for the idea goes to Leslie Young (plyoung)
//======================================================================================

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
	public class AspectRatioContainer : VisualElement
	{
		private int _ratioWidth = 16;
		private int _ratioHeight = 9;
		private VerticalAlignment _verticalAlign = VerticalAlignment.Middle;
		private HorizontalAlignment _horizontalAlign = HorizontalAlignment.Middle;
		private readonly VisualElement _contentContainer;

		public static readonly string UssBaseClass = "aspect-ratio-container";
		public static readonly string UssContentClass = $"{UssBaseClass}__content";

		public new class UxmlFactory : UxmlFactory<AspectRatioContainer, UxmlTraits> { }

		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			private readonly UxmlIntAttributeDescription _ratioWidth = new() { name = "ratio-width", defaultValue = 16 };
			private readonly UxmlIntAttributeDescription _ratioHeight = new() { name = "ratio-height", defaultValue = 9 };
			
			private readonly UxmlEnumAttributeDescription<VerticalAlignment> _verticalAlign = new()
				{ name = "vertical-align", defaultValue = VerticalAlignment.Middle };
			private readonly UxmlEnumAttributeDescription<HorizontalAlignment> _horizontalAlign = new()
				{ name = "horizontal-align", defaultValue = HorizontalAlignment.Middle };

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				if (ve is not AspectRatioContainer aspectRatioPadding) return;
				aspectRatioPadding.RatioWidth = _ratioWidth.GetValueFromBag(bag, cc);
				aspectRatioPadding.RatioHeight = _ratioHeight.GetValueFromBag(bag, cc);
				aspectRatioPadding.VerticalAlign = _verticalAlign.GetValueFromBag(bag, cc);
				aspectRatioPadding.HorizontalAlign = _horizontalAlign.GetValueFromBag(bag, cc);
			}
		}

		public override VisualElement contentContainer => _contentContainer;

		public int RatioWidth
		{
			get => _ratioWidth;
			set
			{
				_ratioWidth = value;
				UpdateElements();
			}
		}

		public int RatioHeight
		{
			get => _ratioHeight;
			set
			{
				_ratioHeight = value;
				UpdateElements();
			}
		}
		
		public VerticalAlignment VerticalAlign
		{
			get => _verticalAlign;
			private set
			{
				_verticalAlign = value;
				UpdateElements();
			}
		}

		public HorizontalAlignment HorizontalAlign
		{
			get => _horizontalAlign;
			private set
			{
				_horizontalAlign = value;
				UpdateElements();
			}
		}

		public AspectRatioContainer()
		{
			AddToClassList(UssBaseClass);

			_contentContainer = new VisualElement
			{
				name = "Content",
				style = {
					flexGrow = 1,
					flexBasis = 0,
				}
			};
			_contentContainer.AddToClassList(UssContentClass);
			hierarchy.Add(_contentContainer);

			RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
		}

		private void OnAttachToPanel(AttachToPanelEvent attachToPanelEvent) => UpdateElements();

		private void OnGeometryChange(GeometryChangedEvent _) => UpdateElements();
	
		private void UpdateElements()
		{
			if (RatioWidth <= 0.0f || RatioHeight <= 0.0f)
			{
				style.marginLeft = 0f;
				style.marginRight = 0f; 
				Debug.LogError($"[{GetType().Name}] Invalid width:{RatioWidth} or height:{RatioHeight}");
				return;
			}

			if (float.IsNaN(resolvedStyle.width) || float.IsNaN(resolvedStyle.height))  return;

			var designRatio = (float)RatioWidth / RatioHeight;
			var currRatio = resolvedStyle.width / resolvedStyle.height;

			if (designRatio > currRatio)
			{
				var verticalPadding = (resolvedStyle.height - (resolvedStyle.width / designRatio));
				switch (VerticalAlign)
				{
					case VerticalAlignment.Top:
						SetSpacing(0f, verticalPadding, 0f, 0f);
						break;
					case VerticalAlignment.Middle:
						SetSpacing(verticalPadding * 0.5f, verticalPadding * 0.5f, 0f, 0f);
						break;
					case VerticalAlignment.Bottom:
						SetSpacing(verticalPadding, 0f , 0f, 0f);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
			else
			{
				var horizontalPadding = resolvedStyle.width - (designRatio * resolvedStyle.height);
				switch (HorizontalAlign)
				{
					case HorizontalAlignment.Left:
						SetSpacing(0f, 0f, 0f, horizontalPadding);
						break;
					case HorizontalAlignment.Middle:
						SetSpacing(0f, 0f, horizontalPadding * 0.5f, horizontalPadding * 0.5f);
						break;
					case HorizontalAlignment.Right:
						SetSpacing(0f, 0f, horizontalPadding, 0f);
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		private void SetSpacing(float top = 0f, float bottom = 0f, float left = 0f, float right = 0f)
		{
			style.paddingTop = top;
			style.paddingBottom = bottom;
			style.paddingLeft = left;
			style.paddingRight = right;
		}

		public enum VerticalAlignment
		{
			Top,
			Middle,
			Bottom,
		}

		public enum HorizontalAlignment
		{
			Left,
			Middle,
			Right,
		}
	}
}
