using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaAndCoffee_DataAccess.Repository.IRepository;
using TeaAndCoffee_Models;
using TeaAndCoffee_Utility;

namespace TeaAndCoffee_DataAccess.Repository
{
    public class InquiryDetailsRepository : Repository<InquiryDetails>, IInquiryDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public InquiryDetailsRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(InquiryDetails obj)
        {
            _db.InquiryDetails.Update(obj);
        }
    }
}
