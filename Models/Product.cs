using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MVCShop.Models
{
    public class Product
    {
        public int ProductID { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "{0} może mieć maksymalnie {1} znaków")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        [Display(Name = "Opis")]
        public string Descritpion { get; set; }

        [Display(Name = "Zdjęcie")]
        public byte[] Picture { get; set; }

        [Display(Name = "Data dodania")]
        public DateTime Date { get; set; }

        [Range(0, 100,ErrorMessage = "{0} musi zawierać się między {1} a {2}")]
        [Display(Name = "Zniżka")]
        public int Discount { get; set; }

        [Required]
        [Range(-1, 100, ErrorMessage = "{0} musi zawierać się między {1} a {2}")]
        public int VAT { get; set; }

        [Display(Name = "Usunięty")]
        public bool Deleted { get; set; }

        [Required]
        [Display(Name = "Ilość")]
        public int Quantity { get; set; }

        [Display(Name = "Sprzedano")]
        public int SalesCounter { get; set; }

        [Display(Name = "Widoczny")]
        public bool Visible { get; set; }

        [Required]
        [Display(Name = "Kategoria")]
        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<File> Files { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}