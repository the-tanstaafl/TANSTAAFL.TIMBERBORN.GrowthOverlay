using System;
using System.Collections.Generic;
using System.Text;

namespace TANSTAAFL.TIMBERBORN.GrowthOverlay
{
    internal class GrowthOverlayToggle
    {
        public bool Enabled { get; private set; }

        public bool Hidden { get; private set; }

        public event EventHandler StateChanged;

        public void EnableOverlay()
        {
            Enabled = true;
            this.StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void DisableOverlay()
        {
            Enabled = false;
            this.StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void HideOverlay()
        {
            Hidden = true;
            this.StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void ShowOverlay()
        {
            Hidden = false;
            this.StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
