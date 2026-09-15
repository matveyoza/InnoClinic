using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Shared.DataTransferObjects
{
    public record UserForAuthDto
    {
        public string Id { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName {  get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}
