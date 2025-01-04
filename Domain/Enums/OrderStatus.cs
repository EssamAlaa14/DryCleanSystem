using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum OrderStatus
    {
        [Display(Name = "جديد")]
        New = 1,
        [Display(Name = "قيد التنفيذ")]
        InProgress = 2,
        [Display(Name = "جاهز للتسليم")]
        Completed = 3,
        [Display(Name = "تم التسليم")]
        Delivered = 4,
        [Display(Name = "لديه مشكلة")]
        Issued = 5,
        [Display(Name = "ملغي")]
        Canceled = 6
    }
}
