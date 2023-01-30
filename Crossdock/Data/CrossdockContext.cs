using System.Data.Entity;

namespace Crossdock.Data
{
    public class CrossdockContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public CrossdockContext() : base("name=CrossdockContext")
        {
        }

        public System.Data.Entity.DbSet<Crossdock.Models.Usuario> Usuarios { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Bodegas> Bodegas { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Clientes> Clientes { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Condiciones> Condiciones { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Delivery> Deliveries { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.EstatusPaquetes> EstatusPaquetes { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Operadores> Operadores { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Roles> Roles { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.TipoEntregas> TipoEntregas { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.TipoOperadores> TipoOperadores { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Unidades> Unidades { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Zonas> Zonas { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Devoluciones> Devoluciones { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.TipoUnidades> TipoUnidades { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.EstatusTareas> EstatusTareas { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.Coberturas> Coberturas { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.SeguimientoPaquetes> SeguimientoPaquetes { get; set; }

        public System.Data.Entity.DbSet<Crossdock.Models.SeguimientoPaquetes> HistorialPaquetes { get; set; }
    }
}
