using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationService.Models;

public class MembershipFee
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int MembershipTypeId { get; set; }
    public string PeriodType { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Fee { get; set; }
    
    public MembershipType? MembershipType { get; set; }
}