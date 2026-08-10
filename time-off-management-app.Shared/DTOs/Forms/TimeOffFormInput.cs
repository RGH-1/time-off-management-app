using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace time_off_management_app.Shared.DTOs.Forms
{
    public class TimeOffFormInput : IValidatableObject
    {
        public bool DetailedDate { get; set; } = false;
        [Required]
        public DateTime DateTimeFrom { get; set; }
        [Required]
        public DateTime DateTimeTo { get; set; }

        public String ReasonCode { get; set; } = string.Empty;
        public String? ReasonDescription { get; set; }

        public IEnumerable<ValidationResult> Validate(
        ValidationContext validationContext)
        {
            if (DateTimeFrom > DateTimeTo)
            {
                yield return new ValidationResult(
                    "The end date must be after the start date.",
                    new[] { nameof(DateTimeTo) });
            }

            if (DateTimeFrom < DateTime.Now || DateTimeTo < DateTime.Now)
            {
                yield return new ValidationResult(
                    "Start and End Date should be in the future."
            );
            }

            if(String.IsNullOrEmpty(ReasonCode))
            {
                yield return new ValidationResult(
                    "Choose a reason for your leave.",
                    new[] {nameof(ReasonCode) });
            }
        }
    }
}
