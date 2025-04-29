using Microsoft.AspNetCore.Mvc;
using TimeZoneConverter;

namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorldTimeController : ControllerBase
    {
        private static readonly Dictionary<string, string> CityToIanaMap = new(StringComparer.OrdinalIgnoreCase)
{
    // Middle East
    { "riyadh", "Asia/Riyadh" },
    { "dubai", "Asia/Dubai" },
    { "doha", "Asia/Qatar" },
    { "manama", "Asia/Bahrain" },
    { "muscat", "Asia/Muscat" },
    { "kuwait", "Asia/Kuwait" },
    { "baghdad", "Asia/Baghdad" },
    { "damascus", "Asia/Damascus" },
    { "amman", "Asia/Amman" },
    { "jerusalem", "Asia/Jerusalem" },
    { "beirut", "Asia/Beirut" },
    { "tehran", "Asia/Tehran" },

    // Africa
    { "cairo", "Africa/Cairo" },
    { "lagos", "Africa/Lagos" },
    { "nairobi", "Africa/Nairobi" },
    { "johannesburg", "Africa/Johannesburg" },
    { "casablanca", "Africa/Casablanca" },
    { "algiers", "Africa/Algiers" },
    { "addis ababa", "Africa/Addis_Ababa" },
    { "khartoum", "Africa/Khartoum" },
    { "tunis", "Africa/Tunis" },

    // Europe
    { "london", "Europe/London" },
    { "paris", "Europe/Paris" },
    { "berlin", "Europe/Berlin" },
    { "rome", "Europe/Rome" },
    { "madrid", "Europe/Madrid" },
    { "amsterdam", "Europe/Amsterdam" },
    { "vienna", "Europe/Vienna" },
    { "oslo", "Europe/Oslo" },
    { "stockholm", "Europe/Stockholm" },
    { "helsinki", "Europe/Helsinki" },
    { "athens", "Europe/Athens" },
    { "istanbul", "Europe/Istanbul" },
    { "moscow", "Europe/Moscow" },
    { "zurich", "Europe/Zurich" },
    { "warsaw", "Europe/Warsaw" },
    { "prague", "Europe/Prague" },

    // Asia
    { "karachi", "Asia/Karachi" },
    { "delhi", "Asia/Kolkata" },
    { "mumbai", "Asia/Kolkata" },
    { "kolkata", "Asia/Kolkata" },
    { "dhaka", "Asia/Dhaka" },
    { "bangkok", "Asia/Bangkok" },
    { "jakarta", "Asia/Jakarta" },
    { "singapore", "Asia/Singapore" },
    { "kuala lumpur", "Asia/Kuala_Lumpur" },
    { "manila", "Asia/Manila" },
    { "beijing", "Asia/Shanghai" },
    { "shanghai", "Asia/Shanghai" },
    { "hong kong", "Asia/Hong_Kong" },
    { "seoul", "Asia/Seoul" },
    { "tokyo", "Asia/Tokyo" },
    { "taipei", "Asia/Taipei" },
    { "hanoi", "Asia/Bangkok" },
    { "kathmandu", "Asia/Kathmandu" },
    { "yangon", "Asia/Yangon" },

    // Oceania
    { "sydney", "Australia/Sydney" },
    { "melbourne", "Australia/Melbourne" },
    { "brisbane", "Australia/Brisbane" },
    { "perth", "Australia/Perth" },
    { "adelaide", "Australia/Adelaide" },
    { "auckland", "Pacific/Auckland" },
    { "wellington", "Pacific/Auckland" },
    { "suva", "Pacific/Fiji" },

    // North America
    { "new york", "America/New_York" },
    { "washington", "America/New_York" },
    { "toronto", "America/Toronto" },
    { "chicago", "America/Chicago" },
    { "dallas", "America/Chicago" },
    { "denver", "America/Denver" },
    { "phoenix", "America/Phoenix" },
    { "los angeles", "America/Los_Angeles" },
    { "vancouver", "America/Vancouver" },
    { "mexico city", "America/Mexico_City" },
    { "montreal", "America/Toronto" },
    { "boston", "America/New_York" },
    { "miami", "America/New_York" },
    { "seattle", "America/Los_Angeles" },

    // South America
    { "sao paulo", "America/Sao_Paulo" },
    { "rio de janeiro", "America/Sao_Paulo" },
    { "buenos aires", "America/Argentina/Buenos_Aires" },
    { "bogota", "America/Bogota" },
    { "lima", "America/Lima" },
    { "caracas", "America/Caracas" },
    { "santiago", "America/Santiago" },
    { "quito", "America/Guayaquil" },

    // Others
    { "reykjavik", "Atlantic/Reykjavik" },
    { "honolulu", "Pacific/Honolulu" },
    { "anchorage", "America/Anchorage" },
    { "barcelona", "Europe/Madrid" },
    { "brussels", "Europe/Brussels" },
    { "lisbon", "Europe/Lisbon" }
};


        [HttpGet("{city}")]
        public IActionResult GetTime(string city)
        {
            if (!CityToIanaMap.TryGetValue(city, out var ianaZone))
            {
                return NotFound(new { error = "City not recognized" });
            }

            try
            {
                var windowsZone = TZConvert.IanaToWindows(ianaZone);
                var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(windowsZone);

                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneInfo);
                var offset = timeZoneInfo.GetUtcOffset(now);

                var result = new
                {
                    abbreviation = offset.ToString(),
                    client_ip = HttpContext.Connection.RemoteIpAddress?.ToString(),
                    datetime = now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffzzz"),
                    day_of_week = (int)now.DayOfWeek,
                    day_of_year = now.DayOfYear,
                    dst = timeZoneInfo.IsDaylightSavingTime(now),
                    dst_from = (string?)null,
                    dst_offset = timeZoneInfo.IsDaylightSavingTime(now) ? offset.TotalSeconds : 0,
                    dst_until = (string?)null,
                    raw_offset = offset.TotalSeconds,
                    timezone = ianaZone,
                    unixtime = ((DateTimeOffset)now).ToUnixTimeSeconds(),
                    utc_datetime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ"),
                    utc_offset = offset.ToString(@"hh\:mm"),
                    week_number = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                        now, System.Globalization.CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday)
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
