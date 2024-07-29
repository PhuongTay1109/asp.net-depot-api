using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace api.Services
{
    public class DepotService
{
    public bool ValidateTime(DateTime time, out string errorCode)
    {
        var timeZoneInfo = TZConvert.GetTimeZoneInfo("Asia/Ho_Chi_Minh");
        var utcOffset = timeZoneInfo.GetUtcOffset(time);

        if (utcOffset != timeZoneInfo.BaseUtcOffset && utcOffset != timeZoneInfo.GetAdjustmentRules().Last().DaylightDelta)
        {
            errorCode = "-2067";
            return false;
        }

        if (Math.Abs((DateTime.UtcNow - time).TotalMinutes) > 5)
        {
            errorCode = "-2071";
            return false;
        }

        errorCode = null;
        return true;
    }
}

}