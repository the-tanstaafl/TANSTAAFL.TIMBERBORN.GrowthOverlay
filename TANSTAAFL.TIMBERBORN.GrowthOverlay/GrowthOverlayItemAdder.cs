using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Timberborn.BlockSystem;
using Timberborn.Buildings;
using Timberborn.ConstructibleSystem;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using Timberborn.Goods;
using Timberborn.Growing;
using Timberborn.InventorySystem;
using Timberborn.NaturalResourcesLifeCycle;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using Timberborn.SoilMoistureSystem;
using Timberborn.TickSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TANSTAAFL.TIMBERBORN.GrowthOverlay
{
    internal class GrowthOverlayItemAdder : TickableComponent, IInitializableEntity, IDeletableEntity
    {
        private static readonly string IconHiddenClass = "icon--hidden";

        private GrowthOverlay _growthOverlay;

        private VisualElementLoader _visualElementLoader;

        private SelectionManager _selectionManager;

        private BlockObjectCenter _blockObjectCenter;

        private Growable _growable;

        private LivingNaturalResource _livingNaturalResource;

        private Button _item;

        private Image _itemIcon;

        private Label _itemText;

        private VisualElement _fillLevel;

        [Inject]
        public void InjectDependencies(GrowthOverlay growthOverlay, VisualElementLoader visualElementLoader, IGoodService goodService, SelectionManager selectionManager)
        {
            _growthOverlay = growthOverlay;
            _visualElementLoader = visualElementLoader;
            _selectionManager = selectionManager;
        }

        public void Awake()
        {
            _blockObjectCenter = GetComponent<BlockObjectCenter>();
            _growable = GetComponent<Growable>();
            _livingNaturalResource = GetComponent<LivingNaturalResource>();
            VisualElement e = _visualElementLoader.LoadVisualElement("Master/StockpileOverlayItem");
            _item = e.Q<Button>("StockpileOverlayItem");
            _item.clicked += delegate
            {
                _selectionManager.Select(_growable.gameObject);
            };
            _itemIcon = _item.Q<Image>("Icon");
            _itemText = _item.Q<Label>("Stock");
            _fillLevel = _item.Q<VisualElement>("Progress");
            _fillLevel.parent.parent.Remove(_fillLevel.parent);
        }

        public void InitializeEntity()
        {
            if (!_livingNaturalResource.IsDead)
            {
                Add();
                UpdateIcon();
                UpdateGrowth();
            }
        }

        public void DeleteEntity()
        {
            Remove();
        }

        private void UpdateIcon()
        {
            _itemIcon.sprite = _growable.GetComponent<LabeledPrefab>().Image;
            _itemIcon.AddToClassList(IconHiddenClass);
        }

        private void UpdateGrowth()
        {
            var num = _growable.GrowthProgress;
            _itemText.text = $"{(float)Math.Floor(_growable.GrowthProgress * 100)}%";
            _itemText.ToggleDisplayStyle(visible: true);
            //_fillLevel.SetHeightAsPercent(num);
            _fillLevel.parent.ToggleDisplayStyle(visible: true);
        }

        private void Add()
        {
            Vector3 worldCenter = _blockObjectCenter.WorldCenter;
            Vector3 worldCenterGrounded = _blockObjectCenter.WorldCenterGrounded;
            float y = (worldCenter.y + worldCenterGrounded.y) * 0.5f;
            Vector3 anchor = new Vector3(worldCenter.x, y, worldCenter.z);
            _growthOverlay.Add(_item, anchor);
        }

        private void Remove()
        {
            _growthOverlay.Remove(_item);
        }

        public override void Tick()
        {
            if (_growthOverlay.IsEnabled && !_livingNaturalResource.IsDead)
            {
                UpdateGrowth();
            }
        }
    }
}
