namespace ReferenceService.WebApi.Mappers
{
    using Entities;
    using Models;

    /// <summary>
    /// Преобразует Reference <see cref="Reference"/> в ReferenceHistoryItem <see cref="ReferenceHistoryItem"/>
    /// </summary>
    public class ReferenceHistoryMapper
    {
        /// <summary>
        /// Преобразует Reference <see cref="Reference"/> в ReferenceHistoryItem <see cref="ReferenceHistoryItem"/>
        /// </summary>
        /// <param name="reference">Сущность справки</param>
        /// <returns>Элемент истории справок</returns>
        public static ReferenceHistoryItem Map(Reference reference)
        {
            return new ReferenceHistoryItem
            {
                Number = reference.Number,
                Code = reference.Code,
                Name = reference.Name,
                CreateDateTime = reference.CreateDateTime,
                RegistrationChannel = reference.RegistrationChannel,
                User = new User
                {
                    ID = reference.UserID,
                    FIO = reference.UserFIO,
                    LocalDateTime = reference.UserDateTime,
                    Workstation = new Workstation
                    {
                        IP = reference.UserWorkstationIP,
                        Name = reference.UserWorkstationName
                    }
                },
                Customer = new Customer
                {
                    CUID = reference.CustomerCUID,
                    FIO = reference.CustomerFIO,
                    PassportSerNum = reference.CustomerPassportSerNum
                }
            };
        }
    }
}
