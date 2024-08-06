namespace HypeLab.RxPatternsResolver.Interfaces
{
    internal interface IEmailValidityChecker
    {
        bool IsValidEmailAddress(string email);
    }
}
