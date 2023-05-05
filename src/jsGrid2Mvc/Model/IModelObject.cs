using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsGrid2Mvc.Model
{
    /// <summary>
    /// abstarcction and decoration of object
    /// </summary>
    public interface IModelObject
    {
        /// <summary>
        /// id of control to be rendered
        /// </summary>
        string ControlId { get; }
    }
}
