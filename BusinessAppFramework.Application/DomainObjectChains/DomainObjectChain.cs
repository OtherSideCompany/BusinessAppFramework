using System.Text.Json.Serialization;

namespace BusinessAppFramework.Application.DomainObjectChains
{
    public class DomainObjectChain
    {
        public List<DomainObjectChainNode> Nodes { get; set; } = new();

        [JsonIgnore]
        public DomainObjectChainNode Current => Nodes.Single(n => n.IsCurrent);
    }
}
