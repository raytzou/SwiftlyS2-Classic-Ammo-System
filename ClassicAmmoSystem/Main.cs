using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;

namespace ClassicAmmoSystem
{
    [PluginMetadata(Author = "cynicat", Name = "Classic Ammo System", Description = "A simple plugin to bring classic ammo system back", Id = "cas", Version = "1.0.0")]
    public class Main : BasePlugin
    {
        public Main(ISwiftlyCore core) : base(core) { }

        public override void Load(bool hotReload)
        {
            
        }

        public override void Unload()
        {
            
        }
    }
}
