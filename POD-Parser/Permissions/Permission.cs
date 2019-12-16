using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Permissions
{
    public abstract class Permission
    {
        protected long _clientId;

        protected Permission(){}
        protected Permission(long clientId)
        {
            _clientId = clientId;
        }

        public long ClientID
        {
            get { return _clientId; }
            set { _clientId = value; }
        }
    }
}
