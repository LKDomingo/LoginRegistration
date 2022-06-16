#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
// Needed for the [NotMapped] functionality
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginRegistration.Models;

public class LogUser
{

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string LogEmail {get;set;}

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string LogPassword {get;set;}
}