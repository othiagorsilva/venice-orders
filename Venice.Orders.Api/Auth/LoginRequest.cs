using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Venice.Orders.Api.Auth
{
    public record LoginRequest(string Username, string Password);
}