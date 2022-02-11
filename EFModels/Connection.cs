using System;
using System.Collections.Generic;

#nullable disable

namespace SingalRAngular1.EFModels
{
    public partial class Connection
    {
        public Guid Id { get; set; }
        public Guid PersonId { get; set; }
        public string SignalrId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
