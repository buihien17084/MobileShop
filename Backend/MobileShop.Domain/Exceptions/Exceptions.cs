using System;
using System.Collections.Generic;
using System.Text;

namespace MobileShop.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string name, object key) : base($"Không tìm thấy {name} với mã ({key}).") { }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Bạn không có quyền truy cập vào tài nguyên này.") : base(message) { }
}
