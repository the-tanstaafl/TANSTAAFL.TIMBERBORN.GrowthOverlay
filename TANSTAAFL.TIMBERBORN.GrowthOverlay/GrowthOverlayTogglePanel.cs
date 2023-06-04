using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.CoreUI;
using Timberborn.SingletonSystem;
using Timberborn.TooltipSystem;
using Timberborn.UILayoutSystem;
using UnityEngine.UIElements;

namespace TANSTAAFL.TIMBERBORN.GrowthOverlay
{
    internal class GrowthOverlayTogglePanel : ILoadableSingleton
    {
        private static readonly string GrowthOverlayClass = "stockpile-overlay-toggle-panel";

        private static readonly string ShowOverlayLocKey = "Inventory.StockpileOverlay.Show";

        private static readonly string HideOverlayLocKey = "Inventory.StockpileOverlay.Hide";

        private readonly GrowthOverlay _growthOverlay;

        private readonly VisualElementLoader _visualElementLoader;

        private readonly UILayout _uiLayout;

        private readonly ITooltipRegistrar _tooltipRegistrar;

        private GrowthOverlayToggle _growthOverlayToggle;

        private Toggle _toggle;

        private bool _enabled;

        private string TooltipLocKey
        {
            get
            {
                if (!_enabled)
                {
                    return ShowOverlayLocKey;
                }
                return HideOverlayLocKey;
            }
        }

        public GrowthOverlayTogglePanel(GrowthOverlay growthOverlay, VisualElementLoader visualElementLoader, UILayout uiLayout, ITooltipRegistrar tooltipRegistrar)
        {
            _growthOverlay = growthOverlay;
            _visualElementLoader = visualElementLoader;
            _uiLayout = uiLayout;
            _tooltipRegistrar = tooltipRegistrar;
        }

        public void Load()
        {
            VisualElement visualElement = _visualElementLoader.LoadVisualElement("Common/SquareToggle");
            _tooltipRegistrar.RegisterLocalizable(visualElement, () => TooltipLocKey);
            _toggle = visualElement.Q<Toggle>("Toggle");
            _toggle.AddToClassList(GrowthOverlayClass);
            _toggle.RegisterCallback(delegate (ChangeEvent<bool> changeEvent)
            {
                OnOverlayToggled(changeEvent.newValue);
            });
            UpdateToggle();
            _uiLayout.AddTopRightButton(visualElement, 3);
            _growthOverlayToggle = _growthOverlay.GetGrowthOverlayToggle();
        }

        private void OnOverlayToggled(bool showOverlay)
        {
            if (showOverlay)
            {
                _growthOverlayToggle.EnableOverlay();
                _enabled = true;
            }
            else
            {
                _growthOverlayToggle.DisableOverlay();
                _enabled = false;
            }
            UpdateToggle();
        }

        private void UpdateToggle()
        {
            _toggle.SetValueWithoutNotify(_enabled);
        }
    }
}
