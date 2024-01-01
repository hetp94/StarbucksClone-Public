using StarbucksModels;
using StarbucksModels.DbModels;
using StarbucksModels.ViewModels;
using System.Linq.Expressions;

namespace StarbucksData.IRepository
{
    public interface ICustomerRepository : IRepository<DummyClass>
    {
        Task<MenuVM> GetMenuAsync();
        Task<MenuVM> GetMenuAsync(string CategoryName);
        Task<ProductVM> GetProductAsync(int id);

        Task<CartVM> GetCartAsync(List<ListofProductIds> listofProductIds, bool Autheticated);

        void AddOrderHeader(OrderHeader orderHeader);

        void AddOrderDetail(OrderDetail orderDetail);

        void UpdateStripeSession(int OrderId, string sessionId, string paymentIntentId);

        Task<OrderHeader> GetFirstOrDefaultOrderHeaderAsync(Expression<Func<OrderHeader, bool>> filter);

        void UpdateStripeSessionToPaid(int OrderId, string paymentIntentId, string PaymentStatusPaid, string StatusApproved);


        Task<IEnumerable<OrderHistoryVM>> GetOrderHistory(string ApplicationUserId, string FirstName, bool IsTestCustomer);
    }
}
