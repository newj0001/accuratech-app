using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI_Mobile.Models
{
    public class QueueItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}
