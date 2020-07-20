namespace ReferenceService.WebApi.Common
{
    /// <summary>
    /// Константы WebApi
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Код ошибки для обязательных полей
        /// </summary>
        public const string ErrorCodeRequired = "1";

        /// <summary>
        /// Код ошибки для наличия значения поля в списка
        /// </summary>
        public const string ErrorCodeExists = "2";

        /// <summary>
        /// Код ошибки для наличия значения поля в списка
        /// </summary>
        public const string ErrorCodeFormatDateTime = "50";

        /// <summary>
        /// Название контроллера ошибок
        /// </summary>
        public const string ErrorControllerName = "ErrorController";

        /// <summary>
        /// Название метода для HTTP-ошибки 404
        /// </summary>
        public const string ErrorNotFoundHandle = "Handle404";

        /// <summary>
        /// Новая справка
        /// </summary>
        public const string ReferenceStatusNew = "NEW";

        /// <summary>
        /// Статус PDF_OK
        /// </summary>
        public const string ReferenceStatusPdfOk = "PDF_OK";

        /// <summary>
        /// Статус PDF_ERROR
        /// </summary>
        public const string ReferenceStatusPdfError = "PDF_ERROR";

        /// <summary>
        /// Статус SENDED
        /// </summary>
        public const string ReferenceStatusSended = "SENDED";

        /// <summary>
        /// Статус SEND_ERROR
        /// </summary>
        public const string ReferenceStatusSendError = "SEND_ERROR";

        /// <summary>
        /// Статус DELIVERED
        /// </summary>
        public const string ReferenceStatusDelivered = "DELIVERED";

        /// <summary>
        /// Код типа справки "Справка о полном погашении задолженности"
        /// </summary>
        public const string WpaRef1 = "1";

        /// <summary>
        /// Код типа справки "Справка об отсутствии действующих кредитных договоров"
        /// </summary>
        public const string WpaRef2 = "2";

        /// <summary>
        /// Код типа справки "Справка о параметрах кредитного договора"
        /// </summary>
        public const string WpaRef3 = "3";

        /// <summary>
        /// Код типа справки "Справка о своевременном погашении кредитного договора"
        /// </summary>
        public const string WpaRef4 = "4";

        /// <summary>
        /// Код типа справки "Выписка по счёту"
        /// </summary>
        public const string WpaRef5 = "5";

        /// <summary>
        /// Справка оформлена в БО
        /// </summary>
        public const string RegistrationChannelBO = "BO";

        /// <summary>
        /// Справка оформлена в КЦ
        /// </summary>
        public const string RegistrationChannelKC = "KC";

        /// <summary>
        /// Справка оформлена в AP
        /// </summary>
        public const string RegistrationChannelAP = "AP";

        /// <summary>
        /// Справка выдана в БО
        /// </summary>
        public const string DeliveryMethodBO = "BO";

        /// <summary>
        /// Справка выдана в AP
        /// </summary>
        public const string DeliveryMethodAP = "AP";

        /// <summary>
        /// Справка отправлена на email
        /// </summary>
        public const string DeliveryMethodEmail = "EMAIL";

        /// <summary>
        /// Самостоятельный способ получения справки, при котором Клиент заходит в личный кабинет,
        /// например Интернет-банка, и получает справку там.
        /// </summary>
        public const string DeliveryMethodDBO = "DBO";

        /// <summary>
        /// Справка отправлена на email
        /// </summary>
        public const string DeliveryMethodPOST_DDS = "POST_DDS";

        /// <summary>
        /// Идентификатор типа Email для EventHub
        /// </summary>
        public const long EventTypeID = 5000;
    }
}
