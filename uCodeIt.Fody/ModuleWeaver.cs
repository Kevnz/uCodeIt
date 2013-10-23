using Mono.Cecil;
using System.Linq;
using uCodeIt.DocumentTypes;

namespace uCodeIt.Fody
{
    public class ModuleWeaver
    {
        public ModuleDefinition ModuleDefinition { get; set; }

        public void Execute()
        {
            //var types = ModuleDefinition.Types.Where(type => type.CustomAttributes.Any(attr => attr.AttributeType.FullName == typeof(DocumentTypeAttribute).FullName));
        }
    }
}
