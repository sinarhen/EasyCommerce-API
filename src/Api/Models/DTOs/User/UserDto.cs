namespace ECommerce.Models.DTOs.User;

public class UserDto
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public List<RoleDto> Roles { get; set; }
    public bool IsBanned { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
}

public class RoleDto 
{
    public string Name { get; set; }

    public int Level { get; set; }

}