using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class RegistrationModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int MenuItemId { get; set; }
        public ICollection<RegistrationValueModel> RegistrationValues { get; set; } = new List<RegistrationValueModel>();
    }
}
