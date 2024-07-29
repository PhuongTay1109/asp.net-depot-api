using System;

namespace api.DTOs
{
    public class ContainerIssuanceOrder
    {
        public int message_type { get; set; }
        public required string message_function { get; set; }
        public required string location_code { get; set; }
        public required string location_name { get; set; }
        public required string vessel { get; set; }
        public required string call_sign { get; set; }
        public required string voyage { get; set; }
        public required DateTime departure_time { get; set; }
        public string? port_of_loading { get; set; }
        public string? load_cy { get; set; }
        public required string shipper_full_name { get; set; }
        public string? shipper_tax_code { get; set; }
        public required string container_release_order_no { get; set; }
        public required DateTime issue_date { get; set; }
        public required DateTime expiry_date { get; set; }
        public int assign_container { get; set; }
        public int stuffing_plan { get; set; }
        public string? container_no { get; set; }
        public required string size_type { get; set; }
        public int grade { get; set; }
        public int quantity { get; set; }
        public required string commodity { get; set; }
        public int payment_term { get; set; }
        public int transport_mode { get; set; }
        public string? driver { get; set; }
        public string? identity_card { get; set; }
        public string? driver_license { get; set; }
        public string? license_plate_number { get; set; }
        public string? remark { get; set; }
    }
}
