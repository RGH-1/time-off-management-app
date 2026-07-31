using Microsoft.AspNetCore.Authentication.Cookies;

namespace time_off_management_app.TicketStoring
{
    public interface ITicketStoreResolver
    {
        ITicketStore? Resolve(String storeName);
    }
}
