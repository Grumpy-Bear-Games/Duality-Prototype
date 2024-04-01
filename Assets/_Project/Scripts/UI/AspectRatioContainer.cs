//======================================================================================
// Based on https://docs.unity3d.com/2023.2/Documentation/Manual/UIE-create-aspect-ratios-custom-control.html
//======================================================================================

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DualityGame.UI
{
	[UxmlElement]
	public partial class AspectRatioContainer : VisualElement
	{
		#region Enums
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
		#endregion


		#region UXML Attributes
		[UxmlAttribute("ratio-width")]
		public int RatioWidth
		{
			get => _ratioWidth;
			set
			{
				_ratioWidth = Mathf.Max(value, 0);
				UpdateElements();
			}
		}

		[UxmlAttribute("ratio-height")]
		public int RatioHeight
		{
			get => _ratioHeight;
			set
			{
				_ratioHeight = Mathf.Max(value, 0);
				UpdateElements();
			}
		}

		[UxmlAttribute("vertical-align")]
		public VerticalAlignment VerticalAlign
		{
			get => _verticalAlign;
			private set
			{
				_verticalAlign = value;
				UpdateElements();
			}
		}

		[UxmlAttribute("horizontal-align")]
		public HorizontalAlignment HorizontalAlign
		{
			get => _horizontalAlign;
			private set
			{
				_horizontalAlign = value;
				UpdateElements();
			}
		}
		#endregion


		#region Private Fields
		private int _ratioWidth = 16;
		private int _ratioHeight = 9;
		private VerticalAlignment _verticalAlign = VerticalAlignment.Middle;
		private HorizontalAlignment _horizontalAlign = HorizontalAlignment.Middle;
		#endregion


		#region Implementation
		public AspectRatioContainer()
		{
			style.flexGrow = 1;
			RegisterCallback<GeometryChangedEvent>(OnGeometryChange);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
		}

		private void OnAttachToPanel(AttachToPanelEvent attachToPanelEvent) => UpdateElements();

		private void OnGeometryChange(GeometryChangedEvent _) => UpdateElements();

		private void UpdateElements()
		{
			if (RatioWidth <= 0.0f || RatioHeight <= 0.0f)
			{
				SetSpacing();
				Debug.LogError($"[{GetType().Name}] Invalid width:{RatioWidth} or height:{RatioHeight}");
				return;
			}

			if (float.IsNaN(resolvedStyle.width) || float.IsNaN(resolvedStyle.height))  return;

			var designRatio = (float)RatioWidth / RatioHeight;
			var currRatio = resolvedStyle.width / resolvedStyle.height;

			switch (currRatio - designRatio)
			{
				case < -0.01f:
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

					break;
				}
				case > 0.01f:
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

					break;
				}
				default:
					SetSpacing();
					break;
			}
		}

		private void SetSpacing(float top = 0f, float bottom = 0f, float left = 0f, float right = 0f)
		{
			style.paddingTop = top;
			style.paddingBottom = bottom;
			style.paddingLeft = left;
			style.paddingRight = right;
		}
		#endregion


	}
}
