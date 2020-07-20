namespace ReferenceService.WebApi.Validators
{
    using Common;
    using FluentValidation;
    using Models;

    /// <summary>
    /// Валидация входящего запроса для отправки справки
    /// </summary>
    public class SendReferenceRequestValidator : AbstractValidator<SendReferenceRequest>
    {
        /// <summary>
        /// Конструктор для <see cref="SendReferenceRequestValidator"/>
        /// </summary>
        public SendReferenceRequestValidator()
        {
            RuleFor(request => request.Delivery)
                .NotNull()
                .WithErrorCode(Constants.ErrorCodeRequired)
                .DependentRules(r =>
                {
                    r.RuleFor(request => request.Delivery.Method)
                        .Cascade(CascadeMode.StopOnFirstFailure)
                        .NotEmpty()
                        .WithErrorCode(Constants.ErrorCodeRequired)
                        .Must(ValidateDeliveryMethod)
                        .WithErrorCode(Constants.ErrorCodeExists)
                        .DependentRules(dr =>
                        {
                            dr.When(request => request.Delivery.Method == Constants.DeliveryMethodEmail, () =>
                            {
                                dr.RuleFor(request => request.Delivery.Email)
                                    .NotEmpty()
                                    .WithErrorCode(Constants.ErrorCodeRequired);
                            });
                        });
                });
        }
        
        private bool ValidateDeliveryMethod(string deliveryMethod)
        {
            return
                deliveryMethod == Constants.DeliveryMethodBO ||
                deliveryMethod == Constants.DeliveryMethodEmail ||
                deliveryMethod == Constants.DeliveryMethodDBO ||
                deliveryMethod == Constants.DeliveryMethodPOST_DDS ||
                deliveryMethod == Constants.DeliveryMethodAP;
        }
    }
}
