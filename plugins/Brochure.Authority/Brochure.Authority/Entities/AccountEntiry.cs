using Brochure.ORM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Authority.Entities
{
    [Table("account")]
    public class AccountEntiry : EntityBase, IEntityKey<string>
    {
        [Column("account_name")]
        public string AccountName { get; set; }

        [Column("account_pwd")]
        public string AccountPassword { get; set; }

        [Column("last_login_time")]
        public DateTime LastLoginTime { get; set; }

        public string Id { get; set; }
    }
}