using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.CameraSystem;
using Timberborn.Common;
using Timberborn.CoreUI;
using Timberborn.SettingsSystem;
using Timberborn.SingletonSystem;
using Timberborn.StockpilesUI;
using Timberborn.TickSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TANSTAAFL.TIMBERBORN.GrowthOverlay
{
    internal class GrowthOverlay : ILoadableSingleton, ILateUpdatableSingleton
    {
        private readonly Underlay _underlay;

        private readonly CameraComponent _cameraComponent;

        private readonly UISettings _uiSettings;

        private readonly Dictionary<VisualElement, Vector3> _items = new Dictionary<VisualElement, Vector3>();

        private readonly List<GrowthOverlayToggle> _toggles = new List<GrowthOverlayToggle>();

        private bool _enabled;

        private bool _isDirty;

        public GrowthOverlay(Underlay underlay, CameraComponent cameraComponent, UISettings uiSettings)
        {
            _underlay = underlay;
            _cameraComponent = cameraComponent;
            _uiSettings = uiSettings;
        }

        public void Load()
        {
            _cameraComponent.CameraPositionOrRotationChanged += delegate
            {
                UpdatePosition();
            };
            _uiSettings.UIScaleFactorChanged += delegate
            {
                UpdatePosition();
            };
        }

        public void LateUpdateSingleton()
        {
            if (_isDirty)
            {
                UpdatePosition();
            }
        }

        public GrowthOverlayToggle GetGrowthOverlayToggle()
        {
            GrowthOverlayToggle growthOverlayToggle = new GrowthOverlayToggle();
            _toggles.Add(growthOverlayToggle);
            growthOverlayToggle.StateChanged += delegate
            {
                UpdateOverlay();
            };
            return growthOverlayToggle;
        }

        public void Add(VisualElement element, Vector3 anchor)
        {
            if (_items.TryAdd(element, anchor) && _enabled)
            {
                _underlay.Add(element);
                _isDirty = true;
            }
        }

        public void Remove(VisualElement element)
        {
            if (_items.Remove(element) && _enabled)
            {
                _underlay.Remove(element);
            }
        }

        private void UpdatePosition()
        {
            if (_enabled)
            {
                foreach (KeyValuePair<VisualElement, Vector3> item2 in _items)
                {
                    item2.Deconstruct(out var key, out var value);
                    VisualElement item = key;
                    Vector3 anchor = value;
                    UpdatePosition(item, anchor);
                }
            }
            _isDirty = false;
        }

        private void UpdateOverlay()
        {
            bool flag = _toggles.FastAny((GrowthOverlayToggle toggle) => toggle.Enabled) && _toggles.FastAll((GrowthOverlayToggle toggle) => !toggle.Hidden);
            if (flag && !_enabled)
            {
                Enable();
            }
            else if (!flag && _enabled)
            {
                Disable();
            }
        }

        private void Enable()
        {
            _enabled = true;
            foreach (VisualElement key in _items.Keys)
            {
                _underlay.Add(key);
            }
            _isDirty = true;
        }

        private void Disable()
        {
            _enabled = false;
            foreach (VisualElement key in _items.Keys)
            {
                _underlay.Remove(key);
            }
        }

        private void UpdatePosition(VisualElement item, Vector3 anchor)
        {
            VisualElement root = _underlay.Root;
            if (item.panel != null)
            {
                bool flag = _cameraComponent.IsInFront(anchor);
                item.ToggleDisplayStyle(flag);
                if (flag)
                {
                    Vector3 vector = _cameraComponent.WorldSpaceToPanelSpace(root, anchor);
                    item.transform.position = new Vector2(vector.x - root.layout.width / 2f, vector.y - root.layout.height / 2f);
                }
            }
        }

        internal bool IsEnabled { get { return _enabled; } }
    }
}
