#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
// Needed for the [NotMapped] functionality
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginRegistration.Models;

public class User
{
    [Key]
    public int UserId {get;set;}

    [Required(ErrorMessage = "First Name is required")]
    [MinLength(2)]
    public string FirstName {get;set;}

    [Required(ErrorMessage = "Last Name is required")]
    [MinLength(2)]
    public string LastName {get;set;}

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email {get;set;}

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8)]
    [DataType(DataType.Password)]
    public string Password {get;set;}

    // Anything under the NotMapped will not go in to the database
    [NotMapped]
    [Required(ErrorMessage = "Confirm Password is required")]
    [Compare("Password")]
    [DataType(DataType.Password)]
    public string PassConfirm {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
}