using System;
using System.Collections.Generic;
using System.Text;

namespace StudentManagement.Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityName, object key)
            : base($"La entidad '{entityName}' con identificador '{key}' no fue encontrada.")
        { }
    }

    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }

}
