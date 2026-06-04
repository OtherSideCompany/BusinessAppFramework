using BusinessAppFramework.Contracts.ApiRoutes;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace BusinessAppFramework.Bootstrapper
{
    public sealed class ClosedGenericRouteConvention : IControllerModelConvention
    {
        private readonly List<(Type ControllerType, string Route)> _controllers;
        public ClosedGenericRouteConvention(List<(Type, string)> controllers)
            => _controllers = controllers;

        public void Apply(ControllerModel controller)
        {
            var match = _controllers.FirstOrDefault(c => c.ControllerType == controller.ControllerType);

            if (match.ControllerType is null) return;

            controller.Selectors[0].AttributeRouteModel = new AttributeRouteModel
            {
                Template = match.Route
            };

            if (controller.ControllerType.IsGenericType)
            {
                controller.ControllerName = controller.ControllerType.GenericTypeArguments[0].Name;
            }
        }
    }
}
