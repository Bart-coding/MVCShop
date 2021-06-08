using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCShop.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [StringLength(30)]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi mieć co najmniej {2} znaków.", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła się nie zgadzają")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Wyświetlać ceny netto?")]
        public bool Netto { get; set; }

        [Display(Name = "Wysyłać newsletter?")]
        public bool Newsletter { get; set; }


        [Required]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"([0-9]{2}-[0-9]{3})", ErrorMessage = "Niepoprawny format")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "{0} musi mieć co najmniej {2} znaków")]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"(^[A-Z].*)", ErrorMessage = "{0} musi zaczynać się wielką literą")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"((^[A-Z]+[A-z]+\s[0-9]+[A-Z]{0,1}$)|(^[A-Z]+[A-z]+\s([0-9]+[A-Z]{0,1}\/+[0-9]+)$))", 
                            ErrorMessage = "{0} musi zaczynać się wielką literą i mieć numer lokalu/mieszkania")]
        [Display(Name = "Ulica")]
        public string StreetAddress { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required]
        [Range(1, 30, ErrorMessage = "Liczba produktów na stronę musi zawierać się w przedziale {1}-{2}")]
        [Display(Name = "Produkty na stronę")]
        public int ProductsPerPage { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Zniżka musi zawierać się w przedziale {1}-{2}")]
        [Display(Name = "Indywidualna zniżka")]
        public int PersonalDiscount { get; set; }

        [Display(Name = "Wyświetlać ceny netto?")]
        public bool Netto { get; set; }

        [Display(Name = "Wysyłać newsletter?")]
        public bool Newsletter { get; set; }

        [Required]
        [DataType(DataType.PostalCode)]
        [RegularExpression(@"([0-9]{2}-[0-9]{3})", ErrorMessage = "Niepoprawny format")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "{0} musi mieć co najmniej {2} znaków")]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"(^[A-Z].*)", ErrorMessage = "{0} musi zaczynać się wielką literą")]
        [Display(Name = "Miasto")]
        public string City { get; set; }

        [Required]
        [StringLength(30)]
        [RegularExpression(@"((^[A-Z]+[A-z]+\s[0-9]+[A-Z]{0,1}$)|(^[A-Z]+[A-z]+\s([0-9]+[A-Z]{0,1}\/+[0-9]+)$))",
                            ErrorMessage = "{0} musi zaczynać się wielką literą i mieć numer lokalu/mieszkania")]
        [Display(Name = "Ulica")]
        public string StreetAddress { get; set; }
    }
}
