using System.ComponentModel.DataAnnotations;

namespace backend.Shared.Enums;

public enum DurationType
{
    [Display(Name = "2 - 3 months")]
    TwoToThreeMonths = 1,

    [Display(Name = "3 - 6 months")]
    ThreeToSixMonths = 2,

    [Display(Name = "6 - 12 months")]
    SixToTwelveMonths = 3,

    [Display(Name = "More than 1 year")]
    MoreThanOneYear = 4
}