using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Interfaces
{
    public interface IUpdate
    {
        int UpdateId { get; set; }
        int ProfileId { get; set; }
        DateTime LastUpdate { get; set; }
        int LifetimeMinutes { get; set; }
        DateTime DateFrom { get; set; }
    }
}
