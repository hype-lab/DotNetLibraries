using System;
using System.Collections.Generic;
using System.Text;

namespace HypeLab.RxPatternsResolver.Interfaces
{
    internal interface IEmailValidityChecker
    {
        bool IsValidEmailAddress(string email);
    }
}
