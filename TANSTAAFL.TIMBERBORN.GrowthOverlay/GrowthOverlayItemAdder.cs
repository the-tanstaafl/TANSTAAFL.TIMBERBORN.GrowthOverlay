using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.UiBuilderSystem;
using Timberborn.BaseComponentSystem;
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
using static UnityEngine.UIElements.Length.Unit;

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

        private Button _item;
        //private Label _itemText;

        //private VisualElement _fillLevel;
        private UIBuilder _builder;

        [Inject]
        public void InjectDependencies(GrowthOverlay growthOverlay, VisualElementLoader visualElementLoader, IGoodService goodService, EntitySelectionService selectionManager, UIBuilder uIBuilder)
        {
            _growthOverlay = growthOverlay;
            _visualElementLoader = visualElementLoader;
            _selectionManager = selectionManager;
            _builder = uIBuilder;
        }

        public void Awake()
        {
            _blockObjectCenter = GetComponentFast<BlockObjectCenter>();
            _growable = GetComponentFast<Growable>();
            _livingNaturalResource = GetComponentFast<LivingNaturalResource>();
            var img = new Image();
            img.sprite = _growable.GetComponentFast<LabeledPrefab>().Image;

            var component = _builder.CreateComponentBuilder()
                .CreateButton()
                .SetName("GrowthOverlayButton")
                .SetHeight(new Length(20, Pixel))
                .SetWidth(new Length(20, Pixel))
                .SetBackgroundColor(new StyleColor(new Color(0.0f, 0.6f, 0.0f, 0.8f)))
                .SetPadding(new Padding(1, 1, 1, 1))
                .AddComponent(_builder.CreateComponentBuilder()
                    .CreateVisualElement()
                    .SetHeight(new Length(18, Pixel))
                    .SetWidth(new Length(18, Pixel))
                    .AddComponent(img)
                    .Build()
                )
                //.AddComponent(_builder.CreateComponentBuilder()
                //    .CreateLabel()
                //    .SetName("GrowthOverlayLabel")
                //    .SetHeight(new Length(18, Pixel))
                //    .SetWidth(new Length(20, Pixel))
                //    .Build()
                //)
                .BuildAndInitialize();

            _item = component.Q<Button>("GrowthOverlayButton");
            _item.clicked += delegate
            {
                _selectionManager.Select(_growable.GameObjectFast.GetComponent<BaseComponent>());
            };
            //_itemText = _item.Q<Label>("GrowthOverlayLabel");

            // Update 3 code
            //VisualElement e = _visualElementLoader.LoadVisualElement("Master/StockpileOverlayItem");
            //_item = e.Q<Button>("StockpileOverlayItem");
            //_item.clicked += delegate
            //{
            //    _selectionManager.Select(_growable.GameObjectFast.GetComponent<BaseComponent>());
            //};
            //_itemIcon = _item.Q<Image>("Icon");
            //_itemText = _item.Q<Label>("Stock");
            //_fillLevel = _item.Q<VisualElement>("Progress");
            //_fillLevel.parent.parent.Remove(_fillLevel.parent);
        }

        public void InitializeEntity()
        {
            if (!_livingNaturalResource.IsDead)
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
            //_itemText.text = $"{(float)Math.Floor(_growable.GrowthProgress * 100)}%";
            //_itemText.ToggleDisplayStyle(visible: true);
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
