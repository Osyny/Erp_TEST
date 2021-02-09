using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Erp_TEST.Models.DbModel
{
    public class User
    {
        public Guid Id { get; set; }

        public AccountUser AccountUser { get; set; }

        public DateTime DateRegister { get; set; }
    }
}
