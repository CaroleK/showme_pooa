using System;
using System.Collections.Generic;
using System.Text;

namespace ShowMe.Services
{
    /// <summary>
    /// Interface to describe the possibility to send a toast message
    /// </summary>
    public interface IMessage
    {
        void Show(string message);
    }
}
