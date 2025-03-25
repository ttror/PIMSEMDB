internal static class UsuarioLogadoHelpers
{
    private static bool _isAdmin;

    public static bool IsAdmin
    {
        get { return _isAdmin; }
        set { _isAdmin = value; }
    }

    // Método para definir o tipo de usuário
    public static void DefinirTipoUsuario(bool isAdmin)
    {
        IsAdmin = isAdmin;
    }
}