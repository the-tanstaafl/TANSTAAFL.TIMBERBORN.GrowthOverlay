using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TimberApi.UiBuilderSystem;
using Timberborn.BlockSystem;
using Timberborn.CoreUI;
using Timberborn.EntitySystem;
using Timberborn.Goods;
using Timberborn.Growing;
using Timberborn.NaturalResourcesLifeCycle;
using Timberborn.PrefabSystem;
using Timberborn.SelectionSystem;
using Timberborn.TickSystem;
using UnityEngine;
using UnityEngine.UIElements;

namespace TANSTAAFL.TIMBERBORN.GrowthOverlay
{
    internal class GrowthOverlayItemAdder : TickableComponent, IInitializableEntity, IDeletableEntity
    {
        private GrowthOverlay _growthOverlay;

        private VisualElementLoader _visualElementLoader;

        private EntitySelectionService _selectionManager;

        private BlockObjectCenter _blockObjectCenter;

        private Growable _growable;

        private LivingNaturalResource _livingNaturalResource;

        private VisualElement _item;
        private Image _itemIcon;
        private Label _itemText;
        private VisualElement _fillLevel;

        [Inject]
        public void InjectDependencies(GrowthOverlay growthOverlay, VisualElementLoader visualElementLoader, EntitySelectionService selectionManager)
        {
            _growthOverlay = growthOverlay;
            _visualElementLoader = visualElementLoader;
            _selectionManager = selectionManager;
        }

        public void Awake()
        {
            _blockObjectCenter = GetComponentFast<BlockObjectCenter>();
            _growable = GetComponentFast<Growable>();
            _livingNaturalResource = GetComponentFast<LivingNaturalResource>();

            _item = _visualElementLoader.LoadVisualElement("Game/StockpileOverlayItem");

            var btn = _item.Q<Button>("EntityButton");

            var selectionButton = _item.Q<NineSliceButton>("SelectionButton");

            selectionButton.clicked += delegate
            {
                _selectionManager.Select(_growable);
            };

            btn.clicked += delegate
            {
                _selectionManager.Select(_growable);
            };

            _itemIcon = _item.Q<Image>("Icon");
            _itemIcon.sprite = _growable.GetComponentFast<LabeledPrefab>().Image;
            _itemIcon.AddToClassList("icon--hidden");

            _itemText = _item.Q<Label>("Stock");

            _fillLevel = _item.Q<VisualElement>("Progress");
            _fillLevel.parent.parent.Remove(_fillLevel.parent);
        }

        public void InitializeEntity()
        {
            if (!_livingNaturalResource.IsDead && _livingNaturalResource.isActiveAndEnabled)
            {
                Add();
                UpdateGrowth();
            }
        }

        public void DeleteEntity()
        {
            _growthOverlay.Remove(_item);
        }

        private void UpdateGrowth()
        {
            _itemText.text = $"{(float)Math.Floor(_growable.GrowthProgress * 100)}%";
            _itemText.ToggleDisplayStyle(visible: true);
        }

        private void Add()
        {
            Vector3 worldCenter = _blockObjectCenter.WorldCenter;
            Vector3 worldCenterGrounded = _blockObjectCenter.WorldCenterGrounded;
            float y = (worldCenter.y + worldCenterGrounded.y) * 0.5f;
            Vector3 anchor = new Vector3(worldCenter.x, y, worldCenter.z);
            _growthOverlay.Add(_item, anchor);
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
