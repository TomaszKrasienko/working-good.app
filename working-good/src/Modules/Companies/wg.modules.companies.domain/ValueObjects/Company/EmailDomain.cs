using wg.modules.companies.domain.Exceptions;

namespace wg.modules.companies.domain.ValueObjects.Company;

public sealed record EmailDomain
{
    public string Value { get; }

    private EmailDomain()
    {
    }
    
    public EmailDomain(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new EmptyEmailDomainException();
        }
        Value = value;
    }

    public static implicit operator string(EmailDomain emailDomain)
        => emailDomain.Value;
    
    public static implicit operator EmailDomain(string value)
        => new EmailDomain(value);
}