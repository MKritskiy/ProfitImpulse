using Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class CheckDataHelper
    {
        public static bool IsDataExpired<T>(T latestUpdate) where T : IUpdate
        {
            if (latestUpdate.UpdateId < 0)
                return true;
            return DateTime.UtcNow > latestUpdate.LastUpdate.AddMinutes(latestUpdate.LifetimeMinutes);
        }
    }
}
