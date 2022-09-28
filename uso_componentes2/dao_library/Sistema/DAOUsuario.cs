using System;
using System.Collections.Generic;
using entity_library.Sistema;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;

namespace dao_library.Sistema
{
	public partial class DAOUsuario
	{
		private ISession session;

		public DAOUsuario(ISession session)
		{
			this.session = session;
		}

		public entity_library.Sistema.Usuario ObtenerUsuario(long id)
		{
			try
			{
				return this.session.Get<entity_library.Sistema.Usuario>(id);
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUsuario.ObtenerUsuario(long id): Error al obtener el item con id = " + id.ToString(), ex);
			}
		}

		public void EliminarUsuario(entity_library.Sistema.Usuario usuario)
		{
			try
			{
				this.session.Delete(usuario);
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUsuario.EliminarUsuario(Usuario usuario): Error al eliminar el usuario", ex);
			}
		}

		public IList<entity_library.Sistema.Usuario> ObtenerListaUsuario(
			string query,
			List<dao_library.Utils.AtributoBusqueda> queryCols,
			dao_library.Utils.Paginado paginado,
			dao_library.Utils.Ordenamiento ordenamiento,
			out long cantidadTotal)
		{
			try
			{
				ICriteria lista = this.session.CreateCriteria<entity_library.Sistema.Usuario>("Usuario");
				ICriteria cantidad = this.session.CreateCriteria<entity_library.Sistema.Usuario>("Usuario");

				dao_library.Utils.UtilidadesNHibernate.AgregarCriteriosDeBusqueda(queryCols, query, lista);
				dao_library.Utils.UtilidadesNHibernate.AgregarCriteriosDeBusqueda(queryCols, query, cantidad);

				dao_library.Utils.UtilidadesNHibernate.AgregarOrdenamiento(ordenamiento, lista);
				dao_library.Utils.UtilidadesNHibernate.AgregarPaginado(paginado, lista);

				cantidadTotal = dao_library.Utils.UtilidadesNHibernate.ObtenerCantidad(cantidad);

				IList<entity_library.Sistema.Usuario> retorno = lista.List<entity_library.Sistema.Usuario>();

				return retorno;
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUsuario.ObtenerListaUsuario: Error al obtener el listado de items", ex);
			}
		}

        public Usuario ObtenerUsuario(string userName, string password)
        {
            ICriteria lista = this.session.CreateCriteria<entity_library.Sistema.Usuario>("Usuario");

			lista.Add(Restrictions.Eq("Usuario.NombreUsuario", userName));
			lista.Add(Restrictions.Eq("Usuario.Password", password));

			IList<entity_library.Sistema.Usuario> retorno = lista.List<entity_library.Sistema.Usuario>();

			if(retorno != null && retorno.Count > 0)
			{
				return retorno[0];
			}

			return null;
        }

        public IList<entity_library.Sistema.Usuario> ObtenerListaUsuario(
			string query,
			List<dao_library.Utils.AtributoBusqueda> queryCols,
			dao_library.Utils.Paginado paginado,
			dao_library.Utils.Ordenamiento ordenamiento,
			List<dao_library.Utils.Asociacion> asociaciones,
			out long cantidadTotal)
		{
			try
			{
				ICriteria lista = this.session.CreateCriteria<entity_library.Sistema.Usuario>("Usuario");
				ICriteria cantidad = this.session.CreateCriteria<entity_library.Sistema.Usuario>("Usuario");

				dao_library.Utils.UtilidadesNHibernate.CrearAsociaciones(asociaciones, lista);
				dao_library.Utils.UtilidadesNHibernate.CrearAsociaciones(asociaciones, cantidad);

				dao_library.Utils.UtilidadesNHibernate.AgregarCriteriosDeBusqueda(queryCols, query, lista);
				dao_library.Utils.UtilidadesNHibernate.AgregarCriteriosDeBusqueda(queryCols, query, cantidad);

				dao_library.Utils.UtilidadesNHibernate.AgregarOrdenamiento(ordenamiento, lista);
				dao_library.Utils.UtilidadesNHibernate.AgregarPaginado(paginado, lista);

				cantidadTotal = dao_library.Utils.UtilidadesNHibernate.ObtenerCantidad(cantidad);

				IList<entity_library.Sistema.Usuario> retorno = lista.List<entity_library.Sistema.Usuario>();

				return retorno;
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUsuario.ObtenerListaUsuario: Error al obtener el listado de items", ex);
			}
		}

		public void Guardar(entity_library.Sistema.Usuario item)
		{
			try
			{
				this.session.Save(item);
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUsuario.Guardar: Error al guardar el item.", ex);
			}
		}
	}
}