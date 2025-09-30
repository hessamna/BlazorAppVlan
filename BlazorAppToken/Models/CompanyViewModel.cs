using System;
using System.ComponentModel.DataAnnotations;

public class CompanyViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Company name is required")]
    [MaxLength(500, ErrorMessage = "Company name cannot exceed 500 characters")]
    public string Name { get; set; }

    public bool IsVerified { get; set; }
}

public class SwitchViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Switch name is required")]
    [MaxLength(500, ErrorMessage = "Switch name cannot exceed 500 characters")]
    public string Name { get; set; }

    [MaxLength(500, ErrorMessage = "IP Interface cannot exceed 500 characters")]
    public string? IpInterface { get; set; }

    [MaxLength(500, ErrorMessage = "Model cannot exceed 500 characters")]
    public string? Model { get; set; }

    [Required(ErrorMessage = "CompanyId is required")]
    public Guid CompanyId { get; set; }
}

public class VlanViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "VLAN ID is required")]
    [MaxLength(500, ErrorMessage = "VLAN ID cannot exceed 500 characters")]
    public string VlanCode { get; set; }   // مثل "20"، "122"، "162"

    [Required(ErrorMessage = "VLAN name is required")]
    [MaxLength(500, ErrorMessage = "VLAN name cannot exceed 500 characters")]
    public string Name { get; set; }

    [MaxLength(500, ErrorMessage = "IP Interface cannot exceed 500 characters")]
    [Display(Name = "IP Interface")]
    public string? IpInterface { get; set; }

    [Required(ErrorMessage = "SwitchId is required")]
    public Guid SwitchId { get; set; }
}

public class DeviceInterfaceViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Port is required")]
    [MaxLength(500, ErrorMessage = "Port cannot exceed 500 characters")]
    public string Port { get; set; }

    [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
    public string? Description { get; set; }

    public bool IsConnected { get; set; }

    [Required(ErrorMessage = "SwitchId is required")]
    public Guid SwitchId { get; set; }

    [Required(ErrorMessage = "VlanId is required")]
    public Guid VlanId { get; set; }

}

public class NeighborViewModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "DeviceId is required")]
    [MaxLength(500, ErrorMessage = "DeviceId cannot exceed 500 characters")]
    public string DeviceId { get; set; }

    [Required(ErrorMessage = "Local Interface is required")]
    [MaxLength(50, ErrorMessage = "Local Interface cannot exceed 50 characters")]
    public string LocalInterface { get; set; }

    [MaxLength(500, ErrorMessage = "Neighbor Switch Name cannot exceed 500 characters")]
    public string? NeighborSWName { get; set; }

    [Required(ErrorMessage = "Neighbor Switch Port is required")]
    [MaxLength(500, ErrorMessage = "Neighbor Switch Port cannot exceed 500 characters")]
    public string NeighborSWNamePortId { get; set; }

    [Required(ErrorMessage = "VlanId is required")]
    public Guid VlanId { get; set; }

    [Required(ErrorMessage = "SwitchId is required")]
    public Guid SwitchId { get; set; }
}
public class SwitchWithPortsViewModel : SwitchViewModel
{
    public List<DeviceInterfaceViewModel> Ports { get; set; } = new();
}
