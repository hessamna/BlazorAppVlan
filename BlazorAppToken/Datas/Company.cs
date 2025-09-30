using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class Company : BaseEntity
{
    [Required, MaxLength(500)]
    public string Name { get; set; }

    public bool IsVerified { get; set; }

    // 🔹 Parent
    public int? ParentCompanyId { get; set; }
    public Company? ParentCompany { get; set; }

    // 🔹 Children
    public ICollection<Company> SubCompanies { get; set; } = new HashSet<Company>();

    // 🔹 ارتباط قبلی با سوئیچ‌ها
    public ICollection<Switch> Switches { get; set; } = new HashSet<Switch>();
}

public class Switch : BaseEntity
{
    [Required, MaxLength(500)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string? IpInterface { get; set; }

    [MaxLength(500)]
    public string? Model { get; set; }

    // FK
    [Required]
    public int CompanyId { get; set; }
    public Company Company { get; set; }

    public ICollection<Vlan> Vlans { get; set; } = new HashSet<Vlan>();
    public ICollection<DeviceInterface> DeviceInterfaces { get; set; } = new HashSet<DeviceInterface>();
    public ICollection<Neighbor> Neighbors { get; set; } = new HashSet<Neighbor>();
}

public class Vlan : BaseEntity
{
    // شماره VLAN به صورت رشته
    [Required, MaxLength(50)]
    public string VlanCode { get; set; }   // مثل "20" یا "122"

    [Required, MaxLength(500)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string? IpInterface { get; set; }

    // FK
    [Required]
    public int SwitchId { get; set; }
    public Switch Switch { get; set; }

    public ICollection<DeviceInterface> DeviceInterfaces { get; set; } = new HashSet<DeviceInterface>();
    public ICollection<Neighbor> Neighbors { get; set; } = new HashSet<Neighbor>();
}

public class DeviceInterface : BaseEntity
{
    [Required, MaxLength(500)]
    public string Port { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    public bool IsConnected { get; set; }

    // FKs
    [Required]
    public int SwitchId { get; set; }
    public Switch Switch { get; set; }

    [Required]
    public int VlanId { get; set; }
    public Vlan Vlan { get; set; }
}

public class Neighbor : BaseEntity
{
    [Required, MaxLength(500)]
    public string DeviceId { get; set; }

    [Required, MaxLength(50)]
    public string LocalInterface { get; set; }

    [MaxLength(500)]
    public string? NeighborSWName { get; set; }

    [MaxLength(500)]
    public string NeighborSWNamePortId { get; set; }

    // FKs
    [Required]
    public int VlanId { get; set; }
    public Vlan Vlan { get; set; }

    [Required]
    public int SwitchId { get; set; }
    public Switch Switch { get; set; }
}
