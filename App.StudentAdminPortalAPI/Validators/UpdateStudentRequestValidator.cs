using App.StudentAdminPortalAPI.DomainModels;
using App.StudentAdminPortalAPI.Repositories;
using FluentValidation;
using System.Linq;

namespace App.StudentAdminPortalAPI.Validators
{
    public class UpdateStudentRequestValidator : AbstractValidator<UpdateStudentRequest>
    {
        public UpdateStudentRequestValidator(IStudentRepository studentRepository)
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.DateOfBirth).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.Mobile).GreaterThan(99999).LessThan(10000000000);
            RuleFor(x => x.GenderId).NotEmpty().Must(id =>
            {
                var gender = studentRepository.GetGendersAsync().Result.ToList().FirstOrDefault(x => x.Id == id);
                if (gender != null)
                {
                    return true;
                }
                return false;
            }).WithMessage("Hãy lựa chọn một giá trị giới tính");
            RuleFor(x => x.PhysicalAddress).NotEmpty();
            RuleFor(x => x.PostalAddress).NotEmpty();
        }
    }
}
