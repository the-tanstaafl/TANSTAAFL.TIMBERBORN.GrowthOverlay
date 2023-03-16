using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.CoreUI;
using Timberborn.SingletonSystem;

namespace TANSTAAFL.TIMBERBORN.GrowthOverlay
{
    internal class GrowthOverlayHider : ILoadableSingleton
    {
        private readonly EventBus _eventBus;

        private readonly GrowthOverlay _growthOverlay;

        private GrowthOverlayToggle _growthOverlayToggle;

        public GrowthOverlayHider(EventBus eventBus, GrowthOverlay growthOverlay)
        {
            _eventBus = eventBus;
            _growthOverlay = growthOverlay;
        }

        public void Load()
        {
            _eventBus.Register(this);
            _growthOverlayToggle = _growthOverlay.GetGrowthOverlayToggle();
        }

        [OnEvent]
        public void OnUIVisibilityChanged(UIVisibilityChangedEvent uiVisibilityChangedEvent)
        {
            if (uiVisibilityChangedEvent.UIVisible)
            {
                _growthOverlayToggle.ShowOverlay();
            }
            else
            {
                _growthOverlayToggle.HideOverlay();
            }
        }
    }
}
