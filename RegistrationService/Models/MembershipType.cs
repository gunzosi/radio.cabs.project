using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RegistrationService.Models;

namespace RegistrationService.Models;

public class MembershipType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string TypeName { get; set; }
    
    public ICollection<MembershipFee> MembershipFees { get; set; }
    public ICollection<Company> Companies { get; set; }
}