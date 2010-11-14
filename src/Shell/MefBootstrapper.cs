using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Reflection;
using Caliburn.Micro;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition;

namespace ILoveLucene
{
    public class MefBootstrapper : Bootstrapper<IShell>
    {
        private CompositionContainer container;

        protected override void Configure()
        {
            container =
                CompositionHost.Initialize(
                    new AggregateCatalog(AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>()));

            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(container);

            container.Compose(batch);
        }

        protected override object GetInstance(System.Type service, string key)
        {
            string contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;
            var exports = container.GetExportedValues<object>(contract);

            if (exports.Count() > 0)
            {
                return exports.First();
            }

            throw new Exception(string.Format("Could not locate any instances of contract {0}", contract));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetExportedValues<object>(AttributedModelServices.GetContractName(service));
        }

        protected override void BuildUp(object instance)
        {
            container.SatisfyImportsOnce(instance);
        }

        protected override IEnumerable<System.Reflection.Assembly> SelectAssemblies()
        {
            // TODO: load more assemblies?
            return new[]
                       {
                           Assembly.GetExecutingAssembly()
                       };
        }

    }
}