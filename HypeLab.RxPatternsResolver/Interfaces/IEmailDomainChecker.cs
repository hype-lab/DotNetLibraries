using HypeLab.RxPatternsResolver.Enums;
using HypeLab.RxPatternsResolver.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver.Interfaces
{
    internal interface IEmailDomainChecker
    {
        Task<EmailCheckerResponseStatus> IsDomainValidAsync(string checkUrl);
    }
}
