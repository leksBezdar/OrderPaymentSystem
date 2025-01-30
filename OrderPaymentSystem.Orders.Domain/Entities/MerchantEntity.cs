﻿namespace OrderPaymentSystem.Orders.Domain.Entities;

public class MerchantEntity : BaseEntity
{
    public required string Name { get; set; }
    public required string Phone { get; set; }
    public string? WebSite { get; set; }
    //public List<UserEntity> Users { get; set; }
}