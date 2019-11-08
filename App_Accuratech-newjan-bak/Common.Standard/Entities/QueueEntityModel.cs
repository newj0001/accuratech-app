using System;

namespace Common.Standard.Entities
{
    public class QueueEntityModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
    }
}
