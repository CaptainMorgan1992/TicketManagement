using System;

namespace TicketManagementSystem
{
    public class InvalidTicketException : Exception
    {
        public InvalidTicketException(string message) : base(message) { }
    }

    public class UnknownUserException : Exception
    {
        public UnknownUserException(string message) : base(message) { }
    }
}
