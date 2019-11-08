using System;
using System.Collections.Generic;

namespace Common.Standard.Entities
{
    public class RegistrationModel
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int MenuItemId { get; set; }
        public ICollection<RegistrationValueModel> RegistrationValues { get; set; } = new List<RegistrationValueModel>();
    }
}
