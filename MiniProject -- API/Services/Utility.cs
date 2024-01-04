using MiniProject____API.Models;

namespace MiniProject____API.Services
{
    public class Utility
    {
        public static string CreateInterestId(string title)
        {
            return string.Join(' ', title[0], title[1], title[2]).Trim();
        }

    }
}
