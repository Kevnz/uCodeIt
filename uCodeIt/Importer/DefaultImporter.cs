using System.Collections.Generic;
using System.Linq;
using uCodeIt.Metadata;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;

namespace uCodeIt.Importer
{
    internal class DefaultImporter : IImporter
    {
        public IContentTypeService ContentTypeService { get; private set; }
        public IDataTypeService DataTypeService { get; private set; }

        protected internal DefaultImporter()
            : this(ApplicationContext.Current.Services.ContentTypeService, ApplicationContext.Current.Services.DataTypeService)
        {
        }

        protected DefaultImporter(IContentTypeService contentTypeService, IDataTypeService dataTypeService)
        {
            ContentTypeService = contentTypeService;
            DataTypeService = dataTypeService;
        }

        public void Process(IEnumerable<DocumentTypeMetadata> types)
        {
            var contentTypes = new List<IContentType>();
            var dataTypeDefinitions = DataTypeService.GetAllDataTypeDefinitions().ToArray();

            foreach (var type in types)
            {
                var ct = ContentTypeService.GetContentType(type.Alias) ?? new ContentType(-1);

                ct.Name = type.Name;
                ct.Alias = type.Alias;
                ct.AllowedAsRoot = type.AllowAsRoot;
                ct.Description = type.Description;
                ct.Icon = type.Icon;
                ct.Thumbnail = type.Thumbnail;

                var properties = ct.PropertyTypes.ToArray();

                var newProperties = type.Properties.Where(p => !properties.Any(x => x.Alias == p.Alias));

                foreach (var property in newProperties)
                {
                    var p = new PropertyType(dataTypeDefinitions.First(dtd => dtd.Name == property.DataType))
                    {
                        Name = property.Name,
                        Alias = property.Alias,
                        Description = property.Description
                    };

                    if (property.Tab == null)
                        ct.AddPropertyType(p);
                    else
                        ct.AddPropertyType(p, property.Tab.ToString());
                }

                var modifiedProperties = type.Properties.Join(properties, p => p.Alias, p => p.Alias, (meta, p) => new
                {
                    Metadata = meta,
                    PropertyType = p
                });

                foreach (var property in modifiedProperties)
                {
                    var p = property.PropertyType;
                    var meta = property.Metadata;

                    p.Name = meta.Name;
                    p.DataTypeDefinitionId = dataTypeDefinitions.First(dtd => dtd.Name == meta.DataType).Id;
                    p.Description = meta.Description;

                    //the following hack is brought to you be the letter F
                    var fn = p.GetType().GetProperty("PropertyGroupId", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(p) as System.Lazy<int>;
                    if (fn != null)
                    {
                        var tabId = fn.Value;
                        var tab = ct.PropertyGroups.FirstOrDefault(t => t.Id == tabId);
                        var newTabName = meta.Tab.ToString();
                        var newTab = ct.PropertyGroups.FirstOrDefault(t => t.Name == newTabName);

                        if (tab != null && tab.Name != newTabName)
                        {
                            if (newTab == null)
                                ct.AddPropertyGroup(newTabName);

                            ct.MovePropertyType(p.Alias, newTabName);
                        }
                    }
                }

                contentTypes.Add(ct);
            }

            ContentTypeService.Save(contentTypes);
        }

    }
}
