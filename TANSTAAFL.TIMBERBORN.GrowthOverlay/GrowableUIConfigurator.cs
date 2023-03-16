using Bindito.Core;
using System;
using System.Collections.Generic;
using System.Text;
using TimberApi.ConfiguratorSystem;
using TimberApi.SceneSystem;
using Timberborn.EntityPanelSystem;
using Timberborn.Growing;
using Timberborn.TemplateSystem;

namespace TANSTAAFL.TIMBERBORN.GrowthOverlay
{
    [Configurator(SceneEntrypoint.InGame)]
    public class GrowableUIConfigurator : IConfigurator
    {
        public void Configure(IContainerDefinition containerDefinition)
        {
            containerDefinition.Bind<GrowthOverlay>().AsSingleton();
            containerDefinition.Bind<GrowthOverlayShower>().AsSingleton();
            containerDefinition.Bind<GrowthOverlayTogglePanel>().AsSingleton();
            containerDefinition.Bind<GrowthOverlayHider>().AsSingleton();
            containerDefinition.MultiBind<TemplateModule>().ToProvider(ProvideTemplateModule).AsSingleton();
        }

        private static TemplateModule ProvideTemplateModule()
        {
            TemplateModule.Builder builder = new TemplateModule.Builder();
            builder.AddDecorator<Growable, GrowthOverlayItemAdder>();
            return builder.Build();
        }
    }
}
