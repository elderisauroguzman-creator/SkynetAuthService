namespace AuthService.DTOs
{
    // AuthService/DTOs/RolDto.cs
    public record RolDto(int Id, string Nombre, bool Activo);

    // AuthService/DTOs/UsuarioDtos.cs
    public record UsuarioListDto(int Id, string Nombre, int IdRolUsuario, string UsuarioNombre, string Email, bool Activo, DateTime FechaCreacion);

    public record UsuarioCreateDto(string Nombre, int IdRolUsuario, string UsuarioNombre, string Contrasena, string Email);

    public record UsuarioUpdateDto(string Nombre, int IdRolUsuario, string Email, bool Activo);

}
