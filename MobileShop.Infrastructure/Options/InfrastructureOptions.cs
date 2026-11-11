using System;
using System.Collections.Generic;
using System.Text;

namespace MobileShop.Infrastructure.Options;

public class JwtOptions
{
    public const string SectionName = "Jwt";
    // Bắt buộc cấu hình qua User Secrets / biến môi trường (tối thiểu 32 ký tự)
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = "PhoneHubAPI";
    public string Audience { get; set; } = "PhoneHubClient";
    public int ExpiryDays { get; set; } = 7;
}

