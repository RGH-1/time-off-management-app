using Microsoft.AspNetCore.Authentication.Cookies;

namespace time_off_management_app.TicketStoring
{
    public class TicketStoreResolver : ITicketStoreResolver
    {
        private readonly IServiceProvider _serviceProvider;

        public TicketStoreResolver(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITicketStore? Resolve(string storeName)
        {
            if (string.IsNullOrEmpty(storeName) || storeName.Equals("Default"))
            {
                return null;
            }

            return _serviceProvider.GetKeyedService<ITicketStore>(storeName);
        }
    }
}
