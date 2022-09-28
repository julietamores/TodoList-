using System;
using NHibernate;

namespace dao_library
{
	public class DAOFactory : IDisposable
	{
		#region atributos privados
		private ISession session = null;
		private ITransaction transaction = null;
		#endregion

		#region Constructor
		public DAOFactory()
		{
			this.session = Database.Instance.SessionFactory.OpenSession();
		}
		#endregion

		#region métodos de la base de datos
		public bool BeginTrans()
		{
			try
			{
				this.transaction = this.session.BeginTransaction();
				return true;
			}
			catch (System.Exception e)
			{
				throw new System.Exception(
					"dao_library.NHibernateDAOFactory.BeginTrans()",
					e);
			}
		}
		public bool Commit()
		{
			try
			{
				this.transaction.Commit();

				this.transaction = null;

				return true;
			}
			catch (System.Exception e)
			{
				throw new System.Exception(
					"dao_library.NHibernateDAOFactory.Commit()",
					e);
			}
		}

		public void Rollback()
		{
			try
			{
				if (this.transaction == null || !this.transaction.IsActive) return;

				this.transaction.Rollback();

				this.transaction = null;
			}
			catch (System.Exception e)
			{
				throw new System.Exception("dao_library.NHibernateDAOFactory.Rollback()", e);
			}
		}

		public void Close()
		{
			try
			{
				if (this.transaction != null && this.transaction.IsActive)
				{
					this.transaction.Rollback();
				}

				this.session.Close();
			}
			catch (System.Exception e)
			{
				throw new System.Exception("dao_library.NHibernateDAOFactory.Close()", e);
			}
		}

		public void Dispose()
		{
			try
			{
				this.Close();
			}
			catch (System.Exception e)
			{
				throw new System.Exception("dao_library.NHibernateDAOFactory.Dispose()", e);
			}
		}
		#endregion

		#region DAOs: Agregar los DAOs de ustedes
		private dao_library.Sistema.DAOUsuario daoUsuario = null;
		public dao_library.Sistema.DAOUsuario DAOUsuario
		{
			get
			{
				if (this.daoUsuario == null)
				{
					this.daoUsuario = new dao_library.Sistema.DAOUsuario(this.session);
				}

				return daoUsuario;
			}
		}
		#endregion
	}
}