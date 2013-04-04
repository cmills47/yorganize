using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yorganize.Business.Repository
{
    public interface IEntity<TKey>
    {
        TKey ID { get; set; }
    }
}
