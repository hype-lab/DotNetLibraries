using HypeLab.RxPatternsResolver.Models;
using System.Threading.Tasks;

namespace HypeLab.RxPatternsResolver.Interfaces
{
    internal interface IEmailDomainChecker
    {
        Task<EmailCheckerResponseStatus> IsDomainValidAsync(string checkUrl);
    }
}
