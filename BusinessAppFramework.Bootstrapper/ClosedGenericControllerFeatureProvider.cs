using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace BusinessAppFramework.Bootstrapper
{
    public sealed class ClosedGenericControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly List<(Type ControllerType, string RouteKey)> _controllers;
        public ClosedGenericControllerFeatureProvider(List<(Type, string)> controllers) 
            => _controllers = controllers;

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var (controllerType, _) in _controllers)
            {
                var info = controllerType.GetTypeInfo();
                if (!feature.Controllers.Contains(info))
                    feature.Controllers.Add(info);
            }
        }
    }
}
