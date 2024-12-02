using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum OrderType
    {
        [Display(Name = "ملابس")]
        Clothes = 1,
        [Display(Name = "مفروشات")]
        Linens = 2,
        [Display(Name = "سجاد")]
        Rugs = 3
    }
}
