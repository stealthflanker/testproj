namespace ReferenceService.WebApi.Mappers
{
    using Common;
    using Entities;
    using Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Маппер для запроса CreateReferenceRequest
    /// </summary>
    public class CreateReferenceMapper
    {
        /// <summary>
        /// Конвертирует запрос в сущность справки
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <returns>Сущность справки</returns>
        public static Reference Map(CreateReferenceRequest request)
        {
            var reference = new Reference
            {
                Number = request.Code,
                Status = Constants.ReferenceStatusNew,
                Code = request.Code,
                Name = Resources.Strings.ResourceManager.GetString("ReferenceCode" + request.Code),
                RegistrationChannel = request.RegistrationChannel,
                CustomerCUID = request.Customer.CUID,
                CustomerFIO = request.Customer.FIO,
                CustomerPassportSerNum = request.Customer.PassportSerNum,
                
            };

            if(request.User != null)
            {
                reference.UserID = request.User.ID;
                reference.UserFIO = request.User.FIO;
                reference.UserDateTime = request.User.LocalDateTime;
                reference.UserWorkstationIP = request.User.Workstation.IP;
                reference.UserWorkstationName = request.User.Workstation.Name;
            }

            return reference;
        }
    }
}
