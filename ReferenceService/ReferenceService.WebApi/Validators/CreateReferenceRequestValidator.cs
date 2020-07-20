namespace ReferenceService.WebApi.Validators
{
    using Common;
    using FluentValidation;
    using Models;

    /// <summary>
    /// Валидация входящих данных для создания справки
    /// </summary>
    public class CreateReferenceRequestValidator : AbstractValidator<CreateReferenceRequest>
    {
        /// <summary>
        /// Конструктор для <see cref="CreateReferenceRequestValidator"/>
        /// </summary>
        public CreateReferenceRequestValidator()
        {
            RuleFor(request => request.Code)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(Constants.ErrorCodeRequired)
                .Must(ValidateCode)
                .WithErrorCode(Constants.ErrorCodeExists)
                .DependentRules(r =>
                {
                    r.RuleFor(request => request.Params)
                        .NotNull()
                        .WithErrorCode(Constants.ErrorCodeRequired);
                });

            RuleFor(request => request.RegistrationChannel)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithErrorCode(Constants.ErrorCodeRequired)
                .Must(ValidateRegistrationChannel)
                .WithErrorCode(Constants.ErrorCodeExists);

            When(request => request.User != null, () =>
            {
                RuleFor(request => request.User.ID)
                    .NotEmpty()
                    .WithErrorCode(Constants.ErrorCodeRequired);

                RuleFor(request => request.User.FIO)
                    .NotEmpty()
                    .WithErrorCode(Constants.ErrorCodeRequired);

                RuleFor(request => request.User.LocalDateTime)
                    .Cascade(CascadeMode.StopOnFirstFailure)
                    .NotEmpty()
                    .WithErrorCode(Constants.ErrorCodeRequired)
                    .WithErrorCode(Constants.ErrorCodeFormatDateTime);

                RuleFor(request => request.User.Workstation)
                    .NotNull()
                    .WithErrorCode(Constants.ErrorCodeRequired)
                    .DependentRules(r =>
                    {
                        r.RuleFor(request => request.User.Workstation.IP)
                            .NotEmpty()
                            .WithErrorCode(Constants.ErrorCodeRequired);

                        r.RuleFor(request => request.User.Workstation.Name)
                            .NotEmpty()
                            .WithErrorCode(Constants.ErrorCodeRequired);
                    });
            });
            

            RuleFor(request => request.Customer)
                .NotNull()
                .WithErrorCode(Constants.ErrorCodeRequired)
                .DependentRules(r =>
                {
                    r.RuleFor(request => request.Customer.CUID)
                        .NotEmpty()
                        .WithErrorCode(Constants.ErrorCodeRequired);

                    r.RuleFor(request => request.Customer.FIO)
                        .NotEmpty()
                        .WithErrorCode(Constants.ErrorCodeRequired);

                    r.RuleFor(request => request.Customer.PassportSerNum)
                        .NotEmpty()
                        .WithErrorCode(Constants.ErrorCodeRequired);
                });
        }

        private bool ValidateCode(string code)
        {
            return
                code == Constants.WpaRef1 ||
                code == Constants.WpaRef2 ||
                code == Constants.WpaRef3 ||
                code == Constants.WpaRef4 ||
                code == Constants.WpaRef5;
        }

        private bool ValidateRegistrationChannel(string regChannel)
        {
            return 
                regChannel == Constants.RegistrationChannelBO ||
                regChannel == Constants.RegistrationChannelKC ||
                regChannel == Constants.RegistrationChannelAP;
        }
    }
}
