using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs
{
    public class ContainerReleaseOrder
    {
        public int message_type { get; set; }
        public required string message_function { get; set; }
        public required string location_code { get; set; }
        public required string location_name { get; set; }
        public required string vessel { get; set; }
        public required string call_sign { get; set; }
        public required string voyage { get; set; }
        public DateTime arrival_time { get; set; }
        public required string port_of_discharge { get; set; }
        public string? empty_return_no { get; set; }
        public DateTime? issue_date { get; set; }
        public required string bill_of_lading_no { get; set; }
        public required string container_no { get; set; }
        public required string size_type { get; set; }
        public string? remark { get; set; }
    }

}