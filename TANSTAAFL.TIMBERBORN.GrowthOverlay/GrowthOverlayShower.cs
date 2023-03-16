using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.InputSystem;
using Timberborn.SingletonSystem;

namespace TANSTAAFL.TIMBERBORN.GrowthOverlay
{
    internal class GrowthOverlayShower : ILoadableSingleton, IInputProcessor
    {
        private readonly GrowthOverlay _growthOverlay;

        private readonly InputService _inputService;

        private GrowthOverlayToggle _growthOverlayToggle;

        private bool _isShown;

        public GrowthOverlayShower(GrowthOverlay growthOverlay, InputService inputService)
        {
            _growthOverlay = growthOverlay;
            _inputService = inputService;
        }

        public void Load()
        {
            _growthOverlayToggle = _growthOverlay.GetGrowthOverlayToggle();
            _inputService.AddInputProcessor(this);
        }

        public bool ProcessInput()
        {
            //if (_inputService.ShowGrowthOverlay)
            //{
            //    if (!_isShown)
            //    {
            //        Enable();
            //    }
            //}
            //else if (_isShown)
            //{
            //    Disable();
            //}
            return false;
        }

        private void Enable()
        {
            _isShown = true;
            _growthOverlayToggle.EnableOverlay();
        }

        private void Disable()
        {
            _isShown = false;
            _growthOverlayToggle.DisableOverlay();
        }
    }
}
