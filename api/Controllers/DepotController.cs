using Microsoft.AspNetCore.Mvc;
using api.DTOs;

namespace api.Controllers
{
    [ApiController]
    [Route("api/depot")]
    public class DepotController : ControllerBase
    {

        // Lệnh cấp vỏ
        [HttpPost("issue")]
        public IActionResult IssueContainer([FromBody] ApiRequest request)
        {
            // 
            // code xử lý lệnh cấp vỏ
            //

            // sample data
            var sampleData = new ContainerIssuanceOrder
            {
                message_type = 1,
                message_function = "original",
                location_code = "DP000",
                location_name = "ABC",
                vessel = "PROGRESS",
                call_sign = "XVGI7",
                voyage = "2413S",
                departure_time = new DateTime(2024, 4, 1, 0, 0, 0),
                port_of_loading = "ABC",
                load_cy = "ABC",
                shipper_full_name = "Công Ty ABC",
                shipper_tax_code = "0123456789",
                container_release_order_no = "RO0000000",
                issue_date = new DateTime(2024, 3, 25, 15, 28, 27),
                expiry_date = new DateTime(2024, 4, 1, 0, 0, 0),
                assign_container = 0,
                stuffing_plan = 1,
                container_no = "",
                size_type = "20DC",
                grade = 3,
                quantity = 2,
                commodity = "BACH HOA",
                payment_term = 1,
                transport_mode = 3,
                driver = "Nguyễn Văn A",
                identity_card = "0123456789",
                driver_license = "",
                license_plate_number = "29H0000",
                remark = "Cấp container sạch tốt"
            };

            // Return sample data
            return Ok(new { Data = sampleData });
        }

        // Lệnh hạ vỏ
        [HttpPost("release")]
        public IActionResult ReleaseContainer([FromBody] ApiRequest request)
        {
            //
            // code xử lý lệnh hạ vỏ
            // 

            // sample data
            var sampleData = new ContainerReleaseOrder
            {
                message_type = 6,
                message_function = "original",
                location_code = "DP000",
                location_name = "ABC",
                vessel = "PROGRESS",
                call_sign = "XVGI7",
                voyage = "2413S",
                arrival_time = DateTime.Parse("2024-04-03 00:00:00"),
                port_of_discharge = "ABC",
                empty_return_no = "ER0000000",
                issue_date = DateTime.Parse("2024-04-03 15:28:27"),
                bill_of_lading_no = "VS000000",
                container_no = "VSCU2000216",
                size_type = "20DC",
                remark = ""
            };

            // Return sample data
            return Ok(new { Data = sampleData });
        }
    }
}
