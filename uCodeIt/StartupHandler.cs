using uCodeIt.DocumentTypes;
using uCodeIt.Strategies;
using Umbraco.Core;
using System.Linq;
using uCodeIt.Metadata;
using System.Reflection;
using uCodeIt.Importer;

namespace uCodeIt
{
    public class StartupHandler : IApplicationEventHandler
    {
        public void OnApplicationInitialized(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }

        public void OnApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            if (!applicationContext.IsConfigured || !CacheValidator.RebuildRequired())
                //The cache already exists and by extension has been run
                return;

            if (ExecutionStrategyFactory.Current.CanRun())
            {
                CacheValidator.EnsureInitialized();
                var types = CacheValidator.GetTypesToUpdate();

                var documentTypes = (from type in types
                                    let attr = type.GetCustomAttribute<DocumentTypeAttribute>(true)
                                    let name = string.IsNullOrEmpty(attr.Name) ? type.Name : attr.Name
                                    let alias = string.IsNullOrEmpty(attr.Alias) ? type.Name : attr.Alias
                                    let properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Select(property => new {
                                        Attribute = property.GetCustomAttribute<PropertyAttribute>(),
                                        Property = property
                                    })
                                    select new DocumentTypeMetadata
                                    {
                                        Name = name,
                                        Alias = alias,
                                        Description = attr.Description,
                                        Icon = attr.Icon ?? DocumentTypeAttribute.DefaultIcon,
                                        Thumbnail = attr.Thumbnail ?? DocumentTypeAttribute.DefaultThumbnail,
                                        AllowAsRoot = attr.AllowAsRoot,
                                        Properties = properties.Select(p => new PropertyMetadata {
                                            Name = string.IsNullOrEmpty(p.Attribute.Name) ? p.Property.Name : p.Attribute.Name,
                                            Alias = string.IsNullOrEmpty(p.Attribute.Alias) ? p.Property.Name : p.Attribute.Alias,
                                            Description = p.Attribute.Description,
                                            DataType = p.Attribute.DataType,
                                            Tab = p.Attribute.Tab
                                        }),
                                        Type = type,
                                        Attribute = attr
                                    }).ToArray();

                foreach (var documentType in documentTypes)
                    documentType.AllowedChildren = documentTypes.Where(t => documentType.Attribute.AllowedChildren.Any(x => x == t.Type));

                ImporterFactory.Instance.Process(documentTypes);

                CacheValidator.RebuildComplete(types);
            }
        }

        public void OnApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
        }
    }
}
