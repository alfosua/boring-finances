using System.ComponentModel.DataAnnotations;

namespace BoringFinances.Operations.Data.Accounts;

public class AccountType
{
    public AccountType()
    {
        Code = nameof(AccountTypeOptions.None);
        Accounts = null!;
    }

    [Key]
    public byte AccountTypeId { get; set; }

    [Required, StringLength(16)]
    public string Code { get; set; }

    public ICollection<Account> Accounts { get; set; }
}
