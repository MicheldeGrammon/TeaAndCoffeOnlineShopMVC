using System.Collections.ObjectModel;

namespace TeaAndCoffee_Utility
{
    public static class WC
    {
        public const string ImagePath = @"\images\product\";

        public const string SessionCart = "ShoppingCartSession";

        public const string AdminRole = "Admin";

        public const string CustomerRole = "Customer";

        public const string CategoryName = "Category";

        public const string ApplicationTypeName = "ApplicationType";

        public const string Success = "Success";
        public const string Error = "Error";
        public const string Warning = "Warning";

        public const string StatusPending = "Pending";
        public const string StatusInProcces = "In Procces";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusСompleted = "Сompleted";

        public static readonly IEnumerable<string> listStatus = new ReadOnlyCollection<string>
            (new List<string>
            {
                StatusPending,
                StatusInProcces,
                StatusShipped,
                StatusCancelled,
                StatusСompleted
            });
    }
}
