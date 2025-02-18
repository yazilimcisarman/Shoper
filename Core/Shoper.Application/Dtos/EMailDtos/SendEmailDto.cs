using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Dtos.EMailDtos
{
    public class SendEmailDto
    {
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public string ReciverEmail { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}
