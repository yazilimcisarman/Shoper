using Shoper.Application.Dtos.EMailDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.EmailServices
{
    public interface IEmailService
    {
        bool SendEmailAsync(SendEmailDto dto);
    }
}
