using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Web
{
    interface IUserIdentity
    {
        /// <summary>
        /// Gets or sets the name of the current user.
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Gets or set the claims of the current user.
        /// </summary>
        IEnumerable<string> Claims { get; set; }
    }
}
