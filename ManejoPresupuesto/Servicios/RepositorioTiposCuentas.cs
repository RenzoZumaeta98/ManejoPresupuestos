using Dapper;
using ManejoPresupuesto.Models;
using Microsoft.Data.SqlClient;

namespace ManejoPresupuesto.Servicios
{
    public interface IRepositorioTiposCuentas
    {
        Task Actualizar(TipoCuenta tipoCuenta);
        Task Borrar(int id);
        Task Crear(TipoCuenta tipoCuenta);
        void CrearSinAsync(TipoCuenta tipoCuenta);
        Task<bool> Existe(string nombre, int usuarioId);
        Task<IEnumerable<TipoCuenta>> Obtener(int usuario);
        Task<TipoCuenta> ObtenerPorId(int id, int usuarioId);
    }
    public class RepositorioTiposCuentas : IRepositorioTiposCuentas
    {
        private readonly string connectionString;
        public RepositorioTiposCuentas(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");

        }

        public async Task Crear(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>(@"INSERT INTO dbo.TiposCuentas(Nombre,UsuarioId,Orden)
                                                Values (@Nombre, @UsuarioId,0);
                                                SELECT SCOPE_IDENTITY();", tipoCuenta);
            tipoCuenta.Id = id;
        }

        public async Task<bool> Existe(string nombre, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            var existe = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM dbo.TiposCuentas
                                                                        WHERE Nombre = @Nombre AND UsuarioId = @UsuarioId;",
                                                                        new {nombre, usuarioId});
            return existe == 1;
        }

        public async Task<IEnumerable<TipoCuenta>> Obtener(int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                                            FROM dbo.TiposCuentas
                                                            Where UsuarioId = @UsuarioId", new {usuarioId});
        }

        public async Task Actualizar(TipoCuenta tipoCuenta)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE dbo.TiposCuentas
                                            SET Nombre = @Nombre 
                                            WHERE Id = @Id", tipoCuenta);

        }

        public async Task<TipoCuenta> ObtenerPorId(int id, int usuarioId)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<TipoCuenta>(@"SELECT Id, Nombre, Orden
                                                                FROM TiposCuentas
                                                                WHERE Id = @Id AND UsuarioId = @UsuarioId", new { id, usuarioId });
        }

        public async Task Borrar(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE dbo.TiposCuentas WHERE Id = @Id", new { id });
        }



        //EJEMPLO METODO CREAR SIN ASYNC AWAIT (MALA PRACTICA)
        public void CrearSinAsync(TipoCuenta tipoCuenta)
        {
          using var connection = new SqlConnection(connectionString);
          var id = connection.QuerySingle<int>($@"INSERT INTO dbo.TiposCuentas(Nombre,UsuarioId,Orden)
                                                Values (@Nombre, @UsuarioId,0);
                                                SELECT SCOPE_IDENTITY();", tipoCuenta);
          tipoCuenta.Id = id;
        }
    }
}
