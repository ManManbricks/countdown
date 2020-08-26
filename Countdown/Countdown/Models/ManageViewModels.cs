using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using Countdown.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Countdown.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        // [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }

    public class CreateTodoViewModel: IValidatableObject
    {

       
        [Required]
        [Display(Name = "Title")]
        [RegularExpression(@"^[a-zA-Z0-9\s]{1,50}$", ErrorMessage = "Todo title can contain only alphanumeric character between 1 and 50")]
        public string Title { get; set; }

        [Display(Name = "Description")]
        [RegularExpression(@"^[a-zA-Z0-9\s]{0,200}$", ErrorMessage = "Todo description can contain only alphanumeric character between 0 and 200")]
        public string Description { get; set; }

        public string Owner { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [RegularExpression(@"^\d{4}/\d{2}/\d{2}$", ErrorMessage = "Date format must be YYYY/MM/DD")]
        public String StartDate { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        [RegularExpression(@"^\d{2}:\d{2}:\d{2}\s+[AaPp]{1}[mM]{1}$", ErrorMessage = "Time format must be hh:mm:ss am or pm")]
        public String StartTime { get; set; }


        [Required]
        [Display(Name = "Due Date")]
        [RegularExpression(@"^\d{4}/\d{2}/\d{2}$", ErrorMessage = "Date format must be yyyy/MM/dd")]
        public String DueDate { get; set; }


        [Required]
        [Display(Name = "Due Time")]
        [RegularExpression(@"^\d{2}:\d{2}:\d{2}\s+[AaPp]{1}[mM]{1}$", ErrorMessage = "Time format must be hh:mm:ss am or pm")]
        public String DueTime { get; set; }

        public string AssignedTo { get; set; }

        public ICollection<SelectListItem> RegisteredUsers { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            String errorMessage = "Due date and time must be greater or equal to start date and time";

            ValidateDate(StartDate, out DateTime startDate);
            ValidateTime(StartTime, out DateTime startTime);

            bool validDueDate = ValidateDate(DueDate, out DateTime dueDate);
            bool validDueTime = ValidateTime(DueTime,  out DateTime dueTime);

            DateTime startDateTime = GetDateTimeFromString(startDate, startTime);
            DateTime dueDateTime = GetDateTimeFromString(dueDate, dueTime);

            if (!validDueDate)
            {
                errorMessage = "Invalid due date. Format = YYYY/MM/DD. e.g. 1991/12/31";
                yield return new ValidationResult(errorMessage,new List<string> { "DueDate" });
            }


            if (!validDueTime)
            {
                errorMessage = "Invalid due time. Format = hh:mm:ss am or pm. e.g. 10:15:01 am";
                yield return new ValidationResult(errorMessage, new List<string> { "DueTime" });
            }

            if (dueDateTime <= startDateTime)
            {
                yield return new ValidationResult(errorMessage, new List<string> { "DueDate", "DueTime" });
            }
            
           
        }

        private bool ValidateDate(String date, out DateTime startDate)
        {
            return DateTime.TryParseExact(date, "yyyy/MM/dd", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out startDate);
        }

        private bool ValidateTime(String time, out DateTime startTime)
        {
            return DateTime.TryParseExact(time, "hh:mm:ss tt", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out startTime);
        }

        private DateTime GetDateTimeFromString(DateTime startDate, DateTime startTime)
        {
            startDate = startDate.Date + new TimeSpan(startTime.Hour, startTime.Minute, startTime.Second);
            return startDate;
        }

    }

    public class TodoViewModel
    {
        public int Id { get; set; }

        public string CurrentUserId;
        public string Title { get; set; }
        public double TimeLeft { get; set; }

        public string Owner { get; set; }

        public string OwnerFirstName { get; set; }

        public string AssignedTo { get; set; }

        public string AssignedToFirstName { get; set; }

        public bool IsCompleted { get; set; }
    }

    public class ViewTodoViewModel
    {
        public TodoFilter TodoFilter { get; set; }

        public IEnumerable<TodoViewModel> TodoViewModels { get; set; }

    }

    public class TodoFilter
    {
        [Display(Name = "Owned by me")]
        public bool OwnedByMe { get; set; }

        [Display(Name = "Owned by others")]
        public bool OwnedByOthers { get; set; }

        [Display(Name = "Assigned to others")]
        public bool AssignedToOthers { get; set; }

        [Display(Name = "Completed")]
        public bool Completed { get; set; }

        [Display(Name = "Pending")]
        public bool Pending { get; set; }
    }
}