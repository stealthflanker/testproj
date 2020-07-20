namespace ReferenceService.WebApi.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// Сущность справки в БД
    /// </summary>
    [Table("REFERENCES", Schema = "sch_reference")]
    public class Reference
    {
        /// <summary>
        /// Конструктор для <see cref="Reference"/>
        /// </summary>
        public Reference()
        {
            CreateDateTime = DateTime.Now;
        }

        /// <summary>
        /// Номер справки
        /// </summary>
        [Key]
        [Column("ref_number"), Required, StringLength(15)]
        public string Number { get; set; }

        /// <summary>
        /// Состояние справки
        /// </summary>
        [Column("ref_status"), Required, StringLength(15)]
        public string Status { get; set; }

        /// <summary>
        /// Код справки
        /// </summary>
        [Column("ref_code"), Required, StringLength(15)]
        public string Code { get; set; }

        /// <summary>
        /// Название справки
        /// </summary>
        [Column("ref_name"), StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// Канал оформления справки
        /// </summary>
        [Column("ref_reg_channel"), Required, StringLength(15)]
        public string RegistrationChannel { get; set; }

        /// <summary>
        /// Способ доставки Справки
        /// </summary>
        [Column("delivery_method"), StringLength(15)]
        public string DeliveryMethod { get; set; }

        /// <summary>
        /// Email, на который отправляется справка
        /// </summary>
        [Column("delivery_email"), StringLength(255)]
        public string DeliveryEmail { get; set; }

        /// <summary>
        /// Номер БО в котором печатается справка
        /// </summary>
        [Column("delivery_bo_number"), StringLength(255)]
        public string DeliveryBONumber { get; set; }

        /// <summary>
        /// Локальная дата и время пользователя сформировавшего справку
        /// </summary>
        [Column("user_date_time")]
        public DateTime? UserDateTime { get; set; }

        /// <summary>
        /// Id пользователя
        /// </summary>
        [Column("user_id"), StringLength(255)]
        public string UserID { get; set; }

        /// <summary>
        /// ФИО пользователя
        /// </summary>
        [Column("user_fio"), StringLength(255)]
        public string UserFIO { get; set; }

        /// <summary>
        /// IP адрес рабочей станции
        /// </summary>
        [Column("user_workstation_ip"), StringLength(255)]
        public string UserWorkstationIP { get; set; }

        /// <summary>
        /// Название рабочей станции
        /// </summary>
        [Column("user_workstation_name"), StringLength(255)]
        public string UserWorkstationName { get; set; }

        /// <summary>
        /// CUID Кклиента
        /// </summary>
        [Column("customer_cuid"), StringLength(255)]
        public string CustomerCUID { get; set; }

        /// <summary>
        /// ФИО Клиента
        /// </summary>
        [Column("customer_fio"), StringLength(255)]
        public string CustomerFIO { get; set; }

        /// <summary>
        /// Серия и номер паспорта клиента
        /// </summary>
        [Column("customer_passportSerNum"), StringLength(10)]
        public string CustomerPassportSerNum { get; set; }

        /// <summary>
        /// Дата и время создания записи
        /// </summary>
        [Column("date_time")]
        public DateTime CreateDateTime { get; set; }
    }
}